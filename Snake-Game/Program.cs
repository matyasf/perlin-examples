using System;
using System.IO;
using System.Numerics;
using Engine;
using Engine.Display;
using SixLabors.Fonts;
using SixLabors.ImageSharp.PixelFormats;
using Veldrid;
using Point = Engine.Geom.Point;

namespace Snake_Game
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
            var family = KestrelApp.Fonts.Install(Path.Combine(AppContext.BaseDirectory, "Assets", "Fonts", "Sunflower-Medium.ttf"));
            var font = family.CreateFont(28);
            
            var highScore = 0;
            var worldSize = new Vector2(Width, Height);
            var world = new World(worldSize, _cellSize);
            var snake = new Snake(world);
            var scoreTextField = new TextField(font, "0", false) {Width = 50, Height = 25,
                                                BackgroundColor = Rgba32.Coral};
            scoreTextField.X = KestrelApp.Stage.Width / 2f - scoreTextField.Width / 2f;
            scoreTextField.Y = 0;
            scoreTextField.FontColor = Rgba32.White;
            scoreTextField.HorizontalAlign = HorizontalAlignment.Center;
            
            KestrelApp.Stage.AddChild(scoreTextField);
            snake.ScoreChanged += () => scoreTextField.Text =snake.Score.ToString();
            snake.ScoreChanged += () => highScore = Math.Max(highScore, snake.Score);
            var headPath = Path.Combine(AppContext.BaseDirectory, "Assets", "snake-head.png");
            var bodyPath = Path.Combine(AppContext.BaseDirectory, "Assets", "snake-3.png");
//            var tf = new TextField(KestrelApp.FontRobotoMono.CreateFont(14)) {Text = "BBA", Width = 62, Height = 32, X = 40, Y = 20};
            var s2 = new Sprite(headPath) {X = 150, Y = 150};
            var s3 = new Sprite(headPath) {X = 0, Y = 50};
            var s4 = new Sprite(bodyPath) {X = 0, Y = 30};
            s2.ScaleX = 2;
            s2.PivotX = 8;
            s2.PivotY = 8;
            s3.Rotation = 6f;
            
            //s3.ScaleX = 2;
            var tf2 = new TextField(KestrelApp.FontRobotoMono.CreateFont(13))
            {
                Text = "ZddedZBféáŰ", 
                Width = 62, Height = 56,
                X = 190, Y = 20,
              //  AutoSize = true,
                BackgroundColor = new Rgba32(255, 0, 0, 200)
            };
            KestrelApp.Stage.AddChild(tf2);
            KestrelApp.Stage.AddChild(s2);
            s2.AddChild(s3);
            s3.AddChild(s4);
            int a = 1;
            KestrelApp.Stage.EnterFrameEvent += (target, secs) =>
            {
               // tf2.Width++;
                s2.Rotation += 1f;
                a++;
                if (a > 99) a = 0;
                //Console.WriteLine(s2.Rotation + " " + s2.GetBoundsWithChildren().Width);
            };
            KestrelApp.ShowStats(HorizontalAlignment.Right, VerticalAlignment.Bottom, 2);
            
            
            Button bb = new Button("Cancel");
            bb.Y = 70;
            bb.X = 5;
            KestrelApp.Stage.AddChild(bb);
            
            // drag test
            var dragging = false;
            Point dragOffset = new Point();
            bb.MouseDown += (target, coords, button) =>
            {
                dragging = true;
                dragOffset = bb.GlobalToLocal(coords);
            };
            bb.MouseMoved += (target, coords) =>
            {
                if (dragging)
                {
                    bb.X = coords.X - dragOffset.X;
                    bb.Y = coords.Y - dragOffset.Y;
                }
            };
            bb.MouseUp += (target, coords, button) => dragging = false;
        }
        
    }
}
