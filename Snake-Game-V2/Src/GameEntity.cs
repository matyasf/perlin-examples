using Engine.Display;
using Engine.Geom;

namespace Snake_Game_V2
{
    public abstract class GameEntity : Sprite
    {
        protected GameEntity(string imagePath) : base(imagePath)
        {
            Globals.Instance.Display.Add(this);
        }

        public void Destroy()
        {
            Globals.Instance.Display.Remove(this);
        }

        public virtual Point Position
        {
            get => new Point(X, Y);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        public bool IsOutOfBounds()
        {
            if (X > Globals.WindowWidth || X < 0 ||
                Y > Globals.WindowHeight || Y < 0)
            {
                return true;
            }
            return false;
        }
    }
}