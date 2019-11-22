using SixLabors.ImageSharp;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Numerics;
using Display;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;

namespace Snake
{
    class Program
    {
        
        [Range(10, 100)] private int Width { get; } = 32;
        
        [Range(10, 100)] private int Height { get; } = 24;

        private readonly GraphicsDevice _gd;
        private readonly Sdl2Window _window;
        private readonly CommandList _cl;
        private readonly RgbaFloat _clearColor = new RgbaFloat(0, 0, 0.2f, 1f);
        private readonly SpriteRenderer _spriteRenderer;
        private readonly World _world;
        private readonly Snake _snake;
        private readonly TextRenderer _textRenderer;
        private readonly TextField _scoreTextField;
        private float _cellSize = 32;
        private int _highScore;

        public static void Main()
        { 
            new Program();
        }
        
        public Program() {
            var worldSize = new Vector2(Width, Height);

            Configuration.Default.MemoryAllocator = new SixLabors.Memory.SimpleGcMemoryAllocator();
            var width = (int)(worldSize.X * _cellSize);
            var height = (int)(worldSize.Y * _cellSize);
            GraphicsDeviceOptions options = new GraphicsDeviceOptions();
            _window = new Sdl2Window("Snake", 50, 50, width, height, SDL_WindowFlags.OpenGL, false);
#if DEBUG
            options.Debug = true;
#endif
            options.SyncToVerticalBlank = true;
            options.ResourceBindingModel = ResourceBindingModel.Improved;
            _gd = VeldridStartup.CreateGraphicsDevice(_window, options);
            _cl = _gd.ResourceFactory.CreateCommandList();
            _spriteRenderer = new SpriteRenderer(_gd);

            _window.Resized += () => _gd.ResizeMainWindow((uint)_window.Width, (uint)_window.Height);
            
            _world = new World(worldSize, _cellSize);
            _snake = new Snake(_world);
            _scoreTextField = new TextField {text = "0", Width = 250, Height = 100};
            _scoreTextField.X = (_window.Width / 2f) - _scoreTextField.Width / 2f;
            _scoreTextField.Y = _window.Height - _scoreTextField.Height - 10f;
            _textRenderer = new TextRenderer(_gd, _scoreTextField);
            _textRenderer.DrawText(_scoreTextField.text);
            _snake.ScoreChanged += () => _textRenderer.DrawText(_snake.Score.ToString());
            _snake.ScoreChanged += () => _highScore = Math.Max(_highScore, _snake.Score);

            Stopwatch sw = Stopwatch.StartNew();
            double previousTime = sw.Elapsed.TotalSeconds;
            while (_window.Exists)
            {
                InputSnapshot snapshot = _window.PumpEvents();
                Input.UpdateFrameInput(snapshot);

                double newTime = sw.Elapsed.TotalSeconds;
                double elapsed = newTime - previousTime;
                previousTime = newTime;
                Update(elapsed);

                if (_window.Exists)
                {
                    DrawFrame();
                }
            }

            _gd.Dispose();

            Console.WriteLine($"Thanks for playing! Your high score was {_highScore}.");
        }

        private void Update(double deltaSeconds)
        {
            _snake.Update(deltaSeconds);

            if (_snake.Dead && Input.GetKeyDown(Key.Space))
            {
                _snake.Revive();
                _world.CollectFood();
            }
        }

        private void DrawFrame()
        {
            _cl.Begin();
            _cl.SetFramebuffer(_gd.MainSwapchain.Framebuffer);
            _cl.ClearColorTarget(0, _clearColor);

            _snake.Render(_spriteRenderer);
            _world.Render(_spriteRenderer);
            _spriteRenderer.Draw(_gd, _cl);
            _spriteRenderer.RenderText(_gd, _cl, _textRenderer.TextureView, _scoreTextField);
            
            _cl.End();
            _gd.SubmitCommands(_cl);
            _gd.SwapBuffers(_gd.MainSwapchain);
        }
    }
}
