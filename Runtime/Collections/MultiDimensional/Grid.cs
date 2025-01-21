using System;
using System.Collections.Generic;
using UnityEngine;

namespace AStar.Collections.MultiDimensional
{
    public class Grid<T> : IModelAGrid<T>
    {
        private readonly T[] _grid;

        public Grid(int height, int width)
        {
            if (width <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(width));
            }

            if (height <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(height));
            }
            
            Width = width;
            Height = height;

            _grid = new T[width * height];
        }

        public int Width { get; }

        public int Height { get; }

        public IEnumerable<Vector2Int> GetSuccessorPositions(Vector2Int node, bool optionsUseDiagonals = false)
        {
            var offsets = GridOffsets.GetOffsets(optionsUseDiagonals);
            foreach (var (row, column) in offsets)
            {
                var successorRow = node.y + row;
                
                if (successorRow < 0 || successorRow >= Height)
                {
                    continue;
                    
                }
                
                var successorColumn = node.x + column;

                if (successorColumn < 0 || successorColumn >= Width)
                {
                    continue;
                }
                
                yield return new Vector2Int(successorColumn, successorRow);
            }
        }

        public T this[Vector2Int position]
        {
            get
            {
                return _grid[ConvertRowColumnToIndex(position.y, position.x)];
            }
            set
            {
                _grid[ConvertRowColumnToIndex(position.y, position.x)] = value;
            }
        }

        public T this[int row, int column]
        {
            get
            {
                return _grid[ConvertRowColumnToIndex(row, column)];
            }
            set
            {
                _grid[ConvertRowColumnToIndex(row, column)] = value;
            }
        }

        private int ConvertRowColumnToIndex(int row, int column)
        {
            return Width * row + column;
        }
    }
}