using SixLabors.Fonts;
using SixLabors.ImageSharp.PixelFormats;

namespace Engine.Display
{
    /// <summary>
    /// A simple button
    /// </summary>
    public class Button : DisplayObject
    {

        private TextField _label;
        private Sprite _upGraphic;
        private Sprite _downGraphic;
        private Sprite _hoverGraphic;
        public Button(string text, bool autoSize = true)
        {
            var font = KestrelApp.FontRobotoMono.CreateFont(14);
            _label = new TextField(font, text, autoSize);
            
            _hoverGraphic = new Sprite(_label.Width + 10, _label.Height + 6, Rgba32.LightGray);
            _hoverGraphic.Visible = false;
            AddChild(_hoverGraphic);
            
            _downGraphic = new Sprite(_label.Width + 10, _label.Height + 6, Rgba32.DarkGray);
            _downGraphic.Visible = false;
            AddChild(_downGraphic);
            
            _upGraphic = new Sprite(_label.Width + 10, _label.Height + 6, Rgba32.LightSlateGray);
            AddChild(_upGraphic);

            _label.X = 5;
            _label.Y = 3;
            //_label.BackgroundColor = Rgba32.Bisque;
            AddChild(_label);
        }
        
    }
}