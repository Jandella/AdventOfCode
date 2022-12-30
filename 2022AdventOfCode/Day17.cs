using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
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
            throw new NotImplementedException();
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
            shape._shapePoints.Add(new ShapeCavePoint(0, 1));
            shape._shapePoints.Add(new ShapeCavePoint(0, 2));
            shape._shapePoints.Add(new ShapeCavePoint(0, 3));
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
            shape._shapePoints.Add(new ShapeCavePoint(0, 3));
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
        /// <summary>
        /// Check if the shape is inside the left and right boundaries
        /// </summary>
        /// <param name="caveSize"></param>
        /// <returns></returns>
        public bool IsInCave(int caveSize)
        {
            return _shapePoints.Any(x => x.Col < 0 || x.Col > caveSize);
        }
        /// <summary>
        /// Check if the shape has reached the bottom row 
        /// </summary>
        /// <param name="bottomRow"></param>
        /// <returns></returns>
        public bool HasStopped(int bottomRow)
        {
            return _shapePoints.Any(x => x.Row == bottomRow + 1);
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
        /// Move the current shape down of one go
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

    }
}
