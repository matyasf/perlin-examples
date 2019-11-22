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
        private static SpriteRenderer _spriteRenderer;
        
        private static GraphicsDevice _gd;
        private static Sdl2Window _window;
        private static CommandList _cl;
        private static RgbaFloat _clearColor = new RgbaFloat(0, 0, 0.2f, 1f);
        private static TextRenderer _textRenderer;
        
        public static void Start(int width, int height)
        {
            Configuration.Default.MemoryAllocator = new SixLabors.Memory.SimpleGcMemoryAllocator();
            GraphicsDeviceOptions options = new GraphicsDeviceOptions();
            _window = new Sdl2Window("Snake", 50, 50, width, height, SDL_WindowFlags.OpenGL, false);
#if DEBUG
            options.Debug = true;
#endif
            options.SyncToVerticalBlank = true;
            options.ResourceBindingModel = ResourceBindingModel.Improved;
            _gd = VeldridStartup.CreateGraphicsDevice(_window, options);
            _cl = _gd.ResourceFactory.CreateCommandList();
            _window.Resized += () => _gd.ResizeMainWindow((uint)_window.Width, (uint)_window.Height);
            _textRenderer = new TextRenderer(_gd);
            _spriteRenderer = new SpriteRenderer(_gd);
            Stopwatch sw = Stopwatch.StartNew();
            double previousTime = sw.Elapsed.TotalSeconds;
            Stage = new Stage(width, height);
            while (_window.Exists)
            {
                InputSnapshot snapshot = _window.PumpEvents();
                Input.UpdateFrameInput(snapshot);

                double newTime = sw.Elapsed.TotalSeconds;
                double elapsed = newTime - previousTime;
                previousTime = newTime;
                /*
               _snake.Update(elpased);
               if (_snake.Dead && Input.GetKeyDown(Key.Space))
               {
                   _snake.Revive();
                   _world.CollectFood();
               }
               */
                if (_window.Exists)
                {
                    _cl.Begin();
                    _cl.SetFramebuffer(_gd.MainSwapchain.Framebuffer);
                    _cl.ClearColorTarget(0, _clearColor);
                    Stage.Render();
                    _cl.End();
                    _gd.SubmitCommands(_cl);
                    _gd.SwapBuffers(_gd.MainSwapchain);
                }
            }
            _gd.Dispose();
            Console.WriteLine($"program end");
        }
    }
}