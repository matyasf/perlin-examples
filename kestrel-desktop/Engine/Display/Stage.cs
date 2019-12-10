using System;
using Engine.Geom;

namespace Engine.Display
{
    /// <summary>
    /// Stage is the root of the display tree; its a singleton.
    /// </summary>
    public class Stage : DisplayObject
    {
        
        public override float Rotation { set => throw new ArgumentException(); }

        public override float X { set => throw new ArgumentException(); }

        public override float Y { set => throw new ArgumentException(); }

        public override DisplayObject Parent { internal set => throw new ArgumentException(); }
        
        
        internal Stage(int width, int height)
        {
            _isOnStage = true;
            Width = width;
            Height = height;
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