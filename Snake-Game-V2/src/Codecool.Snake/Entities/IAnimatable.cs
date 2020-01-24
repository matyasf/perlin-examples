namespace Codecool.Snake.Entities
{
    /// <summary>
    /// Interface for animated game entities. If a GameEntity implements this, the Step() method will be called
    /// 60 times per second.
    /// </summary>
    public interface IAnimatable
    {
        /// <summary>
        /// Called every frame, executes AI and updates position.
        /// </summary>
        void Step();
    }
}