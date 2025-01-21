using System.Collections.Generic;
using UnityEngine;

namespace AStar.Collections.MultiDimensional
{
    public interface IModelAGrid<T>
    {
        int Width { get; }
        int Height { get; }
        /// <summary>
        /// Unlike standard C# arrays, note that the order is columns and rows.
        /// </summary>
        /// <param name="x">x index</param>
        /// <param name="y">y index</param>
        /// <returns></returns>
        T this[int x, int y] { get; set; }
        T this[Vector2Int position] { get; set; }
        IEnumerable<Vector2Int> GetSuccessorPositions(Vector2Int node, bool optionsUseDiagonals = false);
    }
}