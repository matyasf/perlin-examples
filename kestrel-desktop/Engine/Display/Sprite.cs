using Engine;
using Engine.Display;
using SixLabors.ImageSharp.PixelFormats;

namespace Display
{
    public class Sprite : DisplayObject
    {
        private string _imagePath;
        public Sprite(string imagePath)
        {
            LoadImage(imagePath);
        }

        public Sprite(uint width, uint height, Rgba32 color)
        {
            Width = width;
            Height = height;
            ResSet = KestrelApp.ImageManager.CreateColoredTexture(width, height, color);
        }

        public void LoadImage(string path, bool resizeToImage = true)
        {
            ResSet?.Dispose();
            var set = KestrelApp.ImageManager.Load(path);
            ResSet = set.ret;
            _imagePath = path;
            if (resizeToImage)
            {
                Width = set.texture.Width;
                Height = set.texture.Height;
            }
        }
        
        public override string ToString()
        {
            return "Sprite " + _imagePath;
        }
        // note: dispose texture in destructor if its not used by anything else?
    }
}

