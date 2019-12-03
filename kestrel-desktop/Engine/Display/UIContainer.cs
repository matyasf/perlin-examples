using System.Collections.Generic;

namespace Engine.Display
{
    /// <summary>
    /// The base class for every UI element.
    /// </summary>
    public abstract class UIContainer
    {
        private readonly List<DisplayObject> _children = new List<DisplayObject>();
        
        public virtual void AddChild(DisplayObject child)
        {
            if (child.Parent != null)
            {
                child.RemoveFromParent();
            }
            _children.Add(child);
            if (this is Stage || this is DisplayObject && ((DisplayObject) this).IsOnStage)
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

        public virtual void Render(float elapsedTimems)
        {
            foreach (var child in _children)
            {
                child.Render(elapsedTimems);
            }
        }
    }
}