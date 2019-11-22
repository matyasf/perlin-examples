
namespace Display
{
    public class Sprite : DisplayObject
    {
        public string Image; // TODO if null draws just the tint color

        public override void Render()
        {
//            _spriteRenderer.Draw(_gd, _cl);
            base.Render();
        }
    }
}

