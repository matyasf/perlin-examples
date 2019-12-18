using System.Numerics;
using Veldrid;

namespace Engine.Rendering
{
    /// <summary>
    /// The data structure that stores a single Vertex on the GPU.
    /// </summary>
    internal struct QuadVertex
    {
        public const uint VertexSize = 24;

        public Vector2 Position; // 8 byte
        public Vector2 Size;
        public RgbaByte Tint; // 4 byte
        public float Rotation; // in radians
    }
}