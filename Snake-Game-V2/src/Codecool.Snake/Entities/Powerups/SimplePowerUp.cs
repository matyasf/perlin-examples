using System;
using Codecool.Snake.Entities.Snakes;

namespace Codecool.Snake.Entities.Powerups
{
    /// <summary>
    /// Example implementation of a powerup
    /// </summary>
    public class SimplePowerUp : GameEntity, IInteractable
    {
        private static readonly Random _rnd = new Random();

        /// <summary>
        /// Initializes a new instance of the <see cref="SimplePowerUp"/> class.
        /// </summary>
        public SimplePowerUp()
            : base("powerup_berry.png")
        {
            X = (float)_rnd.NextDouble() * Globals.WindowWidth;
            Y = (float)_rnd.NextDouble() * Globals.WindowHeight;
        }

        /// <inheritdoc/>
        public void Apply(GameEntity entity)
        {
            if (entity is SnakeHead)
            {
                Console.WriteLine(GetMessage());
                Destroy();
            }
        }

        /// <inheritdoc/>
        public string GetMessage()
        {
            return "Got power-up :)";
        }
    }
}