using System;

namespace Codecool.Quest.Logic.Actors
{
    public abstract class Actor : IDrawable
    {
        public Cell Cell { get; private set; }
        public int Health { get; private set; }

        public Actor(Cell cell)
        {
            Cell = cell;
            Cell.Actor = this;
        }

        public void Move(int dx, int dy)
        {
            Cell nextCell = Cell.GetNeighbor(dx, dy);
            Cell.Actor = null;
            nextCell.Actor = this;
            Cell = nextCell;
        }
        
        public int X => Cell.X;

        public int Y => Cell.Y;

        public abstract string Tilename { get; }
    }
}