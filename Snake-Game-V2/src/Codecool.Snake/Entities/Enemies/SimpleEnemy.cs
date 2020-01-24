using System;
using Codecool.Snake.Entities.Snakes;
using Perlin.Geom;

namespace Codecool.Snake.Entities.Enemies
{
    /// <summary>
    /// Simple enemy that just moves in a line
    /// </summary>
    public sealed class SimpleEnemy : Enemy, IAnimatable, IInteractable
    {
        private Point _heading;
        private Random _rnd = new Random();

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleEnemy"/> class.
        /// </summary>
        public SimpleEnemy()
            : base(10, "simple_enemy.png")
        {
            X = (float)_rnd.NextDouble() * Globals.WindowWidth;
            Y = (float)_rnd.NextDouble() * Globals.WindowHeight;

            var direction = (float)_rnd.NextDouble() * 360f;
            Rotation = direction;

            int speed = 1;
            _heading = Utils.DirectionToVector(direction, speed);
        }

        /// <inheritdoc/>
        public void Step()
        {
            if (IsOutOfBounds())
            {
                Destroy();
            }

            X += _heading.X;
            Y += _heading.Y;
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
            return Damage + " damage";
        }
    }
}