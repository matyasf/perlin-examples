using System;
using Engine.Geom;

namespace Engine.Display
{
    /// <summary>
    /// Stage is the root of the display tree. If you want to show something you have to add it to the Stage
    /// (via <code>AddChild()</code> or to one of its descendants.)
    /// </summary>
    public class Stage : DisplayObject
    {
        
        /// <summary>
        /// Stage cannot be rotated, this will throw an exception
        /// </summary>
        public override float Rotation { set => throw new ArgumentException(); }
        
        /// <summary>
        /// Sets the X coordinate of the window. Does not do anything on Android/iOS.
        /// </summary>
        public override float X
        {
            set => KestrelApp.Window.X = (int)value;
            get => KestrelApp.Window.X;
        }

        /// <summary>
        /// Sets the Y coordinate of the window. Does not do anything on Android/iOS.
        /// </summary>
        public override float Y
        {
            set => KestrelApp.Window.Y = (int)value;
            get => KestrelApp.Window.Y;
        }

        /// <summary>
        /// Stage has no parent. Trying to set it will throw an exception.
        /// </summary>
        public override DisplayObject Parent { internal set => throw new ArgumentException(); }
        
        
        internal Stage(int width, int height)
        {
            IsOnStageProperty = true;
            OriginalWidth = width;
            OriginalHeight = height;
        }

        public override DisplayObject HitTest(Point p)
        {
            // locations outside of the stage area shouldn't be accepted
            if (p.X < 0 || p.X > Width || p.Y < 0 || p.Y > Height)
            {
                return null;
            }
            // if nothing else is hit, the stage returns itself as target
            DisplayObject target = base.HitTest(p);
            if (target == null)
            {
                target = this;
            }
            return target;
        }

        public override void Render(float elapsedTimeSecs)
        {
            InvokeEnterFrameEvent(elapsedTimeSecs);
            foreach (var child in Children)
            {
                child.Render(elapsedTimeSecs);
            }
        }
    }
}