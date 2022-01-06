using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021AdventOfCode
{
    /// <summary>
    /// --- Day 5: Hydrothermal Venture ---
    /// You come across a field of hydrothermal vents on the ocean floor! These vents constantly produce large, opaque clouds, so it would be best to avoid them if possible.
    /// </summary>
    public class Day05
    {
        private string _day5Input = "";

        public Day05()
        {
            _day5Input = System.IO.File.ReadAllText(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Inputs", "Day05Input.txt"));
        }
        public int Quiz1()
        {
            var linesParts = _day5Input.Split('\n').Select(x => x.Trim());
            var strightLines = linesParts.Select(x => new Line(x)).Where(x => x.IsStright).ToList();
            var res = strightLines.SelectMany(x => x.Points)
                .GroupBy(x => x)
                .Where(group => group.Count() >= 2).Count();
            return res;
        }
        public int Quiz2()
        {
            var linesParts = _day5Input.Split('\n').Select(x => x.Trim());
            var lines = linesParts.Select(x => new Line(x)).ToList();
            var res = lines.SelectMany(x => x.Points)
                .GroupBy(x => x)
                .Where(group => group.Count() >= 2).Count();
            return res;
        }
    }

    public class Coordinate : IEquatable<Coordinate>
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Coordinate()
        {

        }
        public Coordinate(string s)
        {
            var xy = s.Split(',');
            X = int.Parse(xy[0]);
            Y = int.Parse(xy[1]);
        }

        public override bool Equals(object obj)
        {
            if (obj is Coordinate)
            {
                return this.Equals((Coordinate)obj);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public bool Equals(Coordinate other)
        {
            return X == other.X &&
                   Y == other.Y;
        }
    }
    public class Line
    {
        public Coordinate Start { get; set; }
        public Coordinate End { get; set; }
        public Coordinate[] Points { get; set; }
        public bool IsStright => Start.X == End.X || Start.Y == End.Y;

        static IEnumerable<int> range(int a, int b) => (a == b)
        ? Enumerable.Repeat(a, int.MaxValue)
        : (a < b) ? Enumerable.Range(a, Math.Abs(a - b) + 1) : Enumerable.Range(b, Math.Abs(a - b) + 1).Reverse();

        public Line(string s)
        {
            var parts = s.Split(" -> ");
            Start = new Coordinate(parts[0]);
            End = new Coordinate(parts[1]);
            Points = GetAllLineCoordinate().ToArray();
        }
        private IEnumerable<Coordinate> GetAllLineCoordinate()
        {
            var iterator = Start;

            var rangeX = range(Start.X, End.X);
            var rangeY = range(Start.Y, End.Y);

            var res = range(Start.X, End.X)
                .Zip(range(Start.Y, End.Y))
                .Select(x => new Coordinate
                {
                    X = x.First,
                    Y = x.Second
                });


            return res;
        }
    }
}
