using Perlin.Display;
using Perlin.Geom;
using Veldrid;

namespace Perlin.Rendering
{
    public class RenderState
    {
        
        public ResourceSet ResSet { get; private set; }
        /// <summary>
        /// The Vertex that will be uploaded to the GPU to render this.
        /// </summary>
        public ref QuadVertex GpuVertex
        {
            get
            {
                _gpuVertex.Position.X = _modelviewMatrix.Tx;
                _gpuVertex.Position.Y = _modelviewMatrix.Ty;
                _gpuVertex.Size.X = _originalWidth * _scaleX;
                _gpuVertex.Size.Y = _originalHeight * _scaleY;
                _gpuVertex.Rotation = _modelviewMatrix.Rotation;
                _gpuVertex.Alpha = _alpha;
                return ref _gpuVertex;
            }
        }
        private QuadVertex _gpuVertex;
        private float _alpha;
        private float _scaleX;
        private float _scaleY;
        private float _originalWidth;
        private float _originalHeight;
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
            _scaleX = _scaleY = 1.0f;
            _alpha = 1.0f;
            if (_modelviewMatrix != null) _modelviewMatrix.Identity();
            else _modelviewMatrix = Matrix2D.Create();
        }

        public void ApplyNewState(RenderState oldState, DisplayObject displayObject)
        {
            _alpha = oldState._alpha * displayObject.Alpha;
            _scaleX = oldState._scaleX * displayObject.ScaleX;
            _scaleY = oldState._scaleY * displayObject.ScaleY;
            _modelviewMatrix.CopyFromMatrix(oldState._modelviewMatrix).PrependMatrix(displayObject.TransformationMatrix);
            _originalWidth = displayObject.Width;
            _originalHeight = displayObject.Height;
            ResSet = displayObject.ResSet;
        }
    }
}