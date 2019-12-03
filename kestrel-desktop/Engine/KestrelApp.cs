using System;
using System.Diagnostics;
using System.IO;
using Engine.Display;
using SixLabors.Fonts;
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
        private static StatsDisplay _statsDisplay;
        internal static CommandList CommandList { get; private set; }
        public static readonly FontCollection Fonts = new FontCollection();

        private static FontFamily _fontRobotoMono;
        public static FontFamily FontRobotoMono
        {
            get
            {
                if (_fontRobotoMono == null)
                {
                    _fontRobotoMono = Fonts.Install(Path.Combine(
                        AppContext.BaseDirectory, "Engine", "Assets", "RobotoMono-Regular.ttf"));
                }
                return _fontRobotoMono;
            }
        }

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
                    Stage.Render((float)elapsed);
                    Renderer.RenderQueue();
                    CommandList.End();
                    DefaultGraphicsDevice.SubmitCommands(CommandList);
                    DefaultGraphicsDevice.SwapBuffers(DefaultGraphicsDevice.MainSwapchain);
                }
            }
            DefaultGraphicsDevice.Dispose();
            Console.WriteLine($"program end");
        }
        
        public static void ShowStats(HorizontalAlignment horizontalAlign = HorizontalAlignment.Left, 
            VerticalAlignment verticalAlign = VerticalAlignment.Top/*, float scale = 1f*/)
        {
            float stageWidth  = Stage.Width;
            float stageHeight = Stage.Height;

            if (_statsDisplay == null)
            {
                _statsDisplay = new StatsDisplay();
                //_statsDisplay.Touchable = false;
            }

            Stage.AddChild(_statsDisplay);
            //_statsDisplay.ScaleX = _statsDisplay.ScaleY = scale;

            if (horizontalAlign == HorizontalAlignment.Left) _statsDisplay.X = 0f;
            else if (horizontalAlign == HorizontalAlignment.Right) _statsDisplay.X = stageWidth - _statsDisplay.Width;
            else if (horizontalAlign == HorizontalAlignment.Center) _statsDisplay.X = (stageWidth - _statsDisplay.Width) / 2;

            if (verticalAlign == VerticalAlignment.Top) _statsDisplay.Y = 0f;
            else if (verticalAlign == VerticalAlignment.Bottom) _statsDisplay.Y = stageHeight - _statsDisplay.Height;
            else if (verticalAlign == VerticalAlignment.Center) _statsDisplay.Y = (stageHeight - _statsDisplay.Height) / 2;
        }

    }
}