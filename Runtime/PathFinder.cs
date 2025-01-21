using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AStar.Collections.PathFinder;
using AStar.Heuristics;
using AStar.Options;

namespace AStar
{
    public class PathFinder
    {
        private const int ClosedValue = 0;
        private const int DistanceBetweenNodes = 1;
        private readonly PathFinderOptions _options;
        private readonly WorldGrid _world;
        private readonly ICalculateHeuristic _heuristic;
        private readonly Stack<Vector2Int> _path;

        public PathFinder(WorldGrid worldGrid, PathFinderOptions pathFinderOptions = null, int bufferReseveSize = 256)
        {
            _world = worldGrid ?? throw new ArgumentNullException(nameof(worldGrid));
            _options = pathFinderOptions ?? new();
            _heuristic = HeuristicFactory.Create(_options.HeuristicFormula);
            _path = new(bufferReseveSize);
        }

        /// <summary>
        /// Determines a path between 2 positions
        /// </summary>
        /// <param name="start">start/current position</param>
        /// <param name="end">target position</param>
        /// <returns>An array of positions from the start to end position or empty[] if unreachable</returns>
        public Vector2Int[] FindPath(Vector2Int start, Vector2Int end) => FindPathAsEnumerable(start, end).ToArray();

        /// <summary>
        /// Determines a path between 2 positions
        /// </summary>
        /// <param name="start">start/current position</param>
        /// <param name="end">target position</param>
        /// <returns>Enumerable object of positions from the start to end position</returns>
        public IEnumerable<Vector2Int> FindPathAsEnumerable(Vector2Int start, Vector2Int end)
        {
            var nodesVisited = 0;
            var graph = new PathFinderGraph(_world.Width, _world.Height, _options.UseDiagonals);
            var startNode = new PathFinderNode(position: start, g: 0, h: 2, parentNodePosition: start);
            graph.OpenNode(startNode);

            while (graph.HasOpenNodes)
            {
                var q = graph.GetOpenNodeWithSmallestF();
                
                if (q.Position == end)
                {
                    OrderClosedNodes(graph, q);
                    foreach(var i in _path)
                    {
                        yield return i;
                    }
                    yield break;
                }

                if (nodesVisited > _options.SearchLimit)
                {
                    yield break;
                }

                foreach (var successor in graph.GetSuccessors(q))
                {
                    if (_world[successor.Position] == ClosedValue)
                    {
                        continue;
                    }

                    var newG = q.G + DistanceBetweenNodes;

                    if (_options.PunishChangeDirection)
                    {
                        newG += CalculateModifierToG(q, successor, end);
                    }

                    var newH = _heuristic.Calculate(successor.Position, end);
                    switch (_options.Weighting)
                    {
                        case Weighting.Positive:
                            newH -= _world[successor.Position];
                            break;
                        case Weighting.Negative:
                            newH += _world[successor.Position];
                            break;
                        case Weighting.None:
                        default:
                            break;
                    }

                    var updatedSuccessor = new PathFinderNode(
                        position: successor.Position,
                        g: newG,
                        h: newH,
                        parentNodePosition: q.Position);
                    
                    if (BetterPathToSuccessorFound(updatedSuccessor, successor))
                    {
                        graph.OpenNode(updatedSuccessor);
                    }
                }

                nodesVisited++;
            }
        }

        private int CalculateModifierToG(PathFinderNode q, PathFinderNode successor, Vector2Int end)
        {
            if (q.Position == q.ParentNodePosition)
            {
                return 0;
            }
            
            var gPunishment = Math.Abs(successor.Position.y - end.y) + Math.Abs(successor.Position.x - end.x);
            
            var successorIsVerticallyAdjacentToQ = successor.Position.y - q.Position.y != 0;

            if (successorIsVerticallyAdjacentToQ)
            {
                var qIsVerticallyAdjacentToParent = q.Position.y - q.ParentNodePosition.y == 0;
                if (qIsVerticallyAdjacentToParent)
                {
                    return gPunishment;
                }
            }

            var successorIsHorizontallyAdjacentToQ = successor.Position.y - q.Position.y != 0;

            if (successorIsHorizontallyAdjacentToQ)
            {
                var qIsHorizontallyAdjacentToParent = q.Position.y - q.ParentNodePosition.y == 0;
                if (qIsHorizontallyAdjacentToParent)
                {
                    return gPunishment;
                }
            }

            if (_options.UseDiagonals)
            {
                var successorIsDiagonallyAdjacentToQ = (successor.Position.x - successor.Position.y) == (q.Position.x - q.Position.y);
                if (successorIsDiagonallyAdjacentToQ)
                {
                    var qIsDiagonallyAdjacentToParent = (q.Position.x - q.Position.y) == (q.ParentNodePosition.x - q.ParentNodePosition.y)
                                                        && IsStraightLine(q.ParentNodePosition, q.Position, successor.Position);
                    if (qIsDiagonallyAdjacentToParent)
                    {
                        return gPunishment;
                    }
                }
            }

            return 0;
        }

        private bool IsStraightLine(Vector2Int a, Vector2Int b, Vector2Int c)
            => (a.x * (b.y - c.y) + b.x * (c.y - a.y) + c.x * (a.y - b.y)) / 2 == 0; // area of triangle == 0

        private bool BetterPathToSuccessorFound(PathFinderNode updateSuccessor, PathFinderNode currentSuccessor)
            => !currentSuccessor.HasBeenVisited || (currentSuccessor.HasBeenVisited && updateSuccessor.F < currentSuccessor.F);

        private void OrderClosedNodes(IModelAGraph<PathFinderNode> graph, PathFinderNode endNode)
        {
            _path.Clear();
            var currentNode = endNode;
            while(currentNode.Position != currentNode.ParentNodePosition)
            {
                _path.Push(currentNode.Position);
                currentNode = graph.GetParent(currentNode);
            }
            _path.Push(currentNode.Position);
        }
    }
}
