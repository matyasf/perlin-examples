using System;
using System.IO;
using System.Numerics;
using Engine;
using Engine.Display;
using SixLabors.Fonts;
using Veldrid;

namespace Snake
{
    class Program
    {
        private const int Width = 20;
        private const int Height = 10;
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
            KestrelApp.Stage.Tint = RgbaByte.Grey;
            // sample snake game
            /*
            var family = KestrelApp.Fonts.Install(Path.Combine(AppContext.BaseDirectory, "Assets", "Fonts", "Sunflower-Medium.ttf"));
            var font = family.CreateFont(28);
            
            var highScore = 0;
            var worldSize = new Vector2(Width, Height);
            var world = new World(worldSize, _cellSize);
            var snake = new Snake(world);
            var scoreTextField = new TextField(font) {Text = "0", Width = 250, Height = 100};
            scoreTextField.X = KestrelApp.Stage.Width / 2f - scoreTextField.Width / 2f;
            scoreTextField.Y = KestrelApp.Stage.Height - scoreTextField.Height - 10f;
            
            KestrelApp.Stage.AddChild(scoreTextField);
            snake.ScoreChanged += () => scoreTextField.Text =snake.Score.ToString();
            snake.ScoreChanged += () => highScore = Math.Max(highScore, snake.Score);
            */
//            var tf = new TextField(KestrelApp.FontRobotoMono.CreateFont(14)) {Text = "BBA", Width = 62, Height = 32, X = 40, Y = 20};
            var s2 = new Sprite("snake-head.png") {X = 150, Y = 150};
            var s3 = new Sprite("snake-head.png") {X = 0, Y = 50};
            var s4 = new Sprite("snake-3.png") {X = 0, Y = 30};
            s2.ScaleX = 2;
            s2.PivotX = 8;
            s2.PivotY = 8;
            s3.Rotation = 0.6f;
            
            //s3.ScaleX = 2;
//            var tf2 = new TextField(KestrelApp.FontRobotoMono.CreateFont(14)) {Text = "ZZB", Width = 62, Height = 32, X = 90, Y = 20};
//            KestrelApp.Stage.AddChild(tf);
            KestrelApp.Stage.AddChild(s2);
            s2.AddChild(s3);
            s3.AddChild(s4);
//            KestrelApp.Stage.AddChild(tf2);
            int a = 1;
            KestrelApp.Stage.EnterFrameEvent += (target, secs) =>
            {
                //tf2.Text = a.ToString();
                s2.Rotation += 0.01f;
                a++;
                if (a > 99) a = 0;
                //Console.WriteLine(s2.Rotation + " " + s2.GetBoundsWithChildren().Width);
            };
            //KestrelApp.ShowStats();
        }
    }
}
