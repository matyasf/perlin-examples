using System.Numerics;
using Veldrid;

namespace Engine
{
    internal struct QuadVertex
    {
        public const uint VertexSize = 24;

        public Vector2 Position;
        public Vector2 Size;
        public RgbaByte Tint;
        public float Rotation; // in radians
    }
}