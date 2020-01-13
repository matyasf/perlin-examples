using Perlin.Display;
using Perlin.Geom;
using Veldrid;

namespace Perlin.Rendering
{
    public class RenderState
    {
        public QuadVertex _gpuVertex;
        
        public float Alpha;
        public float ScaleX;
        public float ScaleY;
        public float OriginalWidth;
        public float OriginalHeight;
        public ResourceSet ResSet;
        
        private Matrix2D _modelviewMatrix;
        
        public RenderState()
        {
            Reset();
        }
        
        /// <summary>
        /// Resets the RenderState to the default settings.
        /// (Check each property documentation for its default value.)
        /// </summary>
        public void Reset()
        {
            ScaleX = ScaleY = 1.0f;
            Alpha = 1.0f;
            if (_modelviewMatrix != null) _modelviewMatrix.Identity();
            else _modelviewMatrix = Matrix2D.Create();
        }
        
        public void ApplyNewState(DisplayObject displayObject)
        {
            Alpha *= displayObject.Alpha;
            ScaleX *= displayObject.ScaleX;
            ScaleY *= displayObject.ScaleY;
            OriginalWidth = displayObject.Width;
            OriginalHeight = displayObject.Height;
            _modelviewMatrix.PrependMatrix(displayObject.TransformationMatrix);
            ResSet = displayObject.ResSet;
        }

        public Matrix2D ModelviewMatrix
        {
            get => _modelviewMatrix;
            set => _modelviewMatrix.CopyFromMatrix(value);
        }

        public void CopyFrom(RenderState renderState)
        {
            Alpha = renderState.Alpha;
            ScaleX = renderState.ScaleX;
            ScaleY = renderState.ScaleY;
            ModelviewMatrix = renderState.ModelviewMatrix;
        }
        
        /// <summary>
        /// The Vertex that will be uploaded to the GPU to render this.
        /// </summary>
        internal ref QuadVertex GetGpuVertex()
        {
            _gpuVertex.Position.X = ModelviewMatrix.Tx;
            _gpuVertex.Position.Y = ModelviewMatrix.Ty;
            _gpuVertex.Size.X = OriginalWidth * ScaleX;
            _gpuVertex.Size.Y = OriginalHeight * ScaleY;
            _gpuVertex.Rotation = ModelviewMatrix.Rotation;
            _gpuVertex.Alpha = Alpha;
            return ref _gpuVertex;
        }
    }
}