using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using Veldrid;
using Veldrid.ImageSharp;

namespace Display
{
    public class SpriteRenderer
    {
        private readonly KestrelPipeline _kestrelPipeline;
        private readonly List<Sprite> _draws = new List<Sprite>();

        private DeviceBuffer _vertexBuffer;

        private readonly Dictionary<string, (Texture, TextureView, ResourceSet)> _loadedImages
            = new Dictionary<string, (Texture, TextureView, ResourceSet)>();

        public SpriteRenderer(GraphicsDevice gd, KestrelPipeline kestrelPipeline)
        {
            _kestrelPipeline = kestrelPipeline;
            ResourceFactory factory = gd.ResourceFactory;

            _vertexBuffer = factory.CreateBuffer(new BufferDescription(1000, BufferUsage.VertexBuffer | BufferUsage.Dynamic));
        }
        
        /// <summary>
        /// Adds the Sprite to the list of things to render
        /// </summary>
        public void BatchSprite(Sprite sp)
        {
            _draws.Add(sp);
        }

        public void Draw(GraphicsDevice gd, CommandList cl)
        {
            if (_draws.Count == 0)
            {
                return;
            }
            float width = gd.MainSwapchain.Framebuffer.Width;
            float height = gd.MainSwapchain.Framebuffer.Height;
            gd.UpdateBuffer(
                _kestrelPipeline.OrthoBuffer,
                0,
                Matrix4x4.CreateOrthographicOffCenter(0, width, 0, height, 0, 1));

            EnsureBufferSize(gd, (uint)_draws.Count * DisplayObject.QuadVertex.VertexSize);
            MappedResourceView<DisplayObject.QuadVertex> writemap = gd.Map<DisplayObject.QuadVertex>(_vertexBuffer, MapMode.Write);
            for (int i = 0; i < _draws.Count; i++)
            {
                writemap[i] = _draws[i].GpuVertex;
            }
            gd.Unmap(_vertexBuffer);

            cl.SetPipeline(_kestrelPipeline.Pipeline);
            cl.SetVertexBuffer(0, _vertexBuffer);
            cl.SetGraphicsResourceSet(0, _kestrelPipeline.OrthoSet);

            for (int i = 0; i < _draws.Count;)
            {
                uint batchStart = (uint)i;
                string spriteName = _draws[i].Image;
                ResourceSet rs = Load(gd, spriteName);
                cl.SetGraphicsResourceSet(1, rs);
                uint batchSize = 0;
                do
                {
                    i += 1;
                    batchSize += 1;
                }
                while (i < _draws.Count && _draws[i].Image == spriteName);
                cl.Draw(4, batchSize, 0, batchStart);
            }
            _draws.Clear();
        }
        
        // combine this with TextRenderer constructor?
        private ResourceSet Load(GraphicsDevice gd, string spriteName)
        {
            if (!_loadedImages.TryGetValue(spriteName, out (Texture, TextureView, ResourceSet) ret))
            {
                string texPath = Path.Combine(AppContext.BaseDirectory, "Assets", spriteName);
                var imTex = new ImageSharpTexture(texPath, false);
                var tex = imTex.CreateDeviceTexture(gd, gd.ResourceFactory);
                TextureView view = gd.ResourceFactory.CreateTextureView(tex);
                ResourceSet set = gd.ResourceFactory.CreateResourceSet(new ResourceSetDescription(
                    _kestrelPipeline.TexLayout,
                    view,
                    gd.PointSampler));
                ret = (tex, view, set);
                _loadedImages.Add(spriteName, ret);
            }
            return ret.Item3;
        }

        private void EnsureBufferSize(GraphicsDevice gd, uint size)
        {
            if (_vertexBuffer.SizeInBytes < size)
            {
                _vertexBuffer.Dispose();
                _vertexBuffer = gd.ResourceFactory.CreateBuffer(
                    new BufferDescription(size, BufferUsage.VertexBuffer | BufferUsage.Dynamic));
            }
        }
    }
}
