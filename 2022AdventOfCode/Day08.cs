using System;
using System.Collections.Generic;
using System.Linq;
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
            throw new NotImplementedException();
        }

        public override ValueTask<string> Solve_2()
        {
            throw new NotImplementedException();
        }
    }
}
