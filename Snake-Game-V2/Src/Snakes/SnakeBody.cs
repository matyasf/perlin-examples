using System.Collections.Generic;
using Engine.Geom;

namespace Snake_Game_V2
{
    public class SnakeBody : GameEntity
    {
        private Queue<Point> _history = new Queue<Point>();
        private static readonly int _historySize = 10;

        public SnakeBody(float xc, float yc) : base("Assets/snake_body.png")
        {
            X = xc;
            Y = yc;

            for (int i = 0; i < _historySize; i++)
            {
                _history.Enqueue(new Point(xc, yc));
            }
        }

        public override Point Position
        {
            set
            {
                var currentPos = _history.Dequeue();
                X = currentPos.X;
                Y = currentPos.Y;
                _history.Enqueue(value);
            }
        }
    }
}