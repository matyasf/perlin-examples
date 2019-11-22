using System.Collections.Generic;

namespace Display
{
    public class Stage : UIContainer
    {
        public int Width { get; }
        public int Height { get; }

        internal Stage(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}