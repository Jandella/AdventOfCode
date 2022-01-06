using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021AdventOfCode
{
    /// <summary>
    /// --- Day 13: Transparent Origami ---
    /// You reach another volcanically active part of the cave.
    /// It would be nice if you could do some kind of thermal imaging so you could tell ahead of time which caves are too hot to safely enter.
    /// </summary>
    public class Day13
    {
        private string _day13input = "";


        public Day13()
        {
            _day13input = System.IO.File.ReadAllText(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Inputs", "Day13Input.txt"));
        }

        public int Quiz1()
        {
            var paper = new TransparentPaper(_day13input);
            //System.Diagnostics.Debug.WriteLine(paper);
            paper.Fold(paper.FoldInstructions.First());
            //System.Diagnostics.Debug.WriteLine(paper);
            return paper.Dots.Count;
        }

        public string Quiz2()
        {
            var paper = new TransparentPaper(_day13input);
            paper.Fold();
            System.Diagnostics.Debug.WriteLine(paper);
            //"BLKJRBAG"
            return paper.ToString();
        }

        
    }

    public class TransparentPaper
    {
        private int _height;
        private int _width;
        public TransparentPaper(string input)
        {
            var lines = input.Split("\n").Select(x => x.Trim()).ToArray();
            var emptyElement = lines.FirstOrDefault(x => string.IsNullOrEmpty(x));
            var indexOfEmpty = Array.IndexOf(lines, emptyElement);
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                if (i < indexOfEmpty)
                {
                    //points
                    var points = line.Split(",");
                    Dots.Add(new TopRightCoordinates
                    {
                        OffsetFromRight = int.Parse(points[0]),
                        OffsetFromTop = int.Parse(points[1])
                    });
                }
                else if (i > indexOfEmpty)
                {
                    //fold instructions
                    var val = line.Replace("fold along ", "");
                    var foldWay = val.Split("=");
                    FoldInstructions.Add(new FoldInstruction
                    {
                        FoldAlong = foldWay[0],
                        FoldLine = int.Parse(foldWay[1])
                    });
                }
            }
            _height = Dots.Max(x => x.OffsetFromTop);
            _width = Dots.Max(x => x.OffsetFromRight);
        }
        public List<TopRightCoordinates> Dots { get; set; } = new List<TopRightCoordinates>();
        public List<FoldInstruction> FoldInstructions { get; set; } = new List<FoldInstruction>();

        public void Fold()
        {
            foreach (var item in FoldInstructions)
            {
                Fold(item);
            }
        }
        public void Fold(FoldInstruction instruction)
        {
            bool foldUp = instruction.FoldAlong == "y";
            List<TopRightCoordinates> newDots;
            if (foldUp)
            {
                newDots = FoldUp(instruction.FoldLine);
            }
            else
            {
                newDots = FoldLeft(instruction.FoldLine);
            }
            Dots = newDots;
        }

        private List<TopRightCoordinates> FoldUp(int line)
        {
            var top = Dots.Where(d => d.OffsetFromTop < line).ToList();
            var bottom = Dots.Where(d => d.OffsetFromTop > line);
            var newDotsFromBottom = new List<TopRightCoordinates>();
            foreach (var dot in bottom)
            {
                var newDot = new TopRightCoordinates
                {
                    OffsetFromRight = dot.OffsetFromRight,
                    OffsetFromTop = line - Math.Abs(dot.OffsetFromTop - line)
                };
                newDotsFromBottom.Add(newDot);
            }
            for (int i = 0; i <= _height; i++)
            {
                for (int j = 0; j <= _width; j++)
                {
                    var dotFromTop = top.FirstOrDefault(d => d.OffsetFromTop == i && d.OffsetFromRight == j);
                    var dotFromBottom = newDotsFromBottom.FirstOrDefault(d => d.OffsetFromTop == i && d.OffsetFromRight == j);
                    if(dotFromTop == null && dotFromBottom != null)
                    {
                        top.Add(dotFromBottom);
                    }
                }
            }
            _height = line - 1; //line count start with 0, height is the actual length from 1 to _height
            return top;
        }

        private List<TopRightCoordinates> FoldLeft(int line)
        {
            var left = Dots.Where(d => d.OffsetFromRight < line).ToList();
            var right = Dots.Where(d => d.OffsetFromRight > line);
            var newDotsFromRight = new List<TopRightCoordinates>();
            foreach (var dot in right)
            {
                var newDot = new TopRightCoordinates
                {
                    OffsetFromRight = line - Math.Abs(dot.OffsetFromRight - line),
                    OffsetFromTop = dot.OffsetFromTop
                };
                newDotsFromRight.Add(newDot);
            }
            for (int i = 0; i <= _height; i++)
            {
                for (int j = 0; j <= _width; j++)
                {
                    var dotFromLeft = left.FirstOrDefault(d => d.OffsetFromTop == i && d.OffsetFromRight == j);
                    var dotFromRight = newDotsFromRight.FirstOrDefault(d => d.OffsetFromTop == i && d.OffsetFromRight == j);
                    if (dotFromLeft == null && dotFromRight != null)
                    {
                        left.Add(dotFromRight);
                    }
                }
            }
            _width = line - 1; //line count start with 0, width is the actual length from 1 to _width
            return left;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int i = 0; i <= _height; i++)
            {
                for (int j = 0; j <= _width; j++)
                {
                    var dot = Dots.FirstOrDefault(d => d.OffsetFromRight == j && d.OffsetFromTop == i);
                    if(dot == null)
                    {
                        sb.Append(' ');
                    }
                    else
                    {
                        sb.Append('#');
                    }
                }
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }
    }

    public class TopRightCoordinates
    {
        public int OffsetFromRight { get; set; }
        public int OffsetFromTop { get; set; }

        public override bool Equals(object obj)
        {
            return obj is TopRightCoordinates coordinates &&
                   OffsetFromRight == coordinates.OffsetFromRight &&
                   OffsetFromTop == coordinates.OffsetFromTop;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(OffsetFromRight, OffsetFromTop);
        }

        public override string ToString()
        {
            return OffsetFromRight + "," + OffsetFromTop;
        }
    }

    public class FoldInstruction
    {
        public string FoldAlong { get; set; } = string.Empty;
        public int FoldLine { get; set; }

        public override string ToString()
        {
            return $"fold along {FoldAlong}={FoldLine}";
        }
    }

}
