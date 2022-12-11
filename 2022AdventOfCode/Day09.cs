using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2022AdventOfCode
{
    public class Day09 : AoCHelper.BaseDay
    {
        private string _input;
        private int _numberOfKnots;
        public Day09(int numberOfKnots = 2)
        {
            _input = System.IO.File.ReadAllText(InputFilePath);
            _numberOfKnots = numberOfKnots;
        }
        public Day09(string input, int numberOfKnots = 2)
        {
            _input = input;
            _numberOfKnots = numberOfKnots;
        }
        public override ValueTask<string> Solve_1()
        {
            var rope = new RopeKnots(0, 0);
            var positions = new HashSet<RopeEdge>();
            positions.Add(rope.Tail);
            using (StringReader r = new StringReader(_input))
            {
                while (r.Peek() != -1)
                {
                    var line = r.ReadLine()??"- 0";
                    var splitted = line.Split(" ");
                    var direction = splitted[0][0];
                    var steps = int.Parse(splitted[1]);
                    for (int i = 0; i < steps; i++)
                    {
                        var head = rope.Head.Move(direction);
                        var tail = rope.Tail.Follow(head);
                        rope = new RopeKnots(head.Row, head.Col, tail.Row, tail.Col);
                        if (!positions.Contains(tail))
                        {
                            positions.Add(rope.Tail);
                        }

                    }
                }
            }
            return new ValueTask<string>(positions.Count.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var list = new List<RopeEdge>();
            for (int i = 0; i < _numberOfKnots; i++)
            {
                list.Add(new RopeEdge(0, 0));
            }
            var rope = new RopeKnots(list);
            var positions = new HashSet<RopeEdge>();
            positions.Add(rope.Tail);
            using (StringReader r = new StringReader(_input))
            {
                while (r.Peek() != -1)
                {
                    var line = r.ReadLine() ?? "- 0";
                    var splitted = line.Split(" ");
                    var direction = splitted[0][0];
                    var steps = int.Parse(splitted[1]);
                    for (int i = 0; i < steps; i++)
                    {
                        var previous = rope.Head.Move(direction);
                        var tempList = new List<RopeEdge>();
                        tempList.Add(previous);
                        for (int j = 1; j < rope.Knots.Count; j++)
                        {
                            var item = rope.Knots[j];
                            var newPos = item.Follow(previous);
                            tempList.Add(newPos);
                            previous = newPos;
                        }
                        rope = new RopeKnots(tempList);
                        if (!positions.Contains(rope.Tail))
                        {
                            positions.Add(rope.Tail);
                        }

                    }
                }
            }
            return new ValueTask<string>(positions.Count.ToString());
        }
    }
    public class RopeKnots
    {
        public RopeKnots(int r, int c)
        {
            Head = new RopeEdge(r, c);
            Tail = new RopeEdge(r, c);
            Knots = new List<RopeEdge> { Head, Tail };
        }
        public RopeKnots(int rHead, int cHead, int rTail, int cTail)
        {
            Head = new RopeEdge(rHead, cHead);
            Tail = new RopeEdge(rTail, cTail);
            Knots = new List<RopeEdge> { Head, Tail };
        }
        public RopeKnots(IEnumerable<RopeEdge> knots)
        {
            if(knots.Count() < 2)
            {
                throw new ArgumentException("Too few knots");
            }
            Knots = new List<RopeEdge>();
            foreach (var item in knots)
            {
                Knots.Add(new RopeEdge(item.Row, item.Col));
            }
            Head = Knots.First();
            Tail = Knots.Last();
        }
        public RopeEdge Head { get; private set; }
        public RopeEdge Tail { get; private set; }
        public List<RopeEdge> Knots { get; set; }
        
    }

    public class RopeEdge
    {
        public RopeEdge(int r, int c)
        {
            Row = r;
            Col = c;
        }
        public int Row { get; private set; }
        public int Col { get; private set; }
        public RopeEdge Move(char direction)
        {
            RopeEdge res;
            switch (direction)
            {
                case 'L':
                    res = new RopeEdge(Row, Col - 1);
                    break;
                case 'R':
                    res = new RopeEdge(Row, Col + 1);
                    break;
                case 'U':
                    res = new RopeEdge(Row + 1, Col);
                    break;
                case 'D':
                    res = new RopeEdge(Row - 1, Col);
                    break;
                default:
                    res = new RopeEdge(Row, Col);
                    break;
            }
            return res;
        }
        public RopeEdge Follow(RopeEdge point)
        {
            var rowDist = Row - point.Row;
            var rowAbs = Math.Abs(rowDist);

            var colDist = Col - point.Col;
            var colAbs = Math.Abs(colDist);

            if (rowAbs <= 1 && colAbs <= 1)
                return new RopeEdge(Row, Col);
            else if (rowDist == 0 && Math.Abs(colDist) > 1)
                return new RopeEdge(Row, Col - Math.Sign(colDist));
            else if (colDist == 0 && Math.Abs(rowDist) > 1)
                return new RopeEdge(Row - Math.Sign(rowDist), Col);
            else
                return new RopeEdge(Row - Math.Sign(rowDist), Col - Math.Sign(colDist));
        }

        public override bool Equals(object? obj)
        {
            return obj is RopeEdge edge &&
                   Row == edge.Row &&
                   Col == edge.Col;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Col);
        }
    }

    
}
