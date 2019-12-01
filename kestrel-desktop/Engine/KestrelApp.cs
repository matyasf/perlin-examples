using System;
using System.Diagnostics;
using Engine.Display;
using SixLabors.ImageSharp;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;

namespace Engine
{
    public class KestrelApp
    {
        public static readonly ImageManager ImageManager = new ImageManager();
        public static Stage Stage { get; private set; }
        internal static KestrelPipeline KestrelPipeline { get; private set; }
        internal static BatchRenderer Renderer { get; private set; }
        internal static GraphicsDevice DefaultGraphicsDevice { get; private set; }
        private static Sdl2Window _window;
        internal static CommandList CommandList { get; private set; }

        public static void Start(int width, int height, string windowTitle, Action onInit)
        {
            Configuration.Default.MemoryAllocator = new SixLabors.Memory.SimpleGcMemoryAllocator();
            GraphicsDeviceOptions options = new GraphicsDeviceOptions();
            _window = new Sdl2Window(windowTitle, 50, 50, width, height, SDL_WindowFlags.OpenGL, false);
#if DEBUG
            options.Debug = true;
#endif
            options.SyncToVerticalBlank = true;
            options.ResourceBindingModel = ResourceBindingModel.Improved;
            
            DefaultGraphicsDevice = VeldridStartup.CreateGraphicsDevice(_window, options);
            CommandList = DefaultGraphicsDevice.ResourceFactory.CreateCommandList();
            _window.Resized += () => DefaultGraphicsDevice.ResizeMainWindow((uint)_window.Width, (uint)_window.Height);
            KestrelPipeline = new KestrelPipeline(DefaultGraphicsDevice);
            Renderer = new BatchRenderer();
            Stopwatch sw = Stopwatch.StartNew();
            double previousTime = sw.Elapsed.TotalSeconds;
            Stage = new Stage(width, height);
            onInit.Invoke();
            // The main loop. This gets repeated every frame.
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