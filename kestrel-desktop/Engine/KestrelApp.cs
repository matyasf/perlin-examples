using System;
using System.Diagnostics;
using Display;
using Engine.Display;
using SixLabors.ImageSharp;
using Snake;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;

namespace Engine
{
    public class KestrelApp
    {
        public static Stage Stage { get; private set; }
        public static TextRenderer TextRenderer { get; private set; }
        public static KestrelPipeline KestrelPipeline { get; private set; }
        internal static BatchRenderer Renderer { get; private set; }
        public static GraphicsDevice DefaultGraphicsDevice { get; private set; }
        private static Sdl2Window _window;
        public static CommandList CommandList { get; private set; }
        public static readonly ImageManager ImageManager = new ImageManager();
        
        public static void Start(int width, int height, Action onInit)
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
            TextRenderer = new TextRenderer();
            Renderer = new BatchRenderer();
            Stopwatch sw = Stopwatch.StartNew();
            double previousTime = sw.Elapsed.TotalSeconds;
            Stage = new Stage(width, height);
            onInit.Invoke();
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
                    CommandList.ClearColorTarget(0, Stage.BackgroundColor);
                    Stage.Render(elapsed);
                    Renderer.RenderQueue();
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