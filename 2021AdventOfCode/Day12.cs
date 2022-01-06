using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021AdventOfCode
{
    /// <summary>
    /// --- Day 12: Passage Pathing ---
    /// With your submarine's subterranean subsystems subsisting suboptimally, the only way you're getting out of this cave anytime soon is by finding a path yourself.Not just a path - the only way to know if you've found the best path is to find all of them.
    /// </summary>
    public class Day12
    {
        private string _day12input = "";
        public Day12()
        {
            _day12input = System.IO.File.ReadAllText(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Inputs", "Day12Input.txt"));
        }
        public int Quiz1()
        {
            var graph = new Graph(_day12input);
            var c = graph.CountAllPaths("start", "end");
            return c;
        }

        public int Quiz2()
        {
            var graph = new Graph(_day12input);
            var c = graph.CountAllPaths2("start", "end");
            return c;
        }
    }

    public class Graph
    {
        private Dictionary<string, List<string>> _vertices = new Dictionary<string, List<string>>();
        private readonly string _toString;



        public Graph(string input)
        {
            _toString = input;
            var lines = input.Split("\n").Select(x => x.Trim());
            foreach (var line in lines)
            {
                var vertices = line.Split("-");
                AddEdge(vertices[0], vertices[1]);
            }
        }

        public void AddEdge(string a, string b)
        {
            if (!_vertices.ContainsKey(a))
            {
                _vertices.Add(a, new List<string>());
            }
            if (!_vertices.ContainsKey(b))
            {
                _vertices.Add(b, new List<string>());
            }
            _vertices[a].Add(b);
            _vertices[b].Add(a);
        }


        public int CountAllPaths(string startingVertex, string endingVertex)
        {
            var currentPath = new Stack<string> ();
            var allPaths = new List<List<string>>();

            DepthFirstSearchUtil(startingVertex, startingVertex, endingVertex, currentPath, allPaths);
            
            return allPaths.Count;

        }

        public int CountAllPaths2(string startingVertex, string endingVertex)
        {
            var currentPath = new Stack<string>();
            var allPaths = new List<List<string>>();

            DepthFirstSearchUtil2(startingVertex, startingVertex, endingVertex, currentPath, allPaths);

            return allPaths.Count;
        }

        private void DepthFirstSearchUtil(string startingVertex, string currentVertex, string endingVertex, Stack<string> currentPath, List<List<string>> allPaths)
        {
            if(currentVertex == startingVertex && currentPath.Contains(startingVertex))
            {
                return;
            }
            currentPath.Push(currentVertex);

            if(currentVertex == endingVertex)
            {
                allPaths.Add(currentPath.Reverse().ToList());
                currentPath.Pop();
                return;
            }

            if(char.IsLower(currentVertex[0]) && currentPath.Count(v => v == currentVertex) > 1)
            {
                currentPath.Pop(); //can visit small caves only once
                return;
            }
            foreach (var item in _vertices[currentVertex])
            {
                DepthFirstSearchUtil(startingVertex, item, endingVertex, currentPath, allPaths);
            }
            currentPath.Pop();
        }

        private void DepthFirstSearchUtil2(string startingVertex, string currentVertex, string endingVertex, Stack<string> currentPath, List<List<string>> allPaths)
        {
            if (currentVertex == startingVertex && currentPath.Contains(startingVertex))
            {
                return;
            }
            currentPath.Push(currentVertex);

            if (currentVertex == endingVertex)
            {
                allPaths.Add(currentPath.Reverse().ToList());
                currentPath.Pop();
                return;
            }

            // can be visited twice if there are no other visited twice

            var anySmallCaveWithTwiceVisit = currentPath.Where(x => char.IsLower(x[0]) && x != currentVertex)
                .GroupBy(x => x)
                .Any(x => x.Count() > 1);

            var isCurrentVisitedTwice = currentPath.Count(v => v == currentVertex) == 2;
            var isCurrentVisitedMoreThanOnce = currentPath.Count(v => v == currentVertex) > 2;

            if (char.IsLower(currentVertex[0]) && (anySmallCaveWithTwiceVisit && isCurrentVisitedTwice || isCurrentVisitedMoreThanOnce))
            {
                
                currentPath.Pop(); //can visit small caves only once or twice
                return;
            }
            foreach (var item in _vertices[currentVertex])
            {
                DepthFirstSearchUtil2(startingVertex, item, endingVertex, currentPath, allPaths);
            }
            currentPath.Pop();
        }

        public override string ToString()
        {
            return _toString;
        }
    }
}
