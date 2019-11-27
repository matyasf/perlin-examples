using System;
using System.Diagnostics;
using SixLabors.ImageSharp;
using Snake;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;

namespace Display
{
    public class KestrelApp
    {
        public static Stage Stage { get; private set; }
        public static SpriteRenderer SpriteRenderer { get; private set; }
        public static TextRenderer TextRenderer { get; private set; }
        public static KestrelPipeline KestrelPipeline { get; private set; }

        public static GraphicsDevice DefaultGraphicsDevice { get; private set; }
        private static Sdl2Window _window;
        public static CommandList CommandList { get; private set; }
        private static RgbaFloat _clearColor = new RgbaFloat(0, 0, 0.2f, 1f);
        
        public static void Start(int width, int height, Action onInit = null)
        {
            Configuration.Default.MemoryAllocator = new SixLabors.Memory.SimpleGcMemoryAllocator();
            GraphicsDeviceOptions options = new GraphicsDeviceOptions();
            _window = new Sdl2Window("Snake", 50, 50, width, height, SDL_WindowFlags.OpenGL, false);
#if DEBUG
            options.Debug = true;
#endif
            options.SyncToVerticalBlank = true;
            options.ResourceBindingModel = ResourceBindingModel.Improved;
            DefaultGraphicsDevice = VeldridStartup.CreateGraphicsDevice(_window, options);
            CommandList = DefaultGraphicsDevice.ResourceFactory.CreateCommandList();
            _window.Resized += () => DefaultGraphicsDevice.ResizeMainWindow((uint)_window.Width, (uint)_window.Height);
            KestrelPipeline = new KestrelPipeline(DefaultGraphicsDevice);
            TextRenderer = new TextRenderer(DefaultGraphicsDevice, KestrelPipeline);
            SpriteRenderer = new SpriteRenderer(DefaultGraphicsDevice, KestrelPipeline);
            Stopwatch sw = Stopwatch.StartNew();
            double previousTime = sw.Elapsed.TotalSeconds;
            Stage = new Stage(width, height);
            onInit?.Invoke();
            while (_window.Exists)
            {
                InputSnapshot snapshot = _window.PumpEvents();
                Input.UpdateFrameInput(snapshot);

                double newTime = sw.Elapsed.TotalSeconds;
                double elapsed = newTime - previousTime;
                previousTime = newTime;
                if (_window.Exists)
                {
                    CommandList.Begin();
                    CommandList.SetFramebuffer(DefaultGraphicsDevice.MainSwapchain.Framebuffer);
                    CommandList.ClearColorTarget(0, _clearColor);
                    Stage.Render(elapsed);
                    SpriteRenderer.Draw(DefaultGraphicsDevice, CommandList);
                    CommandList.End();
                    DefaultGraphicsDevice.SubmitCommands(CommandList);
                    DefaultGraphicsDevice.SwapBuffers(DefaultGraphicsDevice.MainSwapchain);
                }
            }
            DefaultGraphicsDevice.Dispose();
            Console.WriteLine($"program end");
        }
    }
}