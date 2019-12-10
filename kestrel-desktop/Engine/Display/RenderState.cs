using Engine.Geom;

namespace Engine.Display
{
    public class RenderState
    {
        public float Alpha;
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
            _modelviewMatrix.CopyFromMatrix(renderState._modelviewMatrix);
        }
    }
}