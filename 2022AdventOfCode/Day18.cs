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
            var externalVolume = new Cube();
            foreach (var lavaDrop in droplets)
            {
                var setOfAdjacentPoints = lavaDrop.GetAdjacentPoints();
                var intersection = droplets.Intersect(setOfAdjacentPoints);
                freeFaces += setOfAdjacentPoints.Count - intersection.Count();
                externalVolume.MinX = Math.Min(lavaDrop.X, externalVolume.MinX);
                externalVolume.MaxX = Math.Max(lavaDrop.X, externalVolume.MaxX);
                externalVolume.MinY = Math.Min(lavaDrop.Y, externalVolume.MinY);
                externalVolume.MaxY = Math.Max(lavaDrop.Y, externalVolume.MaxY);
                externalVolume.MinZ = Math.Min(lavaDrop.Z, externalVolume.MinZ);
                externalVolume.MaxZ = Math.Max(lavaDrop.Z, externalVolume.MaxZ);
            }
            var cubesToOutside = new HashSet<LavaPoint>();
            var cubesInside = new HashSet<LavaPoint>();

            int faceToOutside = 0;
            foreach (var lavaDrop in droplets)
            {
                var setOfAdjacentPoints = lavaDrop.GetAdjacentPoints();
                foreach (var adjacent in setOfAdjacentPoints)
                {
                    if(HasPathToOutside(externalVolume, adjacent, droplets, cubesToOutside, cubesInside))
                    {
                        cubesToOutside.Add(adjacent);
                        faceToOutside++;
                    }
                    else
                    {
                        if (!droplets.Contains(adjacent))
                        {
                            cubesInside.Add(adjacent);
                        }
                    }
                }
            }
            return new ValueTask<string>(faceToOutside.ToString());
        }

        private bool HasPathToOutside(Cube externalVolume, LavaPoint c, HashSet<LavaPoint> droplets, HashSet<LavaPoint> airOutside, HashSet<LavaPoint> airInsede)
        {
            var frontier = new Queue<LavaPoint>();
            frontier.Enqueue(c);
            var explored = new HashSet<LavaPoint>() { c };
            while (frontier.Count > 0)
            {
                var currentCube = frontier.Dequeue();
                if (airOutside.Contains(currentCube))
                    return true;
                if (airInsede.Contains(currentCube))
                    continue;
                if (droplets.Contains(currentCube))
                    continue;

                if (currentCube.X > externalVolume.MaxX || currentCube.Y > externalVolume.MaxY || currentCube.Z > externalVolume.MaxZ)
                    return true;
                if (currentCube.X < externalVolume.MinX || currentCube.Y < externalVolume.MinY || currentCube.Z < externalVolume.MinZ)
                    return true;


                var neighbours = c.GetAdjacentPoints();
                foreach (var neighbour in neighbours)
                {
                    if (!explored.Contains(neighbour))
                    {
                        frontier.Enqueue(neighbour);
                        explored.Add(neighbour);
                    }
                }
            }
            return false;
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

    public class Cube
    {
        public int MinX { get; set; }
        public int MinY { get; set; }
        public int MinZ { get; set; }
        public int MaxX { get; set; }
        public int MaxY { get; set; }
        public int MaxZ { get; set; }
    }
}
