using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using Veldrid;

namespace Engine.Display
{
    /// <summary>
    /// TextField is used to render text. Make sure its big enough for your text otherwise it will not show!
    /// </summary>
    public class TextField : DisplayObject
    {
        private Image<Rgba32> _image;
        private TextureView _textureView;
        private string _text;
        private bool _textInvalid;
        private bool _sizeInvalid;
        public Font Font;
        public Rgba32 FontColor = Rgba32.White;
        public HorizontalAlignment HorizontalAlign = HorizontalAlignment.Left;
        public VerticalAlignment VerticalAlign = VerticalAlignment.Top;

        internal Texture Texture { get; set; }

        /// <summary>
        /// Creates a new TextField instance. For text to be displayed, its Width, Height, Font and Text properties
        /// must be set.
        /// </summary>
        /// <param name="font">The font to use. An example to load one:
        /// <code>
        /// var fc = new FontCollection();
        /// var family = fc.Install(Path.Combine(AppContext.BaseDirectory, "Assets", "Fonts", "Sunflower-Medium.ttf"));
        /// var font = family.CreateFont(28);
        /// </code>
        /// </param>
        public TextField(Font font)
        {
            Font = font;
        }
        public string Text
        {
            get => _text;
            set
            {
                if (_text == value)
                {
                    return;
                }
                // TODO Calculate width and height automatically here if not set
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
            ResSet?.Dispose();
            
            GraphicsDevice gd = KestrelApp.DefaultGraphicsDevice;
            Texture = gd.ResourceFactory.CreateTexture(
                TextureDescription.Texture2D((uint)Width, (uint)Height, 1, 1, 
                    PixelFormat.R8_G8_B8_A8_UNorm, TextureUsage.Sampled));
            _textureView = gd.ResourceFactory.CreateTextureView(Texture);
            ResSet = gd.ResourceFactory.CreateResourceSet(
                new ResourceSetDescription(
                    KestrelApp.KestrelPipeline.TexLayout,
                    _textureView,
                    gd.PointSampler));
            _image = new Image<Rgba32>((int)Width, (int)Height);
        }

        public override void Render(float elapsedTimeSecs)
        {
            if (_sizeInvalid)
            {
                RecreateTexture();
                _sizeInvalid = false;
            }
            if (_textInvalid)
            {
                DrawText(_text, _image, Texture, Font, FontColor);
                _textInvalid = false;
            }
            base.Render(elapsedTimeSecs);
        }
        
        /// <summary>
        /// Called when text changes
        /// </summary>
        private unsafe void DrawText(string text, Image<Rgba32> image, Texture texture, Font font, Rgba32 fontColor)
        {
            // ImageSharp bug (beta6): if text overflows it'll throw an exception                
            SizeF txtSize = TextMeasurer.Measure(text, new RendererOptions(font));
            if (txtSize.Width > image.Width || txtSize.Height > image.Height)
            {
                Console.WriteLine("Cannot render text '" + text + "', it would take up " +
                                  txtSize + " and the textField is smaller. (" + image.Width + 
                                  "x" + image.Height + ")");
                return;
            }
            fixed (void* data = &MemoryMarshal.GetReference(image.GetPixelSpan()))
            {
                Unsafe.InitBlock(data, 0, (uint)(image.Width * image.Height * 4));
            }
            image.Mutate(ctx =>
            {
                ctx.DrawText(
                    new TextGraphicsOptions
                    {
                        WrapTextWidth = image.Width,
                        Antialias = true,
                        HorizontalAlignment = HorizontalAlign, 
                        VerticalAlignment = VerticalAlign
                    },
                    text,
                    font,
                    fontColor,
                    new PointF());
            });
            fixed (void* data = &MemoryMarshal.GetReference(image.GetPixelSpan()))
            {
                uint size = (uint)(image.Width * image.Height * 4);
                KestrelApp.DefaultGraphicsDevice.UpdateTexture(texture, 
                    (IntPtr)data, size, 0, 0, 0, texture.Width, texture.Height,
                    1, 0, 0);
            }
        }
    }
}