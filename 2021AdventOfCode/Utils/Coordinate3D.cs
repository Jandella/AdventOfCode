using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021AdventOfCode.Utils
{
    public class Coordinate3D : IEquatable<Coordinate3D>, IComparable<Coordinate3D>
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public Coordinate3D()
        {

        }
        public Coordinate3D(string coordinates)
        {
            var splitted = coordinates.Split(",");
            X = int.Parse(splitted[0]);
            Y = int.Parse(splitted[1]);
            Z = int.Parse(splitted[2]);
        }
        public int CompareTo(Coordinate3D other)
        {
            if (X < other.X) return -1;
            if (X > other.X) return 1;

            if (Y < other.Y) return -1;
            if (Y > other.Y) return 1;

            if (Z < other.Z) return -1;
            if (Z > other.Z) return 1;

            return 0;
        }

        public override bool Equals(object obj)
        {
            return obj is Coordinate3D d &&
                   this.Equals(d);
        }

        public bool Equals(Coordinate3D other)
        {
            return X == other.X &&
                   Y == other.Y &&
                   Z == other.Z;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }
        public static Coordinate3D operator +(Coordinate3D a, Coordinate3D b)
        {
            return new Coordinate3D()
            {
                X = a.X + b.X,
                Y = a.Y + b.Y,
                Z = a.Z + b.Z
            };
        }
        public static Coordinate3D operator -(Coordinate3D a, Coordinate3D b)
        {
            return new Coordinate3D()
            {
                X = a.X - b.X,
                Y = a.Y - b.Y,
                Z = a.Z - b.Z
            };
        }

        public override string ToString()
        {
            return $"<{X},{Y},{Z}>";
        }
    }
}
