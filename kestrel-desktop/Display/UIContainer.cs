using System;
using System.Collections.Generic;

namespace Display
{
    public abstract class UIContainer
    {
        private readonly List<DisplayObject> _children = new List<DisplayObject>();
        
        public virtual void AddChild(DisplayObject child)
        {
            _children.Add(child);
            if (this is Stage || (this is DisplayObject && ((DisplayObject) this).IsOnStage))
            {
                child.IsOnStage = true;
            }
            child.Parent = this;
        }

        public virtual void RemoveChild(DisplayObject child)
        {
            _children.Remove(child);
            if (this is DisplayObject && ((DisplayObject) this).IsOnStage)
            {
                child.IsOnStage = false;
            }
            child.Parent = null;
        }

        public virtual void Render(double elapsedTimems)
        {
            foreach (var child in _children)
            {
                child.Render(elapsedTimems);
            }
        }
    }
}