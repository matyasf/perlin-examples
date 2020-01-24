using Perlin;

namespace Codecool.Snake
{
    /// <summary>
    /// Main class
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Entry point
        /// </summary>
        public static void Main()
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