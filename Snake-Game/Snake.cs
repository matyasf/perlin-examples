using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using Perlin;
using Perlin.Display;
using Veldrid;

namespace Snake_Game
{
    public class Snake
    {
        private const string BodySprite = "snake-3.png";
        private const string HeadSprite = "snake-head.png";
        private const double InitialUpdatePeriod = 0.2125; // seconds
        private const double SpeedupFactor = 0.97;
        private const double MinPeriod = 0.05;
        private const int InitialSize = 3;

        private readonly World _world;

        private readonly List<Sprite> _positions = new List<Sprite>();
        private Vector2 _direction;
        private Vector2 _previousDir;
        private double _updateTimer;
        private int _currentFood;
        private double _updatePeriod;
        private bool _dead;

        public int Score { get; private set; }
        public event Action ScoreChanged;

        public bool Dead => _dead;

        public Snake(World world)
        {
            _world = world;
            Revive();
            PerlinApp.Stage.EnterFrameEvent += Update;
        }

        public void Revive()
        {
            _dead = false;
            _updatePeriod = InitialUpdatePeriod;
            _currentFood = 0;
            _direction = Vector2.UnitX;
            foreach (var sprite in _positions)
            {
                sprite.RemoveFromParent();
            }
            _positions.Clear();
            SetScore(0);
            for (int i = 0; i < InitialSize; i++)
            {
                float rotation = GetRotation(_direction);
                var sp = new Sprite(Path.Combine(AppContext.BaseDirectory, "Assets", BodySprite))
                {
                    X = (3 + i) * _world.CellSize, Y = 3 * _world.CellSize,
                    Rotation = rotation,
                    ScaleX = 2, ScaleY = 2,
                    PivotX = 8, PivotY = 8
                };
                _positions.Add(sp);
                PerlinApp.Stage.AddChild(sp);
            }
            var headPath = Path.Combine(AppContext.BaseDirectory, "Assets", HeadSprite);
            _positions.Last().LoadImage(headPath);
        }

        private void SetScore(int newScore)
        {
            Score = newScore;
            ScoreChanged?.Invoke();
        }

        private float GetRotation(Vector2 direction)
        {
            if (direction == Vector2.UnitY) { return 180; }
            if (direction == Vector2.UnitX) { return 90; }
            if (direction == -Vector2.UnitX) { return -90; }
            return 0;
        }

        public void Update(DisplayObject target, float deltaSeconds)
        {
            if (Dead && KeyboardInput.IsKeyPressedThisFrame(Key.Space))
            {
                Revive();
                _world.CollectFood();
            }
            if (_dead)
            {
                return;
            }

            if (KeyboardInput.IsKeyPressedThisFrame(Key.Left))
            {
                TryChangeDirection(new Vector2(-1, 0));
            }
            else if (KeyboardInput.IsKeyPressedThisFrame(Key.Right))
            {
                TryChangeDirection(new Vector2(1, 0));
            }
            else if (KeyboardInput.IsKeyPressedThisFrame(Key.Up))
            {
                TryChangeDirection(new Vector2(0, -1));
            }
            else if (KeyboardInput.IsKeyPressedThisFrame(Key.Down))
            {
                TryChangeDirection(new Vector2(0, 1));
            }

            _updateTimer -= deltaSeconds;
            if (KeyboardInput.IsKeyDown(Key.Space))
            {
                _updateTimer -= deltaSeconds * 2;
            }
            if (_updateTimer > 0)
            {
                return;
            }
            _updateTimer = _updatePeriod;
            
            var newX = _positions.Last().X + _direction.X * _world.CellSize;
            var newY = _positions.Last().Y + _direction.Y * _world.CellSize;

            if (Collides(newX, newY))
            {
                _positions[_positions.Count - 1].Rotation = GetRotation(_direction);
                Die();
                return;
            }

            if (newX == _world.FoodSprite.X && newY == _world.FoodSprite.Y)
            {
                _world.CollectFood();
                _updatePeriod = Math.Max(MinPeriod, SpeedupFactor * _updatePeriod);
                _currentFood += 2;
                SetScore(Score + 1);
            }

            _previousDir = _direction;
            _positions.Last().LoadImage(Path.Combine(AppContext.BaseDirectory, "Assets", BodySprite));
            var sp = new Sprite(Path.Combine(AppContext.BaseDirectory, "Assets", HeadSprite))
            {
                X = newX, Y = newY,
                ScaleX = 2, ScaleY = 2,
                Rotation = GetRotation(_direction),
                PivotX = 8, PivotY = 8
            };
            PerlinApp.Stage.AddChild(sp);
            _positions.Add(sp);

            if (_currentFood > 0)
            {
                _currentFood--;
            }

            if (_currentFood == 0)
            {
                _positions.First().RemoveFromParent();
                _positions.RemoveAt(0);
            }
        }

        private void TryChangeDirection(Vector2 newDirection)
        {
            if (newDirection != -_previousDir)
            {
                _direction = newDirection;
            }
        }

        private bool Collides(float x, float y)
        {
            foreach (var part in _positions)
            {
                if (x == part.X && y == part.Y)
                {
                    return true;
                }
            }
            return OffWorld(x, y);
        }

        private bool OffWorld(float x, float y)
        {
            return x < 0 || x >= _world.Size.X * _world.CellSize
                || y < 0 || y >= _world.Size.Y * _world.CellSize;
        }

        private void Die()
        {
            _dead = true;
            foreach (var sprite in _positions)
            {
                sprite.Alpha = 0.8f;
            }
        }
    }
}
