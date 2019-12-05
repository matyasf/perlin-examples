using System;
using System.Collections.Generic;
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
        
        public DisplayObject()
        {
            GpuVertex.Tint = RgbaByte.White;
            _transformationMatrix = Matrix2D.Create();
        }
        
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

        public virtual DisplayObject Parent { get; internal set; }
        public virtual float X
        {
            get => GpuVertex.Position.X;
            set => GpuVertex.Position.X = value; // not good, needs to be positioned relative to the parent!
        }

        public virtual float Y
        {
            get => GpuVertex.Position.Y;
            set => GpuVertex.Position.Y = value; // not good, needs to be positioned relative to the parent!
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
        
        private float _pivotX;
        private float _pivotY;

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
        
        private Matrix2D _transformationMatrix;
        /// <summary>
        /// The transformation matrix of the object relative to its parent.
        /// <returns>CAUTION: not a copy, but the actual object!</returns>
        /// </summary>
        public Matrix2D TransformationMatrix
        {
            get
            {
                // Note: cache this!
                _transformationMatrix.Identity();
                //_transformationMatrix.Scale(_scaleX, _scaleY);
                //_transformationMatrix.Skew(_skewX, _skewY);
                _transformationMatrix.Rotate(Rotation);
                _transformationMatrix.Translate(X, Y);

                if (_pivotX != 0.0f || _pivotY != 0.0f)
                {
                    // prepend pivot transformation
                    _transformationMatrix.Tx = X - _transformationMatrix.A * _pivotX
                                                  - _transformationMatrix.C * _pivotY;
                    _transformationMatrix.Ty = Y - _transformationMatrix.B * _pivotX
                                                  - _transformationMatrix.D * _pivotY;
                }
                return _transformationMatrix;
            }
        }
        
        /// <summary>
        /// Creates a matrix that represents the transformation from the local coordinate system to another.
        /// </summary>
        public Matrix2D GetTransformationMatrix(DisplayObject targetSpace)
        {
            DisplayObject commonParent;
            DisplayObject currentObject;
            Matrix2D outMatrix = Matrix2D.Create();
            outMatrix.Identity();
            if (targetSpace == this)
            {
                return outMatrix;
            }
            if (targetSpace == Parent || (targetSpace == null && Parent == null))
            {
                outMatrix.CopyFromMatrix(TransformationMatrix);
                return outMatrix;
            }
            if (targetSpace == null || targetSpace == Root)
            {
                // if targetSpace 'null', we assume that we need it in the space of the Base object.
                // -> move up from this to base
                currentObject = this;
                while (currentObject != targetSpace)
                {
                    outMatrix.AppendMatrix(currentObject.TransformationMatrix);
                    currentObject = currentObject.Parent;
                }
                return outMatrix;
            }
            if (targetSpace.Parent == this)
            {
                outMatrix = targetSpace.GetTransformationMatrix(this);
                outMatrix.Invert();
                return outMatrix;
            }
            // targetSpace is not an ancestor
            // 1.: Find a common parent of this and the target coordinate space.
            commonParent = FindCommonParent(this, targetSpace);

            // 2.: Move up from this to common parent
            currentObject = this;
            while (currentObject != commonParent)
            {
                outMatrix.AppendMatrix(currentObject.TransformationMatrix);
                currentObject = currentObject.Parent;
            }

            if (commonParent == targetSpace)
            {
                return outMatrix;
            }

            // 3.: Now move up from target until we reach the common parent
            var sHelperMatrix = Matrix2D.Create();
            sHelperMatrix.Identity();
            currentObject = targetSpace;
            while (currentObject != commonParent)
            {
                sHelperMatrix.AppendMatrix(currentObject.TransformationMatrix);
                currentObject = currentObject.Parent;
            }

            // 4.: Combine the two matrices
            sHelperMatrix.Invert();
            outMatrix.AppendMatrix(sHelperMatrix);

            return outMatrix;
        }
        
        private static readonly List<DisplayObject> _commonParentHelper = new List<DisplayObject>();
        private static DisplayObject FindCommonParent(DisplayObject object1, DisplayObject object2)
        {
            DisplayObject currentObject = object1;
            while (currentObject != null)
            {
                _commonParentHelper.Add(currentObject);
                currentObject = currentObject.Parent;
            }
            currentObject = object2;
            while (currentObject != null && _commonParentHelper.Contains(currentObject) == false)
            {
                currentObject = currentObject.Parent;
            }
            _commonParentHelper.Clear();
            if (currentObject != null)
            {
                return currentObject;
            }
            throw new ArgumentException("Object not connected to target");
        }
        
        /// <summary>
        /// The topmost object in the display tree the object is part of.
        /// </summary>
        public DisplayObject Root
        {
            get
            {
                DisplayObject currentObject = this;
                while (currentObject.Parent != null)
                {
                    currentObject = currentObject.Parent;
                }
                return currentObject;
            }
        }
    }
}