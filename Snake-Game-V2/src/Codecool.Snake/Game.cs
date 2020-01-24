using Codecool.Snake.Entities.Enemies;
using Codecool.Snake.Entities.Powerups;
using Perlin.Display;
using Perlin.Geom;

namespace Codecool.Snake
{
    /// <summary>
    /// The initial class to start the game.
    /// </summary>
    public class Game : Sprite
    {
        private Entities.Snakes.Snake _snake;

        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class.
        /// </summary>
        public Game()
        {
            Globals.Instance.Game = this;
            Globals.Instance.Display = new Display(this);
            Init();
        }

        /// <summary>
        /// Initializes the game.
        /// </summary>
        public void Init()
        {
            SpawnSnake();
            SpawnEnemies(4);
            SpawnPowerUps(4);

            var gameLoop = new GameLoop(_snake);
            Globals.Instance.GameLoop = gameLoop;
        }

        /// <summary>
        /// Starts the game.
        /// </summary>
        public void Start()
        {
            Globals.Instance.StartGame();
        }

        private void SpawnSnake()
        {
            _snake = new Entities.Snakes.Snake(new Point(500, 500));
        }

        private void SpawnEnemies(int numEnemies)
        {
            for (int i = 0; i < numEnemies; i++)
            {
                new SimpleEnemy();
            }
        }

        private void SpawnPowerUps(int numOfPowerUps)
        {
            for (int i = 0; i < numOfPowerUps; i++)
            {
                new SimplePowerUp();
            }
        }
    }
}