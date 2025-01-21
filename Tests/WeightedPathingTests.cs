using UnityEngine;
using AStar.Options;
using NUnit.Framework;

namespace AStar.Tests
{
    [TestFixture]
    public class WeightedPathingTests
    {
        [Test]
        public void ShouldPathWithWeight()
        {
            var level = @"1111115
                          1511151
                          1155511
                          1111111";
            var world = Helper.ConvertStringToPathfinderGrid(level);
            var opts = new PathFinderOptions { Weighting = Weighting.Positive };
            var pathfinder = new PathFinder(world, opts);

            var path = pathfinder.FindPath(new Vector2Int(1, 1), new Vector2Int(5, 1));

            Assert.That(path, Is.EquivalentTo(new[] {
                new Vector2Int(1, 1),
                new Vector2Int(2, 2),
                new Vector2Int(3, 2),
                new Vector2Int(4, 2),
                new Vector2Int(5, 1),
            }));
        }

        [Test]
        public void ShouldPathWithInvertedWeight()
        {
            var level = @"9999995
                          9599959
                          9955599
                          9999999";

            var world = Helper.ConvertStringToPathfinderGrid(level);
            var opts = new PathFinderOptions { Weighting = Weighting.Negative };
            var pathfinder = new PathFinder(world, opts);

            var path = pathfinder.FindPath(new Vector2Int(1, 1), new Vector2Int(5, 1));

            Assert.That(path, Is.EquivalentTo(new[] {
                new Vector2Int(1, 1),
                new Vector2Int(2, 2),
                new Vector2Int(3, 2),
                new Vector2Int(4, 2),
                new Vector2Int(5, 1),
            }));
        }
    }
}