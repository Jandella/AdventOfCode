using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021AdventOfCode
{
    public class RowCol
    {
        public int Row { get; set; }
        public int Col { get; set; }

        public override bool Equals(object obj)
        {
            return obj is RowCol col &&
                   Row == col.Row &&
                   Col == col.Col;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Col);
        }
        public override string ToString()
        {
            return $"({Row},{Col})";
        }
    }
}
