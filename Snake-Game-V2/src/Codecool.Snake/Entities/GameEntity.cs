using Perlin.Display;
using Perlin.Geom;

namespace Codecool.Snake.Entities
{
    /// <summary>
    /// Base class for every entity in the game.
    /// </summary>
    public abstract class GameEntity : Sprite
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameEntity"/> class.
        /// </summary>
        /// <param name="imagePath">file path for the graphic of this entity</param>
        protected GameEntity(string imagePath)
            : base(imagePath)
        {
            Globals.Instance.Display.Add(this);
        }

        /// <summary>
        /// Removes this enemy from the display
        /// </summary>
        public virtual void Destroy()
        {
            Globals.Instance.Display.Remove(this);
        }

        /// <summary>
        /// Gets or sets the position of this entity
        /// </summary>
        public virtual Point Position
        {
            get => new Point(X, Y);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        /// <summary>
        /// Return whether this entity is outside of the game's window.
        /// </summary>
        /// <returns>Whether this is outside the window</returns>
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