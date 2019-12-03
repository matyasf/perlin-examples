using Display;
using Veldrid;

namespace Engine.Display
{
    /// <summary>
    /// Stage is the root of the display tree; its a singleton.
    /// </summary>
    public class Stage : UIContainer
    {
        public int Width { get; }
        public int Height { get; }
        
        /// <summary>
        /// Background color of the app.
        /// </summary>
        public readonly RgbaFloat BackgroundColor = new RgbaFloat(0, 0, 0f, 1f);

        public event DisplayObject.EnterFrame EnterFrameEvent;

        internal Stage(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public override void Render(float elapsedTimems)
        {
            EnterFrameEvent?.Invoke(null, elapsedTimems); // null is not nice here...
            base.Render(elapsedTimems);
        }
    }
}