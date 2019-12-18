using System;
using System.Diagnostics;
using System.IO;
using Engine.Display;
using Engine.Rendering;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;

namespace Engine
{
    public static class KestrelApp
    {
        // bug ImageSharp beta7: if text overflows it'll throw an exception, see
        // https://github.com/SixLabors/ImageSharp/issues/688
        // As a workaround this project uses a nightly build.
        
        /// <summary>
        /// The engine's image manager. You can use this to load images to your project.
        /// </summary>
        public static readonly ImageManager ImageManager = new ImageManager();
        
        /// <summary>
        /// Stage is the root of the display. Anything that you want to show must be added with
        /// <code>AddChild</code> to the Stage.
        /// </summary>
        public static Stage Stage { get; private set; }
        internal static KestrelPipeline KestrelPipeline { get; private set; }
        internal static BatchRenderer Renderer { get; private set; }
        internal static GraphicsDevice DefaultGraphicsDevice { get; private set; }
        internal static Sdl2Window Window { get; private set; }
        private static StatsDisplay _statsDisplay;
        internal static CommandList CommandList { get; private set; }
        
        /// <summary>
        /// The default font collection for the engine. You should create fonts with this instance.
        /// </summary>
        public static readonly FontCollection Fonts = new FontCollection();

        private static FontFamily _fontRobotoMono;
        /// <summary>
        /// Built-in Roboto Mono Regular font.
        /// </summary>
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
        
        /// <summary>
        /// Starts you application. Call this method once when you want to start your app.
        /// </summary>
        /// <param name="width">The app window's width</param>
        /// <param name="height">The app window's height</param>
        /// <param name="windowTitle">The app window's title</param>
        /// <param name="onInit">Method to call when the app started. At this point the app is ready,
        /// you can add things to the stage, add event listeners etc.</param>
        public static void Start(int width, int height, string windowTitle, Action onInit)
        {
            Configuration.Default.MemoryAllocator = new SixLabors.Memory.SimpleGcMemoryAllocator();
            GraphicsDeviceOptions options = new GraphicsDeviceOptions();
            Window = new Sdl2Window(windowTitle, 50, 50, width, height, SDL_WindowFlags.OpenGL, false);
#if DEBUG
            options.Debug = true;
#endif
            options.SyncToVerticalBlank = true;
            options.ResourceBindingModel = ResourceBindingModel.Improved;
            //options.PreferStandardClipSpaceYDirection = true;
            
            DefaultGraphicsDevice = VeldridStartup.CreateGraphicsDevice(Window, options);
            CommandList = DefaultGraphicsDevice.ResourceFactory.CreateCommandList();
            Window.Resized += () => DefaultGraphicsDevice.ResizeMainWindow((uint)Window.Width, (uint)Window.Height);
            KestrelPipeline = new KestrelPipeline(DefaultGraphicsDevice);
            Renderer = new BatchRenderer();
            Stopwatch sw = Stopwatch.StartNew();
            double previousTime = sw.Elapsed.TotalSeconds;
            Stage = new Stage(width, height);
            onInit.Invoke();
            // The main loop. This gets repeated every frame.
            while (Window.Exists)
            {
                InputSnapshot snapshot = Window.PumpEvents();
                Input.UpdateFrameInput(snapshot);

                double newTime = sw.Elapsed.TotalSeconds;
                double elapsed = newTime - previousTime;
                previousTime = newTime;
                if (Window.Exists)
                {
                    CommandList.Begin();
                    CommandList.SetFramebuffer(DefaultGraphicsDevice.MainSwapchain.Framebuffer);
                    CommandList.ClearColorTarget(0, new RgbaFloat(
                        Stage.Tint.R/255f,
                        Stage.Tint.G/255f,
                        Stage.Tint.B/255f,
                        Stage.Tint.A/255f));
                    Stage.Render((float)elapsed);
                    Renderer.RenderQueue();
                    CommandList.End();
                    DefaultGraphicsDevice.SubmitCommands(CommandList);
                    DefaultGraphicsDevice.SwapBuffers(DefaultGraphicsDevice.MainSwapchain);
                }
            }
            DefaultGraphicsDevice.Dispose();
            Console.WriteLine("program end");
        }
        
        /// <summary>
        /// Shows a small debug statistic overlay on one of the corners.
        /// </summary>
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