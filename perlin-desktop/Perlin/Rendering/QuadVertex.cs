using System.Numerics;

namespace Perlin.Rendering
{
    /// <summary>
    /// The data structure that stores a single Vertex on the GPU.
    /// </summary>
    internal struct QuadVertex
    {
        public const uint VertexSize = 24; // in bytes

        public Vector2 Position; // 8 bytes
        public Vector2 Size; // 8 bytes
        public float Alpha; // 4 bytes
        public float Rotation; // 4 bytes, in radians
    }
}