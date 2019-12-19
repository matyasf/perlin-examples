using SixLabors.ImageSharp.PixelFormats;

namespace Engine.Display
{
    /// <summary>
    /// A lightweight class to create images to and colored rectangles to show on the display.
    /// Note that you need to add the created instance to the display tree to be displayed!
    /// </summary>
    public class Sprite : DisplayObject
    {
        private string _imagePath;
        
        /// <summary>
        /// Creates a Sprite that displays the given image, its size will be the image's dimensions.
        /// Note that it uses the <code>ImageManager</code> class to load the image, the image will stay there
        /// even if this Sprite is removed from the Stage.
        /// </summary>
        /// <param name="imagePath">the path to the image.</param>
        public Sprite(string imagePath)
        {
            LoadImage(imagePath);
        }

        /// <summary>
        /// Creates a Sprite with the given color.
        /// </summary>
        /// <param name="width">Width of the Sprite</param>
        /// <param name="height">Height of the Sprite</param>
        /// <param name="color">the color + transparency (alpha) of the image</param>
        public Sprite(uint width, uint height, Rgba32 color)
        {
            OriginalWidth = width;
            OriginalHeight = height;
            ResSet = KestrelApp.ImageManager.CreateColoredTexture(width, height, color);
        }
        
        /// <summary>
        /// Loads the given image to the Sprite.
        /// </summary>
        /// <param name="path">The path to the image</param>
        /// <param name="resizeToImage">Whether to resize this Sprite to the given image's dimensions.</param>
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
    }
}

