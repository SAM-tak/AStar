using UnityEngine;
using AStar.Options;
using NUnit.Framework;

namespace AStar.Tests
{
    [TestFixture]
    public class PathfinderTests
    {
        private WorldGrid _world;
        private PathFinder _pathFinder;

        [SetUp]
        public void SetUp()
        {
            _world = CreateGridInitializedToOpen(8, 8);
            _pathFinder = new PathFinder(_world);
        }

        [Test]
        public void ShouldPathRectangleGrid()
        {
            var grid = CreateGridInitializedToOpen(5, 3);
            var pathfinder = new PathFinder(grid);

            var path = pathfinder.FindPath(new Vector2Int(0, 0), new Vector2Int(4, 2));

            Assert.That(path, Is.EquivalentTo(new[] {
                new Vector2Int(0, 0),
                new Vector2Int(1, 1),
                new Vector2Int(2, 2),
                new Vector2Int(3, 2),
                new Vector2Int(4, 2),
            }));
        }

        [Test]
        public void ShouldPathToSelf()
        {
            var path = _pathFinder.FindPath(new Vector2Int(1, 1), new Vector2Int(1, 1));

            Assert.That(path, Is.EquivalentTo(new[] {
                new Vector2Int(1, 1),
            }));
        }

        [Test]
        public void ShouldPathToAdjacent()
        {
            var path = _pathFinder.FindPath(new Vector2Int(1, 1), new Vector2Int(1, 2));

            Assert.That(path, Is.EquivalentTo(new[] {
                new Vector2Int(1, 1),
                new Vector2Int(1, 2),
            }));
        }

        [Test]
        public void ShouldDoSimplePath()
        {
            var path = _pathFinder.FindPath(new Vector2Int(1, 1), new Vector2Int(2, 4));

            Assert.That(path, Is.EquivalentTo(new[] {
                new Vector2Int(1, 1),
                new Vector2Int(2, 2),
                new Vector2Int(2, 3),
                new Vector2Int(2, 4),
            }));
        }

        [Test]
        public void ShouldDoSimplePathWithNoDiagonal()
        {
            var pathfinderOptions = new PathFinderOptions { UseDiagonals = false };
            _pathFinder = new PathFinder(_world, pathfinderOptions);

            var path = _pathFinder.FindPath(new Vector2Int(1, 1), new Vector2Int(2, 4));

            Assert.That(path, Is.EquivalentTo(new[] {
                new Vector2Int(1, 1),
                new Vector2Int(1, 2),
                new Vector2Int(1, 3),
                new Vector2Int(1, 4),
                new Vector2Int(2, 4),
            }));
        }

        [Test]
        public void ShouldDoSimplePathWithNoDiagonalAroundObstacle()
        {
            var pathfinderOptions = new PathFinderOptions { UseDiagonals = false };
            _pathFinder = new PathFinder(_world, pathfinderOptions);

            _world[0, 2] = 0;
            _world[1, 2] = 0;
            _world[2, 2] = 0;

            var path = _pathFinder.FindPath(new Vector2Int(1, 1), new Vector2Int(2, 4));

            Assert.That(path, Is.EquivalentTo(new[] {
                new Vector2Int(1, 1),
                new Vector2Int(2, 1),
                new Vector2Int(3, 1),
                new Vector2Int(3, 2),
                new Vector2Int(3, 3),
                new Vector2Int(2, 3),
                new Vector2Int(2, 4),
            }));
        }
        [Test]
        public void ShouldPathAroundObstacle()
        {
            _world[0, 2] = 0;
            _world[1, 2] = 0;
            _world[2, 2] = 0;
            _world[3, 2] = 0;

            var path = _pathFinder.FindPath(new Vector2Int(1, 1), new Vector2Int(2, 4));

            Assert.That(path, Is.EquivalentTo(new[] {
                new Vector2Int(1, 1),
                new Vector2Int(2, 1),
                new Vector2Int(3, 1),
                new Vector2Int(4, 2),
                new Vector2Int(3, 3),
                new Vector2Int(2, 4),
            }));
        }

        [Test]
        public void ShouldReturnEmptyPathIfUnreachable()
        {
            _world[0, 2] = 0;
            _world[1, 2] = 0;
            _world[2, 2] = 0;
            _world[3, 2] = 0;
            _world[4, 2] = 0;
            _world[5, 2] = 0;
            _world[6, 2] = 0;
            _world[7, 2] = 0;
            var path = _pathFinder.FindPath(new Vector2Int(1, 1), new Vector2Int(2, 4));
            Assert.That(path, Is.Empty);
        }

        private static WorldGrid CreateGridInitializedToOpen(int width, int height)
        {
            var grid = new WorldGrid(width, height);

            for (var row = 0; row < grid.Height; row++)
            {
                for (var column = 0; column < grid.Width; column++)
                {
                    grid[column, row] = 1;
                }
            }

            return grid;
        }
    }
}