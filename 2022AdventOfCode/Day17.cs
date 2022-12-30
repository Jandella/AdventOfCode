using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace _2022AdventOfCode
{
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
            throw new NotImplementedException();
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
        public int GetJetMove()
        {
            if (_jetIndex >= Jets.Length)
            {
                _jetIndex = 0;
            }
            int push = 0;
            var currentChar = Jets[_jetIndex];
            if (currentChar == PushToLeft)
            {
                push = -1;
            }
            else if (currentChar == PushToRight)
            {
                push = 1;
            }
            _jetIndex++;

            return push;
        }
        public RockShape GetNextShape(ShapeCavePoint p)
        {
            if(_fallingShapeIndex >= FallingOrder.Length)
            {
                _fallingShapeIndex = 0;
            }
            var shape = FallingOrder[_fallingShapeIndex];
            var nextShape = shape.MoveToStartingPoint(p);
            _fallingShapeIndex++;
            return nextShape;
        }
        public StackResult SimulateFalling(int numberOfRocks)
        {
            var fallenRocks = new HashSet<RockShape>();
            var currentHeight = BottomRow;
            while (fallenRocks.Count < numberOfRocks)
            {
                //Each rock appears so that its left edge is two
                //units away from the left wall and its bottom edge
                //is three units above the highest rock in the room (or the floor, if there isn't one).
                var nextRock = GetNextShape(new ShapeCavePoint(currentHeight + 3, 2));
                bool hasStopped = false;
                while (!hasStopped)
                {
                    var push = GetJetMove();
                    var tmp = nextRock.MoveLeftRight(push);
                    if (tmp.IsInCave(fallenRocks, Width))
                    {
                        nextRock = tmp;
                    }
                    tmp = nextRock.MoveDown();
                    if (!tmp.HasStopped(fallenRocks, BottomRow))
                    {
                        nextRock = tmp;
                    }
                    else
                    {
                        hasStopped = true;
                    }
                }
                fallenRocks.Add(nextRock);
                currentHeight = fallenRocks.Max(x => x.GetTopPoint()) + 1;
            }
            return new StackResult(Width, currentHeight)
            {
                FallenRocks = fallenRocks
            };
        }
    }
    public class ShapeCavePoint
    {
        public ShapeCavePoint()
        {

        }
        public ShapeCavePoint(int r, int c)
        {
            Row = r;
            Col = c;
        }
        public int Row { get; set; }
        public int Col { get; set; }

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
            shape._shapePoints.Add(new ShapeCavePoint(1, 3));
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
        public int GetHeight()
        {
            return _shapePoints.Max(x => x.Row) - _shapePoints.Min(x => x.Row) + 1;
        }
        public int GetTopPoint()
        {
            return _shapePoints.Max(x => x.Row);
        }
        /// <summary>
        /// Check if the shape is inside the left and right boundaries
        /// </summary>
        /// <param name="caveSize"></param>
        /// <returns></returns>
        public bool IsInCave(HashSet<RockShape> fallenRocks, int caveSize)
        {
            var inCave = !_shapePoints.Any(x => x.Col < 0 || x.Col >= caveSize);
            if (inCave)
            {
                return inCave;
            }
            for (int i = fallenRocks.Count - 1; i >= 0; i--)
            {
                var previousRock = fallenRocks.ElementAt(i);
                //check if any of the current shape is touching any of the previous rock shape
                var intersect = this._shapePoints.Intersect(previousRock._shapePoints);
                if (intersect.Any())
                {
                    return false;
                }
            }
            return inCave;
        }
        /// <summary>
        /// Check if the shape has reached the bottom row 
        /// </summary>
        /// <param name="bottomRow"></param>
        /// <returns></returns>
        public bool HasStopped(HashSet<RockShape> falledRocks, int bottomRow)
        {       
            for (int i = falledRocks.Count - 1; i >= 0; i--)
            {
                var previousRock = falledRocks.ElementAt(i);
                //check if any of the current shape is touching any of the previous rock shape
                var intersect = this._shapePoints.Intersect(previousRock._shapePoints);
                if (intersect.Any())
                {
                    return true;
                }
            }
            return _shapePoints.Any(x => x.Row < bottomRow);
        }
        
        /// <summary>
        /// Move the shape left or right
        /// </summary>
        /// <param name="push"></param>
        /// <returns>The shape in the new position</returns>
        public RockShape MoveLeftRight(int push)
        {
            var shapes = new HashSet<ShapeCavePoint>();
            foreach (var item in _shapePoints)
            {
                shapes.Add(new ShapeCavePoint(item.Row, item.Col + push));
            }
            var res = new RockShape();
            res._shapePoints = shapes;
            return res;
        }
        /// <summary>
        /// Move the current shape down of one step
        /// </summary>
        /// <returns>The shape in the new position</returns>
        public RockShape MoveDown()
        {
            var shapes = new HashSet<ShapeCavePoint>();
            foreach (var item in _shapePoints)
            {
                shapes.Add(new ShapeCavePoint(item.Row - 1, item.Col));
            }
            var res = new RockShape();
            res._shapePoints = shapes;
            return res;
        }
        /// <summary>
        /// move the shape to the current starting point p
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public RockShape MoveToStartingPoint(ShapeCavePoint p)
        {
            var shapes = new HashSet<ShapeCavePoint>();
            foreach (var item in _shapePoints)
            {
                shapes.Add(new ShapeCavePoint(item.Row + p.Row, item.Col + p.Col));
            }
            var res = new RockShape();
            res._shapePoints = shapes;
            return res;
        }

        public override string ToString()
        {
            return string.Join(", ", _shapePoints);
        }

    }

    public class StackResult
    {
        public StackResult(int w, int h)
        {
            Width = w;
            Height = h;
        }
        public int Width { get; set; }
        public int Height { get; set; }
        public HashSet<RockShape> FallenRocks { get; set; } = new HashSet<RockShape>();
    }
}
