using Engine;

namespace Snake_Game_V2
{
    public class Globals
    {
        private static Globals _instance;

        public static readonly int WindowWidth = 1000;
        public static readonly int WindowHeight = 1000;

        public Display Display;
        public Game Game;
        public GameLoop GameLoop;
        
        public static Globals Instance {
            get
            {
                if (_instance == null) _instance = new Globals();
                return _instance;   
            }
        }
        
        // singleton needs the class to have private constructor
        private Globals() {}

        public void StartGame()
        {
            GameLoop.Start();
        }

        public void StopGame()
        {
            GameLoop.Stop();
        }
        
    }
}