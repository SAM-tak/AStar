using System;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AStar.Tests
{
    public static class Helper
    {
        public static string PrintGrid(WorldGrid worldGrid, bool appendSpace = true)
        {
            var s = new StringBuilder();

            for (var row = 0; row < worldGrid.Height; row++)
            {
                for (var column = 0; column < worldGrid.Width; column++)
                {
                    s.Append(worldGrid[column, row]);
                    if (appendSpace)
                    {
                        s.Append(' ');
                    }
                }
                s.Append(Environment.NewLine);
            }

            return s.ToString();
        }

        public static string PrintPath(WorldGrid world, Vector2Int[] path, bool appendSpace = true)
        {
            var s = new StringBuilder();
            
            for (var row = 0; row < world.Height; row++)
            {
                for (var column = 0; column < world.Width; column++)
                {
                    if (path.Any(n => n.y == row && n.x == column))
                    {
                        s.Append("*");
                    }
                    else
                    {
                        s.Append(world[column, row]);
                    }
                    s.Append(' ');
                }
                s.Append(Environment.NewLine);
            }
            return s.ToString();
        }
        
        public static void Print(WorldGrid world, Vector2Int[] path)
        {
            Console.WriteLine(PrintGrid(world));
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(PrintPath(world, path));
            
            PrintAssertions(path);
        }

        public static void PrintAssertions(Vector2Int[] path)
        {
            StringBuilder s = new StringBuilder();
            s.AppendLine("path.ShouldBe(new[] {");
            foreach (var position in path)
            {
                s.AppendLine($"new Position({position.y}, {position.x}),");
            }
            s.AppendLine("});");
            Console.WriteLine(s.ToString());
        }

        public static WorldGrid ConvertStringToPathfinderGrid(string level)
        {
            var closedCharacter = 'X';
            
            var splitLevel = level.Split('\n')
                .Select(row => row.Trim())
                .ToList();
            
            var world = new WorldGrid(splitLevel[0].Length, splitLevel.Count);

            for (var row = 0; row < splitLevel.Count; row++)
            {
                for (var column = 0; column < splitLevel[row].Length; column++)
                {
                    if (splitLevel[row][column] != closedCharacter)
                    {
                        world[column, row] = short.Parse(splitLevel[row][column].ToString());
                    }
                }
            }

            return world;
        }
    }
}
