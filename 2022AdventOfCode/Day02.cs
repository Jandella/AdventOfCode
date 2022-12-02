using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2022AdventOfCode
{
    public class Day02 : AoCHelper.BaseDay
    {
        private string _input;
        public Day02()
        {
            _input = System.IO.File.ReadAllText(InputFilePath);
        }

        public Day02(string input)
        {
            _input = input;
        }

        private Dictionary<string, int> GetPermutations()
        {
            //A: rock (1 point)
            //B: paper (2 points)
            //C: scissor (3 points)
            //X: rock (1 point)
            //Y: paper (2 points)
            //Z: scissor (3 points)
            //match scores:
            //-loss 0 point
            //-draw 3 points
            //-win 6 points
            var dict = new Dictionary<string, int>()
            {
                ["A X"] = 1 + 3,
                ["A Y"] = 2 + 6,
                ["A Z"] = 3 + 0,
                ["B X"] = 1 + 0,
                ["B Y"] = 2 + 3,
                ["B Z"] = 3 + 6,
                ["C X"] = 1 + 6,
                ["C Y"] = 2 + 0,
                ["C Z"] = 3 + 3
            };
            return dict;
        }

        private Dictionary<string, int> GetPermutations_Solve2()
        {
            //A: rock (1 point)
            //B: paper (2 points)
            //C: scissor (3 points)
            //X: loss (0 point)
            //Y: draw (3 points)
            //Z: win (6 points)
            //match scores:
            //-loss 0 point
            //-draw 3 points
            //-win 6 points
            var dict = new Dictionary<string, int>()
            {
                ["A X"] = 0 + 3,
                ["A Y"] = 3 + 1,
                ["A Z"] = 6 + 2,
                ["B X"] = 0 + 1,
                ["B Y"] = 3 + 2,
                ["B Z"] = 6 + 3,
                ["C X"] = 0 + 2,
                ["C Y"] = 3 + 3,
                ["C Z"] = 6 + 1
            };
            return dict;
        }
        public override ValueTask<string> Solve_1()
        {
            var permutations = GetPermutations();
            var totalScore = 0;
            using (StringReader r = new(_input))
            {
                while (r.Peek() != -1)
                {
                    var line = r.ReadLine() ?? string.Empty;
                    if (permutations.ContainsKey(line))
                    {
                        totalScore+= permutations[line];
                    }
                }
            }
            return new (totalScore.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var permutations = GetPermutations_Solve2();
            var totalScore = 0;
            using (StringReader r = new(_input))
            {
                while (r.Peek() != -1)
                {
                    var line = r.ReadLine() ?? string.Empty;
                    if (permutations.ContainsKey(line))
                    {
                        totalScore += permutations[line];
                    }
                }
            }
            return new(totalScore.ToString());
        }
    }
}
