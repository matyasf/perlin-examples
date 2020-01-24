using Codecool.Quest.Logic.Actors;

namespace Codecool.Quest.Logic
{
    public class GameMap
    {
        public Player Player;
        public Skeleton Skeleton;
        private readonly Cell[,] _cells;

        public GameMap(int width, int height, CellType defaultCellType)
        {
            Width = width;
            Height = height;
            _cells = new Cell[width, height];
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    _cells[x, y] = new Cell(this, x, y, defaultCellType);
                }
            }
        }

        public Cell GetCell(int x, int y) => _cells[x, y];

        public int Width { get; }

        public int Height { get; }
    }
}