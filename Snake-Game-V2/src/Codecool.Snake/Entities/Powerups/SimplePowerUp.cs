using System;
using Snake_Game_V2.Entities.Snakes;

namespace Snake_Game_V2.Entities.Powerups
{
    public class SimplePowerUp : GameEntity, IInteractable
    {
        private static readonly Random _rnd = new Random();

        public SimplePowerUp() : base("powerup_berry.png")
        {
            X = (float)_rnd.NextDouble() * Globals.WindowWidth;
            Y = (float)_rnd.NextDouble() * Globals.WindowHeight;
        }

        public void Apply(GameEntity entity)
        {
            if (entity is SnakeHead)
            {
                Console.WriteLine(GetMessage());
                Destroy();
            }
        }

        public string GetMessage()
        {
            return "Got power-up :)";
        }
    }
}