using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2022AdventOfCode
{
    public class Day04 : AoCHelper.BaseDay
    {
        private string _input;
        public Day04()
        {
            _input = System.IO.File.ReadAllText(InputFilePath);
        }
        public Day04(string input)
        {
            _input = input;
        }

        public override ValueTask<string> Solve_1()
        {
            int countOverlappingSections = 0;
            using (StringReader r = new(_input))
            {
                while (r.Peek() != -1)
                {
                    string pairs = r.ReadLine() ?? string.Empty;
                    ElfPairSections elfPairs = new (pairs);
                    if(elfPairs.FirstElfSection.Contains(elfPairs.SecondElfSection)
                        || elfPairs.SecondElfSection.Contains(elfPairs.FirstElfSection))
                    {
                        countOverlappingSections++;
                    }
                }
            }
            return new(countOverlappingSections.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            int countOverlappingSections = 0;
            using (StringReader r = new(_input))
            {
                while (r.Peek() != -1)
                {
                    string pairs = r.ReadLine() ?? string.Empty;
                    ElfPairSections elfPairs = new(pairs);
                    if (elfPairs.FirstElfSection.Overlaps(elfPairs.SecondElfSection)
                        || elfPairs.SecondElfSection.Overlaps(elfPairs.FirstElfSection))
                    {
                        countOverlappingSections++;
                    }
                }
            }
            return new(countOverlappingSections.ToString());
        }
    }
    //defining some classes down here to help tidy up
    public class Section {
        public Section()
        {

        }
        public Section(string section)
        {
            var splitted = section.Split('-');
            Start = int.Parse(splitted[0]);
            End = int.Parse(splitted[1]);
        }
        public int Start { get; set; }
        public int End { get; set; }
        public bool Contains(Section b)
        {
            return Start <= b.Start && b.End <= End;
        }
        public bool Overlaps(Section b)
        {
            return (End >= b.Start && End <= b.End) 
                || (Start >= b.Start && Start <= b.End)
                || Contains(b);
        }
        public override string ToString()
        {
            return Start + "-" + End;
        }
    }

    public class ElfPairSections
    {
        public ElfPairSections()
        {
            FirstElfSection = new Section();
            SecondElfSection = new Section();
        }
        public ElfPairSections(string pairs)
        {
            var splitted = pairs.Split(",");
            FirstElfSection = new Section(splitted[0]);
            SecondElfSection = new Section(splitted[1]);
        }
        public Section FirstElfSection { get; set; }
        public Section SecondElfSection { get; set; }
    }
}
