using Codecool.Quest.Logic;
using Perlin;
using Perlin.Display;
using SixLabors.Fonts;
using Veldrid;

namespace Codecool.Quest
{
    class Program
    {
        private GameMap _map;
        private TextField _healthTextField;
        private Sprite _mapContainer;
        private Sprite _playerGfx;
        
        static void Main(string[] args)
        {
            new Program();
        }

        private Program()
        {
            _map = MapLoader.LoadMap();
            PerlinApp.Start(_map.Width * Tiles.TileWidth,
                _map.Height * Tiles.TileWidth,
                "Codecool Quest",
                OnStart);
        }

        private void OnStart()
        {
            var stage = PerlinApp.Stage;
            
            // health textField
            _healthTextField = new TextField(
                PerlinApp.FontRobotoMono.CreateFont(14),
                _map.Player.Health.ToString(),
                false);
            _healthTextField.HorizontalAlign = HorizontalAlignment.Center;
            _healthTextField.Width = 100;
            _healthTextField.Height = 20;
            _healthTextField.X = (_map.Width * Tiles.TileWidth) / 2 - 50;
            stage.AddChild(_healthTextField);
            
            stage.EnterFrameEvent += StageOnEnterFrameEvent;
            
            _mapContainer = new Sprite();
            stage.AddChild(_mapContainer);
            DrawMap();
            
            var skeletonGfx = new Sprite("tiles.png", false, Tiles.SkeletonTile);
            skeletonGfx.X = _map.Skeleton.X * Tiles.TileWidth;
            skeletonGfx.Y = _map.Skeleton.Y * Tiles.TileWidth;
            stage.AddChild(skeletonGfx);
            
            _playerGfx = new Sprite("tiles.png", false, Tiles.PlayerTile);
            stage.AddChild(_playerGfx);
        }

        private void DrawMap()
        {
            _mapContainer.RemoveAllChildren();
            for (int x = 0; x < _map.Width; x++)
            {
                for (int y = 0; y < _map.Height; y++)
                {
                    var cell = _map.GetCell(x, y);
                    var tile = Tiles.GetMapTile(cell);
                    // tiles are 16x16 pixels
                    var sp = new Sprite("tiles.png", false, tile);
                    sp.X = x * Tiles.TileWidth;
                    sp.Y = y * Tiles.TileWidth;
                    _mapContainer.AddChild(sp);
                }
            }
        }
        
        // this gets called every frame
        private void StageOnEnterFrameEvent(DisplayObject target, float elapsedtimesecs)
        {
            // process inputs
            if (KeyboardInput.IsKeyPressedThisFrame(Key.Up))
            {
                _map.Player.Move(0, -1);
            }
            if (KeyboardInput.IsKeyPressedThisFrame(Key.Down))
            {
                _map.Player.Move(0, 1);
            }
            if (KeyboardInput.IsKeyPressedThisFrame(Key.Left))
            {
                _map.Player.Move(-1, 0);
            }
            if (KeyboardInput.IsKeyPressedThisFrame(Key.Right))
            {
                _map.Player.Move(1, 0);
            }
            // render changes
            _playerGfx.X = _map.Player.X * Tiles.TileWidth;
            _playerGfx.Y = _map.Player.Y * Tiles.TileWidth;
        }
    }
}