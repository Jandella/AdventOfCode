using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2022AdventOfCode
{
    public class Day01 : AoCHelper.BaseDay
    {
        private string _input;
        public Day01()
        {
            _input = System.IO.File.ReadAllText(InputFilePath);
        }

        public Day01(string input)
        {
            _input = input;
        }

        public override ValueTask<string> Solve_1()
        {
            //trying to go for a single iteration
            var elvesCalories = new List<int>();
            int max = -1;
            int currentElfCalories = 0;
            using (StringReader r = new(_input))
            {
                while (r.Peek() != -1)
                {
                    var line = r.ReadLine();
                    if (int.TryParse(line, out int calories))
                    {
                        currentElfCalories += calories;
                    }
                    else
                    {
                        elvesCalories.Add(currentElfCalories);
                        if(max < currentElfCalories)
                        {
                            max = currentElfCalories;
                        }
                        currentElfCalories = 0;
                    }
                }
            }
            return new(max.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            //trying to go for a single iteration
            var topThreeElvesCalories = new List<int>();
            int currentElfCalories = 0;
            using (StringReader r = new(_input))
            {
                while (r.Peek() != -1)
                {
                    var line = r.ReadLine();
                    if (int.TryParse(line, out int calories))
                    {
                        currentElfCalories += calories;
                    }
                    else
                    {
                        if(topThreeElvesCalories.Count < 3)
                        {
                            topThreeElvesCalories.Add(currentElfCalories);
                        }
                        else
                        {
                            if(topThreeElvesCalories.ElementAt(0) < currentElfCalories)
                            {
                                topThreeElvesCalories.RemoveAt(0);
                                topThreeElvesCalories.Add(currentElfCalories);
                            }
                            else if(topThreeElvesCalories.ElementAt(1) < currentElfCalories)
                            {
                                topThreeElvesCalories.RemoveAt(1);
                                topThreeElvesCalories.Add(currentElfCalories);
                            }
                            else if (topThreeElvesCalories.ElementAt(2) < currentElfCalories)
                            {
                                topThreeElvesCalories.RemoveAt(2);
                                topThreeElvesCalories.Add(currentElfCalories);
                            }
                        }
                        currentElfCalories = 0;
                    }
                }
            }
            //manage last one
            if(currentElfCalories > 0)
            {
                if (topThreeElvesCalories.ElementAt(0) < currentElfCalories)
                {
                    topThreeElvesCalories.RemoveAt(0);
                    topThreeElvesCalories.Add(currentElfCalories);
                }
                else if (topThreeElvesCalories.ElementAt(1) < currentElfCalories)
                {
                    topThreeElvesCalories.RemoveAt(1);
                    topThreeElvesCalories.Add(currentElfCalories);
                }
                else if (topThreeElvesCalories.ElementAt(2) < currentElfCalories)
                {
                    topThreeElvesCalories.RemoveAt(2);
                    topThreeElvesCalories.Add(currentElfCalories);
                }
            }

            var sum = topThreeElvesCalories.Sum();
            return new(sum.ToString());
        }
    }
}
