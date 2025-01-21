using System.Collections.Generic;
using UnityEngine;

namespace AStar.Collections.MultiDimensional
{
    public interface IModelAGrid<T>
    {
        int Height { get; }
        int Width { get; }
        T this[int row, int column] { get; set; }
        T this[Vector2Int position] { get; set; }
        IEnumerable<Vector2Int> GetSuccessorPositions(Vector2Int node, bool optionsUseDiagonals = false);
    }
}