using System;
using System.Collections.Generic;
using System.Linq;
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
                        else if(precedent.Y == current.Y)
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
            var rocks = ParseScan();
            throw new NotImplementedException();
        }

        public override ValueTask<string> Solve_2()
        {
            throw new NotImplementedException();
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
    }
}
