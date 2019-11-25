using System.Numerics;
using Veldrid;

namespace Display
{
    public class DisplayObject : UIContainer
    {
        public delegate void EnterFrame(double elapsed);

        public event EnterFrame EnterFrameEvent;
        
        public bool IsOnStage { get; internal set; }
        
        internal QuadVertex GpuVertex;
        
        public DisplayObject()
        {
            GpuVertex.Tint = RgbaByte.White;
        }
        
        public void RemoveFromParent()
        {
            if (Parent != null)
            {
                Parent.RemoveChild(this);
                Parent = null;
                IsOnStage = false;
            }
        }

        public override void Render(double elapsedTimems)
        {
            if (IsOnStage)
            {
                EnterFrameEvent?.Invoke(elapsedTimems);
            }
            base.Render(elapsedTimems);
        }

        public UIContainer Parent { get; internal set; }
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

        public float Width
        {
            get => GpuVertex.Size.X;
            set => GpuVertex.Size.X = value;
        }

        public float Height
        {
            get => GpuVertex.Size.Y;
            set => GpuVertex.Size.Y = value;
        }
        
        // in radians
        public float Rotation
        {
            get => GpuVertex.Rotation;
            set => GpuVertex.Rotation = value;
        }
        
        public RgbaByte Tint
        {
            get => GpuVertex.Tint;
            set => GpuVertex.Tint = value;
        }

        internal struct QuadVertex
        {
            public const uint VertexSize = 24;

            public Vector2 Position;
            public Vector2 Size;
            public RgbaByte Tint;
            public float Rotation; // in radians
        }
    }
}