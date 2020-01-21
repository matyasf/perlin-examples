namespace Codecool.Quest.Logic.Actors
{
    public class Player : Actor
    {
        public Player(Cell cell) : base(cell)
        {
        }

        public override string Tilename => "player";
    }
}