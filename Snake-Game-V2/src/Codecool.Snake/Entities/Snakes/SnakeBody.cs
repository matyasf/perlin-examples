using System.Collections.Generic;
using Perlin.Geom;

namespace Codecool.Snake.Entities.Snakes
{
    /// <summary>
    /// Body of the snake
    /// </summary>
    public class SnakeBody : GameEntity
    {
        private const int HistorySize = 10;
        private readonly Queue<Point> _history = new Queue<Point>();

        /// <summary>
        /// Initializes a new instance of the <see cref="SnakeBody"/> class.
        /// </summary>
        /// <param name="xc">X coordinate</param>
        /// <param name="yc">Y coordinate</param>
        public SnakeBody(float xc, float yc)
            : base("snake_body.png")
        {
            X = xc;
            Y = yc;
            PivotX = Width / 2;
            PivotY = Height / 2;

            for (int i = 0; i < HistorySize; i++)
            {
                _history.Enqueue(new Point(xc, yc));
            }
        }

        /// <inheritdoc/>
        public override Point Position
        {
            set
            {
                var currentPos = _history.Dequeue(); // remove the oldest item from the history
                X = currentPos.X;
                Y = currentPos.Y;
                _history.Enqueue(value); // add the parent's current position to the beginning of the history
            }
        }
    }
}