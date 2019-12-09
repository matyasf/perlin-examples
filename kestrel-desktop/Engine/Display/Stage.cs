using System;
using Veldrid;

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