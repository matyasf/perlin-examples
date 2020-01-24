using System;
using Codecool.Snake.Entities.Enemies;
using Codecool.Snake.Entities.Powerups;
using Perlin.Geom;

namespace Codecool.Snake.Entities.Snakes
{
    /// <summary>
    /// Head of the snake
    /// </summary>
    public class SnakeHead : GameEntity, IInteractable
    {
        private static readonly float _turnRate = 2;
        private readonly Snake _snake;

        /// <summary>
        /// Initializes a new instance of the <see cref="SnakeHead"/> class.
        /// </summary>
        /// <param name="snake">Refernce to the Snake</param>
        /// <param name="position">position</param>
        public SnakeHead(Snake snake, Point position)
            : base("snake_head.png")
        {
            _snake = snake;
            X = position.X;
            Y = position.Y;
            PivotX = Width / 2;
            PivotY = Height / 2;
        }

        /// <summary>
        /// Update rotation
        /// </summary>
        /// <param name="turnDirection">Turn direction</param>
        /// <param name="speed">speed in points</param>
        public void UpdateRotation(SnakeControl turnDirection, float speed)
        {
            if (turnDirection == SnakeControl.TurnLeft)
            {
                Rotation -= _turnRate;
            }

            if (turnDirection == SnakeControl.TurnRight)
            {
                Rotation += _turnRate;
            }

            var heading = Utils.DirectionToVector(Rotation, speed);
            X += heading.X;
            Y += heading.Y;
        }

        /// <inheritdoc/>
        public void Apply(GameEntity entity)
        {
            if (entity is Enemy)
            {
                Console.WriteLine(GetMessage());
                _snake.ChangeHealth(((Enemy)entity).Damage);
            }

            if (entity is SimplePowerUp)
            {
                Console.WriteLine(GetMessage());
                _snake.AddPart(4);
            }
        }

        /// <inheritdoc/>
        public string GetMessage()
        {
            return "IMMA SNAEK HED! SPITTIN' MAH WENOM! SPITJU-SPITJU!";
        }
    }
}