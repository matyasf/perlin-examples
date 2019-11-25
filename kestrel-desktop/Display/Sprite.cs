
namespace Display
{
    public class Sprite : DisplayObject
    {
        public string Image; // TODO if null draws just the tint color

        public override void Render(double elapsedTimems)
        {
            KestrelApp.SpriteRenderer.RenderSprite(this);
            base.Render(elapsedTimems);
        }
    }
}

