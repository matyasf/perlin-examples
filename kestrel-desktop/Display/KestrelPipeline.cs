using System;
using System.IO;
using Veldrid;
using Veldrid.SPIRV;

namespace Display
{
    public class KestrelPipeline
    {
        private Pipeline _pipeline;
        private ResourceLayout _texLayout;
        private ResourceSet _orthoSet;

        public void CreatePipeline(GraphicsDevice gd)
        {
            ResourceFactory factory = gd.ResourceFactory;
            
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
        }
        
        private byte[] LoadShaderBytes(string name)
        {
            return File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, "Assets", "Shaders", name));
        }
    }
}