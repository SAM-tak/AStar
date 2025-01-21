using System;
using UnityEngine;

namespace AStar.Heuristics
{
    public class Euclidean : ICalculateHeuristic
    {
        public int Calculate(Vector2Int source, Vector2Int destination)
        {
            var heuristicEstimate = 2;
            var h = (int)(heuristicEstimate * Math.Sqrt(Math.Pow(source.y - destination.y, 2) + Math.Pow(source.x - destination.x, 2)));
            return h;
        }
    }
}