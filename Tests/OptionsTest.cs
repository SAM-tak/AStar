using UnityEngine;
using AStar.Options;
using NUnit.Framework;

namespace AStar.Tests
{
    [TestFixture]
    public class OptionsTest
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
        public void ShouldEnforceSearchLimit()
        {
            var pathfinder = new PathFinder(_world, new PathFinderOptions { SearchLimit = 2 });

            var path = pathfinder.FindPath(new Vector2Int(1, 1), new Vector2Int(5, 1));

            Assert.That(path, Is.Empty);
        }
    }
}