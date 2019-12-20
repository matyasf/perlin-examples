using System;
using System.Numerics;
using Veldrid;
using Point = Engine.Geom.Point;

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
            // if nothing else is hit, the stage returns itself as target
            DisplayObject target = base.HitTest(p);
            if (target == null)
            {
                target = this;
            }
            return target;
        }

        internal void OnMouseMoveInternal(float x, float y)
        {
            var p = Point.Create(x, y);
            // TODO somehow determine who we entered and left to calculate MOUSE_ENTER and MOUSE_OUT events
            //Console.WriteLine("move x:" + x + " y:" + y + " " + res);
            var hit = HitTest(p);
            //hit.DispatchMouseMoved(x, y); // + send local coordinates too
            var current = hit;
            do
            {
                current = current.Parent;
               // current.DispatchMouseMovedInChild(x, y);
            } while (current != null);
        }
        
        internal DisplayObject DispatchMouseDownInternal(MouseButton button, Vector2 mousePosition)
        {
            var p = Point.Create(mousePosition.X, mousePosition.Y);
            var target = HitTest(p);
            target.DispatchMouseDown(button, p); // + send local coordinates too
            //Console.WriteLine("DOWN " + p + " " + target);
            return target;
            // + DispatchMouseDownInChild(x, y);
        }
        
        internal DisplayObject DispatchMouseUpInternal(MouseButton button, Vector2 mousePosition)
        {
            var p = Point.Create(mousePosition.X, mousePosition.Y);
            var target = HitTest(p);
            target.DispatchMouseUp(button, p); // + send local coordinates too
            //Console.WriteLine("UP " + p + " " + target);
            return target;
            // + DispatchMouseUPInChild
        }

        public override void Render(float elapsedTimeSecs)
        {
            InvokeEnterFrameEvent(elapsedTimeSecs);
            foreach (var child in Children)
            {
                child.Render(elapsedTimeSecs);
            }
        }

        public override string ToString()
        {
            return "Stage";
        }
    }
}