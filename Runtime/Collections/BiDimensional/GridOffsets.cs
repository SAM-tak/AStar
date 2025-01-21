using System.Collections.Generic;
using System.Linq;

namespace AStar.Collections.BiDimensional
{
    public static class GridOffsets
    {
        private static IEnumerable<(sbyte column, sbyte row)> CardinalDirectionOffsets
        {
            get
            {
                yield return (-1, 0);
                yield return (0, 1);
                yield return (1, 0);
                yield return (0, -1);
            }
        }

        private static IEnumerable<(sbyte column, sbyte row)> DiagonalsOffsets
        {
            get
            {
                yield return (-1, 1);
                yield return (1, 1);
                yield return (1, -1);
                yield return (-1, -1);
            }
        }

        public static IEnumerable<(sbyte column, sbyte row)> GetOffsets(bool withDiagonals = false)
        {
            return withDiagonals 
                ? CardinalDirectionOffsets.Concat(DiagonalsOffsets) 
                : CardinalDirectionOffsets;
        }

        public static bool IsCardinalOffset((sbyte column, sbyte row) offset)
        {
            return offset.column != 0 && offset.row != 0;
        }

        public static bool IsDiagonal((sbyte column, sbyte row) offset)
        {
            return offset.column != 0 || offset.row != 0;
        }
    }
}