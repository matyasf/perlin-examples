using System.Collections.Generic;
using Codecool.Quest.Logic;
using Perlin.Geom;

namespace Codecool.Quest
{
    public class Tiles
    {
        public const int TileWidth = 16;
        
        private static readonly Dictionary<string, Rectangle> TileMap = new Dictionary<string, Rectangle>();
        
        public static readonly Rectangle PlayerTile = CreateTile(27, 0);
        public static readonly Rectangle SkeletonTile = CreateTile(29, 6);
        
        static Tiles()
        {
            TileMap["empty"] = CreateTile(0, 0);
            TileMap["wall"] = CreateTile(10, 17);
            TileMap["floor"] = CreateTile(2, 0);
        }

        public static Rectangle GetMapTile(IDrawable d)
        {
            return TileMap[d.Tilename.ToLower()];
        }

        private static Rectangle CreateTile(int i, int j)
        {
            return new Rectangle( i * (TileWidth + 1), j * (TileWidth + 1),
                TileWidth, TileWidth);
        }
    }
}