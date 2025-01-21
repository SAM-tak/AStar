using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AStar.Collections.MultiDimensional;
using AStar.Collections.PriorityQueue;

namespace AStar.Collections.PathFinder
{
    internal class PathFinderGraph : IModelAGraph<PathFinderNode>
    {
        private readonly bool _allowDiagonalTraversal;
        private readonly Grid<PathFinderNode> _internalGrid;
        private readonly SimplePriorityQueue<PathFinderNode> _open = new(new ComparePathFinderNodeByFValue());

        public bool HasOpenNodes => _open.Count > 0;

        public PathFinderGraph(int width, int height, bool allowDiagonalTraversal)
        {
            _allowDiagonalTraversal = allowDiagonalTraversal;
            _internalGrid = new Grid<PathFinderNode>(width, height);
            Initialise();
        }

        private void Initialise()
        {
            for (var y = 0; y < _internalGrid.Height; y++)
            {
                for (var x = 0; x < _internalGrid.Width; x++)
                {
                    _internalGrid[x, y] = new PathFinderNode(position: new Vector2Int(x, y),
                        g: 0,
                        h: 0,
                        parentNodePosition: default);
                }
            }
            
            _open.Clear();
        }

        public IEnumerable<PathFinderNode> GetSuccessors(PathFinderNode node)
        {
            return _internalGrid
                .GetSuccessorPositions(node.Position, _allowDiagonalTraversal)
                .Select(successorPosition => _internalGrid[successorPosition]);
        }

        public PathFinderNode GetParent(PathFinderNode node)
        {
            return _internalGrid[node.ParentNodePosition];
        }

        public void OpenNode(PathFinderNode node)
        {
            _internalGrid[node.Position] = node;
            _open.Push(node);
        }

        public PathFinderNode GetOpenNodeWithSmallestF()
        {
            return _open.Pop();
        }
    }
}