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
        public Rgba32 FontColor = Rgba32.Black;
        public Rgba32 BackgroundColor = new Rgba32(255, 255, 255, 0);
        public HorizontalAlignment HorizontalAlign = HorizontalAlignment.Left;
        public VerticalAlignment VerticalAlign = VerticalAlignment.Top;

        /// <summary>
        /// Gets or sets a value indicating when a text should wrap.
        /// Default is 0, in this case no automatic wrapping will happen.
        /// </summary>
//        public float WrapTextWidth = 0;
        private Texture Texture { get; set; }

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
        
        private bool _autoSize = false;
        public string Text
        {
            get => _text;
            set
            {
                if (_text == value)
                {
                    return;
                }
                _text = value;
                _textInvalid = true;
                if (_autoSize)
                {
                    PerformAutoSize();
                }
            }
        }

        public new float Width
        {
            get => base.Width;
            set
            {
                OriginalWidth = value;
                _sizeInvalid = true;
            }
        }

        public new float Height
        {
            get => base.Height;
            set
            {
                OriginalHeight = value;
                _sizeInvalid = true;
            }
        }

        /// <summary>
        /// If true after setting text the textfield is resized automatically to the text size.
        /// </summary>
        public bool AutoSize
        {
            get => _autoSize;
            set
            {
                if (_autoSize == value)
                {
                    return;
                }
                _autoSize = value;
                PerformAutoSize();
            }
        }

        private void PerformAutoSize()
        {
            var size = MeasureText();
            Width = size.Width;
            Height = size.Height;
        }

        private void RecreateTexture()
        {
            _sizeInvalid = false;
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
                DrawText();
            }
            if (_textInvalid)
            {
                DrawText();
            }
            base.Render(elapsedTimeSecs);
        }
        
        /// <summary>
        /// Called when text changes
        /// </summary>
        private unsafe void DrawText()
        {
            _textInvalid = false;
            SizeF txtSize = MeasureText();
            if (txtSize.Width > _image.Width || txtSize.Height > _image.Height)
            {
                // ImageSharp bug (beta6): if text overflows it'll throw an exception
                Console.WriteLine("Cannot render text '" + _text + "', it would take up " +
                                  txtSize + " and the textField is smaller. (" + _image.Width + 
                                  "x" + _image.Height + ")");
                return;
            }
            fixed (void* data = &MemoryMarshal.GetReference(_image.GetPixelSpan()))
            {
                Unsafe.InitBlock(data, 0, (uint)(_image.Width * _image.Height * 4));
            }
            _image.Mutate(ctx =>
            {
                if (BackgroundColor.A != 0)
                {
                    ctx.BackgroundColor(Color.FromRgba(BackgroundColor.R, BackgroundColor.G, BackgroundColor.B, BackgroundColor.A));   
                }
                ctx.DrawText(
                    new TextGraphicsOptions
                    {
                        //WrapTextWidth = WrapTextWidth, // buggy! was the whole width
                        WrapTextWidth = _image.Width,
                        Antialias = true,
                        HorizontalAlignment = HorizontalAlign, 
                        VerticalAlignment = VerticalAlign
                    },
                    _text,
                    Font,
                    FontColor,
                    new PointF());
            });
            fixed (void* data = &MemoryMarshal.GetReference(_image.GetPixelSpan()))
            {
                uint size = (uint)(_image.Width * _image.Height * 4);
                KestrelApp.DefaultGraphicsDevice.UpdateTexture(Texture, 
                    (IntPtr)data, size, 0, 0, 0, Texture.Width, Texture.Height,
                    1, 0, 0);
            }
        }

        public SizeF MeasureText()
        {
            var size =  TextMeasurer.Measure(_text, new RendererOptions(Font));
            size.Width = (float)Math.Ceiling(size.Width);
            size.Height = (float)Math.Ceiling(size.Height);
            return size;
        }
    }
}