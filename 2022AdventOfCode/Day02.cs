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
            //A: rock
            //B: paper
            //C: scissor
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
            throw new NotImplementedException();
        }
    }
}
