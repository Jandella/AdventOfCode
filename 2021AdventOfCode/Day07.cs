using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021AdventOfCode
{
    /// <summary>
    /// --- Day 7: The Treachery of Whales ---
    /// A giant whale has decided your submarine is its next meal, and it's much faster than you are. There's nowhere to run!
    /// Suddenly, a swarm of crabs(each in its own tiny submarine - it's too deep for them otherwise) zooms in to rescue you! They seem to be preparing to blast a hole in the ocean floor; sensors indicate a massive underground cave system just beyond where they're aiming!
    /// </summary>
    public class Day07
    {
        private string _day07input = "";
        public Day07()
        {
            _day07input = System.IO.File.ReadAllText(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Inputs", "Day07Input.txt"));
        }
        public int Quiz1()
        {
            var positions = _day07input.Split(',').Select(x => int.Parse(x)).ToArray();
            var min = positions.Min();
            var max = positions.Max();
            var enumerateAllPos = Enumerable.Range(min, max);
            var costs = enumerateAllPos
                .Select(x => positions.Select(p => Math.Abs(x - p)));
            var minFuel = costs.Min(d => d.Sum());
            return minFuel;
        }

        public int Quiz2()
        {
            var positions = _day07input.Split(',').Select(x => int.Parse(x)).ToArray();
            var min = positions.Min();
            var max = positions.Max();
            var enumerateAllPos = Enumerable.Range(min, max);
            var costs = enumerateAllPos
                .Select(x => positions.Select(p => (Math.Abs(x - p)*(Math.Abs(x - p) +1))/2m));
            var minFuel = (int)costs.Min(d => d.Sum());
            return minFuel;
        }
    }
}
