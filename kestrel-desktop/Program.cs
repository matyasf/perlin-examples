using System;
using System.Numerics;
using Display;

namespace Snake
{
    class Program
    {
        
        private float _cellSize = 32;

        public static void Main()
        { 
            new Program();
        }
        
        public Program()
        {
            var width = 32;
            var height = 20;
            KestrelApp.Start((int)(width *_cellSize), (int)(height * _cellSize));
            var highScore = 0;
            var worldSize = new Vector2(width, height);
            var _world = new World(worldSize, _cellSize);
            var _snake = new Snake(_world);
            var _scoreTextField = new TextField {Text = "0", Width = 250, Height = 100};
            _scoreTextField.X = KestrelApp.Stage.Width / 2f - _scoreTextField.Width / 2f;
            _scoreTextField.Y = KestrelApp.Stage.Height - _scoreTextField.Height - 10f;
            
            KestrelApp.Stage.AddChild(_scoreTextField);
            
            //_textRenderer.DrawText(_scoreTextField.Text);
            //_snake.ScoreChanged += () => _textRenderer.DrawText(_snake.Score.ToString());
            _snake.ScoreChanged += () => _scoreTextField.Text =_snake.Score.ToString();
            _snake.ScoreChanged += () => highScore = Math.Max(highScore, _snake.Score);
        }
    }
}
