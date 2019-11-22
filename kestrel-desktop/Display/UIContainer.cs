using System.Collections.Generic;

namespace Display
{
    public abstract class UIContainer
    {
        private readonly List<DisplayObject> _children = new List<DisplayObject>();
        
        public void AddChild(DisplayObject child)
        {
            _children.Add(child);
        }
        
        public List<DisplayObject> Children => _children;

        public virtual void Render()
        {
            foreach (var child in _children)
            {
                child.Render();
            }
        }
    }
}