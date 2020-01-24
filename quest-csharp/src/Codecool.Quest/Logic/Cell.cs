using Codecool.Quest.Logic.Actors;

namespace Codecool.Quest.Logic
{
    /// <summary>
    /// Represents a cell in the map.
    /// </summary>
    public class Cell : IDrawable
    {
        private readonly GameMap _gameMap;

        /// <summary>
        /// Initializes a new instance of the <see cref="Cell"/> class.
        /// </summary>
        /// <param name="gameMap">The game map</param>
        /// <param name="x">X coordinate of the cell</param>
        /// <param name="y">Y coordinate of the cell</param>
        /// <param name="type">Type of the cell</param>
        public Cell(GameMap gameMap, int x, int y, CellType type)
        {
            _gameMap = gameMap;
            X = x;
            Y = y;
            Type = type;
        }

        /// <summary>
        /// Type of the cell
        /// </summary>
        public CellType Type;

        /// <summary>
        /// The actor on the cell, null of none.
        /// </summary>
        public Actor Actor;

        /// <summary>
        /// Returns a cell in the given distance
        /// </summary>
        /// <param name="dx">X distance from this cell</param>
        /// <param name="dy">Y distance from this cell</param>
        /// <returns>The cell in the given distance</returns>
        public Cell GetNeighbor(int dx, int dy)
        {
            return _gameMap.GetCell(X + dx, Y + dy);
        }

        /// <summary>
        /// Gets the type of this cell as string.
        /// </summary>
        public string Tilename => Type.ToString();

        /// <summary>
        /// Gets the X coordinate
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        /// Gets the Y coordinate
        /// </summary>
        public int Y { get; private set; }
    }
}