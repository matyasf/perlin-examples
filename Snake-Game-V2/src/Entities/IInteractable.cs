using System;

namespace Snake_Game_V2.Entities
{
    /// <summary>
    /// interface that all game objects that can be interacted with must implement.
    /// </summary>
    public interface IInteractable
    {
        void Apply(GameEntity entity);

        String GetMessage();
    }
}