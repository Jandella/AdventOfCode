using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace _2022AdventOfCode
{
    public class Day12 : AoCHelper.BaseDay
    {
        private string _input;
        public Day12()
        {
            _input = System.IO.File.ReadAllText(InputFilePath);
        }
        public Day12(string input)
        {
            _input = input;
        }

        public override ValueTask<string> Solve_1()
        {
            var map = new DeviceMap(_input);
            var path = map.BFS();
            var steps = path.Count - 1; //not counting a step (I'm already at Start point) 
            return new ValueTask<string>(steps.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var map = new DeviceMap(_input);
            var path = map.BFS_Part2();
            var steps = path.Count; 
            return new ValueTask<string>(steps.ToString());
        }
    }

    public class DeviceMap
    {
        public DeviceMap(string input)
        {
            var lines = input.Split(Environment.NewLine);
            MapPoints = new DeviceMapPoint[lines.Length, lines[0].Length];
            for (int r = 0; r < MapPoints.GetLength(0); r++)
            {
                for (int c = 0; c < MapPoints.GetLength(1); c++)
                {
                    char mapPoint = lines[r][c];
                    DeviceMapPoint newPoint;
                    if (mapPoint== 'S')
                    {
                        newPoint= new DeviceMapPoint(r, c, 'a');
                        Start = newPoint;
                    }
                    else if(mapPoint == 'E')
                    {
                        newPoint = new DeviceMapPoint(r, c, 'z');
                        End = newPoint;
                    }
                    else
                    {
                        newPoint = new DeviceMapPoint(r, c, mapPoint);
                    }
                    MapPoints[r, c] = newPoint;
                }
            }
            if(Start == null)
            {
                throw new ArgumentNullException(nameof(Start), "Starting point not defined");
            }
            if(End == null)
            {
                throw new ArgumentNullException(nameof(End), "Ending point not defined");
            }
        }
        public DeviceMapPoint[,] MapPoints { get; set; }
        public DeviceMapPoint Start { get; set; }
        public DeviceMapPoint End { get; set; }

        private DeviceMapPath[,] BFS_Private()
        {
            var visited = new Queue<DeviceMapPoint>();
            var pointToVisit = new Queue<DeviceMapPoint>();
            var mapPath = new DeviceMapPath[MapPoints.GetLength(0), MapPoints.GetLength(1)];
            mapPath[Start.Row, Start.Col] = new DeviceMapPath(Start);
            mapPath[End.Row, End.Col] = new DeviceMapPath(End);
            pointToVisit.Enqueue(Start);
            while (pointToVisit.Count > 0 || !visited.Contains(End))
            {
                //check adjacent nodes
                var p = pointToVisit.Dequeue();
                if (mapPath[p.Row, p.Col] == null)
                {
                    mapPath[p.Row, p.Col] = new DeviceMapPath(p);
                }
                var parent = mapPath[p.Row, p.Col];
                //up
                if (p.Row - 1 >= 0)
                {
                    var upPoint = MapPoints[p.Row - 1, p.Col];
                    if (!pointToVisit.Contains(upPoint) && !visited.Contains(upPoint)
                        && upPoint.GetHeight - p.GetHeight <= 1)
                    {
                        pointToVisit.Enqueue(upPoint);
                        mapPath[upPoint.Row, upPoint.Col] = new DeviceMapPath(upPoint, parent);
                    }
                }
                //down
                if (p.Row + 1 < MapPoints.GetLength(0))
                {
                    var downPoint = MapPoints[p.Row + 1, p.Col];
                    if (!pointToVisit.Contains(downPoint) && !visited.Contains(downPoint)
                        && downPoint.GetHeight - p.GetHeight <= 1)
                    {
                        pointToVisit.Enqueue(downPoint);
                        mapPath[downPoint.Row, downPoint.Col] = new DeviceMapPath(downPoint, parent);
                    }
                }
                //left
                if (p.Col - 1 >= 0)
                {
                    var leftPoint = MapPoints[p.Row, p.Col - 1];
                    if (!pointToVisit.Contains(leftPoint) && !visited.Contains(leftPoint)
                        && leftPoint.GetHeight - p.GetHeight <= 1)
                    {
                        pointToVisit.Enqueue(leftPoint);
                        mapPath[leftPoint.Row, leftPoint.Col] = new DeviceMapPath(leftPoint, parent);
                    }
                }
                //right
                if (p.Col + 1 < MapPoints.GetLength(1))
                {
                    var rightPoint = MapPoints[p.Row, p.Col + 1];
                    if (!pointToVisit.Contains(rightPoint) && !visited.Contains(rightPoint)
                        && rightPoint.GetHeight - p.GetHeight <= 1)
                    {
                        pointToVisit.Enqueue(rightPoint);
                        mapPath[rightPoint.Row, rightPoint.Col] = new DeviceMapPath(rightPoint, parent);
                    }
                }
                visited.Enqueue(p);
            }
            return mapPath;
        }

        public List<DeviceMapPath> BFS()
        {
            var path = new List<DeviceMapPath>();
            var mapPath = BFS_Private();
            bool trovato = false;
            var i = mapPath[End.Row, End.Col];
            path.Add(i);
            while (!trovato)
            {
                if(i.Parent == null)
                {
                    trovato = true;
                }
                else
                {
                    path.Insert(0, i.Parent);
                    i = i.Parent;
                }
            }
            return path;
        }
        private DeviceMapPath BFS_Private2(DeviceMapPoint start, IEnumerable<DeviceMapPoint> ends)
        {
            //Do Repeat yourself sometimes
            var visited = new Queue<DeviceMapPoint>();
            var pointToVisit = new Queue<DeviceMapPoint>();
            var mapPath = new DeviceMapPath[MapPoints.GetLength(0), MapPoints.GetLength(1)];
            mapPath[start.Row, start.Col] = new DeviceMapPath(start);
            foreach (var item in ends)
            {
                mapPath[item.Row, item.Col] = new DeviceMapPath(item);
            }
            pointToVisit.Enqueue(start);
            while (pointToVisit.Count > 0 && !visited.Intersect(ends).Any())
            {
                //check adjacent nodes
                var p = pointToVisit.Dequeue();
                if (mapPath[p.Row, p.Col] == null)
                {
                    mapPath[p.Row, p.Col] = new DeviceMapPath(p);
                }
                var parent = mapPath[p.Row, p.Col];
                //up
                if (p.Row - 1 >= 0)
                {
                    var upPoint = MapPoints[p.Row - 1, p.Col];
                    if (!pointToVisit.Contains(upPoint) && !visited.Contains(upPoint)
                        && p.GetHeight - upPoint.GetHeight <= 1)
                    {
                        pointToVisit.Enqueue(upPoint);
                        mapPath[upPoint.Row, upPoint.Col] = new DeviceMapPath(upPoint, parent);
                    }
                }
                //down
                if (p.Row + 1 < MapPoints.GetLength(0))
                {
                    var downPoint = MapPoints[p.Row + 1, p.Col];
                    if (!pointToVisit.Contains(downPoint) && !visited.Contains(downPoint)
                        && p.GetHeight - downPoint.GetHeight <= 1)
                    {
                        pointToVisit.Enqueue(downPoint);
                        mapPath[downPoint.Row, downPoint.Col] = new DeviceMapPath(downPoint, parent);
                    }
                }
                //left
                if (p.Col - 1 >= 0)
                {
                    var leftPoint = MapPoints[p.Row, p.Col - 1];
                    if (!pointToVisit.Contains(leftPoint) && !visited.Contains(leftPoint)
                        && p.GetHeight - leftPoint.GetHeight <= 1)
                    {
                        pointToVisit.Enqueue(leftPoint);
                        mapPath[leftPoint.Row, leftPoint.Col] = new DeviceMapPath(leftPoint, parent);
                    }
                }
                //right
                if (p.Col + 1 < MapPoints.GetLength(1))
                {
                    var rightPoint = MapPoints[p.Row, p.Col + 1];
                    if (!pointToVisit.Contains(rightPoint) && !visited.Contains(rightPoint)
                        && p.GetHeight - rightPoint.GetHeight <= 1)
                    {
                        pointToVisit.Enqueue(rightPoint);
                        mapPath[rightPoint.Row, rightPoint.Col] = new DeviceMapPath(rightPoint, parent);
                    }
                }
                visited.Enqueue(p);
            }
            var endPoint = visited.Intersect(ends).First();
            return mapPath[endPoint.Row, endPoint.Col];
        }
        public List<DeviceMapPath> BFS_Part2()
        {
            var path = new List<DeviceMapPath>();
            var possibleEndingPoints = new List<DeviceMapPoint>();
            for (int r = 0; r < MapPoints.GetLength(0); r++)
            {
                for (int c = 0; c < MapPoints.GetLength(1); c++)
                {
                    if (MapPoints[r,c].Height == 'a')
                    {
                        possibleEndingPoints.Add(MapPoints[r,c]);
                    }
                }
            }
            var i = BFS_Private2(End, possibleEndingPoints);
            bool trovato = false;
            
            while (!trovato)
            {
                if(i.Parent == null)
                {
                    trovato = true;
                }
                else
                {
                    path.Insert(0, i.Parent);
                    i = i.Parent;
                }
            }
            return path;
        }
    }
    public class DeviceMapPath
    {
        public DeviceMapPath()
        {
            Point = new DeviceMapPoint(0, 0, 'a');
        }
        public DeviceMapPath(int r, int c, char height)
        {
            Point = new DeviceMapPoint(r, c, height);
        }
        public DeviceMapPath(DeviceMapPoint point, DeviceMapPath? parent = null)
        {
            Point = point;
            Parent = parent;
        }

        public DeviceMapPoint Point { get; set; }
        public DeviceMapPath? Parent { get; set; }
    }
    public class DeviceMapPoint
    {
        public DeviceMapPoint()
        {

        }
        public DeviceMapPoint(int r, int c, char height)
        {
            Row = r;
            Col = c;
            Height = height;
        }
        public int Row { get; set; }
        public int Col { get; set; }
        public char Height { get; set; }
        public int GetHeight => (int)Height;

        public override bool Equals(object? obj)
        {
            return obj is DeviceMapPoint point &&
                   Row == point.Row &&
                   Col == point.Col &&
                   Height == point.Height;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Col, Height);
        }

        public override string ToString()
        {
            return Height.ToString();
        }
        
    }
}
