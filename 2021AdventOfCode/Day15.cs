using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021AdventOfCode
{
    /// <summary>
    /// --- Day 15: Chiton ---
    /// You've almost reached the exit of the cave, but the walls are getting closer together. Your submarine can barely still fit, though; the main problem is that the walls of the cave are covered in chitons, and it would be best not to bump any of them.
    /// </summary>
    public class Day15
    {
        private string _day15input = "";
        public Day15()
        {
            _day15input = System.IO.File.ReadAllText(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Inputs", "Day15Input.txt"));
        }
        public int Quiz1()
        {
            var g = new Grid(_day15input);
            Astar(g);
            var last = g.GetFinalNode();
            return last.GlobalGoal;
        }

        private void Astar(Grid g)
        {
            var queue = new List<Node>();
            var start = g.GetNode(0, 0);
            start.LocalGoal = 0;
            start.GlobalGoal = Heuristic(g, start);
            queue.Add(start);
            while (queue.Count > 0)
            {
                queue = queue.OrderBy(x => x.GlobalGoal).ToList();
                while (queue.Count > 0 && queue.First().Visited)
                {
                    queue.RemoveAt(0);
                }

                if (queue.Count == 0)
                    break;

                var currentNode = queue.First();
                queue.Remove(currentNode);
                currentNode.Visited = true;
                var neighbours = g.FindNeighbours(currentNode);

                foreach (var n in neighbours)
                {
                    if (!n.Visited)
                        queue.Add(n);

                    var possiblyLowerGoal = currentNode.LocalGoal + n.Weight;
                    if (possiblyLowerGoal < n.LocalGoal)
                    {
                        n.LocalGoal = possiblyLowerGoal;
                        n.GlobalGoal = n.LocalGoal + Heuristic(g, n);
                    }
                }
            }
        }

        private int Heuristic(Grid g, Node a)
        {
            var end = g.GetFinalNode();
            return (end.X - a.X) + (end.Y - a.Y);
        }

        public int Quiz2()
        {
            var g = new Grid(_day15input, 5);
            Astar(g);
            var last = g.GetFinalNode();
            return last.GlobalGoal;
        }
    }

    public class Grid
    {
        private Node[,] _grid;
        private int _heigth;
        private int _width;
        public Grid(string input)
        {
            var splitted = input.Split('\n').Select(x => x.Trim());
            _heigth = splitted.Count();
            _width = splitted.First().Length;
            _grid = new Node[_heigth, _width];
            for (int i = 0; i < _heigth; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    var c = splitted.ElementAt(i)[j].ToString();
                    _grid[i, j] = new Node(j, i, int.Parse(c));
                }
            }
        }

        public Grid(string input, int expandBy)
        {
            var splitted = input.Split('\n').Select(x => x.Trim());
            var baseHeigth = splitted.Count();
            var baseWidth = splitted.First().Length;
            _heigth = baseHeigth * expandBy;
            _width = baseWidth * expandBy;
            _grid = new Node[_heigth, _width];
            for (int i = 0; i < baseHeigth; i++)
            {
                for (int j = 0; j < baseWidth; j++)
                {
                    for (int k = 0; k < expandBy; k++)
                    {
                        var newX = j + k * baseWidth;
                        for (int l = 0; l < expandBy; l++)
                        {
                            var c = splitted.ElementAt(i)[j].ToString();
                            var weight = int.Parse(c);
                            weight += k;
                            weight += l;
                            if(weight > 9)
                            {
                                weight -= 9;
                            }
                            var newY = i + l * baseHeigth;
                            _grid[newY, newX] = new Node(newX, newY, weight);
                        }
                    }
                    
                    
                }
            }
        }

        public Node GetNode(int x, int y)
        {
            return _grid[y, x];
        }

        public Node GetFinalNode()
        {
            return _grid[_heigth - 1, _width - 1];
        }

        public List<Node> FindNeighbours(Node n)
        {
            var neighbours = new List<Node>();
            if (n.X < _width - 1)
            {
                neighbours.Add(_grid[n.Y, n.X + 1]);
            }
            if (n.X > 0)
            {
                neighbours.Add(_grid[n.Y, n.X - 1]);
            }
            if (n.Y < _heigth - 1)
            {
                neighbours.Add(_grid[n.Y + 1, n.X]);
            }
            if (n.Y > 0)
            {
                neighbours.Add(_grid[n.Y - 1, n.X]);
            }
            return neighbours;
        }

        public string Print(int Xfrom, int Yfrom, int Xto, int Yto)
        {
            var sb = new StringBuilder();
            for (int i = Yfrom; i < Yto; i++)
            {
                for (int j = Xfrom; j < Xto; j++)
                {
                    sb.Append(_grid[i, j]);
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }

    public class Node
    {
        public int GlobalGoal { get; set; }
        public int LocalGoal { get; set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Weight { get; private set; }
        public bool Visited { get; set; }
        public Node(int x, int y, int weight)
        {
            X = x;
            Y = y;
            Weight = weight;
            GlobalGoal = int.MaxValue;
            LocalGoal = int.MaxValue;
            Visited = false;
        }
        public override string ToString()
        {
            return $"[({X}, {Y} - {Weight}) - {GlobalGoal}]";
        }

        
    }
}

