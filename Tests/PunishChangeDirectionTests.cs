using System;
using UnityEngine;
using AStar.Heuristics;
using AStar.Options;
using NUnit.Framework;

namespace AStar.Tests
{
    [TestFixture]
    public class PunishChangeDirectionTests
    {
        [Test]
        public void ShouldPunishChangingDirections()
        {
            var level = @"  111111111X
                            111111111X
                            11111X111X
                            11111X111X
                            11111X111X
                            111111111X
                            111111111X
                            111111111X
                            111XXX111X
                            111111111X
                            111X1X111X
                            111111111X
                            111111111X
                            1111XX111X
                            11XXXX111X
                            1111XXX11X
                            1111XXX11X
                            1111XXX11X
                            111111111X
                            111111111X";

            var world = Helper.ConvertStringToPathfinderGrid(level);

            var pathFinderOptions = new PathFinderOptions { UseDiagonals = true, PunishChangeDirection = true};
            var pathfinder = new PathFinder(world, pathFinderOptions);

            var path = pathfinder.FindPath(new Vector2Int(9, 2), new Vector2Int(3, 15));

            Assert.That(path, Is.EquivalentTo(new[]
            {
                new Vector2Int(9, 2),
                new Vector2Int(8, 3),
                new Vector2Int(7, 4),
                new Vector2Int(6, 5),
                new Vector2Int(5, 6),
                new Vector2Int(5, 7),
                new Vector2Int(6, 8),
                new Vector2Int(5, 9),
                new Vector2Int(4, 10),
                new Vector2Int(3, 11),
                new Vector2Int(3, 12),
                new Vector2Int(2, 13),
                new Vector2Int(1, 14),
                new Vector2Int(2, 15),
                new Vector2Int(3, 15),
            }));
        }

        [Test]
        public void ShouldCalculateAdjacentCorrectly()
        {
            var level = @"  110111
                            110111
                            100111
                            111111
                            101111
                            111111";

            var world = Helper.ConvertStringToPathfinderGrid(level);
            var pathfinder = new PathFinder(world, new PathFinderOptions { UseDiagonals = false, PunishChangeDirection = true, HeuristicFormula = HeuristicFormula.MaxDXDY });

            var path = pathfinder.FindPath(new Vector2Int(4, 4), new Vector2Int(1, 1));

            var expected = new[]
            {
                new Vector2Int(4, 4),
                new Vector2Int(4, 3),
                new Vector2Int(3, 3),
                new Vector2Int(2, 3),
                new Vector2Int(1, 3),
                new Vector2Int(0, 3),
                new Vector2Int(0, 2),
                new Vector2Int(0, 1),
                new Vector2Int(1, 1),
            };

            Console.WriteLine("actual");
            Console.WriteLine("expected");

            Assert.That(path, Is.EqualTo(expected));
        }
    }
}