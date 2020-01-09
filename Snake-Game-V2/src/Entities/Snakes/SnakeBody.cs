using System.Collections.Generic;
using Perlin.Geom;

namespace Snake_Game_V2.Entities.Snakes
{
    public class SnakeBody : GameEntity
    {
        private readonly Queue<Point> _history = new Queue<Point>();
        private const int HistorySize = 10;

        public SnakeBody(float xc, float yc) : base("Assets/snake_body.png")
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

        public override Point Position
        {
            set
            {
                var currentPos = _history.Dequeue();// remove the oldest item from the history
                X = currentPos.X;
                Y = currentPos.Y;
                _history.Enqueue(value); // add the parent's current position to the beginning of the history
            }
        }
    }
}