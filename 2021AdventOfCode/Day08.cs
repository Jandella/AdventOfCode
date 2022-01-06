using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021AdventOfCode
{
    /// <summary>
    /// --- Day 8: Seven Segment Search ---
    /// You barely reach the safety of the cave when the whale smashes into the cave mouth, collapsing it.Sensors indicate another exit to this cave at a much greater depth, so you have no choice but to press on.
    /// As your submarine slowly makes its way through the cave system, you notice that the four-digit seven-segment displays in your submarine are malfunctioning; they must have been damaged during the escape. You'll be in a lot of trouble without them, so you'd better figure out what's wrong.
    /// </summary>
    public class Day08
    {
        private string _day8input = "";
        public Day08()
        {
            _day8input = System.IO.File.ReadAllText(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Inputs", "Day08Input.txt"));
        }
        private List<Display> ParseInput()
        {
            var lines = _day8input.Split('\n').Select(x => x.Trim());
            var res = new List<Display>();
            foreach (var line in lines)
            {
                var tmp = line.Split(" | ");
                res.Add(new Display()
                {
                    SignalPatterns = tmp[0].Split(' '),
                    FourDigitOutputValue = tmp[1].Split(' ')
                });
            }
            return res;
        }
        public int Quiz1()
        {
            var lines = ParseInput();
            var allDigitOutput = lines.SelectMany(x => x.FourDigitOutputValue);
            int count1 = allDigitOutput.Count(x => x.Length == Display.NumberOfSegmentsFor1);
            int count4 = allDigitOutput.Count(x => x.Length == Display.NumberOfSegmentsFor4);
            int count7 = allDigitOutput.Count(x => x.Length == Display.NumberOfSegmentsFor7);
            int count8 = allDigitOutput.Count(x => x.Length == Display.NumberOfSegmentsFor8);
            return count1 + count4 + count7 + count8;
        }

        public int Quiz2()
        {
            var lines = ParseInput();
            int sum = 0;
            var okDisplay = new Dictionary<string, int>()
            {
                ["abcefg"] = 0,
                ["cf"] = 1,
                ["acdeg"] = 2,
                ["acdfg"] = 3,
                ["bcdf"] = 4,
                ["abdfg"] = 5,
                ["abdefg"] = 6,
                ["acf"] = 7,
                ["abcdefg"] = 8,
                ["abcdfg"] = 9
            };
            foreach (var line in lines)
            {
                var cipher = GetCipher(line.SignalPatterns);
                foreach (var item in line.FourDigitOutputValue)
                {
                    var digit = "";
                    foreach (var c in item)
                    {
                        digit += cipher[c];
                    }
                    var ordered = string.Join("", digit.OrderBy(x => x));
                    line.FourDigits.Add(okDisplay[ordered]);
                }
                sum += int.Parse(string.Join("", line.FourDigits));
            }
            return sum;
        }

        Dictionary<char, char> GetCipher(string[] input)
        {
            var chars = "abcdefg".ToArray();

            var d1 = input.Where(i => i.Length == 2).First();
            var d4 = input.Where(i => i.Length == 4).First();
            var d7 = input.Where(i => i.Length == 3).First();

            var cipher = new Dictionary<char, char>();
            var a = d7.Except(d1);
            cipher[a.First()] = 'a';
            var b = chars.Where(c => input.Count(i => i.Contains(c)) == 6);
            cipher[b.First()] = 'b';
            cipher[chars.Where(c => input.Count(i => i.Contains(c)) == 8).Except(a).First()] = 'c';
            var d = d4.Except(d1).Except(b);
            cipher[d.First()] = 'd';
            cipher[chars.Where(c => input.Count(i => i.Contains(c)) == 4).First()] = 'e';
            cipher[chars.Where(c => input.Count(i => i.Contains(c)) == 9).First()] = 'f';
            cipher[chars.Where(c => input.Count(i => i.Contains(c)) == 7).Except(d).First()] = 'g';

            return cipher;
        }
        private void Boh(Display d)
        {
            var d1 = d.SignalPatterns.First(x => x.Length == Display.NumberOfSegmentsFor1);
            var d4 = d.SignalPatterns.First(x => x.Length == Display.NumberOfSegmentsFor4);
            var d7 = d.SignalPatterns.First(x => x.Length == Display.NumberOfSegmentsFor7);
            var d8 = d.SignalPatterns.First(x => x.Length == Display.NumberOfSegmentsFor8);
            var display = new Dictionary<string, int>();
            foreach (var item in d.SignalPatterns)
            {
                switch (item.Length)
                {
                    case Display.NumberOfSegmentsFor1:
                        display[item] = 1;
                        break;
                    case Display.NumberOfSegmentsFor4:
                        display[item] = 4;
                        break;
                    case Display.NumberOfSegmentsFor7:
                        display[item] = 7;
                        break;
                    case Display.NumberOfSegmentsFor8:
                        display[item] = 8;
                        break;
                    case Display.NumberOfSegmentFor2_3_5:
                        break;
                    case Display.NumberOfSegmentFor0_6_9:
                        string sorted = item.OrderBy(x => x).ToString();
                        break;
                    default:
                        break;
                }
            }

            
        }
    }

    public class Display
    {
        public const int NumberOfSegmentsFor1 = 2;
        public const int NumberOfSegmentsFor4 = 4;
        public const int NumberOfSegmentsFor7 = 3;
        public const int NumberOfSegmentsFor8 = 7;
        public const int NumberOfSegmentFor0_6_9 = 6;
        public const int NumberOfSegmentFor2_3_5 = 5;
        public string[] SignalPatterns { get; set; } = new string[0];
        public string[] FourDigitOutputValue { get; set; } = new string[0];
        public List<int> FourDigits { get; set; } = new List<int>();
    }


}
