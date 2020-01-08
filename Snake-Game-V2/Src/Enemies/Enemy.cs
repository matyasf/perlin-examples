namespace Snake_Game_V2.Enemies
{
    public abstract class Enemy : GameEntity
    {
        public int Damage { get; private set; }

        public Enemy(int damage, string imagePath) : base(imagePath)
        {
            Damage = damage;
        }
    }
}