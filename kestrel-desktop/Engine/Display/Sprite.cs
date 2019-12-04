using Engine;
using Engine.Display;
using SixLabors.ImageSharp.PixelFormats;
using Veldrid;

namespace Display
{
    public class Sprite : DisplayObject
    {
        private string _imagePath;
        public Sprite(string imagePath)
        {
            ImagePath = imagePath;
        }

        public Sprite(uint width, uint height, Rgba32 color)
        {
            Width = width;
            Height = height;
            ResSet = KestrelApp.ImageManager.CreateColoredTexture(width, height, color);
        }

        public string ImagePath
        {
            set
            {
                ResSet?.Dispose();
                var set = KestrelApp.ImageManager.Load(value);
                ResSet = set.ret;
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

