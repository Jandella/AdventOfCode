using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace _2022AdventOfCode
{
    public class Day15Parameters
    {
        public string? Input { get; set; }
        public int Y { get; set; }
        public int MaxDistressBeaconCoordinate { get; set; }
        public int MinDistressBeaconCoordinate { get; set; }
    }
    public class Day15 : AoCHelper.BaseDay
    {
        private string _input;
        private int _y = 10;
        private int _maxDistressBeaconCoordinate { get; set; }
        private int _minDistressBeaconCoordinate { get; set; }
        public Day15(Day15Parameters p)
        {
            _input = p.Input ?? System.IO.File.ReadAllText(InputFilePath);
            _y = p.Y;
            _maxDistressBeaconCoordinate = p.MaxDistressBeaconCoordinate;
            _minDistressBeaconCoordinate = p.MinDistressBeaconCoordinate;
        }



        public override ValueTask<string> Solve_1()
        {
            var m = new SensorBeaconMap(_input);
            int count = 0;
            for (int i = m.MinX; i <= m.MaxX; i++)
            {
                var coordinate = new Coordinate(i, _y);
                foreach (var item in m.ObjectList)
                {
                    var beaconDistance = SensorBeaconMap.ManhattanDistance(item.LockedBeacon, item.Position);
                    var distance = SensorBeaconMap.ManhattanDistance(coordinate, item.Position);

                    if (distance <= beaconDistance && SensorBeaconMap.ManhattanDistance(coordinate, item.LockedBeacon) != 0)
                    {
                        count++;
                        break;
                    }
                }
            }

            return new ValueTask<string>(count.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var m = new SensorBeaconMap(_input);
            Coordinate? distressBeacon = null;
            long res = 0;
            foreach (var sensor in m.ObjectList)
            {
                var perimeter = sensor.OutsidePerimeter();
                
                foreach (var point in perimeter)
                {
                    
                    if(point.X < _minDistressBeaconCoordinate || point.Y < _minDistressBeaconCoordinate 
                        || point.X > _maxDistressBeaconCoordinate || point.Y > _maxDistressBeaconCoordinate)
                    {
                        continue;
                    }
                    bool insideRange = false;
                    foreach (var sensor2 in m.ObjectList)
                    {
                        
                        if(sensor2.Position == sensor.Position)
                        {
                            continue; //skip
                        }
                        var beaconDistance = SensorBeaconMap.ManhattanDistance(sensor2.LockedBeacon, sensor2.Position);
                        var distance = SensorBeaconMap.ManhattanDistance(point, sensor2.Position);
                        if(distance <= beaconDistance)
                        {
                            //inside range of sensor2
                            insideRange = true;
                        }
                    }
                    if (!insideRange)
                    {
                        distressBeacon = point;
                        break;
                    }
                    
                }
                if(distressBeacon is not null)
                {
                    res = ((long)distressBeacon.X * (long)4000000) + (long)distressBeacon.Y;
                    break;
                }
            }
            return new ValueTask<string>(res.ToString());
        }
    }

    public class SensorBeaconMap
    {
        public SensorBeaconMap()
        {

        }

        public SensorBeaconMap(string input)
        {
            MaxX = int.MinValue;
            MaxY = int.MinValue;
            MinX = int.MaxValue;
            MinY = int.MaxValue;
            using (StringReader sr = new StringReader(input))
            {
                while (sr.Peek() != -1)
                {
                    //line like this
                    //Sensor at x=2, y=18: closest beacon is at x=-2, y=15
                    string line = sr.ReadLine() ?? string.Empty;
                    var sensorBeacon = line.Split(':');
                    var sensor = ParseChunk(sensorBeacon[0]);
                    var beacon = ParseChunk(sensorBeacon[1]);

                    ObjectList.Add(new Pair(sensor, beacon));
                    SetMinMax(sensor, beacon);
                }
            }

        }
        private Coordinate ParseChunk(string input)
        {
            //2 kind of input
            //(1) "Sensor at x=2, y=18"
            //(2) " closest beacon is at x=-2, y=15"
            var indexOfx = input.IndexOf('x');
            var coordinateChunck = input.Substring(indexOfx);
            var splitted = coordinateChunck.Split(',');
            var xPart = splitted[0].Replace('x', ' ').Replace('=', ' ').Trim();
            var yPart = splitted[1].Replace('y', ' ').Replace('=', ' ').Trim();
            return new Coordinate(int.Parse(xPart), int.Parse(yPart));
        }
        private void SetMinMax(Coordinate sensor, Coordinate beacon)
        {
            var minX = sensor.X - ManhattanDistance(sensor, beacon);
            var maxX = sensor.X + ManhattanDistance(sensor, beacon);
            var minY = sensor.Y - ManhattanDistance(sensor, beacon);
            var maxY = sensor.Y + ManhattanDistance(sensor, beacon);
            if (minX < MinX)
            {
                MinX = minX;
            }
            if (minY < MinY)
            {
                MinY = minY;
            }
            if (maxX > MaxX)
            {
                MaxX = maxX;
            }
            if (maxY > MaxY)
            {
                MaxY = maxY;
            }
        }
        public int MinX { get; private set; }
        public int MaxX { get; private set; }
        public int MinY { get; private set; }
        public int MaxY { get; private set; }
        public static int ManhattanDistance(Coordinate a, Coordinate b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }
        public List<Pair> ObjectList { get; set; } = new List<Pair>();




    }
    public class Pair
    {

        public Pair(Coordinate position, Coordinate lockedBeacon)
        {
            Position = position;
            LockedBeacon = lockedBeacon;
        }

        public Coordinate Position { get; private set; }
        public Coordinate LockedBeacon { get; set; }

        /// <summary>
        /// Outside perimeter is a list of coordinate where the sensor do not reach a beacon (the area outside the #).
        /// <code>
        ///               1    1    2    2 
        ///     0    5    0    5    0    5
        ///-2 ..........#.................
        ///-1 .........###................
        /// 0 ....S...#####...............
        /// 1 .......#######........S.....
        /// 2 ......#########S............
        /// 3 .....###########SB..........
        /// 4 ....#############...........
        /// 5 ...###############..........
        /// 6 ..#################.........
        /// 7 .#########S#######S#........
        /// 8 ..#################.........
        /// 9 ...###############..........
        ///10 ....B############...........
        ///11 ..S..###########............
        ///12 ......#########.............
        ///13 .......#######..............
        ///14 ........#####.S.......S.....
        ///15 B........###................
        ///16 ..........#SB...............
        ///17 ................S..........B
        ///18 ....S.......................
        ///19 ............................
        ///20 ............S......S........
        ///21 ............................
        ///22 .......................B....
        ///</code>
        /// </summary>
        /// <returns></returns>
        public List<Coordinate> OutsidePerimeter()
        {
            var distance = SensorBeaconMap.ManhattanDistance(Position, LockedBeacon);
            var res = new List<Coordinate>();
            var previousC = new Coordinate(Position.X, Position.Y - (distance + 1));
            //starting from top (same x as sensor, y is the max distance + 1, top) and going clockwise
            while (previousC.Y - Position.Y != 0)
            {
                var current = new Coordinate(previousC.X + 1, previousC.Y + 1);
                res.Add(current);
                previousC = current;
            }
            //first quadrant done, the point is at same y but x = distance + 1
            while (previousC.X - Position.X != 0)
            {
                var current = new Coordinate(previousC.X - 1, previousC.Y + 1);
                res.Add(current);
                previousC = current;
            }
            //second quadrant done, the point is now with the same x as sensor, with y max distance + 1, bottom
            while (previousC.X != Position.X - (distance + 1))
            {
                var current = new Coordinate(previousC.X - 1, previousC.Y - 1);
                res.Add(current);
                previousC = current;
            }
            //third quadrant done, the point is now with the same y as sensor, with x = -(distance + 1)
            while (previousC.Y != Position.Y - (distance + 1))
            {
                var current = new Coordinate(previousC.X + 1, previousC.Y - 1);
                res.Add(current);
                previousC = current;
            }
            return res;
        }
    }
}
