using UnityEngine;
using AStar.Options;
using NUnit.Framework;

namespace AStar.Tests
{
    [TestFixture]
    public class LongerPathingTests
    {
        private WorldGrid _world;

        [SetUp]
        public void SetUp()
        {
            var level = @"XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                          X111XXXX11111111111111111111111X
                          X111XXXX11111111111111111111111X
                          X111XXXX11111111111111111111111X
                          X111XXXX11111111111111111111111X
                          X111XXXX11111111111111111111111X
                          X111XXXX11111111111111111111111X
                          X111XXXX11111111111111111111111X
                          X111111111111111111111111111111X
                          X111111111111111111111111111111X
                          X111111111111111111111111111111X
                          XXXXXXXXXXXXXXXXXXXXXX111111111X
                          XXXXXXXXXXXXXXXXXXXXXX111111111X
                          XXXXXXXXXXXXXXXXXXXXXX111111111X
                          X1111111111111111XXXXX111111111X
                          X1111111111111111XXXXX111111111X
                          X1111111111111111XXXXX111111111X
                          X1111111111111111XXXXX111111111X
                          X1111111111111111XXXXX111111111X
                          X1111111111111111XXXXX111111111X
                          X1111111111111111XXXXX111111111X
                          X1111111111111111XXXXX111111111X
                          X1111111111111111XXXXX111111111X
                          X1111111111111111XXXXX111111111X
                          X1111111111111111XXXXX111111111X
                          X1111111111111111XXXXX111111111X
                          X111111111111111111111111111111X
                          X111111111111111111111111111111X
                          X111111XXXXXXXXXXXXXXXXXXXXXXXXX
                          X111111XXXXXXXXXXXXXXXXXXXXXXXXX
                          X111111111111111111111111111111X
                          XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";

            _world = Helper.ConvertStringToPathfinderGrid(level);
        }

        [Test]
        public void TestPathingOptions()
        {
            var pathfinderOptions = new PathFinderOptions { PunishChangeDirection = true };

            var pathfinder = new PathFinder(_world, pathfinderOptions);
            var path = pathfinder.FindPath(new Vector2Int(1, 1), new Vector2Int(30, 30));
            
        }

        [Test]
        public void ShouldPathEnvironment()
        {
            var pathfinder = new PathFinder(_world);
            
            var path = pathfinder.FindPath(new Vector2Int(1, 1), new Vector2Int(30, 30));

            Assert.That(path, Is.EquivalentTo(new[] {
                new Vector2Int(1, 1),
                new Vector2Int(2, 2),
                new Vector2Int(3, 3),
                new Vector2Int(3, 4),
                new Vector2Int(3, 5),
                new Vector2Int(3, 6),
                new Vector2Int(3, 7),
                new Vector2Int(4, 8),
                new Vector2Int(5, 9),
                new Vector2Int(6, 10),
                new Vector2Int(7, 10),
                new Vector2Int(8, 10),
                new Vector2Int(9, 10),
                new Vector2Int(10, 10),
                new Vector2Int(11, 10),
                new Vector2Int(12, 10),
                new Vector2Int(13, 10),
                new Vector2Int(14, 10),
                new Vector2Int(15, 10),
                new Vector2Int(16, 10),
                new Vector2Int(17, 10),
                new Vector2Int(18, 10),
                new Vector2Int(19, 10),
                new Vector2Int(20, 10),
                new Vector2Int(21, 10),
                new Vector2Int(22, 11),
                new Vector2Int(23, 12),
                new Vector2Int(24, 13),
                new Vector2Int(25, 14),
                new Vector2Int(26, 15),
                new Vector2Int(27, 16),
                new Vector2Int(28, 17),
                new Vector2Int(29, 18),
                new Vector2Int(28, 19),
                new Vector2Int(27, 20),
                new Vector2Int(26, 21),
                new Vector2Int(25, 22),
                new Vector2Int(24, 23),
                new Vector2Int(23, 24),
                new Vector2Int(22, 25),
                new Vector2Int(21, 26),
                new Vector2Int(20, 27),
                new Vector2Int(19, 27),
                new Vector2Int(18, 27),
                new Vector2Int(17, 27),
                new Vector2Int(16, 27),
                new Vector2Int(15, 27),
                new Vector2Int(14, 27),
                new Vector2Int(13, 27),
                new Vector2Int(12, 27),
                new Vector2Int(11, 27),
                new Vector2Int(10, 27),
                new Vector2Int(9, 27),
                new Vector2Int(8, 27),
                new Vector2Int(7, 27),
                new Vector2Int(6, 28),
                new Vector2Int(6, 29),
                new Vector2Int(7, 30),
                new Vector2Int(8, 30),
                new Vector2Int(9, 30),
                new Vector2Int(10, 30),
                new Vector2Int(11, 30),
                new Vector2Int(12, 30),
                new Vector2Int(13, 30),
                new Vector2Int(14, 30),
                new Vector2Int(15, 30),
                new Vector2Int(16, 30),
                new Vector2Int(17, 30),
                new Vector2Int(18, 30),
                new Vector2Int(19, 30),
                new Vector2Int(20, 30),
                new Vector2Int(21, 30),
                new Vector2Int(22, 30),
                new Vector2Int(23, 30),
                new Vector2Int(24, 30),
                new Vector2Int(25, 30),
                new Vector2Int(26, 30),
                new Vector2Int(27, 30),
                new Vector2Int(28, 30),
                new Vector2Int(29, 30),
                new Vector2Int(30, 30),
            }));
        }
    }
}
