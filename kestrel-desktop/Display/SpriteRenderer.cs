using System.Collections.Generic;
using System.Numerics;
using Veldrid;

namespace Display
{
    public class SpriteRenderer
    {
        private readonly List<Sprite> _draws = new List<Sprite>();
        private DeviceBuffer _vertexBuffer;

        public SpriteRenderer()
        {
            _vertexBuffer = KestrelApp.DefaultGraphicsDevice.ResourceFactory.CreateBuffer(new BufferDescription(1000, BufferUsage.VertexBuffer | BufferUsage.Dynamic));
        }
        
        /// <summary>
        /// Adds the Sprite to the list of things to render
        /// </summary>
        public void BatchSprite(Sprite sp)
        {
            _draws.Add(sp);
        }

        public void Draw(CommandList cl)
        {
            if (_draws.Count == 0)
            {
                return;
            }
            GraphicsDevice gd = KestrelApp.DefaultGraphicsDevice;
            float width = gd.MainSwapchain.Framebuffer.Width;
            float height = gd.MainSwapchain.Framebuffer.Height;
            gd.UpdateBuffer(
                KestrelApp.KestrelPipeline.OrthoBuffer,
                0,
                Matrix4x4.CreateOrthographicOffCenter(0, width, 0, height, 0, 1));

            EnsureBufferSize((uint)_draws.Count * DisplayObject.QuadVertex.VertexSize);
            MappedResourceView<DisplayObject.QuadVertex> writeMap = gd.Map<DisplayObject.QuadVertex>(_vertexBuffer, MapMode.Write);
            for (int i = 0; i < _draws.Count; i++)
            {
                writeMap[i] = _draws[i].GpuVertex;
            }
            gd.Unmap(_vertexBuffer);

            cl.SetPipeline(KestrelApp.KestrelPipeline.Pipeline);
            cl.SetVertexBuffer(0, _vertexBuffer);
            cl.SetGraphicsResourceSet(0, KestrelApp.KestrelPipeline.OrthoSet);

            for (int i = 0; i < _draws.Count;)
            {
                uint batchStart = (uint)i;
                string spriteName = _draws[i].Image;
                ResourceSet rs = KestrelApp.ImageManager.Load(spriteName);
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

        private void EnsureBufferSize(uint size)
        {
            if (_vertexBuffer.SizeInBytes < size)
            {
                _vertexBuffer.Dispose();
                _vertexBuffer = KestrelApp.DefaultGraphicsDevice.ResourceFactory.CreateBuffer(
                    new BufferDescription(size, BufferUsage.VertexBuffer | BufferUsage.Dynamic));
            }
        }
    }
}
