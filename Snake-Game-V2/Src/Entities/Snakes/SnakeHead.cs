using System;
using Perlin.Geom;
using Snake_Game_V2.Entities.Enemies;
using Snake_Game_V2.Entities.Powerups;

namespace Snake_Game_V2.Entities.Snakes
{
    public class SnakeHead : GameEntity, IInteractable
    {
        private static readonly float _turnRate = 2;
        private Snake Snake;
        
        public SnakeHead(Snake snake, Point position) : base("Assets/snake_head.png")
        {
            Snake = snake;
            X = position.X;
            Y = position.Y;
            PivotX = Width / 2;
            PivotY = Height / 2;
        }

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

        public void Apply(GameEntity entity)
        {
            if (entity is Enemy)
            {
                Console.WriteLine(GetMessage());
                Snake.ChangeHealth(((Enemy)entity).Damage);
            }
            if (entity is SimplePowerUp){
                Console.WriteLine(GetMessage());
                Snake.AddPart(4);
            }
        }

        public string GetMessage()
        {
            return "IMMA SNAEK HED! SPITTIN' MAH WENOM! SPITJU-SPITJU!";
        }
    }
}