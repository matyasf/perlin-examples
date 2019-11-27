using System;
using System.Numerics;
using Display;

namespace Snake
{
    class Program
    {
        private const int Width = 32;
        private const int Height = 20;
        private float _cellSize = 32;

        public static void Main()
        { 
            new Program();
        }
        
        public Program()
        {
            KestrelApp.Start(
                (int)(Width *_cellSize),
                (int)(Height * _cellSize), 
                OnInit);
        }

        private void OnInit()
        {
            var highScore = 0;
            var worldSize = new Vector2(Width, Height);
            var _world = new World(worldSize, _cellSize);
            var _snake = new Snake(_world);
            var _scoreTextField = new TextField {Text = "0", Width = 250, Height = 100};
            _scoreTextField.X = KestrelApp.Stage.Width / 2f - _scoreTextField.Width / 2f;
            _scoreTextField.Y = KestrelApp.Stage.Height - _scoreTextField.Height - 10f;
            
            KestrelApp.Stage.AddChild(_scoreTextField);
            _snake.ScoreChanged += () => _scoreTextField.Text =_snake.Score.ToString();
            _snake.ScoreChanged += () => highScore = Math.Max(highScore, _snake.Score);

        }
    }
}
