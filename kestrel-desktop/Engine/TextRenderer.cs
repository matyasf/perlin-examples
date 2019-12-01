using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using Veldrid;

namespace Engine
{
    public class TextRenderer
    {

        /// <summary>
        /// Called when text changes
        /// </summary>
        public unsafe void DrawText(string text, Image<Rgba32> image, Texture texture, Font font, Rgba32 fontColor)
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
                        HorizontalAlignment = HorizontalAlignment.Center
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
