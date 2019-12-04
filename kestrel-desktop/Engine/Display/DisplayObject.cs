using System.Numerics;
using Veldrid;

namespace Engine.Display
{
    /// <summary>
    /// DisplayObject is the base class for every renderable object.
    /// </summary>
    public abstract class DisplayObject : UIContainer
    {
        public delegate void EnterFrame(DisplayObject target, float elapsedTimeSecs);
        public delegate void UIChange(DisplayObject target);

        public event EnterFrame EnterFrameEvent;
        public event UIChange AddedToStage;
        public event UIChange RemovedFromStage;
        public bool Visible = true;

        protected bool _isOnStage;
        /// <summary>
        /// Whether this instance is on the Stage. If something is not on the Stage, it will not render.
        /// </summary>
        public bool IsOnStage
        {
            get => _isOnStage;
            internal set
            {
                if (value != _isOnStage)
                {
                    _isOnStage = value;
                    if (value)
                    {
                        AddedToStage?.Invoke(this);
                    }
                    else
                    {
                        RemovedFromStage?.Invoke(this);
                    }
                }
            }
        }
        
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

        public override void Render(float elapsedTimeSecs)
        {
            if (IsOnStage)
            {
                InvokeEnterFrameEvent(elapsedTimeSecs);
            }
            if (Visible)
            {
                if (Width > 0 && Height > 0)
                {
                    KestrelApp.Renderer.AddToRenderQueue(this);      
                }
                base.Render(elapsedTimeSecs);
            }
        }

        public virtual UIContainer Parent { get; internal set; }
        public virtual float X
        {
            get => GpuVertex.Position.X;
            set => GpuVertex.Position.X = value;
        }

        public virtual float Y
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
        public virtual float Rotation
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
        
        /// <summary>
        ///  Returns the object that is found topmost on a point in local coordinates, or null if the test fails.
        /// </summary>
        public virtual DisplayObject HitTest(Point p)
        {
            for (var i = _children.Count - 1; i >= 0; --i) // front to back!
            {
                DisplayObject child = _children[i];
                if (child.Visible)
                {
                    DisplayObject target = child.HitTest(p);
                    if (target != null)
                    {
                        return target;
                    }
                }
            }
            // just tests the bounding rectangle. TODO Take rotation into account!!
            if (X > p.X && X + Width < p.X && Y > p.Y && Y < p.Y + Height)
            {
                return this;
            }
            return null;
        }

        internal void InvokeEnterFrameEvent(float elapsedTimeSecs)
        {
            EnterFrameEvent?.Invoke(this, elapsedTimeSecs);
        }
    }
}