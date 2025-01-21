using System;
using System.Collections.Generic;
using System.Drawing;
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
        private readonly Stack<Position> _path = new(256);

        public PathFinder(WorldGrid worldGrid, PathFinderOptions pathFinderOptions = null)
        {
            _world = worldGrid ?? throw new ArgumentNullException(nameof(worldGrid));
            _options = pathFinderOptions ?? new PathFinderOptions();
            _heuristic = HeuristicFactory.Create(_options.HeuristicFormula);
        }

        /// <summary>
        /// Determines a path between 2 positions where the point's X
        /// represents the column and the point's Y represents the row
        /// </summary>
        /// <param name="start">start position</param>
        /// <param name="end">target position</param>
        /// <returns>An array of points from the start to end points or empty[] if unreachable</returns>
        public Vector2Int[] FindPath(Vector2Int start, Vector2Int end)
            => FindPathAsEnumerable(start.ToPosition(), end.ToPosition())
                .Select(position => position.ToVector2Int())
                .ToArray();

        /// <summary>
        /// Determines a path between 2 positions where the point's X
        /// represents the column and the point's Y represents the row
        /// </summary>
        /// <param name="start">start position</param>
        /// <param name="end">target position</param>
        /// <returns>An array of points from the start to end points or empty[] if unreachable</returns>
        public Point[] FindPath(Point start, Point end)
            => FindPathAsEnumerable(start.ToPosition(), end.ToPosition())
                .Select(position => position.ToPoint())
                .ToArray();

        /// <summary>
        /// Determines a path between 2 positions
        /// </summary>
        /// <param name="start">start/current position</param>
        /// <param name="end">target position</param>
        /// <returns>An array of positions from the start to end position or empty[] if unreachable</returns>
        public Position[] FindPath(Position start, Position end) => FindPathAsEnumerable(start, end).ToArray();

        /// <summary>
        /// Determines a path between 2 positions
        /// </summary>
        /// <param name="start">start/current position</param>
        /// <param name="end">target position</param>
        /// <returns>Enumerable object of positions from the start to end position</returns>
        public IEnumerable<Position> FindPathAsEnumerable(Position start, Position end)
        {
            var nodesVisited = 0;
            IModelAGraph<PathFinderNode> graph = new PathFinderGraph(_world.Height, _world.Width, _options.UseDiagonals);

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

        private int CalculateModifierToG(PathFinderNode q, PathFinderNode successor, Position end)
        {
            if (q.Position == q.ParentNodePosition)
            {
                return 0;
            }
            
            var gPunishment = Math.Abs(successor.Position.Row - end.Row) + Math.Abs(successor.Position.Column - end.Column);
            
            var successorIsVerticallyAdjacentToQ = successor.Position.Row - q.Position.Row != 0;

            if (successorIsVerticallyAdjacentToQ)
            {
                var qIsVerticallyAdjacentToParent = q.Position.Row - q.ParentNodePosition.Row == 0;
                if (qIsVerticallyAdjacentToParent)
                {
                    return gPunishment;
                }
            }

            var successorIsHorizontallyAdjacentToQ = successor.Position.Row - q.Position.Row != 0;

            if (successorIsHorizontallyAdjacentToQ)
            {
                var qIsHorizontallyAdjacentToParent = q.Position.Row - q.ParentNodePosition.Row == 0;
                if (qIsHorizontallyAdjacentToParent)
                {
                    return gPunishment;
                }
            }

            if (_options.UseDiagonals)
            {
                var successorIsDiagonallyAdjacentToQ = (successor.Position.Column - successor.Position.Row) == (q.Position.Column - q.Position.Row);
                if (successorIsDiagonallyAdjacentToQ)
                {
                    var qIsDiagonallyAdjacentToParent = (q.Position.Column - q.Position.Row) == (q.ParentNodePosition.Column - q.ParentNodePosition.Row)
                                                        && IsStraightLine(q.ParentNodePosition, q.Position, successor.Position);
                    if (qIsDiagonallyAdjacentToParent)
                    {
                        return gPunishment;
                    }
                }
            }

            return 0;
        }

        private bool IsStraightLine(Position a, Position b, Position c)
        {
            // area of triangle == 0
            return (a.Column * (b.Row - c.Row) + b.Column * (c.Row - a.Row) + c.Column * (a.Row - b.Row)) / 2 == 0;
        }

        private bool BetterPathToSuccessorFound(PathFinderNode updateSuccessor, PathFinderNode currentSuccessor)
        {
            return !currentSuccessor.HasBeenVisited ||
                (currentSuccessor.HasBeenVisited && updateSuccessor.F < currentSuccessor.F);
        }

        private void OrderClosedNodes(IModelAGraph<PathFinderNode> graph, PathFinderNode endNode)
        {
            _path.Clear();
            var currentNode = endNode;

            while (currentNode.Position != currentNode.ParentNodePosition)
            {
                _path.Push(currentNode.Position);
                currentNode = graph.GetParent(currentNode);
            }

            _path.Push(currentNode.Position);
        }
    }
}
