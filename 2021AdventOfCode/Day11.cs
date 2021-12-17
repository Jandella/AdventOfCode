using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021AdventOfCode
{
    /// <summary>
    /// --- Day 11: Dumbo Octopus ---
    /// You enter a large cavern full of rare bioluminescent dumbo octopuses! They seem to not like the Christmas lights on your submarine, so you turn them off for now.
    /// There are 100 octopuses arranged neatly in a 10 by 10 grid.Each octopus slowly gains energy over time and flashes brightly for a moment when its energy is full.Although your lights are off, maybe you could navigate through the cave without disturbing the octopuses if you could predict when the flashes of light will happen.
    /// </summary>
    public class Day11
    {
        private string _day11input = @"6744638455
3135745418
4754123271
4224257161
8167186546
2268577674
7177768175
2662255275
4655343376
7852526168";
        private int[,] ParseInput(string input)
        {
            var lines = input.Split("\n").Select(x => x.Trim());
            var res = new int[lines.Count(), lines.Count()];
            for (int i = 0; i < lines.Count(); i++)
            {
                var line = lines.ElementAt(i);
                for (int j = 0; j < line.Length; j++)
                {
                    res.SetValue(int.Parse(line[j].ToString()), i, j);
                }
            }
            return res;

        }
        public int Quiz1()
        {
            var octopus = ParseInput(_day11input);
            var flashing = 0;
            for (int step = 0; step < 100; step++)
            {
                flashing += PerformStep(octopus);
            }
            return flashing;
        }

        public int Quiz2()
        {
            var octopus = ParseInput(_day11input);
            var flashing = 0;
            var numberOfOctopus = octopus.GetLength(0) * octopus.GetLength(1);
            int step = 0;
            while (flashing < numberOfOctopus)
            {
                flashing = PerformStep(octopus);
                step++;
            }
            return step;
        }

        private int PerformStep(int[,] octopus)
        {
            var maxLines = octopus.GetLength(0);
            var maxCols = octopus.GetLength(1);
            var flashed = 0;
            for (int i = 0; i < maxLines; i++)
            {
                for (int j = 0; j < maxCols; j++)
                {
                    octopus[i, j]++;
                }
            }

            for (int i = 0; i < maxLines; i++)
            {
                for (int j = 0; j < maxCols; j++)
                {
                    if (octopus[i, j] > 9)
                    {
                        flashed+= Flash(i, j, octopus);
                    }
                }
            }

            //for (int i = 0; i < maxLines; i++)
            //{
            //    for (int j = 0; j < maxCols; j++)
            //    {
            //        System.Diagnostics.Debug.Write(octopus[i, j]);
            //    }
            //    System.Diagnostics.Debug.WriteLine("");
            //}
            return flashed;
        }

        private int Flash(int flashingOctopusRow, int flashingOctopusCol, int[,] octopus)
        {
            var maxLines = octopus.GetLength(0);
            var maxCols = octopus.GetLength(1);
            var queue = new Queue<RowCol>();
            var flashingOctopus = new RowCol { Row = flashingOctopusRow, Col = flashingOctopusCol };
            queue.Enqueue(flashingOctopus); 
            var flashing = new List<RowCol>();
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                var currentValue = octopus[current.Row, current.Col];
                if(currentValue > 0)
                {
                    octopus[current.Row, current.Col]++;
                    currentValue = octopus[current.Row, current.Col];
                }

                if (currentValue > 9)
                {
                    octopus[current.Row, current.Col] = 0;
                    flashing.Add(current);
                    //west
                    if (current.Col - 1 >= 0) 
                        queue.Enqueue(new RowCol { Row = current.Row, Col = current.Col - 1 });
                    //north-west
                    if (current.Col - 1 >= 0 && current.Row - 1 >= 0)
                        queue.Enqueue(new RowCol { Row = current.Row - 1, Col = current.Col - 1 });
                    //north
                    if (current.Row - 1 >= 0)
                        queue.Enqueue(new RowCol { Row = current.Row - 1, Col = current.Col });
                    //north-east
                    if (current.Row - 1 >= 0 && current.Col + 1 < maxCols)
                        queue.Enqueue(new RowCol { Row = current.Row - 1, Col = current.Col + 1 });
                    //east
                    if (current.Col + 1 < maxCols)
                        queue.Enqueue(new RowCol { Row = current.Row, Col = current.Col + 1 });
                    //south-east
                    if (current.Row + 1 < maxLines && current.Col + 1 < maxCols)
                        queue.Enqueue(new RowCol { Row = current.Row + 1, Col = current.Col + 1 });
                    //south
                    if (current.Row + 1 < maxLines)
                        queue.Enqueue(new RowCol { Row = current.Row + 1, Col = current.Col });
                    //south-west
                    if (current.Row + 1 < maxLines && current.Col - 1 >= 0)
                        queue.Enqueue(new RowCol { Row = current.Row + 1, Col = current.Col - 1 });
                }
            }
            return flashing.Count;
        }
    }
}
