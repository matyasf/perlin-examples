using System;
using Perlin.Geom;
using Snake_Game_V2.Entities.Snakes;

namespace Snake_Game_V2.Entities.Enemies
{
    public class SimpleEnemy : Enemy, IAnimatable, IInteractable
    {

        private Point _heading;
        private Random _rnd = new Random();
        
        public SimpleEnemy() : base(10, "simple_enemy.png")
        {
            X = (float)_rnd.NextDouble() * Globals.WindowWidth;
            Y = (float)_rnd.NextDouble() * Globals.WindowHeight;

            var direction = (float) _rnd.NextDouble() * 360f;
            Rotation = direction;

            int speed = 1;
            _heading = Utils.DirectionToVector(direction, speed);
        }

        public void Step()
        {
            if (IsOutOfBounds())
            {
                Destroy();
            }
            X += _heading.X;
            Y += _heading.Y;
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
            return Damage + " damage";
        }
    }
}