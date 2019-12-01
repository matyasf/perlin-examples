using Engine;
using Engine.Display;

namespace Display
{
    public class Sprite : DisplayObject
    {
        private string _imagePath;
        public Sprite(string imagePath)
        {
            ImagePath = imagePath;
        }

        public string ImagePath
        {
            set
            {
                ResSet?.Dispose();
                ResSet = KestrelApp.ImageManager.Load(value);
                _imagePath = value;
            }
            get => _imagePath;
        }

        public override string ToString()
        {
            return "Sprite " + _imagePath;
        }

        // note: dispose texture in destructor if its not used by anything else?
    }
}

