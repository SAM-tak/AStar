using System;
using UnityEngine;

namespace AStar.Heuristics
{
    public class Manhattan : ICalculateHeuristic
    {
        public int Calculate(Vector2Int source, Vector2Int destination)
        {
            var heuristicEstimate = 2;
            var h = heuristicEstimate * (Math.Abs(source.y - destination.y) + Math.Abs(source.x - destination.x));
            return h;
        }
    }
}