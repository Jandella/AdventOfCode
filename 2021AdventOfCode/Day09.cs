using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace _2021AdventOfCode
{

    /// <summary>
    /// --- Day 9: Smoke Basin ---
    /// These caves seem to be lava tubes.Parts are even still volcanically active; small hydrothermal vents release smoke into the caves that slowly settles like rain.
    /// If you can model how the smoke flows through the caves, you might be able to avoid it and be that much safer.
    /// </summary>
    public class Day09
    {
        private string _day9input = "";
        public Day09()
        {
            _day9input = System.IO.File.ReadAllText(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Inputs", "Day09Input.txt"));
        }
        private int[][] ParseInput(string input)
        {
            var lines = input.Split('\n').Select(x => x.Trim());
            var res = new int[lines.Count()][];
            int row = 0;
            foreach (var line in lines)
            {
                res[row] = new int[line.Length];
                int col = 0;
                foreach (var c in line)
                {
                    res[row][col] = int.Parse(c.ToString());
                    col++;
                }
                row++;
            }
            return res;
        }
        public int Quiz1()
        {
            var map = ParseInput(_day9input);
            var lowestPoints = new List<int>();
            for (int row = 0; row < map.Length; row++)
            {
                for (int col = 0; col < map[row].Length; col++)
                {
                    var examinedPoint = map[row][col];
                    if(IsLowest(row, col, map))
                    {
                        lowestPoints.Add(examinedPoint);
                    }

                }
            }
            return lowestPoints.Sum(x => x + 1);
        }

        public int Quiz2()
        {
            var map = ParseInput(_day9input);
            var lowestPoints = new List<int>();
            var basinSizes = new List<int>();
            for (int row = 0; row < map.Length; row++)
            {
                for (int col = 0; col < map[row].Length; col++)
                {
                    
                    if (IsLowest(row, col, map))
                    {
                        var sum = CountBasinSize(row, col, map);
                        basinSizes.Add(sum);
                    }

                }
            }
            var biggestThree = basinSizes.OrderByDescending(x => x).Take(3);
            var m = 1;
            foreach (var item in biggestThree)
            {
                m = m * item;
            }
            return m;
        }

        public bool IsLowest(int row, int col, int[][] map)
        {
            var examnedPoint = map[row][col];
            var up = row - 1;
            var down = row + 1;
            var left = col - 1;
            var right = col + 1;
            var nearbyPoints = new List<int>();
            if (up >= 0 && up < map.Length)
            {
                nearbyPoints.Add(map[up][col]);
            }
            if (down >= 0 && down < map.Length)
            {
                nearbyPoints.Add(map[down][col]);
            }
            if(left >= 0 && left < map[row].Length)
            {
                nearbyPoints.Add(map[row][left]);
            }
            if (right >= 0 && right < map[row].Length)
            {
                nearbyPoints.Add(map[row][right]);
            }
            return nearbyPoints.All(x => x > examnedPoint);
        }

        public int CountBasinSize(int row, int col, int [][] map)
        {
            var queue = new Queue<RowCol>();
            queue.Enqueue(new RowCol { Row = row, Col = col });
            var points = new List<RowCol>();
            while(queue.Count > 0)
            {
                var current = queue.Dequeue();
                if(map[current.Row][current.Col] < 9 && !points.Contains(current))
                {
                    points.Add(current);
                    if (current.Col - 1 >= 0)
                        queue.Enqueue(new RowCol { Row = current.Row, Col = current.Col - 1 });
                    if (current.Row - 1 >= 0)
                        queue.Enqueue(new RowCol { Row = current.Row - 1, Col = current.Col });
                    if (current.Col + 1 < map.First().Length)
                        queue.Enqueue(new RowCol { Row = current.Row, Col = current.Col + 1 });
                    if (current.Row + 1 < map.Length)
                        queue.Enqueue(new RowCol { Row = current.Row + 1, Col = current.Col });
                }
            }
            
            return points.Count();

        }

        
    }

    
}
