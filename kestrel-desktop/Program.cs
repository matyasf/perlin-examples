using System;
using System.IO;
using System.Numerics;
using Engine;
using Engine.Display;
using SixLabors.Fonts;

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
                "Snake Game",
                OnInit);
        }

        private void OnInit()
        {
            // sample snake game
            var fc = new FontCollection();
            var family = fc.Install(Path.Combine(AppContext.BaseDirectory, "Assets", "Fonts", "Sunflower-Medium.ttf"));
            var font = family.CreateFont(28);
            
            var highScore = 0;
            var worldSize = new Vector2(Width, Height);
            var _world = new World(worldSize, _cellSize);
            var _snake = new Snake(_world);
            var _scoreTextField = new TextField(font) {Text = "0", Width = 250, Height = 100};
            _scoreTextField.X = KestrelApp.Stage.Width / 2f - _scoreTextField.Width / 2f;
            _scoreTextField.Y = KestrelApp.Stage.Height - _scoreTextField.Height - 10f;
            
            KestrelApp.Stage.AddChild(_scoreTextField);
            _snake.ScoreChanged += () => _scoreTextField.Text =_snake.Score.ToString();
            _snake.ScoreChanged += () => highScore = Math.Max(highScore, _snake.Score);
            
            /*
            var s1 = new Sprite("snake-3.png") {Width = 32, Height = 32, X = 20, Y = 20};
            var tf = new TextField {Text = "BBA", Width = 62, Height = 32, X = 40, Y = 20};
            var s2 = new Sprite("snake-3.png") {Width = 32, Height = 32, X = 60, Y = 20};
            var s3 = new Sprite("snake-head.png") {Width = 32, Height = 32, X = 80, Y = 20};
            var tf2 = new TextField {Text = "ZZB", Width = 62, Height = 32, X = 90, Y = 20};
            
            KestrelApp.Stage.AddChild(s1);
            KestrelApp.Stage.AddChild(tf);
            KestrelApp.Stage.AddChild(s2);
            KestrelApp.Stage.AddChild(s3);
            KestrelApp.Stage.AddChild(tf2);
            int a = 1;
            KestrelApp.Stage.EnterFrameEvent += elapsed =>
            {
                tf2.Text = a.ToString();
                a++;
                if (a > 99) a = 0;
            };
            */
        }
    }
}
