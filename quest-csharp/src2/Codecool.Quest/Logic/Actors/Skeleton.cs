namespace Codecool.Quest.Logic.Actors
{
    public class Skeleton : Actor
    {
        public Skeleton(Cell cell) : base(cell)
        {
        }

        public override string Tilename => "skeleton";
    }
}