using SixLabors.ImageSharp;

namespace Snake_Game
{
    using System;
    using System.IO;
    using System.Numerics;
    using Perlin;
    using Perlin.Display;
    using SixLabors.Fonts;
    using SixLabors.ImageSharp.PixelFormats;
    using Point = Perlin.Geom.Point;
    using Rectangle = Perlin.Geom.Rectangle;

    /// <summary>
    /// A small sandbox app to test Perlin features.
    /// </summary>
    internal class Program
    {
        private const int Width = 20;
        private const int Height = 10;
        private float _cellSize = 32;

        public static void Main()
        {
            new Program();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Program"/> class.
        /// </summary>
        public Program()
        {
            PerlinApp.Start(
                (int)(Width * _cellSize),
                (int)(Height * _cellSize),
                "Snake Game",
                OnInit);
        }

        private void OnInit()
        {
            PerlinApp.Stage.BackgroundColor = new Rgb24(200, 200, 200);
            //TextFieldTests();
            //TestDragging();
            //TestTextureSubRegion();
            //TestTransparency();
            
            // sample snake game
            var scoreTextField = new TextField(PerlinApp.FontRobotoMono.CreateFont(18));
            scoreTextField.BackgroundColor = Color.Coral;
            scoreTextField.X = 260;
            scoreTextField.Y = 5;
            scoreTextField.FontColor = Color.White;
            PerlinApp.Stage.AddChild(scoreTextField);
            var highScore = 0;
            var worldSize = new Vector2(Width, Height);
            var world = new World(worldSize, _cellSize);
            var snake = new Snake(world);
            snake.ScoreChanged += () => scoreTextField.Text = snake.Score.ToString();
            snake.ScoreChanged += () => highScore = Math.Max(highScore, snake.Score);
            var headPath = Path.Combine(AppContext.BaseDirectory, "Assets", "snake-head.png");
            var bodyPath = Path.Combine(AppContext.BaseDirectory, "Assets", "snake-3.png");

            var s2 = new Sprite(headPath) { X = 150, Y = 150 };
            var s3 = new Sprite(headPath) { X = 0, Y = 50 };
            var s4 = new Sprite(bodyPath) { X = 0, Y = 30 };
            s2.ScaleX = 2;
            s2.PivotX = 8;
            s2.PivotY = 8;
            s3.Rotation = 6f;
            PerlinApp.Stage.AddChild(s2);
            s2.AddChild(s3);
            s3.AddChild(s4);
            int a = 1;
            PerlinApp.Stage.EnterFrameEvent += (target, secs) =>
            {
                s2.Rotation += 1f;
                a++;
                if (a > 99) a = 0;
                // Console.WriteLine(s2.Rotation + " " + s2.GetBoundsWithChildren().Width);
            };
            
            PerlinApp.ShowStats(HorizontalAlignment.Right, VerticalAlignment.Bottom);
        }

        private void TestTransparency()
        {
            var t1 = new Sprite(10, 50, new Rgba32(123, 123, 123, 123)) { X = 10, Y = 200, Name = "t1" };
            var t2 = new Sprite(20, 50, new Rgba32(123, 123, 123, 123)) { X = 10, Y = 210, Name = "t2" };
            var t3 = new Sprite(30, 50, new Rgba32(123, 123, 123, 123)) { X = 10, Y = 220, Name = "t3" };
            PerlinApp.Stage.AddChild(t1);
            PerlinApp.Stage.AddChild(t1);
            PerlinApp.Stage.AddChild(t2);
            PerlinApp.Stage.AddChild(t3);

            var transparencyBug = new Sprite(50, 50, new Rgba32(233, 12, 12, 255));
            transparencyBug.Alpha = 0.49f;
            transparencyBug.Name = "bottomS";
            var transparencyBug2 = new Sprite(50, 50, new Rgba32(233, 212, 12, 255));
            transparencyBug2.X = 20;
            transparencyBug2.Y = 20;
            transparencyBug2.Alpha = 0.5f;
            transparencyBug2.Name = "topS";
            transparencyBug.AddChild(transparencyBug2);
            PerlinApp.Stage.AddChild(transparencyBug);
        }

        private void TestTextureSubRegion()
        {
            var subTex = new Sprite(
                    Path.Combine(AppContext.BaseDirectory, "Assets", "grizzly.png"), false,
                    new Rectangle(50, 50, 50, 50))
                    { X = 400, Y = 10 };
            PerlinApp.Stage.AddChild(subTex);
        }

        private void TestDragging()
        {
            Button bb = new Button("Cancel");
            bb.Y = 70;
            bb.X = 5;
            PerlinApp.Stage.AddChild(bb);
            
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

        private void TextFieldTests()
        {
            var font = PerlinApp.FontRobotoMono.CreateFont(18);
            
            var tf1 = new TextField(font, "jMÍ", false)
            {
                BackgroundColor = Color.Azure,
                Width = 50,
                Height = 50,
                X = 5,
                Y = 5,
                VerticalAlign = VerticalAlignment.Top,
                HorizontalAlign = HorizontalAlignment.Left
            };
            PerlinApp.Stage.AddChild(tf1);
            
            var tf2 = new TextField(font, "jMÍ", false)
            {
                BackgroundColor = Color.Azure,
                Width = 50,
                Height = 50,
                X = 60,
                Y = 5,
                VerticalAlign = VerticalAlignment.Center,
                HorizontalAlign = HorizontalAlignment.Center
            };
            PerlinApp.Stage.AddChild(tf2);
            
            var tf3 = new TextField(font, "jee", false)
            {
                BackgroundColor = Color.Azure,
                Width = 50,
                Height = 50,
                X = 115,
                Y = 5,
                VerticalAlign = VerticalAlignment.Bottom,
                HorizontalAlign = HorizontalAlignment.Right
            };
            PerlinApp.Stage.AddChild(tf3);
            
            var tf4 = new TextField(font, "Hello\n World!")
            {
                BackgroundColor = Color.Azure,
                X = 170,
                Y = 5
            };
            PerlinApp.Stage.AddChild(tf4);
        }
    }
}
