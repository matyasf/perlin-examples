using Codecool.Quest.Logic.Actors;

namespace Codecool.Quest.Logic
{
    public class Cell : IDrawable
    {
        private readonly GameMap _gameMap;
        public Cell(GameMap gameMap, int x, int y, CellType type)
        {
            _gameMap = gameMap;
            X = x;
            Y = y;
            Type = type;
        }

        public CellType Type;

        public Actor Actor;

        public Cell GetNeighbor(int dx, int dy)
        {
            return _gameMap.GetCell(X + dx, Y + dy);
        }
        
        public string Tilename => Type.ToString();
        
        public int X { get; private set; }
        
        public int Y { get; private set; }
    }
}