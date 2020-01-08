namespace Snake_Game_V2.Entities
{
    /// <summary>
    /// Interface for animated game entities. If a GameEntity implements this, the Step() method will be called
    /// 60 times per second.
    /// </summary>
    public interface IAnimatable
    {
        void Step();
    }
}