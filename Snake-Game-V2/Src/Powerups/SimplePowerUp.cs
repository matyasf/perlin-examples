using System;

namespace Snake_Game_V2.Powerups
{
    public class SimplePowerUp : GameEntity, IInteractable
    {
        private static readonly Random _rnd = new Random();

        public SimplePowerUp() : base("Assets/powerup_berry.png")
        {
            X = (float) _rnd.NextDouble() * Globals.WindowWidth;
            Y = (float) _rnd.NextDouble() * Globals.WindowHeight;
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