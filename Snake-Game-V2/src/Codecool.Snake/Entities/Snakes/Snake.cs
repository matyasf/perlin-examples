using System;
using Perlin;
using Veldrid;
using Point = Perlin.Geom.Point;

namespace Codecool.Snake.Entities.Snakes
{
    /// <summary>
    /// Snake
    /// </summary>
    public class Snake : IAnimatable
    {
        private static readonly float _speed = 2;
        private int _health = 100;

        private SnakeHead _head;
        private DelayedModificationList<GameEntity> _body;

        /// <summary>
        /// Changes health by the given value
        /// </summary>
        /// <param name="diff">Health change value</param>
        public void ChangeHealth(int diff)
        {
            _health += diff;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Snake"/> class.
        /// </summary>
        /// <param name="position">starting position</param>
        public Snake(Point position)
        {
            _head = new SnakeHead(this, position);
            _body = new DelayedModificationList<GameEntity>();

            AddPart(4);
        }

        /// <summary>
        /// Adds the given number of parts to the snake
        /// </summary>
        /// <param name="numParts">number of parts to add</param>
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

        /// <inheritdoc/>
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
            if (KeyboardInput.IsKeyDown(Key.Left))
            {
                turnDir = SnakeControl.TurnLeft;
            }

            if (KeyboardInput.IsKeyDown(Key.Right))
            {
                turnDir = SnakeControl.TurnRight;
            }

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
            if (result != null)
            {
                return result;
            }

            return _head;
        }
    }
}