namespace Codecool.Snake.Entities.Enemies
{
    /// <summary>
    /// Base class for all enemies
    /// </summary>
    public abstract class Enemy : GameEntity
    {
        /// <summary>
        /// Gets how much damage this enemy can inflict.
        /// </summary>
        public int Damage { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Enemy"/> class.
        /// </summary>
        /// <param name="damage">How much damage this enemy can inflict</param>
        /// <param name="imagePath">The file path for the graphic of this enemy.</param>
        public Enemy(int damage, string imagePath)
            : base(imagePath)
        {
            Damage = damage;
        }
    }
}