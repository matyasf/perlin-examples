using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Veldrid;

namespace Display
{
    public class TextField : DisplayObject
    {
        private Image<Rgba32> _image;
        private TextureView _textureView;
        private ResourceSet _textSet;
        // + set font here
        private string _text;
        private bool _textInvalid;
        private bool _sizeInvalid;
        public string Text
        {
            get => _text;
            set
            {
                if (_text == value)
                {
                    return;
                }
                // +calculate width and height automatically here if not set
                _text = value;
                _textInvalid = true;
            }
        }

        public override float Width
        {
            get => base.Width;
            set
            {
                if (value == base.Width)
                {
                    return;
                }
                base.Width = value;
                _sizeInvalid = true;
            }
        }

        public override float Height
        {
            get => base.Height;
            set
            {
                if (value == base.Height)
                {
                    return;
                }
                base.Height = value;
                _sizeInvalid = true;
            }
        }
        
        private void RecreateTexture()
        {
            if (Width <= 0f || Height <= 0f)
            {
                Console.WriteLine("Warning: TextField size is 0 or less " + this._text);
                return;
            }
            Texture?.Dispose();
            _textureView?.Dispose();
            _image?.Dispose();
            _textSet?.Dispose();
            Texture = KestrelApp.DefaultGraphicsDevice.ResourceFactory.CreateTexture(
                TextureDescription.Texture2D((uint)Width, (uint)Height, 1, 1, 
                    PixelFormat.R8_G8_B8_A8_UNorm, TextureUsage.Sampled));
            _textureView = KestrelApp.DefaultGraphicsDevice.ResourceFactory.CreateTextureView(Texture);
            _textSet = KestrelApp.DefaultGraphicsDevice.ResourceFactory.CreateResourceSet(
                new ResourceSetDescription(
                    KestrelApp.KestrelPipeline.TexLayout,
                    _textureView,
                    KestrelApp.DefaultGraphicsDevice.PointSampler));
            _image = new Image<Rgba32>((int)Width, (int)Height);
        }

        public override void Render(double elapsedTimems)
        {
            if (_sizeInvalid)
            {
                RecreateTexture();
                _sizeInvalid = false;
            }
            if (_textInvalid)
            {
                KestrelApp.TextRenderer.DrawText(_text, _image, Texture);
                _textInvalid = false;
            }
            KestrelApp.TextRenderer.Draw(_textSet, GpuVertex);
            base.Render(elapsedTimems);
        }
    }
}