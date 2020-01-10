using Perlin.Geom;

namespace Perlin.Rendering
{
    /// <summary>
    /// Internal class to keep track of the current render state.
    /// </summary>
    public class RenderState  // TODO merge with GPUVertex
    {
        public float Alpha;
        public float ScaleX;
        public float ScaleY;
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
        
        /// <summary>
        /// Prepends the given matrix to the 2D modelview matrix.
        /// </summary>
        public void TransformModelviewMatrix(Matrix2D matrix)
        {
            _modelviewMatrix.PrependMatrix(matrix);
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
            _modelviewMatrix.CopyFromMatrix(renderState._modelviewMatrix);
        }
    }
}