namespace Display
{
    public class Stage : UIContainer
    {
        public int Width { get; }
        public int Height { get; }

        public event DisplayObject.EnterFrame EnterFrameEvent;

        internal Stage(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public override void Render(double elapsedTimems)
        {
            EnterFrameEvent?.Invoke(elapsedTimems);
            base.Render(elapsedTimems);
        }
    }
}