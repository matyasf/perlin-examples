namespace Codecool.Quest.Logic.Actors
{
    /// <summary>
    /// Sample enemy
    /// </summary>
    public class Skeleton : Actor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Skeleton"/> class.
        /// </summary>
        /// <param name="cell">The starting cell</param>
        public Skeleton(Cell cell)
            : base(cell)
        {
        }

        /// <inheritdoc/>
        public override string Tilename => "skeleton";
    }
}