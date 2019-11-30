using System;
using System.Collections.Generic;
using System.IO;
using Veldrid;
using Veldrid.ImageSharp;

namespace Display
{
    public class ImageManager
    {
        private readonly Dictionary<string, (Texture, TextureView, ResourceSet)> _loadedImages
            = new Dictionary<string, (Texture, TextureView, ResourceSet)>();

        public ResourceSet Load(string spriteName)
        {
            if (!_loadedImages.TryGetValue(spriteName, out (Texture, TextureView, ResourceSet) ret))
            {
                GraphicsDevice gd = KestrelApp.DefaultGraphicsDevice;
                string texPath = Path.Combine(AppContext.BaseDirectory, "Assets", spriteName);
                var imTex = new ImageSharpTexture(texPath, false);
                var tex = imTex.CreateDeviceTexture(gd, gd.ResourceFactory);
                TextureView view = gd.ResourceFactory.CreateTextureView(tex);
                ResourceSet set = gd.ResourceFactory.CreateResourceSet(new ResourceSetDescription(
                    KestrelApp.KestrelPipeline.TexLayout,
                    view,
                    gd.PointSampler));
                ret = (tex, view, set);
                _loadedImages.Add(spriteName, ret);
            }
            return ret.Item3;
        }
    }
}