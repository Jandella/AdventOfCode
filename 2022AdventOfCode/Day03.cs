using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2022AdventOfCode
{
    public class Day03 : AoCHelper.BaseDay
    {
        private string _input;
        public Day03()
        {
            _input = System.IO.File.ReadAllText(InputFilePath);
        }
        public Day03(string input)
        {
            _input = input;
        }
        private Dictionary<char, int> GetPriorities()
        {
            var count = 1;
            Dictionary<char, int> priorities = new Dictionary<char, int>();
            for (char i = 'a'; i <= 'z'; i++)
            {
                priorities.Add(i, count++);
            }
            for (char i = 'A'; i <= 'Z'; i++)
            {
                priorities.Add(i, count++);
            }
            return priorities;
        }
        public string[] GetCompartments(string rucksack)
        {
            var len = rucksack.Length;
            var half = len / 2;
            var result = new string[2];
            result[0] = rucksack.Substring(0, half);
            result[1] = rucksack.Substring(half);
            return result;
        } 
        public char FindSame(string compartment1, string compartment2)
        {
            var res = '\0';
            foreach (var item in compartment1)
            {
                if (compartment2.Contains(item))
                {
                    res = item;
                    break;
                }
            }
            return res;
        }
        public char FindBadge(List<string> group)
        {
            var res = '\0';
            var orderedGroup = group.OrderByDescending(x => x.Length).ToList();
            var sack1 = orderedGroup.ElementAt(0);
            var sack2 = orderedGroup.ElementAt(1);
            var sack3 = orderedGroup.ElementAt(2);
            foreach (var item in sack1)
            {
                if(sack2.Contains(item) && sack3.Contains(item))
                {
                    res = item;
                    break;
                }
            }
            return res;
        }
        public override ValueTask<string> Solve_1()
        {
            var priorities = GetPriorities();
            int totalScore = 0;
            using (StringReader r = new(_input))
            {
                while (r.Peek() != -1)
                {
                    string rucksack = r.ReadLine() ?? string.Empty;
                    var compartments = GetCompartments(rucksack);
                    var item = FindSame(compartments[0], compartments[1]);

                    totalScore += priorities[item];
                }
            }

            return new(totalScore.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var priorities = GetPriorities();
            int totalScore = 0;
            var group = new List<string>();            
            using (StringReader r = new(_input))
            {
                while (r.Peek() != -1)
                {
                    string rucksack = r.ReadLine() ?? string.Empty;
                    group.Add(rucksack);
                    if (group.Count == 3)
                    {
                        var badge = FindBadge(group);
                        totalScore += priorities[badge];
                        group.Clear();
                    }
                }
            }
            
            return new(totalScore.ToString());
        }
    }
}
