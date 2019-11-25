using SixLabors.Fonts;

namespace Display
{
    public class TextField : Sprite
    {
        public string Text;

        //public Font Font;

        public override void Render(double elapsedTimems)
        {
            //_spriteRenderer.RenderText(_gd, _cl, _textRenderer.TextureView, _scoreTextField);
            base.Render(elapsedTimems);
        }
    }
}