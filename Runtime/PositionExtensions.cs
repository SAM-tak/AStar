using System.Drawing;
using UnityEngine;

namespace AStar
{
    public static class PositionExtensions
    {
        public static Point ToPoint(this Position position)
        {
            return new Point(position.Column, position.Row);
        }

        public static Vector2Int ToVector2Int(this Position position)
        {
            return new Vector2Int(position.Column, position.Row);
        }

        public static Position ToPosition(this Point point)
        {
            return new Position(point.Y, point.X);
        }

        public static Position ToPosition(this Vector2Int point)
        {
            return new Position(point.y, point.x);
        }
    }
}