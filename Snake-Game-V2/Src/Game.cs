using Perlin.Display;
using Perlin.Geom;
using SixLabors.ImageSharp.PixelFormats;
using Snake_Game_V2.Entities.Enemies;
using Snake_Game_V2.Entities.Powerups;
using Snake_Game_V2.Entities.Snakes;

namespace Snake_Game_V2
{
    public class Game : Sprite
    {
        private Snake _snake;
        
        public Game() : base(1, 1, Rgba32.Transparent)
        {
            Globals.Instance.Game = this;
            Globals.Instance.Display = new Display(this);
            Init();
        }

        public void Init()
        {
            SpawnSnake();
            SpawnEnemies(4);
            SpawnPowerUps(4);

            var gameLoop = new GameLoop(_snake);
            Globals.Instance.GameLoop = gameLoop;
        }

        public void Start()
        {
            Globals.Instance.StartGame();
        }

        private void SpawnSnake()
        {
            _snake = new Snake(new Point(500, 500));
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