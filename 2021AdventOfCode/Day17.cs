using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _2021AdventOfCode
{
    /// <summary>
    /// --- Day 17: Trick Shot ---
    /// You finally decode the Elves' message. HI, the message says. You continue searching for the sleigh keys.
    /// Ahead of you is what appears to be a large ocean trench.Could the keys have fallen into it? You'd better send a probe to investigate.
    /// </summary>
    public class Day17
    {
        private string _day17input = @"target area: x=102..157, y=-146..-90";
        public int Quiz1()
        {
            var area = new TargetArea(_day17input);
            var n = Math.Abs(area.Ymin) - 1;
            var hMax = n * (n + 1) / 2;
            return hMax;
        }

        public int Quiz2()
        {
            var area = new TargetArea(_day17input);
            var yMax = Math.Max(Math.Abs(area.Ymax), Math.Abs(area.Ymin));
            int distances = 0;
            for (int x = 0; x <= area.Xmax; x++)
            {
                for (int y = -yMax; y <= yMax; y++)
                {
                    var testProbeLaunch = new ProbeLaunch(x, y);
                    if (testProbeLaunch.HitsTarget(area))
                    {
                        distances++;
                    }
                }
            }
            return distances;
        }
    }

    public class ProbeLaunch
    {
        private int _startXvelocity, _startYvelocity;
        public ProbeLaunch(int xvelocity, int yvelocity)
        {
            _startXvelocity = xvelocity;
            _startYvelocity = yvelocity;
        }
        public int StartXvelocity => _startXvelocity;
        public int StartYvelocity => _startYvelocity;
        /// <summary>
        /// On each step, these changes occur in the following order:
        /// The probe's x position increases by its x velocity.
        /// The probe's y position increases by its y velocity.
        /// Due to drag, the probe's x velocity changes by 1 toward the value 0; that is, it decreases by 1 if it is greater than 0, increases by 1 if it is less than 0, or does not change if it is already 0.
        /// Due to gravity, the probe's y velocity decreases by 1.
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        private (int,int) PerformStep(RowCol pos, int xv, int yv)
        {
            int xvres = xv;
            int yvres = yv;

            pos.Col += xv;
            pos.Row += yv;
            if (xv > 0)
            {
                xvres--;
            }
            else if (xv < 0)
            {
                xvres++;
            }
            yvres--;
            return (xvres, yvres);
        }

        public bool HitsTarget(TargetArea area)
        {
            var iterator = new RowCol()
            {
                Row = 0,
                Col = 0
            };

            bool breakingConditions = false;
            int xv = StartXvelocity;
            int yv = StartYvelocity;
            while(!area.Contains(iterator) && !breakingConditions)
            {
                
                var tmp = PerformStep(iterator, xv, yv);
                xv = tmp.Item1;
                yv = tmp.Item2;
                breakingConditions = iterator.Col > area.Xmax;
                breakingConditions = breakingConditions || xv == 0 && !(area.Xmin <= iterator.Col && iterator.Col <= area.Xmax);
                breakingConditions = breakingConditions || xv == 0 && iterator.Row < area.Ymax;
            }
            return area.Contains(iterator);
        }
    }

    public class TargetArea
    {
        public TargetArea()
        {

        }
        public TargetArea(string input)
        {
            //examlpe string:
            //"target area: x=20..30, y=-10..-5"
            var splitted = input.Split(',');
            var xx = splitted[0].Split("..");
            var yy = splitted[1].Split("..");
            Xmin = int.Parse(xx[0].Substring(xx[0].IndexOf("=") + 1));
            Xmax = int.Parse(xx[1]);
            Ymin = int.Parse(yy[0].Substring(yy[0].IndexOf("=") + 1)); 
            Ymax = int.Parse(yy[1]);
        }
        public int Xmin { get; set; }
        public int Xmax { get; set; }
        public int Ymin { get; set; }
        public int Ymax { get; set; }
        public bool Contains(RowCol pos)
        {
            return Xmin <= pos.Col && pos.Col <= Xmax
                && Ymin <= pos.Row && pos.Row <= Ymax;
        }

        public override string ToString()
        {
            return $"target area: x={Xmin}..{Xmax}, y={Ymin}..{Ymax}";
        }
    }
}
