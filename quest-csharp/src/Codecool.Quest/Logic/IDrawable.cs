namespace Codecool.Quest.Logic
{
    /// <summary>
    /// Interface for objects that can be drawn of the display
    /// </summary>
    public interface IDrawable
    {
        /// <summary>
        /// Gets the name of the tile, <see cref="Tiles"/>
        /// </summary>
        string Tilename { get; }
    }
}