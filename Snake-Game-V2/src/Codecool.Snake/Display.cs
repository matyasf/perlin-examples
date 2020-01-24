using System.Collections.Immutable;
using Perlin.Display;
using Snake_Game_V2.Entities;

namespace Snake_Game_V2
{
    public class Display
    {
        private readonly Sprite _displayPane;
        private readonly DelayedModificationList<GameEntity> _gameObjects = new DelayedModificationList<GameEntity>();

        public Display(Sprite pane)
        {
            _displayPane = pane;
        }

        public void Add(GameEntity entity)
        {
            _displayPane.AddChild(entity);
            _gameObjects.Add(entity);
        }

        public void Remove(GameEntity entity)
        {
            _displayPane.RemoveChild(entity);
            _gameObjects.Remove(entity);
        }

        public IImmutableList<GameEntity> ObjectList => _gameObjects.List;

        public void FrameFinished()
        {
            _gameObjects.DoPendingModifications();
        }

        public void UpdateSnakeHeadDrawPosition(GameEntity snakeHead)
        {
            _displayPane.AddChildAt(snakeHead, _displayPane.NumChildren - 1);
        }

        public void Clear()
        {
            _displayPane.RemoveAllChildren();
            _gameObjects.Clear();
        }
    }
}