using System;
using Perlin;
using Veldrid;
using Point = Perlin.Geom.Point;

namespace Snake_Game_V2.Entities.Snakes
{
    public class Snake : IAnimatable
    {
        private static readonly float _speed = 2;
        private int _health = 100;

        private SnakeHead _head;
        private DelayedModificationList<GameEntity> _body;

        public void ChangeHealth(int diff)
        {
            _health += diff;
        }

        public Snake(Point position)
        {
            _head = new SnakeHead(this, position);
            _body = new DelayedModificationList<GameEntity>();

            AddPart(4);
        }

        public void AddPart(int numParts)
        {
            var parent = GetLastPart();

            for (int i = 0; i < numParts; i++)
            {
                var newPart = new SnakeBody(parent.X, parent.Y);
                _body.Add(newPart);
            }

            Globals.Instance.Display.UpdateSnakeHeadDrawPosition(_head);
        }

        public void Step()
        {
            var turnDir = GetUserInput();
            _head.UpdateRotation(turnDir, _speed);

            UpdateSnakeBodyHistory();
            CheckForGameOverConditions();

            _body.DoPendingModifications();
        }

        private SnakeControl GetUserInput()
        {
            SnakeControl turnDir = SnakeControl.Invalid;
            if (KeyboardInput.IsKeyDown(Key.Left)) turnDir = SnakeControl.TurnLeft;
            if (KeyboardInput.IsKeyDown(Key.Right)) turnDir = SnakeControl.TurnRight;
            return turnDir;
        }

        private void CheckForGameOverConditions()
        {
            if (_head.IsOutOfBounds() || _health <= 0)
            {
                Console.WriteLine("Game over");
                Globals.Instance.StopGame();
            }
        }

        private void UpdateSnakeBodyHistory()
        {
            GameEntity prev = _head;
            foreach (var currentPart in _body.List)
            {
                currentPart.Position = prev.Position;
                prev = currentPart;
            }
        }

        private GameEntity GetLastPart()
        {
            var result = _body.Last;
            if (result != null) return result;
            return _head;
        }
    }
}