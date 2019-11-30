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
        private readonly DeviceBuffer _textBuffer;
        private readonly Font _font;

        public TextRenderer()
        {
            _textBuffer = KestrelApp.DefaultGraphicsDevice.ResourceFactory.CreateBuffer(
                new BufferDescription(DisplayObject.QuadVertex.VertexSize,
                        BufferUsage.VertexBuffer | BufferUsage.Dynamic));
            
            FontCollection fc = new FontCollection(); // todo move this to a font class
            FontFamily family = fc.Install(Path.Combine(AppContext.BaseDirectory, "Assets", "Fonts", "Sunflower-Medium.ttf"));
            _font = family.CreateFont(28); // needs to cache somehow?
        }
        
        /// <summary>
        /// call on each frame
        /// </summary>
        internal void Draw(ResourceSet textSet, DisplayObject.QuadVertex vertex)
        {
            var cl = KestrelApp.CommandList;
            cl.SetPipeline(KestrelApp.KestrelPipeline.Pipeline);
            cl.SetVertexBuffer(0, _textBuffer);
            cl.SetGraphicsResourceSet(0, KestrelApp.KestrelPipeline.OrthoSet);
            cl.SetGraphicsResourceSet(1, textSet);
            cl.UpdateBuffer(_textBuffer, 0, vertex);
            cl.Draw(4, 1, 0, 0);
        }

        /// <summary>
        /// Called when text changes
        /// </summary>
        public unsafe void DrawText(string text, Image<Rgba32> image, Texture texture)
        {
            // ImageSharp bug (beta6): if text overflows it'll throw an exception                
            SizeF txtSize = TextMeasurer.Measure(text, new RendererOptions(_font));
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
                        HorizontalAlignment = HorizontalAlignment.Center
                    },
                    text,
                    _font,
                    Rgba32.White,
                    new PointF());
            });
            fixed (void* data = &MemoryMarshal.GetReference(image.GetPixelSpan()))
            {
                uint size = (uint)(image.Width * image.Height * 4);
                KestrelApp.DefaultGraphicsDevice.UpdateTexture(texture, (IntPtr)data, size, 0, 0, 0, texture.Width, texture.Height, 1, 0, 0);
            }
        }
    }
}
