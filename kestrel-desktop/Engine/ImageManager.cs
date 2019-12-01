using System;
using System.Collections.Generic;
using System.IO;
using Veldrid;
using Veldrid.ImageSharp;

namespace Engine
{
    /// <summary>
    /// This class manages the loading, storing and retrieving of images used in your app.
    /// </summary>
    public class ImageManager
    {
        private readonly Dictionary<string, ResourceSet> _loadedImages = new Dictionary<string, ResourceSet>();

        /// <summary>
        /// Loads an stores an image from the disk.
        /// </summary>
        /// <param name="imageName">the image filename</param>
        public ResourceSet Load(string imageName)
        {
            if (!_loadedImages.TryGetValue(imageName, out ResourceSet ret))
            {
                GraphicsDevice gd = KestrelApp.DefaultGraphicsDevice;
                string texPath = Path.Combine(AppContext.BaseDirectory, "Assets", imageName);
                var imTex = new ImageSharpTexture(texPath, false);
                var tex = imTex.CreateDeviceTexture(gd, gd.ResourceFactory);
                TextureView view = gd.ResourceFactory.CreateTextureView(tex);
                ResourceSet set = gd.ResourceFactory.CreateResourceSet(new ResourceSetDescription(
                    KestrelApp.KestrelPipeline.TexLayout,
                    view,
                    gd.PointSampler)); 
                ret = set;
                _loadedImages.Add(imageName, ret);
            }
            return ret;
        }
    }
}