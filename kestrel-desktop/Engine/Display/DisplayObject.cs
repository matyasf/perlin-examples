using System;
using System.Collections.Generic;
using Engine.Geom;
using Veldrid;
using Point = Engine.Geom.Point;
using Rectangle = Engine.Geom.Rectangle;

namespace Engine.Display
{
    /// <summary>
    /// DisplayObject is the base class for every renderable object.
    /// </summary>
    public abstract class DisplayObject
    {
        public delegate void EnterFrame(DisplayObject target, float elapsedTimeSecs);
        public delegate void UIChange(DisplayObject target);

        public event EnterFrame EnterFrameEvent;
        public event UIChange AddedToStage;
        public event UIChange RemovedFromStage;
        public bool Visible = true;

        public float PivotX;
        public float PivotY;
        public float ScaleX = 1.0f;
        public float ScaleY = 1.0f;
        
        private float _skewX;
        private float _skewY;
        /// <summary>
        /// The GPU resource set for this object. Its the same object for objects with the same image.
        /// </summary>
        internal ResourceSet ResSet;
        private RenderState _renderState;
        private QuadVertex _gpuVertex;
        
        public DisplayObject()
        {
            _gpuVertex.Tint = RgbaByte.White;
            _transformationMatrix = Matrix2D.Create();
        }
        
        internal QuadVertex GetGpuVertex()
        {
            _gpuVertex.Position.X = _renderState.ModelviewMatrix.Tx;
            _gpuVertex.Position.Y = _renderState.ModelviewMatrix.Ty;
            _gpuVertex.Rotation = _renderState.ModelviewMatrix.Rotation;
            _gpuVertex.Size.X = OriginalWidth * _renderState.ModelviewMatrix.ScaleX;
            _gpuVertex.Size.Y = OriginalHeight * _renderState.ModelviewMatrix.ScaleY;
            //GpuVertex.Tint.A = _renderState.Alpha;
            // + set  pivot, scale
            return _gpuVertex;
        }

        protected bool IsOnStageProperty;
        /// <summary>
        /// Whether this instance is on the Stage. If something is not on the Stage, it will not render.
        /// </summary>
        public bool IsOnStage
        {
            get => IsOnStageProperty;
            internal set
            {
                if (value != IsOnStageProperty)
                {
                    IsOnStageProperty = value;
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
        
        public void RemoveFromParent()
        {
            if (Parent != null)
            {
                Parent.RemoveChild(this);
                Parent = null;
                IsOnStage = false;
            }
        }

        public virtual void Render(float elapsedTimeSecs)
        {
            if (IsOnStage)
            {
                InvokeEnterFrameEvent(elapsedTimeSecs);
            }
            if (Visible)
            {
                _renderState = KestrelApp.Renderer.PushRenderState(1.0f, TransformationMatrix);
                var bounds = GetBounds();
                if (bounds.Width > 0 && bounds.Height > 0)
                {
                    KestrelApp.Renderer.AddToRenderQueue(this);
                }
                foreach (var child in Children)
                {
                    child.Render(elapsedTimeSecs);
                }
                KestrelApp.Renderer.PopRenderState();
            }
        }

        public virtual DisplayObject Parent { get; internal set; }

        private float _x;
        public virtual float X
        {
            get => _x;
            set => _x = value;
        }

        private float _y;
        public virtual float Y
        {
            get => _y;
            set => _y = value; 
        }

        protected float OriginalWidth;
        /// <summary>
        /// The width of the object without transformations.
        /// </summary>
        public virtual float Width => OriginalWidth;

        protected float OriginalHeight;
        /// <summary>
        /// The height of the object without transformations.
        /// </summary>
        public virtual float Height => OriginalHeight;

        /// <summary>
        /// The width of the object after scaling
        /// </summary>
        public virtual float WidthScaled
        {
            get => OriginalWidth * ScaleX;
            set => ScaleX = value / OriginalWidth;
        }

        /// <summary>
        /// The height of the object after scaling
        /// </summary>
        public virtual float HeightScaled
        {
            get => OriginalHeight * ScaleY;
            set => ScaleY = value / OriginalHeight;
        }
        
        private float _rotation;
        /// <summary>
        /// Rotation in Radians.
        /// </summary>
        public virtual float Rotation
        {
            get => _rotation;
            set => _rotation = value;
        }
        
        public RgbaByte Tint
        {
            get => _gpuVertex.Tint;
            set => _gpuVertex.Tint = value;
        }

        /// <summary>
        /// Returns the object that is found topmost on a point in local coordinates, or null if the test fails.
        /// </summary>
        public virtual DisplayObject HitTest(Point p)
        {
            if (!Visible)
            {
                return null;
            }
            for (var i = Children.Count - 1; i >= 0; --i) // front to back!
            {
                DisplayObject child = Children[i];
                if (child.Visible)
                {
                    Matrix2D transformationMatrix = Matrix2D.Create();
                    transformationMatrix.CopyFromMatrix(child.TransformationMatrix);
                    transformationMatrix.Invert();

                    Point transformedPoint = transformationMatrix.TransformPoint(p);
                    DisplayObject target = child.HitTest(transformedPoint);
                    if (target != null)
                    {
                        return target;
                    }
                }
            }
            return null;
        }

        internal void InvokeEnterFrameEvent(float elapsedTimeSecs)
        {
            EnterFrameEvent?.Invoke(this, elapsedTimeSecs);
        }
        
        /// <summary>
        /// Returns the bounds of this object after transformations
        /// </summary>
        public virtual Rectangle GetBounds()
        {
            return GetBounds(Parent);
        }
        
        public virtual Rectangle GetBounds(DisplayObject targetSpace)
        {
            Rectangle outRect = Rectangle.Create();
            if (targetSpace == this) // Optimization
            {
                outRect.Width = OriginalWidth;
                outRect.Height = OriginalHeight;
            }
            else if (targetSpace == Parent && !IsRotated) // Optimization
            {
                outRect = Rectangle.Create(X - PivotX * ScaleX,
                    Y - PivotY * ScaleY,
                    OriginalWidth * ScaleX,
                    OriginalHeight * ScaleY);
                if (ScaleX < 0.0f)
                {
                    outRect.Width *= -1.0f;
                    outRect.X -= outRect.Width;
                }
                if (ScaleY < 0.0f)
                {
                    outRect.Height *= -1.0f;
                    outRect.Top -= outRect.Height;
                }
            }
            else
            {
                outRect.Width = OriginalWidth;
                outRect.Height = OriginalHeight;
                Matrix2D sMatrix = GetTransformationMatrix(targetSpace);
                outRect = outRect.GetBounds(sMatrix);
            }
            return outRect;
        }

        public virtual Rectangle GetBoundsWithChildren()
        {
            return GetBoundsWithChildren(Parent);
        }
        public virtual Rectangle GetBoundsWithChildren(DisplayObject targetSpace)
        {
            var ownBounds = GetBounds(targetSpace);
            float minX = ownBounds.X, maxX = ownBounds.Right;
            float minY = ownBounds.Y, maxY = ownBounds.Bottom;
            foreach (DisplayObject child in Children)
            {
                Rectangle childBounds = child.GetBoundsWithChildren(targetSpace);
                minX = Math.Min(minX, childBounds.X);
                maxX = Math.Max(maxX, childBounds.X + childBounds.Width);
                minY = Math.Min(minY, childBounds.Top);
                maxY = Math.Max(maxY, childBounds.Top + childBounds.Height);
            }
            return Rectangle.Create(minX, minY, maxX - minX, maxY - minY);
        }

        private readonly Matrix2D _transformationMatrix;
        /// <summary>
        /// The transformation matrix of the object relative to its parent.
        /// <returns>CAUTION: not a copy, but the actual object!</returns>
        /// </summary>
        public Matrix2D TransformationMatrix
        {
            get // TODO cache this!
            {
                _transformationMatrix.Identity();
                _transformationMatrix.Scale(ScaleX, ScaleY);
                _transformationMatrix.Skew(_skewX, _skewY);
                _transformationMatrix.Rotate(_rotation);
                _transformationMatrix.Translate(X, Y);

                if (PivotX != 0.0f || PivotY != 0.0f)
                {
                    // prepend pivot transformation
                    _transformationMatrix.Tx = X - _transformationMatrix.A * PivotX
                                                  - _transformationMatrix.C * PivotY;
                    _transformationMatrix.Ty = Y - _transformationMatrix.B * PivotX
                                                  - _transformationMatrix.D * PivotY;
                }
                Console.WriteLine(this + " " + _transformationMatrix.ScaleX + " " + ScaleX + " " + _rotation);
                return _transformationMatrix;
            }
        }
        
        /// <summary>
        /// Creates a matrix that represents the transformation from the local coordinate system to another.
        /// </summary>
        public Matrix2D GetTransformationMatrix(DisplayObject targetSpace)
        {
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
            var commonParent = FindCommonParent(this, targetSpace);

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
        
        private static readonly List<DisplayObject> CommonParentHelper = new List<DisplayObject>();
        private static DisplayObject FindCommonParent(DisplayObject object1, DisplayObject object2)
        {
            DisplayObject currentObject = object1;
            while (currentObject != null)
            {
                CommonParentHelper.Add(currentObject);
                currentObject = currentObject.Parent;
            }
            currentObject = object2;
            while (currentObject != null && CommonParentHelper.Contains(currentObject) == false)
            {
                currentObject = currentObject.Parent;
            }
            CommonParentHelper.Clear();
            if (currentObject != null)
            {
                return currentObject;
            }
            throw new ArgumentException("Object not connected to target");
        }
        
        /// <summary>
        /// Indicates if the object is rotated or skewed in any way.
        /// </summary>
        internal bool IsRotated => _rotation != 0.0 || _skewX != 0.0 || _skewY != 0.0;
        
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
        
        protected readonly List<DisplayObject> Children = new List<DisplayObject>();
        
        public virtual void AddChild(DisplayObject child)
        {
            if (child.Parent != null)
            {
                child.RemoveFromParent();
            }
            Children.Add(child);
            if (IsOnStage)
            {
                child.IsOnStage = true;
            }
            child.Parent = this;
        }

        public virtual void RemoveChild(DisplayObject child)
        {
            Children.Remove(child);
            child.IsOnStage = false;
            child.Parent = null;
        }
    }
}