using System.Linq;
using UnityEngine;
using NUnit.Framework;

namespace AStar.Tests
{
    [TestFixture]
    public class GridTests
    {
        [Test]
        public void ShouldInstantiateWithCorrectDimensions()
        {
            var grid = new WorldGrid(12, 10);

            Assert.That(grid.Height, Is.EqualTo(12));
            Assert.That(grid.Width, Is.EqualTo(10));
        }

        [Test]
        public void ShouldReadAndWriteByIndex()
        {
            var grid = new WorldGrid(2, 3)
            {
                [0, 0] = 1,
                [0, 1] = 2,
                [0, 2] = 3,
                [1, 0] = 4,
                [1, 1] = 5,
                [1, 2] = 6,
            };

            Assert.That(grid[0, 0], Is.EqualTo((short)1));
            Assert.That(grid[0, 1], Is.EqualTo((short)2));
            Assert.That(grid[0, 2], Is.EqualTo((short)3));

            Assert.That(grid[1, 0], Is.EqualTo((short)4));
            Assert.That(grid[1, 1], Is.EqualTo((short)5));
            Assert.That(grid[1, 2], Is.EqualTo((short)6));
        }
        
        [Test]
        public void ShouldInstantiateWith2DArray()
        {
            var grid = new WorldGrid(new short[,]
            {
                { 1, 2, 3 },
                { 4, 5, 6 },
                { 7, 8, 9 },
            });

            Assert.That(grid[0, 0], Is.EqualTo((short)1));
            Assert.That(grid[0, 1], Is.EqualTo((short)2));
            Assert.That(grid[0, 2], Is.EqualTo((short)3));

            Assert.That(grid[1, 0], Is.EqualTo((short)4));
            Assert.That(grid[1, 1], Is.EqualTo((short)5));
            Assert.That(grid[1, 2], Is.EqualTo((short)6));
        }

        [Test]
        public void ShouldGetCardinalSuccessorPositions()
        {
            var grid = new WorldGrid(3, 3);

            var successors = grid
                .GetSuccessorPositions(new Vector2Int(1,1))
                .ToArray();

            Assert.That(successors.Length, Is.EqualTo(4));

            Assert.That(successors[0], Is.EqualTo(new Vector2Int(0, 1)));
            Assert.That(successors[1], Is.EqualTo(new Vector2Int(1, 2)));
            Assert.That(successors[2], Is.EqualTo(new Vector2Int(2, 1)));
            Assert.That(successors[3], Is.EqualTo(new Vector2Int(1, 0)));
        }
        
        [Test]
        public void ShouldGetCardinalAndDiagonalSuccessorPositions()
        {
            var grid = new WorldGrid(3, 3);

            var successors = grid
                .GetSuccessorPositions(new Vector2Int(1, 1), true)
                .ToArray();

            Assert.That(successors.Length, Is.EqualTo(8));

            Assert.That(successors[0], Is.EqualTo(new Vector2Int(0, 1)));
            Assert.That(successors[1], Is.EqualTo(new Vector2Int(1, 2)));
            Assert.That(successors[2], Is.EqualTo(new Vector2Int(2, 1)));
            Assert.That(successors[3], Is.EqualTo(new Vector2Int(1, 0)));

            Assert.That(successors[4], Is.EqualTo(new Vector2Int(0, 2)));
            Assert.That(successors[5], Is.EqualTo(new Vector2Int(2, 2)));
            Assert.That(successors[6], Is.EqualTo(new Vector2Int(2, 0)));
            Assert.That(successors[7], Is.EqualTo(new Vector2Int(0, 0)));
        }
        
        [Test]
        public void ShouldGetSuccessorsWithoutGoingOutOfBounds()
        {
            var grid = new WorldGrid(3, 3);

            var successors = grid
                .GetSuccessorPositions(new Vector2Int(2,2), true)
                .ToArray();

            Assert.That(successors.Length, Is.EqualTo(3));

            Assert.That(successors[0], Is.EqualTo(new Vector2Int(1, 2)));
            Assert.That(successors[1], Is.EqualTo(new Vector2Int(2, 1)));
            Assert.That(successors[2], Is.EqualTo(new Vector2Int(1, 1)));
        }
    }
}