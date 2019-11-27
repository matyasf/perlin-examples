using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Veldrid;

namespace Display
{
    public class TextField : DisplayObject
    {
        private string _text;
        public string Text
        {
            get => _text;
            set
            {
                // calculate width and height automatically here
                _text = value;
                if (_image != null && _texture != null)
                {
                    KestrelApp.TextRenderer.DrawText(_text, _image, _texture);   
                }
            }
        }

        public override float Width
        {
            get => base.Width;
            set
            {
                base.Width = value;
                // make some invalidation logic here in the future
                RecreateTexture();
            }
        }

        public override float Height
        {
            get => base.Height;
            set
            {
                base.Height = value;
                // make some invalidation logic here in the future
                RecreateTexture();
            }
        }
        
        private void RecreateTexture()
        {
            if (Width <= 0f || Height <= 0f)
            {
                Console.WriteLine("Warning: TextField size is 0 or less " + this._text);
                return;
            }
            _texture?.Dispose();
            _textureView?.Dispose();
            _image?.Dispose();
            _texture = KestrelApp.DefaultGraphicsDevice.ResourceFactory.CreateTexture(
                TextureDescription.Texture2D((uint)Width, (uint)Height, 1, 1, PixelFormat.R8_G8_B8_A8_UNorm, TextureUsage.Sampled));
            _textureView = KestrelApp.DefaultGraphicsDevice.ResourceFactory.CreateTextureView(_texture); // needed for TextRenderer.Render
            _image = new Image<Rgba32>((int)Width, (int)Height);            
        }

        private Texture _texture;
        private Image<Rgba32> _image;
        private TextureView _textureView;

        public override void Render(double elapsedTimems)
        {
            KestrelApp.TextRenderer.Draw(_textureView, GpuVertex);
            base.Render(elapsedTimems);
        }
    }
}