using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2022AdventOfCode
{
    public class Day06 : AoCHelper.BaseDay
    {
        private string _input;
        public Day06()
        {
            _input = System.IO.File.ReadAllText(InputFilePath);
        }
        public Day06(string input)
        {
            _input = input;
        }
        public int Marker(int length)
        {
            var set = new List<char>();
            bool trovato = false;
            int index = 1;
            using (StringReader r = new(_input))
            {
                while (r.Peek() != -1 && !trovato)
                {
                    var nextChar = Convert.ToChar(r.Read());

                    set.Add(nextChar);

                    if (set.Count > length)
                    {
                        set.RemoveAt(0);
                    }

                    if (set.Count == length)
                    {
                        if (set.Distinct().Count() == length)
                        {
                            trovato = true;
                        }
                    }

                    if (!trovato)
                    {
                        index++;
                    }
                }

            }
            return index;
        }
        public override ValueTask<string> Solve_1()
        {
            int index = Marker(4);
            return new ValueTask<string>(index.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            int index = Marker(14);
            return new ValueTask<string>(index.ToString());
        }
    }
}
