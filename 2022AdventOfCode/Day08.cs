using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace _2022AdventOfCode
{
    public class Day08 : AoCHelper.BaseDay
    {
        private string _input;
        public Day08()
        {
            _input = System.IO.File.ReadAllText(InputFilePath);
        }
        public Day08(string input)
        {
            _input = input;
        }

        public int[,] GetTreeMap()
        {
            int[,] res = new int[0, 0];
            int row = 0;
            using (StringReader r = new StringReader(_input))
            {
                while (r.Peek() != -1)
                {
                    string? line = r.ReadLine();
                    if (line != null)
                    {
                        if (row == 0)
                        {
                            res = new int[line.Length, line.Length];
                        }
                        for (int i = 0; i < line.Length; i++)
                        {
                            res[row, i] = int.Parse(line[i].ToString());
                        }
                        row++;
                    }
                }
            }
            return res;
        }

        public override ValueTask<string> Solve_1()
        {
            var map = GetTreeMap();
            var visibleColumns = new int[] { 0, map.GetLength(1) - 1 };
            var numberOfVisibleTrees = map.GetLength(0) * 2; // the first and last row are all visible trees
            var listOfVisibleTree = new List<string>();
            for (int r = 1; r < map.GetLength(0) - 1; r++) //skipping first e last row
            {
                for (int c = 0; c < map.GetLength(1); c++)
                {
                    if (visibleColumns.Contains(c))
                    {
                        numberOfVisibleTrees++;
                        listOfVisibleTree.Add($"{map[r, c]} ({r},{c})");
                    }
                    else
                    {
                        if (!IsHiddenTop(map, r, c)
                            || !IsHiddenBottom(map, r, c)
                            || !IsHiddenLeft(map, r, c)
                            || !IsHiddenRight(map, r, c))
                        {
                            numberOfVisibleTrees++;
                            listOfVisibleTree.Add($"{map[r, c]} ({r},{c})");
                        }
                    }
                }
            }
            return new ValueTask<string>(numberOfVisibleTrees.ToString());
        }

        private bool IsHiddenTop(int[,] map, int r, int c)
        {
            var visibleTopRow = 0;
            var visibleBottomRow = map.GetLength(0) - 1;
            if (r == visibleTopRow || r == visibleBottomRow)
            {
                return false;
            }
            var currentTreeHeight = map[r, c];
            for (int i = r - 1; i >= 0; i--)
            {
                var topTreeHeight = map[i, c];
                if (currentTreeHeight <= topTreeHeight)
                    return true;
            }
            return false;
        }
        private bool IsHiddenBottom(int[,] map, int r, int c)
        {
            var visibleTopRow = 0;
            var visibleBottomRow = map.GetLength(0) - 1;
            if (r == visibleTopRow || r == visibleBottomRow)
            {
                return false;
            }
            var currentTreeHeight = map[r, c];
            for (int i = r + 1; i <= visibleBottomRow; i++)
            {
                var bottomHeight = map[i, c];
                if (currentTreeHeight <= bottomHeight)
                    return true;
            }
            return false;
        }
        private bool IsHiddenRight(int[,] map, int r, int c)
        {
            var visibleLeftColumn = 0;
            var visibleRightColumn = map.GetLength(1) - 1;
            if (r == visibleLeftColumn || r == visibleRightColumn)
            {
                return false;
            }
            var currentTreeHeight = map[r, c];
            for (int i = c + 1; i <= visibleRightColumn; i++)
            {
                var rightTreeHeight = map[r, i];
                if (currentTreeHeight <= rightTreeHeight)
                    return true;
            }
            return false;
        }
        private bool IsHiddenLeft(int[,] map, int r, int c)
        {
            var visibleLeftColumn = 0;
            var visibleRightColumn = map.GetLength(1) - 1;
            if (r == visibleLeftColumn || r == visibleRightColumn)
            {
                return false;
            }
            var currentTreeHeight = map[r, c];
            for (int i = c - 1; i >= 0; i--)
            {
                var leftTreeHeight = map[r, i];
                if (currentTreeHeight <= leftTreeHeight)
                    return true;
            }
            return false;
        }
        public override ValueTask<string> Solve_2()
        {
            var map = GetTreeMap();
            int maxScore = int.MinValue;
            for (int r = 1; r < map.GetLength(0) - 1; r++) //skipping first e last row
            {
                for (int c = 0; c < map.GetLength(1); c++)
                {
                    var currentScore = TopScore(map, r, c) * BottomScore(map, r, c)
                        * RightScore(map, r, c) * LeftScore(map, r, c);
                    if (currentScore > maxScore)
                    {
                        maxScore = currentScore;
                    }
                }
            }
            return new ValueTask<string>(maxScore.ToString());
        }

        private int TopScore(int[,] map, int r, int c)
        {
            var visibleTopRow = 0;
            var visibleBottomRow = map.GetLength(0) - 1;
            if (r == visibleTopRow || r == visibleBottomRow)
            {
                return 0;
            }
            var score = 0;
            var currentTreeHeight = map[r, c];
            for (int i = r - 1; i >= 0; i--)
            {
                var topTreeHeight = map[i, c];

                score++;
                if (currentTreeHeight <= topTreeHeight)
                    break;
            }
            return score;
        }
        private int BottomScore(int[,] map, int r, int c)
        {
            var visibleTopRow = 0;
            var visibleBottomRow = map.GetLength(0) - 1;
            if (r == visibleTopRow || r == visibleBottomRow)
            {
                return 0;
            }
            var score = 0;
            var currentTreeHeight = map[r, c];
            for (int i = r + 1; i <= visibleBottomRow; i++)
            {
                var bottomHeight = map[i, c];

                score++;
                if (currentTreeHeight <= bottomHeight)
                    break;
            }
            return score;
        }
        private int RightScore(int[,] map, int r, int c)
        {
            var visibleLeftColumn = 0;
            var visibleRightColumn = map.GetLength(1) - 1;
            if (r == visibleLeftColumn || r == visibleRightColumn)
            {
                return 0;
            }
            var score = 0;
            var currentTreeHeight = map[r, c];
            for (int i = c + 1; i <= visibleRightColumn; i++)
            {
                var rightTreeHeight = map[r, i];

                score++;
                if (currentTreeHeight <= rightTreeHeight)
                    break;
            }
            return score;
        }
        private int LeftScore(int[,] map, int r, int c)
        {
            var visibleLeftColumn = 0;
            var visibleRightColumn = map.GetLength(1) - 1;
            if (r == visibleLeftColumn || r == visibleRightColumn)
            {
                return 0;
            }
            var score = 0;
            var currentTreeHeight = map[r, c];
            for (int i = c - 1; i >= 0; i--)
            {
                var leftTreeHeight = map[r, i];
                score++;
                if (currentTreeHeight <= leftTreeHeight)
                    break;
            }
            return score;
        }
    }
}
