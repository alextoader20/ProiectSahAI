using System;
using System.Collections.Generic;

namespace SahLogica
{
    public class Position
    {
        public int Row { get; }
        public int Column { get; }
        public object HashCode { get; private set; }

        public Position(int row, int column)
        {
            Row = row;
            Column = column;
            HashCode = CombineHashCodes(row.GetHashCode(), column.GetHashCode());
        }

        private static int CombineHashCodes(int h1, int h2)
        {
            unchecked
            {
                return ((h1 << 5) + h1) ^ h2;
            }
        }

        public Player SquareColor()
        {
            if ((Row + Column) % 2 == 0)
            {
                return Player.Alb;
            }

            return Player.Negru;
        }

        public override bool Equals(object obj)
        {
            return obj is Position position &&
                   Row == position.Row &&
                   Column == position.Column;
        }

        public override int GetHashCode()
        {
            return HashCode.GetHashCode();
        }

        public static bool operator ==(Position left, Position right)
        {
            return EqualityComparer<Position>.Default.Equals(left, right);
        }

        public static bool operator !=(Position left, Position right)
        {
            return !(left == right);
        }

        public static Position operator +(Position pos, Direction dir)
        {
            return new Position(pos.Row + dir.RowDelta, pos.Column + dir.ColumnDelta);
        }
    }
}