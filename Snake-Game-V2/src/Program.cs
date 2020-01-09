using Perlin;

namespace Snake_Game_V2
{
    class Program
    {
        static void Main(string[] args)
        {
            PerlinApp.Start(
                Globals.WindowWidth,
                Globals.WindowHeight, 
                "Snake Game",
                OnInit);
        }

        private static void OnInit()
        {
            var g = new Game();
            PerlinApp.Stage.AddChild(g);
            g.Start();
        }
    }
}