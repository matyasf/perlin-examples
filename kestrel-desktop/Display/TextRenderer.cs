using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.Fonts;
using Veldrid;
using SixLabors.ImageSharp.Advanced;
using System;
using SixLabors.Primitives;
using System.Runtime.CompilerServices;
using System.IO;
using System.Runtime.InteropServices;
using Display;

namespace Snake
{
    public class TextRenderer
    {
        private readonly KestrelPipeline _kestrelPipeline;
        private readonly GraphicsDevice _gd;
        private ResourceSet _textSet;
        private readonly DeviceBuffer _textBuffer;
        private readonly Font _font;

        public TextRenderer(GraphicsDevice gd, KestrelPipeline kestrelPipeline)
        { 
            _gd = gd;
            _kestrelPipeline = kestrelPipeline;
            ResourceFactory factory = gd.ResourceFactory;
            _textBuffer = factory.CreateBuffer(new BufferDescription(DisplayObject.QuadVertex.VertexSize, BufferUsage.VertexBuffer | BufferUsage.Dynamic));
            
            FontCollection fc = new FontCollection(); // todo move this to a font class
            FontFamily family = fc.Install(Path.Combine(AppContext.BaseDirectory, "Assets", "Fonts", "Sunflower-Medium.ttf"));
            _font = family.CreateFont(28); // needs to cache somehow?
        }
        
        /// <summary>
        /// call on each frame
        /// </summary>
        internal void Draw(TextureView textureView, DisplayObject.QuadVertex vertex)
        {
            var cl = KestrelApp.CommandList;
            cl.SetPipeline(_kestrelPipeline.Pipeline);
            cl.SetVertexBuffer(0, _textBuffer);
            cl.SetGraphicsResourceSet(0, _kestrelPipeline.OrthoSet);
            if (_textSet == null)
            {
                _textSet = _gd.ResourceFactory.CreateResourceSet(new ResourceSetDescription(
                    _kestrelPipeline.TexLayout,
                    textureView,
                    _gd.PointSampler));
            }
            cl.SetGraphicsResourceSet(1, _textSet);
            cl.UpdateBuffer(_textBuffer, 0, vertex);
            cl.Draw(4, 1, 0, 0);
        }

        /// <summary>
        /// Called when text changes
        /// </summary>
        public unsafe void DrawText(string text, Image<Rgba32> _image, Texture _texture)
        {
            fixed (void* data = &MemoryMarshal.GetReference(_image.GetPixelSpan()))
            {
                Unsafe.InitBlock(data, 0, (uint)(_image.Width * _image.Height * 4));
            }
            _image.Mutate(ctx =>
            {
                ctx.DrawText(
                    new TextGraphicsOptions
                    {
                        WrapTextWidth = _image.Width,
                        Antialias = true,
                        HorizontalAlignment = HorizontalAlignment.Center
                    },
                    text,
                    _font,
                    Rgba32.White,
                    new PointF());
            });

            fixed (void* data = &MemoryMarshal.GetReference(_image.GetPixelSpan()))
            {
                uint size = (uint)(_image.Width * _image.Height * 4);
                _gd.UpdateTexture(_texture, (IntPtr)data, size, 0, 0, 0, _texture.Width, _texture.Height, 1, 0, 0);
            }
        }
    }
}
