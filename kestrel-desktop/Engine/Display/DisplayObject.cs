using System.Numerics;
using Display;
using Veldrid;

namespace Engine.Display
{
    /// <summary>
    /// DisplayObject is the base class for every renderable object.
    /// </summary>
    public abstract class DisplayObject : UIContainer
    {
        public delegate void EnterFrame(double elapsed);

        public event EnterFrame EnterFrameEvent;

        /// <summary>
        /// Whether this instance is on the Stage. If something is not on the Stage, it will not render.
        /// </summary>
        public bool IsOnStage { get; internal set; }
        
        /// <summary>
        /// The GPU resource set for this object. Its the same object for objects with the same image.
        /// </summary>
        internal ResourceSet ResSet;

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
            KestrelApp.Renderer.AddToRenderQueue(this);
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

        public virtual float Width
        {
            get => GpuVertex.Size.X;
            set => GpuVertex.Size.X = value;
        }

        public virtual float Height
        {
            get => GpuVertex.Size.Y;
            set => GpuVertex.Size.Y = value;
        }
        
        /// <summary>
        /// Rotation in Radians.
        /// </summary>
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