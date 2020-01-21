using System.IO;
using Codecool.Quest.Logic.Actors;

namespace Codecool.Quest.Logic
{
    public static class MapLoader
    {
        public static GameMap LoadMap()
        {
            var lines = File.ReadAllLines("data/map.txt");
            var dimensions = lines[0].Split(" ");
            var width = int.Parse(dimensions[0]);
            var height = int.Parse(dimensions[1]);
            
            GameMap map = new GameMap(width, height, CellType.Empty);
            for (var y = 0; y < height; y++) 
            {
                var line = lines[y + 1];
                for (int x = 0; x < width; x++)
                {
                    if (x < line.Length)
                    {
                        Cell cell = map.GetCell(x, y);
                        switch (line[x])
                        {
                            case ' ':
                                cell.Type = CellType.Empty;
                                break;
                            case '#':
                                cell.Type = CellType.Wall;
                                break;
                            case '.':
                                cell.Type = CellType.Floor;
                                break;
                            case 's':
                                cell.Type = CellType.Floor;
                                // TODO change this code to allow more than one enemy
                                map.Skeleton = new Skeleton(cell);
                                break;
                            case '@':
                                cell.Type = CellType.Floor;
                                map.Player = new Player(cell);
                                break;
                            default:
                                throw new InvalidDataException($"Unrecognized character: '{line[x]}'");
                        }
                    }
                }
            }
            return map;
        }
    }
}