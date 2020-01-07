using System.Collections.Immutable;
using Engine.Display;

namespace Snake_Game_V2
{
    public class Display
    {
        private Sprite _displayPane;
        private DelayedModificationList<GameEntity> _gameObjects = new DelayedModificationList<GameEntity>();

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

        public IImmutableList<GameEntity> GetObjectList()
        {
            return _gameObjects.List;
        }

        public void FrameFinished()
        {
            _gameObjects.DoPendingModifications();
        }

        public void UpdateSnakeHeadDrawPosition(GameEntity snakeHead)
        {
            _displayPane.AddChildAt(snakeHead, _displayPane.NumChildren); // TODO check if index is OK
        }

        public void Clear()
        {
            _displayPane.RemoveAllChildren();
            _gameObjects.Clear();
        }
    }
}