using UnityEngine;
using AStar.Heuristics;
using AStar.Options;
using NUnit.Framework;

namespace AStar.Tests
{
    [TestFixture]
    public class PunishChangeDirectionIssueTests
    {
        private WorldGrid _world;

        [SetUp]
        public void SetUp()
        {
            var level = @"  11111111111111111111
                            11111111111111111111
                            11111111111111X11111
                            11111111X1X111X11111
                            11111111X1111XXXXX11
                            11XXX111X1X11XXXXX11
                            111111111111111XXX11
                            11111111111111111111
                            11111111111111111111";

            _world = Helper.ConvertStringToPathfinderGrid(level);
        }
        
        [Test]
        public void ShouldPunishChangingDirectionsIssue()
        {
            var pathFinderOptions = new PathFinderOptions { UseDiagonals = false, PunishChangeDirection = false, HeuristicFormula = HeuristicFormula.Euclidean};
            var pathfinder = new PathFinder(_world, pathFinderOptions);

            var path = pathfinder.FindPath(new Vector2Int(2, 7), new Vector2Int(17, 1));

            Assert.That(path, Is.EquivalentTo(new[] {
                new Vector2Int(2, 7),
                new Vector2Int(3, 7),
                new Vector2Int(4, 7),
                new Vector2Int(5, 7),
                new Vector2Int(6, 7),
                new Vector2Int(7, 7),
                new Vector2Int(8, 7),
                new Vector2Int(9, 7),
                new Vector2Int(10, 7),
                new Vector2Int(11, 7),
                new Vector2Int(12, 7),
                new Vector2Int(12, 6),
                new Vector2Int(12, 5),
                new Vector2Int(12, 4),
                new Vector2Int(12, 3),
                new Vector2Int(13, 3),
                new Vector2Int(13, 2),
                new Vector2Int(13, 1),
                new Vector2Int(14, 1),
                new Vector2Int(15, 1),
                new Vector2Int(16, 1),
                new Vector2Int(17, 1),
            }));
        }

        [Test]
        public void ShouldCorrectIssue()
        {
            var pathFinderOptions = new PathFinderOptions { UseDiagonals = true, PunishChangeDirection = false};
            var pathfinder = new PathFinder(_world, pathFinderOptions);

            var path = pathfinder.FindPath(new Vector2Int(2, 1), new Vector2Int(14, 8));

            Assert.That(path, Is.EquivalentTo(new[] {
                new Vector2Int(2, 1),
                new Vector2Int(3, 2),
                new Vector2Int(4, 3),
                new Vector2Int(5, 4),
                new Vector2Int(6, 5),
                new Vector2Int(7, 6),
                new Vector2Int(8, 7),
                new Vector2Int(9, 8),
                new Vector2Int(10, 8),
                new Vector2Int(11, 8),
                new Vector2Int(12, 8),
                new Vector2Int(13, 8),
                new Vector2Int(14, 8),
            }));
        }
    }
}