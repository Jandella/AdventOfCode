using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2022AdventOfCode
{
    public class Day18 : AoCHelper.BaseDay
    {
        private string _input;
        public Day18()
        {
            _input = System.IO.File.ReadAllText(InputFilePath);
        }
        public Day18(string input)
        {
            _input = input;
        }

        public override ValueTask<string> Solve_1()
        {
            var droplets = new HashSet<LavaPoint>();
            using (var sr = new StringReader(_input))
            {
                while (sr.Peek() != -1)
                {
                    var line = sr.ReadLine() ?? string.Empty;
                    droplets.Add(new LavaPoint(line));
                }
            }
            var freeFaces = 0;
            foreach (var lavaDrop in droplets)
            {
                var setOfAdjacentPoints = lavaDrop.GetAdjacentPoints();
                var intersection = droplets.Intersect(setOfAdjacentPoints);
                freeFaces += setOfAdjacentPoints.Count - intersection.Count();
            }
            return new ValueTask<string>(freeFaces.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            throw new NotImplementedException();
        }
    }

    public class LavaPoint
    {
        public LavaPoint()
        {

        }
        public LavaPoint(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public LavaPoint(string input)
        {
            var splitted = input.Split(',');
            X = int.Parse(splitted[0]);
            Y = int.Parse(splitted[1]);
            Z = int.Parse(splitted[2]);
        }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is LavaPoint point &&
                   X == point.X &&
                   Y == point.Y &&
                   Z == point.Z;
        }
        public HashSet<LavaPoint> GetAdjacentPoints()
        {
            var faces = new HashSet<LavaPoint>();
            foreach (var item in FaceMoves)
            {
                faces.Add(this + item);
            }
            return faces;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }
        public override string ToString()
        {
            return string.Format("{0},{1},{2}", X, Y, Z);
        }
        public static LavaPoint operator +(LavaPoint a, LavaPoint b) => new LavaPoint(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static LavaPoint operator -(LavaPoint a, LavaPoint b) => new LavaPoint(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        public static LavaPoint[] FaceMoves = {
            new(-1,0,0), // left face
            new(1,0,0), //right face
            new(0,-1,0), // bottom face
            new(0,1,0), // top face
            new(0,0,-1), //behind face
            new(0,0,1) //front face
        };
    }
}
