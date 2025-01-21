using UnityEngine;
using AStar.Options;
using NUnit.Framework;

namespace AStar.Tests
{
    [TestFixture]
    public class PathingTests
    {
        private WorldGrid _world;

        [SetUp]
        public void SetUp()
        {
            var level = @"XXXXXXX
                          X11X11X
                          X11111X
                          XXXXXXX";

            _world = Helper.ConvertStringToPathfinderGrid(level);
        }

        [Test]
        public void ShouldPathPredictably()
        {
            var pathfinder = new PathFinder(_world);

            var path = pathfinder.FindPath(new Vector2Int(1, 1), new Vector2Int(3, 2));

            Assert.That(path, Is.EquivalentTo(new[] {
                new Vector2Int(1, 1),
                new Vector2Int(2, 2),
                new Vector2Int(3, 2),
            }));
        }

        [Test]
        public void ShouldPathPredictably2()
        {
            var pathfinder = new PathFinder(_world);

            var path = pathfinder.FindPath(new Vector2Int(1, 1), new Vector2Int(5, 1));

            Assert.That(path, Is.EquivalentTo(new[] {
                new Vector2Int(1, 1),
                new Vector2Int(2, 1),
                new Vector2Int(3, 2),
                new Vector2Int(4, 1),
                new Vector2Int(5, 1),
            }));

        }

        [Test]
        public void ShouldPathPredictably3()
        {
            var pathfinder = new PathFinder(_world, new PathFinderOptions { UseDiagonals = false });

            var path = pathfinder.FindPath(new Vector2Int(1, 1), new Vector2Int(5, 1));

            Assert.That(path, Is.EquivalentTo(new[] {
                new Vector2Int(1, 1),
                new Vector2Int(2, 1),
                new Vector2Int(2, 2),
                new Vector2Int(3, 2),
                new Vector2Int(4, 2),
                new Vector2Int(5, 2),
                new Vector2Int(5, 1),
            }));
        }
    }
}