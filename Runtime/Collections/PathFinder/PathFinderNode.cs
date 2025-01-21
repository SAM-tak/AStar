using System.Runtime.InteropServices;
using UnityEngine;

namespace AStar.Collections.PathFinder
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal readonly struct PathFinderNode
    {
        /// <summary>
        /// The position of the node
        /// </summary>
        public Vector2Int Position { get; }
        
        /// <summary>
        /// Distance from home
        /// </summary>
        public int G { get; }
        
        /// <summary>
        /// Heuristic
        /// </summary>
        public int H { get; }

        /// <summary>
        /// This nodes parent
        /// </summary>
        public Vector2Int ParentNodePosition { get; }

        /// <summary>
        /// Gone + Heuristic (H)
        /// </summary>
        public int F { get; }

        /// <summary>
        /// If the node has been considered yet
        /// </summary>
        public bool HasBeenVisited => F > 0;

        public PathFinderNode(Vector2Int position, int g, int h, Vector2Int parentNodePosition)
        {
            Position = position;
            G = g;
            H = h;
            ParentNodePosition = parentNodePosition;
            
            F = g + h;
        }
    }
}
