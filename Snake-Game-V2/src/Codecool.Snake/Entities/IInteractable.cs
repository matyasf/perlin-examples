using System;

namespace Codecool.Snake.Entities
{
    /// <summary>
    /// interface that all game objects that can be interacted with must implement.
    /// </summary>
    public interface IInteractable
    {
        /// <summary>
        /// Applies an interaction caused by entity
        /// </summary>
        /// <param name="entity">The entity that caused the interaction</param>
        void Apply(GameEntity entity);

        /// <summary>
        /// Gets a message
        /// </summary>
        /// <returns>A message fro the this object</returns>
        String GetMessage();
    }
}