using UnityEngine;

namespace AStar.Heuristics
{
    public interface ICalculateHeuristic
    {
        int Calculate(Vector2Int source, Vector2Int destination);
    }
}