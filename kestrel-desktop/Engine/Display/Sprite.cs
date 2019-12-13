using SixLabors.ImageSharp.PixelFormats;

namespace Engine.Display
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
            OriginalWidth = width;
            OriginalHeight = height;
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
                OriginalWidth = set.texture.Width;
                OriginalHeight = set.texture.Height;
            }
        }
        
        public override string ToString()
        {
            return "Sprite " + _imagePath;
        }
        // note: dispose texture in destructor if its not used by anything else?
    }
}

