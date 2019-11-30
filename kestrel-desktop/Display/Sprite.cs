
namespace Display
{
    public class Sprite : DisplayObject
    {
        public string Image; // TODO if null draws just the tint color

        public override void Render(double elapsedTimems)
        {
            KestrelApp.SpriteRenderer.BatchSprite(this);
          //  KestrelApp.SpriteRenderer.Draw(KestrelApp.DefaultGraphicsDevice, KestrelApp.CommandList);
            base.Render(elapsedTimems);
        }
    }
}

