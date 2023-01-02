using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace _2022AdventOfCode
{
    /// <summary>
    /// Many thanks for https://aoc.just2good.co.uk/2022/17 for the great explanation of day 17, 
    /// witch helped me debug my part 1 and explained well how to takle part 2. 
    /// The code used was python and not C#, but was helpfull neverless
    /// </summary>
    public class Day17 : AoCHelper.BaseDay
    {
        private string _input;
        public Day17()
        {
            _input = System.IO.File.ReadAllText(InputFilePath);
        }
        public Day17(string input)
        {
            _input = input;
        }

        public override ValueTask<string> Solve_1()
        {
            var cave = new TallNarrowChamber(_input);
            var res = cave.SimulateFalling(2022);

            return new ValueTask<string>(res.Height.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var cave = new TallNarrowChamber(_input);
            var res = cave.SimulateLargeFalling(1000000000000);

            return new ValueTask<string>(res.Height.ToString());
        }
    }

    public class TallNarrowChamber
    {
        private const char PushToLeft = '<';
        private const char PushToRight = '>';
        private int _jetIndex = 0;
        private int _fallingShapeIndex = 0;
        public TallNarrowChamber(string input)
        {
            Jets = input;
            FallingOrder = new RockShape[]
            {
                RockShape.GenerateHorizontalBarShape(),
                RockShape.GeneratePlusBarShape(),
                RockShape.GenerateReverseLShape(),
                RockShape.GenerateVerticalBarShape(),
                RockShape.GenerateSquareShape()
            };
        }
        public int Width { get; } = 7;
        public int BottomRow { get; } = 0;
        public string Jets { get; set; }
        public RockShape[] FallingOrder { get; set; }
        public ShapeCavePoint GetJetMove()
        {
            if (_jetIndex >= Jets.Length)
            {
                _jetIndex = 0;
            }
            ShapeCavePoint push = new ShapeCavePoint(0, 0);
            var currentChar = Jets[_jetIndex];
            if (currentChar == PushToLeft)
            {
                push.Col = -1;
            }
            else if (currentChar == PushToRight)
            {
                push.Col = 1;
            }
            _jetIndex++;

            return push;
        }
        public RockShape GetNextShape(ShapeCavePoint p)
        {
            if (_fallingShapeIndex >= FallingOrder.Length)
            {
                _fallingShapeIndex = 0;
            }
            var shape = FallingOrder[_fallingShapeIndex];
            var nextShape = shape.Move(p);
            _fallingShapeIndex++;
            return nextShape;
        }
        public StackResult SimulateFalling(int numberOfRocks)
        {
            var fallenRocks = new HashSet<RockShape>();
            var rocksPoint = new HashSet<ShapeCavePoint>();
            long currentHeight = BottomRow;
            var moveDownVector = new ShapeCavePoint(-1, 0);
            while (fallenRocks.LongCount() < numberOfRocks)
            {
                //Each rock appears so that its left edge is two
                //units away from the left wall and its bottom edge
                //is three units above the highest rock in the room (or the floor, if there isn't one).
                var nextRock = GetNextShape(new ShapeCavePoint(currentHeight + 3, 2));
                bool hasStopped = false;
                while (!hasStopped)
                {
                    var push = GetJetMove();
                    var tmp = nextRock.Move(push);
                    if (tmp.IsInCave(rocksPoint, Width))
                    {
                        nextRock = tmp;
                    }
                    //Debug.WriteLine(PrintStatus(fallenRocks, currentHeight + nextRock.GetTopPoint(), nextRock));
                    tmp = nextRock.Move(moveDownVector);
                    if (!tmp.HasStopped(rocksPoint, BottomRow))
                    {
                        nextRock = tmp;
                        //Debug.WriteLine(PrintStatus(fallenRocks, currentHeight + nextRock.GetTopPoint(), nextRock));
                    }
                    else
                    {
                        hasStopped = true;
                    }
                }
                if (!fallenRocks.Add(nextRock))
                {
                    throw new InvalidOperationException("Shape not added");
                }
                var allPoints = nextRock.GetAllPoints();
                foreach (var item in allPoints)
                {
                    rocksPoint.Add(item);
                }
                currentHeight = fallenRocks.Max(x => x.GetTopPoint()) + 1;
            }
            return new StackResult(Width, currentHeight)
            {
                FallenRocks = fallenRocks
            };
        }

        public RockShape DropRock(long currentHeight, HashSet<ShapeCavePoint> rocksPoint)
        {
            var moveDownVector = new ShapeCavePoint(-1, 0);
            //Each rock appears so that its left edge is two
            //units away from the left wall and its bottom edge
            //is three units above the highest rock in the room (or the floor, if there isn't one).
            var nextRock = GetNextShape(new ShapeCavePoint(currentHeight + 3, 2));
            bool hasStopped = false;
            while (!hasStopped)
            {
                var push = GetJetMove();
                var tmp = nextRock.Move(push);
                if (tmp.IsInCave(rocksPoint, Width))
                {
                    nextRock = tmp;
                }
                tmp = nextRock.Move(moveDownVector);
                if (!tmp.HasStopped(rocksPoint, BottomRow))
                {
                    nextRock = tmp;
                }
                else
                {
                    hasStopped = true;
                }
            }
            return nextRock;
        }
        /// <summary>
        /// Simulate a large fall, using buffers for set comparing optimization
        /// </summary>
        /// <param name="rocksToDrop"></param>
        /// <returns></returns>
        public StackResultBase SimulateLargeFalling(long rocksToDrop)
        {
            int bufferSize = 200;//arbitrary
            //Storing only last {bufferSize} point and shapes to keep operation faster
            var lastFallenRocksBuffer = new Queue<RockShape>(bufferSize);
            var rocksPointBuffer = new PointBufferQueue(bufferSize);
            
            var stateCache = new Dictionary<RockTowerState, StackResultBase>();
            long currentHeight = BottomRow;
            long fallenRocksNumber = 0;
            bool repetitionFound = false;
            long repetitionOfFallenRockHeight = 0;
            long repetitionOfFallenRockCount = 0;
            while (!repetitionFound && fallenRocksNumber < rocksToDrop)
            {
                var nextRock = DropRock(currentHeight, rocksPointBuffer.GetCurrentSet());
                rocksPointBuffer.Add(nextRock);
                
                lastFallenRocksBuffer.Enqueue(nextRock);
                while (lastFallenRocksBuffer.Count > bufferSize)
                {
                    lastFallenRocksBuffer.Dequeue();
                }
                
                currentHeight = lastFallenRocksBuffer.Max(x => x.GetTopPoint()) + 1;
                fallenRocksNumber++;
                var currentState = new RockTowerState()
                {
                    IndexOfJet = _jetIndex,
                    IndexOfRocks = _fallingShapeIndex,
                    LastFallenRockShapes = string.Empty
                };
                //taking last 20 rocks
                var rocksTaken = 0;
                for (int i = lastFallenRocksBuffer.Count - 1; i >= 0; i--)
                {
                    var item = lastFallenRocksBuffer.ElementAtOrDefault(i);
                    if (item != null)
                    {
                        if(currentState.LastFallenRockShapes != string.Empty)
                        {
                            currentState.LastFallenRockShapes += Environment.NewLine;
                        }
                        currentState.LastFallenRockShapes+= item.PrintShape();
                        rocksTaken++;
                    }
                    if(rocksTaken >= 20)
                    {
                        break;
                    }
                }
                //checking cache for repetition
                if (!repetitionFound)
                {
                    if (stateCache.ContainsKey(currentState))
                    {
                        //repeat found!
                        repetitionFound = true;
                        repetitionOfFallenRockCount = fallenRocksNumber - stateCache[currentState].NumberOfFallenRocks;
                        repetitionOfFallenRockHeight = currentHeight - stateCache[currentState].Height;
                    }
                    else
                    {
                        //storing state in cache
                        stateCache.Add(currentState, new StackResultBase(Width, currentHeight)
                        {
                            NumberOfFallenRocks = fallenRocksNumber
                        });
                    }
                }

            }

            //repetition found, calculating height
            var remainingDrop = rocksToDrop - fallenRocksNumber;
            long repeatsRequired = 0;
            long heightRepeatFound = currentHeight;
            long numberOfFallenRockCountRepeatFound = fallenRocksNumber;
            var newHeight = currentHeight;
            var newNumberOfFallenRockCount = fallenRocksNumber;
            if (repetitionFound)
            {
                repeatsRequired = (long)Math.Floor(((double)remainingDrop / (double)repetitionOfFallenRockCount));
                remainingDrop %= repetitionOfFallenRockCount;
                var heightDelta = repetitionOfFallenRockHeight * repeatsRequired;
                newHeight = currentHeight + heightDelta;
                newNumberOfFallenRockCount += repetitionOfFallenRockCount * repeatsRequired;
            }


            //remaining
            //drop shapes again with buffer (we continue from the last dropped shapes,
            //we have the old height and have a buffer where we got a repetition)
            while (remainingDrop > 0)
            {
                var nextRock = DropRock(currentHeight, rocksPointBuffer.GetCurrentSet());
                rocksPointBuffer.Add(nextRock);
                
                lastFallenRocksBuffer.Enqueue(nextRock);
                while (lastFallenRocksBuffer.Count > bufferSize)
                {
                    lastFallenRocksBuffer.Dequeue();
                }
                currentHeight = lastFallenRocksBuffer.Max(x => x.GetTopPoint()) + 1;
                remainingDrop--;
                fallenRocksNumber++;
            }
            var finalHeight = newHeight + currentHeight - heightRepeatFound;



            return new StackResultBase(Width, finalHeight)
            {
                NumberOfFallenRocks = newNumberOfFallenRockCount + fallenRocksNumber - numberOfFallenRockCountRepeatFound
            };
        }
        public string PrintStatus(HashSet<RockShape> fallenRocks, long heigth, RockShape? current)
        {
            var rows = new List<string>();
            var firstLine = "    ";
            firstLine += "+";
            for (int i = 0; i < Width; i++)
            {
                firstLine += "-";
            }
            firstLine += "+";
            rows.Add(firstLine);
            for (long i = BottomRow; i < heigth + 1; i++)
            {
                var line = string.Format("{0:D3} ", i);

                for (int j = -1; j <= Width; j++)
                {
                    var point = new ShapeCavePoint(i, j);
                    if (j == -1 || j == Width)
                    {
                        line += "|";
                    }
                    else if (fallenRocks.Any(r => r.ContainsPoint(point)))
                    {
                        line += "#";
                    }
                    else if (current != null && current.ContainsPoint(point))
                    {
                        line += "@";
                    }
                    else
                    {
                        line += ".";
                    }
                }

                rows.Add(line);
            }
            rows.Reverse();
            var header = string.Format("Tower Height = {0}, fallen rocks = {1}", heigth, fallenRocks.Count);
            return header + Environment.NewLine + string.Join(Environment.NewLine, rows);
        }
    }
    public class ShapeCavePoint
    {
        public ShapeCavePoint()
        {

        }
        public ShapeCavePoint(long r, int c)
        {
            Row = r;
            Col = c;
        }
        public long Row { get; set; }
        public int Col { get; set; }

        public ShapeCavePoint Sum(ShapeCavePoint other)
        {
            return new ShapeCavePoint(Row + other.Row, Col + other.Col);
        }
        public override bool Equals(object? obj)
        {
            return obj is ShapeCavePoint point &&
                   Row == point.Row &&
                   Col == point.Col;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Col);
        }
        public override string ToString()
        {
            return string.Format("({0},{1})", Row, Col);
        }
    }
    public class RockShape
    {
        private HashSet<ShapeCavePoint> _shapePoints;
        public RockShape()
        {
            _shapePoints = new HashSet<ShapeCavePoint>();
        }
        /// <summary>
        /// Generate a horizontal bar shape. Coordinates starting from bottom left.
        /// <code>
        /// ####
        /// </code>
        /// </summary>
        /// <returns></returns>
        public static RockShape GenerateHorizontalBarShape()
        {
            var shape = new RockShape();
            shape._shapePoints.Add(new ShapeCavePoint(0, 0));
            shape._shapePoints.Add(new ShapeCavePoint(0, 1));
            shape._shapePoints.Add(new ShapeCavePoint(0, 2));
            shape._shapePoints.Add(new ShapeCavePoint(0, 3));
            return shape;
        }
        /// <summary>
        /// Generate a horizontal bar shape. Coordinates starting from bottom left.
        /// <code>
        /// .#.
        /// ###
        /// .#.
        /// </code>
        /// </summary>
        /// <returns></returns>
        public static RockShape GeneratePlusBarShape()
        {
            var shape = new RockShape();
            shape._shapePoints.Add(new ShapeCavePoint(0, 1));
            shape._shapePoints.Add(new ShapeCavePoint(1, 1));
            shape._shapePoints.Add(new ShapeCavePoint(2, 1));
            shape._shapePoints.Add(new ShapeCavePoint(1, 0));
            shape._shapePoints.Add(new ShapeCavePoint(1, 2));
            return shape;
        }
        /// <summary>
        /// Generate a reverse L shape. Coordinates starting from bottom left.
        /// <code>
        /// ..#
        /// ..#
        /// ###
        /// </code>
        /// </summary>
        /// <returns></returns>
        public static RockShape GenerateReverseLShape()
        {
            var shape = new RockShape();
            shape._shapePoints.Add(new ShapeCavePoint(0, 0));
            shape._shapePoints.Add(new ShapeCavePoint(0, 1));
            shape._shapePoints.Add(new ShapeCavePoint(0, 2));
            shape._shapePoints.Add(new ShapeCavePoint(1, 2));
            shape._shapePoints.Add(new ShapeCavePoint(2, 2));
            return shape;
        }
        /// <summary>
        /// Generate a vertical bar shape. Coordinates starting from bottom left.
        /// <code>
        /// #
        /// #
        /// #
        /// #
        /// </code>
        /// </summary>
        /// <returns></returns>
        public static RockShape GenerateVerticalBarShape()
        {
            var shape = new RockShape();
            shape._shapePoints.Add(new ShapeCavePoint(0, 0));
            shape._shapePoints.Add(new ShapeCavePoint(1, 0));
            shape._shapePoints.Add(new ShapeCavePoint(2, 0));
            shape._shapePoints.Add(new ShapeCavePoint(3, 0));
            return shape;
        }
        /// <summary>
        /// Generate a square shape. Coordinates starting from bottom left.
        /// <code>
        /// ##
        /// ##
        /// </code>
        /// </summary>
        /// <returns></returns>
        public static RockShape GenerateSquareShape()
        {
            var shape = new RockShape();
            shape._shapePoints.Add(new ShapeCavePoint(0, 0));
            shape._shapePoints.Add(new ShapeCavePoint(0, 1));
            shape._shapePoints.Add(new ShapeCavePoint(1, 0));
            shape._shapePoints.Add(new ShapeCavePoint(1, 1));
            return shape;
        }
        public long GetHeight()
        {
            return _shapePoints.Max(x => x.Row) - _shapePoints.Min(x => x.Row) + 1;
        }
        public long GetTopPoint()
        {
            return _shapePoints.Max(x => x.Row);
        }
        public HashSet<ShapeCavePoint> GetAllPoints()
        {
            var res = new HashSet<ShapeCavePoint>();
            foreach (var item in _shapePoints)
            {
                res.Add(new ShapeCavePoint(item.Row, item.Col));
            }
            return _shapePoints;
        }
        /// <summary>
        /// Check if the shape is inside the left and right boundaries
        /// </summary>
        /// <param name="caveSize"></param>
        /// <returns></returns>
        public bool IsInCave(HashSet<ShapeCavePoint> fallenRocks, int caveSize)
        {
            var inCave = !_shapePoints.Any(x => x.Col < 0 || x.Col >= caveSize);
            if (inCave)
            {
                //is in cave, check if overlaps other shapes
                if (fallenRocks.Any())
                {
                    var intersect = this._shapePoints.Intersect(fallenRocks);
                    if (intersect.Any())
                    {
                        return false;
                    }
                }
            }
            return inCave;
        }
        /// <summary>
        /// Check if the shape has reached the bottom row 
        /// </summary>
        /// <param name="bottomRow"></param>
        /// <returns></returns>
        public bool HasStopped(HashSet<ShapeCavePoint> falledRocks, int bottomRow)
        {
            if (falledRocks.Any())
            {
                var intersect = this._shapePoints.Intersect(falledRocks);
                if (intersect.Any())
                {
                    return true;
                }
            }
            return _shapePoints.Any(x => x.Row < bottomRow);
        }

        public RockShape Move(ShapeCavePoint p)
        {
            var shapes = new HashSet<ShapeCavePoint>();
            foreach (var item in _shapePoints)
            {
                shapes.Add(item.Sum(p));
            }
            var res = new RockShape();
            res._shapePoints = shapes;
            return res;
        }

        public bool ContainsPoint(ShapeCavePoint p)
        {
            return _shapePoints.Contains(p);
        }
        public override string ToString()
        {
            return string.Join(", ", _shapePoints);
        }

        public override bool Equals(object? obj)
        {
            return obj is RockShape shape &&
                   EqualityComparer<HashSet<ShapeCavePoint>>.Default.Equals(_shapePoints, shape._shapePoints);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_shapePoints);
        }
        /// <summary>
        /// Graphic representation of shape in a string 
        /// </summary>
        /// <returns></returns>
        public string PrintShape()
        {
            var minRow = _shapePoints.Min(x => x.Row);
            var maxRow = _shapePoints.Max(x => x.Row);
            var minCol = _shapePoints.Min(x => x.Col);
            var maxCol = _shapePoints.Max(x => x.Col);
            var rows = new List<string>();
            for (long i = minRow; i <= maxRow; i++)
            {
                var line = "";
                for (int j = minCol; j <= maxCol; j++)
                {
                    var item = _shapePoints.FirstOrDefault(x => x.Equals((new ShapeCavePoint(i, j))));
                    if (item == null)
                    {
                        line += ".";
                    }
                    else
                    {
                        line += "#";
                    }
                }
                rows.Add(line);
            }
            rows.Reverse();
            return string.Join(Environment.NewLine, rows);
        }
    }
    public class StackResultBase
    {
        public StackResultBase(int w, long h)
        {
            Width = w;
            Height = h;
        }
        public int Width { get; set; }
        public long Height { get; set; }
        public long NumberOfFallenRocks { get; set; }
    }

    public class StackResult : StackResultBase
    {
        public StackResult(int w, long h) : base(w, h)
        {

        }
        public new long NumberOfFallenRocks
        {
            get => FallenRocks.LongCount();
            set => _ = value;
        }
        public HashSet<RockShape> FallenRocks { get; set; } = new HashSet<RockShape>();
    }

    public class RockTowerState
    {
        public int IndexOfJet { get; set; }
        public int IndexOfRocks { get; set; }
        public string LastFallenRockShapes { get; set; } = string.Empty;

        public override bool Equals(object? obj)
        {
            return obj is RockTowerState state &&
                   IndexOfJet == state.IndexOfJet &&
                   IndexOfRocks == state.IndexOfRocks &&
                   LastFallenRockShapes == state.LastFallenRockShapes;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IndexOfJet, IndexOfRocks, LastFallenRockShapes);
        }
    }
    /// <summary>
    /// A buffer for storing the current set of points
    /// </summary>
    public class PointBufferQueue
    {
        private int _bufferSize = 0;
        private HashSet<ShapeCavePoint> _rocksPoint;
        private Queue<ShapeCavePoint> _lastRocksPointBuffer;
        public PointBufferQueue(int bufferSize)
        {
            _bufferSize = bufferSize;
            _rocksPoint = new HashSet<ShapeCavePoint>();
            _lastRocksPointBuffer = new Queue<ShapeCavePoint>();
        }
        /// <summary>
        /// Add a point to the buffer, if buffer becames full, 
        /// remove first buffered element
        /// </summary>
        /// <param name="p"></param>
        public void Add(ShapeCavePoint p)
        {
            if (_rocksPoint.Add(p))
            {
                _lastRocksPointBuffer.Enqueue(p);
            }
            while (_lastRocksPointBuffer.Count > _bufferSize)
            {
                var item = _lastRocksPointBuffer.Dequeue();
                _rocksPoint.Remove(item);
            }
        }
        /// <summary>
        /// Add all the points of a shape.
        /// </summary>
        /// <param name="r"></param>
        public void Add(RockShape r)
        {
            var allPoints = r.GetAllPoints();
            foreach (var p in allPoints)
            {
                Add(p);
            }
        }
        /// <summary>
        /// Return a copy of the current set
        /// </summary>
        /// <returns></returns>
        public HashSet<ShapeCavePoint> GetCurrentSet()
        {
            var set = new HashSet<ShapeCavePoint>();
            foreach (var item in _rocksPoint)
            {
                set.Add(new ShapeCavePoint(item.Row, item.Col));
            }
            return set;
        }

    }
}
