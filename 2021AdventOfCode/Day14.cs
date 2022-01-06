using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace _2021AdventOfCode
{

    /// <summary>
    /// --- Day 14: Extended Polymerization ---
    /// The incredible pressures at this depth are starting to put a strain on your submarine. 
    /// The submarine has polymerization equipment that would produce suitable materials to reinforce the submarine, and the nearby volcanically-active caves should even have the necessary input elements in sufficient quantities.
    /// </summary>
    public class Day14
    {
        private string _day14Input = "";
        public Day14()
        {
            _day14Input = System.IO.File.ReadAllText(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Inputs", "Day14Input.txt"));
        }
        public int Quiz1()
        {
            var formula = new PolymerFormula(_day14Input);
            var iterator = formula.PolymerTemplate;
            for (int i = 1; i <= 10; i++)
            {
                iterator = formula.PerformStep(iterator);
            }
            var groupedChar = iterator.GroupBy(x => x);
            var mostCommonElement = groupedChar.Max(x => x.Count());
            var leastCommonElement = groupedChar.Min(x => x.Count());
            return mostCommonElement - leastCommonElement;
        }

        public ulong Quiz2()
        {
            var formula = new PolymerFormula(_day14Input);
            var pairs = formula.PerformStepsTrackingPairs(40);
            var charDict = new Dictionary<char, ulong>();
            foreach (var item in pairs)
            {
                if (!charDict.ContainsKey(item.Key[0]))
                    charDict.Add(item.Key[0], 0);
                charDict[item.Key[0]] += item.Value;

            }
            var mostCommonElement = charDict.Max(x => x.Value);
            var leastCommonElement = charDict.Min(x => x.Value);
            return mostCommonElement - leastCommonElement;
        }
    }

    public class PolymerFormula
    {
        public PolymerFormula(string input)
        {
            var lines = input.Split("\n").Select(x => x.Trim()).ToArray();
            PolymerTemplate = lines[0];
            for (int i = 2; i < lines.Length; i++)
            {
                var line = lines[i];
                var instruction = line.Split(" -> ");
                PairInsertions.Add(instruction[0], instruction[1][0]);
            }
        }
        public string PolymerTemplate { get; set; } = string.Empty;
        public Dictionary<string, char> PairInsertions { get; set; } = new Dictionary<string, char>();

        public string PerformStep(string input)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < input.Length - 1; i++)
            {
                var charsSb = new StringBuilder();
                charsSb.Append(input[i]);
                charsSb.Append(input[i + 1]);
                sb.Append(input[i]);
                sb.Append(PairInsertions[charsSb.ToString()]);
            }
            //adding last char
            sb.Append(input[input.Length - 1]);
            return sb.ToString();
        }

        public Dictionary<string, ulong> PerformStepsTrackingPairs(int numberOfSteps)
        {
            var trackPair = new Dictionary<string, ulong>();
            var charsSb = new StringBuilder();
            for (int i = 0; i < PolymerTemplate.Length - 1; i++)
            {
                charsSb.Clear();
                charsSb.Append(PolymerTemplate[i]).Append(PolymerTemplate[i + 1]);
                trackPair.Add(charsSb.ToString(), 1);
            }

            for (int i = 1; i <= numberOfSteps; i++)
            {
                var updatePair = new Dictionary<string, ulong>();
                foreach (var item in trackPair)
                {
                    charsSb.Clear();
                    charsSb.Append(item.Key[0]).Append(PairInsertions[item.Key]);
                    var firstCouple = charsSb.ToString();
                    charsSb.Clear();
                    charsSb.Append(PairInsertions[item.Key]).Append(item.Key[1]);
                    var secondCouple = charsSb.ToString();
                    if (!updatePair.ContainsKey(firstCouple))
                        updatePair.Add(firstCouple, 0);
                    if (!updatePair.ContainsKey(secondCouple))
                        updatePair.Add(secondCouple, 0);
                    updatePair[firstCouple]+= item.Value;
                    updatePair[secondCouple]+= item.Value;
                }
                trackPair = updatePair;
            }
            return trackPair;
        }
    }
}
