using System;
using SixLabors.Fonts;
using SixLabors.ImageSharp.PixelFormats;

namespace Engine.Display
{
    /// <summary>
    /// A simple button
    /// </summary>
    public class Button : Sprite
    {

        private TextField _label;
        private Sprite _upGraphic;
        private Sprite _downGraphic;
        private Sprite _hoverGraphic;
        
        public Button(string text, bool autoSize = true) : base(10, 10, Rgba32.Transparent)
        {
            Name = "buttonBase";
            var font = KestrelApp.FontRobotoMono.CreateFont(14);
            _label = new TextField(font, text, autoSize);
            _label.MouseOrTouchEnabled = false;
            _label.Name = "label";

            _upGraphic = new Sprite(_label.Width + 10, _label.Height + 6, Rgba32.Beige);
            _upGraphic.MouseOrTouchEnabled = false;
            _upGraphic.Name = "upg";
            AddChild(_upGraphic);
            
            _hoverGraphic = new Sprite(_label.Width + 10, _label.Height + 6, Rgba32.LightGray);
            _hoverGraphic.Visible = false;
            _hoverGraphic.MouseOrTouchEnabled = false;
            _hoverGraphic.Name = "hoverg";
            AddChild(_hoverGraphic);

            _downGraphic = new Sprite(_label.Width + 10, _label.Height + 6, Rgba32.DarkGray);
            _downGraphic.MouseOrTouchEnabled = false;
            _downGraphic.Visible = false;
            _downGraphic.Name = "downg";
            AddChild(_downGraphic);

            _label.X = 5;
            _label.Y = 3;
            AddChild(_label);

            OriginalWidth = _label.Width + 10;
            OriginalHeight = _label.Height + 6;
            ResSet = KestrelApp.ImageManager.CreateColoredTexture((uint)OriginalWidth, (uint)OriginalHeight, Rgba32.Transparent);

            MouseEnter += (target, coords) =>
            {
                Console.WriteLine("ENTER");
                _hoverGraphic.Visible = true;
            };
            MouseExit += (target, coords) =>
            {
                Console.WriteLine("EXIT");
                _hoverGraphic.Visible = false;
            };
            MouseDown += (target, coords, button) =>
            {
                Console.WriteLine("DOWN");
                _downGraphic.Visible = true;
            };
            MouseUp += (target, coords, button) =>
            {
                Console.WriteLine("UP");
                _downGraphic.Visible = false;
            };
        }
        
    }
}