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
            int[,] res = new int[0,0];
            int row = 0;
            using (StringReader r = new StringReader(_input))
            {
                while(r.Peek()!= -1)
                {
                    string? line = r.ReadLine();
                    if(line != null)
                    {
                        if(row == 0)
                        {
                            res = new int[line.Length, line.Length];     
                        }
                        for (int i = 0; i < line.Length; i++)
                        {
                            res[row,i] = int.Parse(line[i].ToString());
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
            for (int r = 1; r < map.GetLength(0) - 1; r++) //skipping first e last row
            {
                for (int c = 0; c < map.GetLength(1); c++)
                {
                    if (visibleColumns.Contains(c))
                    {
                        numberOfVisibleTrees++;
                    }
                    else
                    {
                        var currentTreeHeight = map[r, c];
                        var topTreeHeight = map[r - 1, c];
                        var bottomTreeHeight = map[r + 1, c];
                        var rightTreeHeight = map[r, c + 1];
                        var leftTreeHeight = map[r, c - 1];
                        if(IsVisibleTop(map, r,c)
                            || IsVisibleBottom(map, r, c)
                            || IsVisibleLeft(map, r, c)
                            || IsVisibleRight(map, r, c))
                        {
                            numberOfVisibleTrees++;
                        }
                    }
                }
            }
            return new ValueTask<string>(numberOfVisibleTrees.ToString());
        }

        private bool IsVisibleTop(int[,]map, int r, int c)
        {
            var visibleTopRow = 0;
            var visibleBottomRow = map.GetLength(0) - 1;
            if (r == visibleTopRow || r == visibleBottomRow)
            {
                return true;
            }
            var currentTreeHeight = map[r, c];
            for (int i = r-1; i >= 0; i--)
            {
                var topTreeHeight = map[i, c];
                if (currentTreeHeight < topTreeHeight)
                    return false;
            }
            return true;
        }
        private bool IsVisibleBottom(int[,]map, int r, int c)
        {
            var visibleTopRow = 0;
            var visibleBottomRow = map.GetLength(0) - 1;
            if (r == visibleTopRow || r == visibleBottomRow)
            {
                return true;
            }
            var currentTreeHeight = map[r, c];
            for (int i = r+1; i <= visibleBottomRow; i++)
            {
                var bottomHeight = map[i, c];
                if (currentTreeHeight < bottomHeight)
                    return false;
            }
            return true;
        }
        private bool IsVisibleRight(int[,] map, int r, int c)
        {
            var visibleLeftColumn = 0;
            var visibleRightColumn = map.GetLength(1) - 1;
            if (r == visibleLeftColumn || r == visibleRightColumn)
            {
                return true;
            }
            var currentTreeHeight = map[r, c];
            for (int i = c + 1; i <= visibleRightColumn; i++)
            {
                var rightTreeHeight = map[r, i];
                if (currentTreeHeight < rightTreeHeight)
                    return false;
            }
            return true;
        }
        private bool IsVisibleLeft(int[,] map, int r, int c)
        {
            var visibleLeftColumn = 0;
            var visibleRightColumn = map.GetLength(1) - 1;
            if (r == visibleLeftColumn || r == visibleRightColumn)
            {
                return true;
            }
            var currentTreeHeight = map[r, c];
            for (int i = c - 1; i >= 0; i--)
            {
                var leftTreeHeight = map[r, i];
                if (currentTreeHeight < leftTreeHeight)
                    return false;
            }
            return true;
        }
        public override ValueTask<string> Solve_2()
        {
            throw new NotImplementedException();
        }
    }
}
