using System.Collections.Immutable;
using Codecool.Snake.Entities;
using Perlin.Display;

namespace Codecool.Snake
{
    /// <summary>
    /// Main class that controls what is displayed
    /// </summary>
    public class Display
    {
        private readonly Sprite _displayPane;
        private readonly DelayedModificationList<GameEntity> _gameObjects = new DelayedModificationList<GameEntity>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Display"/> class.
        /// </summary>
        /// <param name="pane">Sprite used for display</param>
        public Display(Sprite pane)
        {
            _displayPane = pane;
        }

        /// <summary>
        /// Adds an entity to display
        /// </summary>
        /// <param name="entity">The entity to display</param>
        public void Add(GameEntity entity)
        {
            _displayPane.AddChild(entity);
            _gameObjects.Add(entity);
        }

        /// <summary>
        /// Removes an entity from the display
        /// </summary>
        /// <param name="entity">The entity to remove</param>
        public void Remove(GameEntity entity)
        {
            _displayPane.RemoveChild(entity);
            _gameObjects.Remove(entity);
        }

        /// <summary>
        /// Gets the list of game objects
        /// </summary>
        public IImmutableList<GameEntity> ObjectList => _gameObjects.List;

        /// <summary>
        /// Called on the end of the frame, executes pending modifications
        /// </summary>
        public void FrameFinished()
        {
            _gameObjects.DoPendingModifications();
        }

        /// <summary>
        /// Puts the given entity to the top of the display
        /// </summary>
        /// <param name="snakeHead">The game entity</param>
        public void UpdateSnakeHeadDrawPosition(GameEntity snakeHead)
        {
            _displayPane.AddChildAt(snakeHead, _displayPane.NumChildren - 1);
        }

        /// <summary>
        /// Removes everything from the display.
        /// </summary>
        public void Clear()
        {
            _displayPane.RemoveAllChildren();
            _gameObjects.Clear();
        }
    }
}