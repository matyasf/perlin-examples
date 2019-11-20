using System.Numerics;
using Veldrid;

namespace Display
{
    public class Sprite
    {
        public float X
        {
            get => GpuVertex.Position.X;
            set => GpuVertex.Position.X = value;
        }

        public float Y
        {
            get => GpuVertex.Position.Y;
            set => GpuVertex.Position.Y = value;
        }

        // TODO if null draws just the tint color
        public string Image;
        
        public float Width
        {
            get => GpuVertex.Size.Y;
            set => GpuVertex.Size.Y = value;
        }

        public float Height
        {
            get => GpuVertex.Size.X;
            set => GpuVertex.Size.X = value;
        }

        public RgbaByte Tint
        {
            get => GpuVertex.Tint;
            set => GpuVertex.Tint = value;
        }

        // in radians
        public float Rotation
        {
            get => GpuVertex.Rotation;
            set => GpuVertex.Rotation = value;
        }
        
        internal QuadVertex GpuVertex;
        
        public Sprite()
        {
            GpuVertex.Tint = RgbaByte.White;
        }

        internal struct QuadVertex
        {
            public const uint VertexSize = 24;

            public Vector2 Position;
            public Vector2 Size;
            public RgbaByte Tint;
            public float Rotation; // in radians
            
            public QuadVertex(Vector2 position, Vector2 size, RgbaByte? tint = null, float rotation = 0f) // TODO remove
            {
                var effectiveTint = tint ?? RgbaByte.White;
                Position = position;
                Size = size;
                Tint = effectiveTint;
                Rotation = rotation;
            }
        }
    }
}

