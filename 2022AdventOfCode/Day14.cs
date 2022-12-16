using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace _2022AdventOfCode
{
    public class Day14 : AoCHelper.BaseDay
    {
        private string _input;
        public Day14()
        {
            _input = System.IO.File.ReadAllText(InputFilePath);
        }
        public Day14(string input)
        {
            _input = input;
        }
        //dictionary key => x
        //set ints => y
        public Dictionary<int, SortedSet<int>> ParseScan()
        {
            var res = new Dictionary<int, SortedSet<int>>();
            using (StringReader r = new StringReader(_input))
            {
                while (r.Peek() != -1)
                {
                    string line = r.ReadLine() ?? "";
                    var splitted = line.Split("->").Select(x => x.Trim()).ToArray();
                    //now there are at least 2 coordinates
                    var precedent = new Coordinate(splitted[0]);
                    for (int i = 1; i < splitted.Length; i++)
                    {
                        var current = new Coordinate(splitted[i]);
                        //creating all rocks in grid
                        if (!res.ContainsKey(precedent.X))
                        {
                            res[precedent.X] = new SortedSet<int>();
                        }
                        //filling y coordinate (same x, different y)
                        if (precedent.X == current.X)
                        {
                            var max = Math.Max(precedent.Y, current.Y);
                            var min = Math.Min(precedent.Y, current.Y);
                            for (int y = min; y <= max; y++)
                            {
                                if (!res[precedent.X].Contains(y))
                                {
                                    res[precedent.X].Add(y);
                                }

                            }
                        }
                        //filling x coordinate (different x, same y)
                        else if (precedent.Y == current.Y)
                        {
                            var max = Math.Max(precedent.X, current.X);
                            var min = Math.Min(precedent.X, current.X);
                            for (int x = min; x <= max; x++)
                            {
                                if (!res.ContainsKey(x))
                                {
                                    res[x] = new SortedSet<int>();
                                }
                                if (!res.ContainsKey(precedent.Y))
                                {
                                    res[x].Add(precedent.Y);
                                }

                            }
                        }
                        else
                        {
                            throw new ArgumentException("diagonals!?");
                        }


                        precedent = current;
                    }

                }
            }
            return res;
        }
        public override ValueTask<string> Solve_1()
        {
            var cave = new CaveSystem();
            cave.Rocks = ParseScan();

            var sandStartingPoint = new Coordinate(500, 0);
            var dropDown = new Coordinate(0, 1);
            var dropDiagonalLeft = new Coordinate(-1, 1);
            var dropDiagonalRight = new Coordinate(1, 1);
            var moves = new List<Coordinate> { dropDown, dropDiagonalLeft, dropDiagonalRight };
            bool intoTheAbyss = false;
            int countGrains = 0;
            while (!intoTheAbyss)
            {
                var currentGrain = new Coordinate(sandStartingPoint.X, sandStartingPoint.Y);
                bool falling = true;
                while (falling)
                {
                    var possiblePositions = new List<Coordinate>();
                    foreach (var move in moves) //storing possible moves
                    {
                        possiblePositions.Add(new Coordinate(currentGrain.X + move.X, currentGrain.Y + move.Y));
                    }

                    if (possiblePositions.All(x => !cave.Empty(x)))
                    {
                        falling = false;
                    }
                    else
                    {
                        foreach (var move in possiblePositions)
                        {
                            if (cave.Empty(move))
                            {
                                var maxY = cave.Rocks.SelectMany(x => x.Value).Max();
                                if (move.X < cave.Rocks.Keys.Min() || move.X > cave.Rocks.Keys.Max() || move.Y > maxY)
                                {
                                    //reached the abyss!
                                    falling = false;
                                    intoTheAbyss = true;
                                }
                                else
                                {
                                    currentGrain = move;
                                }

                                break;
                            }
                        }
                    }
                }
                if (!intoTheAbyss)
                {
                    cave.AddGrainAtRest(currentGrain);
                    countGrains++;
                }

            }
            return new ValueTask<string>(countGrains.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var cave = new CaveSystem();
            cave.Rocks = ParseScan();
            cave.UpdateFloorValue();

            var sandStartingPoint = new Coordinate(500, 0);
            var dropDown = new Coordinate(0, 1);
            var dropDiagonalLeft = new Coordinate(-1, 1);
            var dropDiagonalRight = new Coordinate(1, 1);
            var moves = new List<Coordinate> { dropDown, dropDiagonalLeft, dropDiagonalRight };
            bool flowStopped = false;
            int countGrains = 0;
            while (!flowStopped)
            {
                var currentGrain = new Coordinate(sandStartingPoint.X, sandStartingPoint.Y);
                bool falling = true;
                while (falling)
                {
                    var possiblePositions = new List<Coordinate>();
                    foreach (var move in moves) //storing possible moves
                    {
                        possiblePositions.Add(new Coordinate(currentGrain.X + move.X, currentGrain.Y + move.Y));
                    }

                    if (possiblePositions.All(x => !cave.Empty(x)))
                    {
                        falling = false;
                    }
                    else
                    {
                        foreach (var move in possiblePositions)
                        {
                            if (cave.Empty(move))
                            {

                                currentGrain = move;


                                break;
                            }
                        }
                    }
                }

                if (currentGrain == sandStartingPoint)
                {
                    flowStopped = true;
                }


                cave.AddGrainAtRest(currentGrain);
                countGrains++;


            }
            return new ValueTask<string>(countGrains.ToString());
        }
    }

    public class CaveSystem
    {
        public CaveSystem()
        {
            Rocks = new Dictionary<int, SortedSet<int>>();
            SandAtRest = new Dictionary<int, SortedSet<int>>();
        }
        public Dictionary<int, SortedSet<int>> Rocks { get; set; }
        public Dictionary<int, SortedSet<int>> SandAtRest { get; set; }
        public int? FloorY { get; set; }
        public void AddGrainAtRest(Coordinate p)
        {
            if (!SandAtRest.ContainsKey(p.X))
            {
                SandAtRest[p.X] = new SortedSet<int>();
            }
            SandAtRest[p.X].Add(p.Y);
        }
        public bool Empty(Coordinate p)
        {
            if (Rocks.ContainsKey(p.X))
            {
                if (Rocks[p.X].Contains(p.Y))
                {
                    return false;
                }
            }
            if (SandAtRest.ContainsKey(p.X))
            {
                if (SandAtRest[p.X].Contains(p.Y))
                {
                    return false;
                }
            }
            if (IsFloor(p))
            {
                return false;
            }
            return true;
        }

        public void UpdateFloorValue()
        {
            var maxY = Rocks.SelectMany(x => x.Value).Max();
            FloorY = maxY + 2;
        }
        public bool IsFloor(Coordinate p)
        {
            return p.Y == FloorY;
        }
    }

    public class Coordinate
    {
        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }
        public Coordinate(string input)
        {
            var splitted = input.Split(",");
            X = int.Parse(splitted[0]);
            Y = int.Parse(splitted[1]);
        }
        public int X { get; }
        public int Y { get; }

        public override bool Equals(object? obj)
        {
            return obj is Coordinate coordinate &&
                   X == coordinate.X &&
                   Y == coordinate.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
        public override string ToString()
        {
            return $"{X},{Y}";
        }

        public static bool operator ==(Coordinate l, Coordinate r) => l.Equals(r);
        public static bool operator !=(Coordinate l, Coordinate r) => !(l == r);
    }
}
