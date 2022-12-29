using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace _2022AdventOfCode
{
    public class Day16 : AoCHelper.BaseDay
    {
        private string _input;
        public Day16()
        {
            _input = System.IO.File.ReadAllText(InputFilePath);
        }
        public Day16(string input)
        {
            _input = input;
        }


        public Dictionary<string, Valve> ParseInput()
        {
            var d = new Dictionary<string, Valve>();
            using (var sr = new StringReader(_input))
            {
                while (sr.Peek() != -1)
                {
                    var line = sr.ReadLine() ?? string.Empty;
                    //Valve AA has flow rate=0; tunnels lead to valves DD, II, BB
                    var splitted = line.Split(';');
                    var firstSpace = splitted[0].IndexOf(' ');
                    var valveName = splitted[0].Substring(firstSpace + 1, 2); //valve name has 2 char
                    var indexOfEquals = splitted[0].IndexOf('=');
                    int flowRate = int.Parse(splitted[0].Substring(indexOfEquals + 1));
                    var v = new Valve(valveName, flowRate);
                    d.Add(v.Name, v);
                    var valvesLabel = "valves";
                    var valveLabel = "valve";
                    var usedLabel = valvesLabel;
                    var indexOfValves = splitted[1].IndexOf(usedLabel);
                    if (indexOfValves == -1)
                    {
                        usedLabel = valveLabel;
                        indexOfValves = splitted[1].IndexOf(usedLabel);
                    }
                    var stringListOfValves = splitted[1][(indexOfValves + usedLabel.Length)..].Trim();
                    v.ConnectedTo = stringListOfValves.Split(',').Select(x => x.Trim()).ToList();

                }
            }
            return d;
        }
        /// <summary>
        /// Compute shortest distance between points
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        private Dictionary<string, ValveDistance> CalculateShortestDistances(Dictionary<string, Valve> d)
        {
            var distances = new Dictionary<string, ValveDistance>();
            foreach (var i in d.Keys)
            {
                foreach (var j in d.Keys)
                {
                    var q = new Queue<NodeValve>();
                    q.Enqueue(new NodeValve(i)
                    {
                        Distance = 0
                    });
                    var visited = new HashSet<string>();
                    while (q.Count > 0)
                    {
                        var current = q.Dequeue();
                        if (current.Name == j)
                        {
                            var k = ValveDistance.GenerateId(i, j);
                            if (!distances.ContainsKey(k))
                            {
                                distances.Add(k, new ValveDistance(i, j));
                            }
                            distances[k].Distance = current.Distance;
                            break;
                        }
                        foreach (var n in d[current.Name].ConnectedTo)
                        {
                            if (visited.Add(n))
                            {
                                q.Enqueue(new NodeValve(n)
                                {
                                    Distance = current.Distance + 1
                                });
                            }
                        }
                    }
                }
            }
            return distances;
        }
        /// <summary>
        /// Backtrack a pat
        /// </summary>
        /// <param name="valves">List of valves graph</param>
        /// <param name="dist">distances between edges</param>
        /// <param name="time">Time elapsed</param>
        /// <param name="cur">current valve</param>
        /// <param name="pressure">current pressure</param>
        /// <param name="cumulpressure">cumulated pressure</param>
        /// <param name="available">list of valves that can be opened</param>
        /// <param name="max">max pressure released</param>
        /// <param name="curPath">current path</param>
        /// <param name="paths">visited paths</param>
        private void Backtrack(Dictionary<string, Valve> valves, Dictionary<string, ValveDistance> dist, int time, string cur, int pressure, int cumulpressure,
    int available, ref int max, List<string> curPath, Dictionary<HashSet<string>, int> paths)
        {
            if (time == 0 || available == 0)
            {
                max = Math.Max(max, cumulpressure + (pressure * time));
                return;
            }

            paths.Add(new HashSet<string>(curPath), cumulpressure + (pressure * time));

            foreach (var n in valves.Keys)
            {
                if (n == cur || valves[n].IsOpen || valves[n].Flow == 0)
                {
                    continue;
                }
                var id = ValveDistance.GenerateId(n, cur);
                var ttg = dist[id].Distance + 1;
                if (ttg > time)
                {
                    ttg = time;
                }
                valves[n].IsOpen = true;
                curPath.Add(n);
                Backtrack(valves, dist, time - ttg, n, pressure + valves[n].Flow, cumulpressure + (pressure * ttg), available - 1, ref max, curPath, paths);
                curPath.RemoveAt(curPath.Count - 1);
                valves[n].IsOpen = false;
            }
        }
        public override ValueTask<string> Solve_1()
        {
            var d = ParseInput();
            var distances = CalculateShortestDistances(d);
            int max = 0;
            var curPath = new List<string>();
            var paths = new Dictionary<HashSet<string>, int>();
            int available = d.Where(v => v.Value.Flow > 0 && !v.Value.IsOpen).Count();
            Backtrack(d, distances, 30, "AA", 0, 0, available, ref max, curPath, paths);

            return new ValueTask<string>(max.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var d = ParseInput();
            var distances = CalculateShortestDistances(d);
            int max = 0;
            var curPath = new List<string>();
            var paths = new Dictionary<HashSet<string>, int>();
            int available = d.Where(v => v.Value.Flow > 0 && !v.Value.IsOpen).Count();
            //less time for second case
            Backtrack(d, distances, 26, "AA", 0, 0, available, ref max, curPath, paths);
            var orderedPaths = paths.Keys.OrderByDescending(o => paths[o]).ToArray();
            //brute force it
            foreach (var a in orderedPaths) 
            {
                foreach (var b in orderedPaths)
                {
                    if (!a.Overlaps(b))
                    {
                        var pressure = paths[a] + paths[b];
                        if (pressure > max)
                        {
                            max = pressure;
                        }
                    }
                }
            }
            

            return new ValueTask<string>(max.ToString());
        }
    }

    public class Valve
    {
        public Valve(string name)
        {
            Name = name;
            ConnectedTo = new List<string>();
        }
        public Valve(string name, int flow) : this(name)
        {
            Flow = flow;
        }
        public Valve(string name, int flow, List<string> connectedTo) : this(name, flow)
        {
            ConnectedTo = connectedTo;
        }

        public string Name { get; set; }
        public int Flow { get; set; }
        public List<string> ConnectedTo { get; set; }
        public bool IsOpen { get; set; }

        public override string ToString()
        {
            return string.Format("Valve {0} has flow rate={1}; tunnels lead to valves {2}", Name, Flow, string.Join(", ", ConnectedTo));
        }
    }
    public class ValveDistance
    {
        public ValveDistance(string a, string b)
        {
            A = a;
            B = b;
            Id = GenerateId(A, B);
        }
        public ValveDistance(Valve a, Valve b)
        {
            A = a.Name;
            B = b.Name;
            Id = GenerateId(A, B);
        }
        public string A { get; set; }
        public string B { get; set; }
        public int Distance { get; set; }

        public string Id { get; private set; }

        public static string GenerateId(Valve a, Valve b)
        {
            return GenerateId(a.Name, b.Name);
        }

        public static string GenerateId(string a, string b)
        {
            var list = new List<string>() { a, b };
            return string.Join("", list.OrderBy(x => x));
        }
        public override string ToString()
        {
            return string.Format("{0}--{1}--{2}", A, Distance, B);
        }
    }

    public class NodeValve
    {
        public NodeValve(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
        public int Distance { get; set; }
        public override string ToString()
        {
            return string.Format("{0}, ({1})", Name, Distance);
        }
    }
}
