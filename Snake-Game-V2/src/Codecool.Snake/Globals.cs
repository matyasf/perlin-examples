namespace Codecool.Snake
{
    /// <summary>
    /// Class that holds global variables
    /// </summary>
    public class Globals
    {
        /// <summary>
        /// Width of the app's window
        /// </summary>
        public static readonly int WindowWidth = 1000;

        /// <summary>
        /// Height of the app's window
        /// </summary>
        public static readonly int WindowHeight = 700;

        /// <summary>
        /// reference to the Display
        /// </summary>
        public Display Display;

        /// <summary>
        /// reference to the game
        /// </summary>
        public Game Game;

        /// <summary>
        /// Reference to the game loop
        /// </summary>
        public GameLoop GameLoop;
        private static Globals _instance;

        /// <summary>
        /// Gets the singleton instance of this class.
        /// </summary>
        public static Globals Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Globals();
                }

                return _instance;
            }
        }

        // singleton needs the class to have private constructor
        private Globals()
        {
        }

        /// <summary>
        /// Starts the game
        /// </summary>
        public void StartGame()
        {
            GameLoop.Start();
        }

        /// <summary>
        /// Stops the game
        /// </summary>
        public void StopGame()
        {
            GameLoop.Stop();
        }
    }
}