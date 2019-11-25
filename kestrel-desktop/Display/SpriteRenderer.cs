using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using Veldrid;
using Veldrid.ImageSharp;
using Veldrid.SPIRV;

namespace Display
{
    public class SpriteRenderer
    {
        private readonly List<Sprite> _draws = new List<Sprite>();

        private DeviceBuffer _vertexBuffer;
        private readonly DeviceBuffer _orthoBuffer;
//        private readonly ResourceSet _orthoSet;
//        private readonly ResourceLayout _texLayout;
//        private readonly Pipeline _pipeline;

        private readonly Dictionary<string, (Texture, TextureView, ResourceSet)> _loadedImages
            = new Dictionary<string, (Texture, TextureView, ResourceSet)>();

        public SpriteRenderer(GraphicsDevice gd)
        {
            ResourceFactory factory = gd.ResourceFactory;

            _vertexBuffer = factory.CreateBuffer(new BufferDescription(1000, BufferUsage.VertexBuffer | BufferUsage.Dynamic));
            _orthoBuffer = factory.CreateBuffer(new BufferDescription(64, BufferUsage.UniformBuffer));
/*
            var orthoLayout = factory.CreateResourceLayout(
                new ResourceLayoutDescription(
                    new ResourceLayoutElementDescription("OrthographicProjection", ResourceKind.UniformBuffer, ShaderStages.Vertex)));
            _orthoSet = factory.CreateResourceSet(new ResourceSetDescription(orthoLayout, _orthoBuffer));

            _texLayout = factory.CreateResourceLayout(
                new ResourceLayoutDescription(
                    new ResourceLayoutElementDescription("SpriteTexture", ResourceKind.TextureReadOnly, ShaderStages.Fragment),
                    new ResourceLayoutElementDescription("SpriteSampler", ResourceKind.Sampler, ShaderStages.Fragment)));

            _pipeline = factory.CreateGraphicsPipeline(new GraphicsPipelineDescription(
                BlendStateDescription.SingleAlphaBlend,
                DepthStencilStateDescription.Disabled,
                RasterizerStateDescription.CullNone,
                PrimitiveTopology.TriangleStrip,
                new ShaderSetDescription(
                    new[]
                    {
                        new VertexLayoutDescription(
                            DisplayObject.QuadVertex.VertexSize,
                            1,
                            new VertexElementDescription("Position", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float2),
                            new VertexElementDescription("Size", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float2),
                            new VertexElementDescription("Tint", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Byte4_Norm),
                            new VertexElementDescription("Rotation", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float1))
                    },
                    factory.CreateFromSpirv(
                        new ShaderDescription(ShaderStages.Vertex, LoadShaderBytes("sprite.vert.spv"), "main"),
                        new ShaderDescription(ShaderStages.Fragment, LoadShaderBytes("sprite.frag.spv"), "main"),
                        new CrossCompileOptions(false, false, new SpecializationConstant(0, false)))),
                new[] { orthoLayout, _texLayout },
                gd.MainSwapchain.Framebuffer.OutputDescription));
*/
        }
        /*
        private byte[] LoadShaderBytes(string name)
        {
            return File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, "Assets", "Shaders", name));
        }
        */
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
                _orthoBuffer,
                0,
                Matrix4x4.CreateOrthographicOffCenter(0, width, 0, height, 0, 1));

            EnsureBufferSize(gd, (uint)_draws.Count * DisplayObject.QuadVertex.VertexSize);
            MappedResourceView<DisplayObject.QuadVertex> writemap = gd.Map<DisplayObject.QuadVertex>(_vertexBuffer, MapMode.Write);
            for (int i = 0; i < _draws.Count; i++)
            {
                writemap[i] = _draws[i].GpuVertex;
            }
            gd.Unmap(_vertexBuffer);

            cl.SetPipeline(_pipeline);
            cl.SetVertexBuffer(0, _vertexBuffer);
            cl.SetGraphicsResourceSet(0, _orthoSet);

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
                    _texLayout,
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
