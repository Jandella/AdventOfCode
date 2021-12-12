using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _2021AdventOfCode
{

    /// <summary>
    /// --- Day 6: Lanternfish ---
    /// The sea floor is getting steeper.Maybe the sleigh keys got carried this way?
    /// A massive school of glowing lanternfish swims past.They must spawn quickly to reach such large numbers - maybe exponentially quickly? You should model their growth rate to be sure.
    /// Although you know nothing about this specific species of lanternfish, you make some guesses about their attributes. Surely, each lanternfish creates a new lanternfish once every 7 days.
    /// However, this process isn't necessarily synchronized between every lanternfish - one lanternfish might have 2 days left until it creates another lanternfish, while another might have 4. So, you can model each fish as a single number that represents the number of days until it creates a new lanternfish.
    /// Furthermore, you reason, a new lanternfish would surely need slightly longer before it's capable of producing more lanternfish: two more days for its first cycle.
    /// </summary>
    public class Day06
    {
        private string _day6input = @"1,1,1,3,3,2,1,1,1,1,1,4,4,1,4,1,4,1,1,4,1,1,1,3,3,2,3,1,2,1,1,1,1,1,1,1,3,4,1,1,4,3,1,2,3,1,1,1,5,2,1,1,1,1,2,1,2,5,2,2,1,1,1,3,1,1,1,4,1,1,1,1,1,3,3,2,1,1,3,1,4,1,2,1,5,1,4,2,1,1,5,1,1,1,1,4,3,1,3,2,1,4,1,1,2,1,4,4,5,1,3,1,1,1,1,2,1,4,4,1,1,1,3,1,5,1,1,1,1,1,3,2,5,1,5,4,1,4,1,3,5,1,2,5,4,3,3,2,4,1,5,1,1,2,4,1,1,1,1,2,4,1,2,5,1,4,1,4,2,5,4,1,1,2,2,4,1,5,1,4,3,3,2,3,1,2,3,1,4,1,1,1,3,5,1,1,1,3,5,1,1,4,1,4,4,1,3,1,1,1,2,3,3,2,5,1,2,1,1,2,2,1,3,4,1,3,5,1,3,4,3,5,1,1,5,1,3,3,2,1,5,1,1,3,1,1,3,1,2,1,3,2,5,1,3,1,1,3,5,1,1,1,1,2,1,2,4,4,4,2,2,3,1,5,1,2,1,3,3,3,4,1,1,5,1,3,2,4,1,5,5,1,4,4,1,4,4,1,1,2";
        private List<LanternFish> ParseInput()
        {
            return _day6input.Split(',').Select(x => new LanternFish(int.Parse(x))).ToList();
        }
        
        internal int Quiz1()
        {
            var lanternFishes = ParseInput();
            for (int day = 1; day <= 80; day++)
            {
                var newFishes = new List<LanternFish>();
                foreach (var fish in lanternFishes)
                {
                    var newFish = fish.Spawn();
                    if(newFish != null)
                    {
                        newFishes.Add(newFish);
                    }
                }
                if (newFishes.Any())
                    lanternFishes.AddRange(newFishes);
            }
            return lanternFishes.Count();
        }

        internal long Quiz2()
        {
            var days = 256;
            var lanternFishes = ParseInput().ToArray();
            long[] newbirths = new long[days];

            IEnumerable<int> births(int initialAge, int daysLeft) => Enumerable.Range(0, 50)
                .Select(x => x * 7 + initialAge)
                .Where(x => x < daysLeft);

            foreach (var fish in lanternFishes)
            {
                foreach (var birthday in births(fish.Age, days))
                {
                    newbirths[birthday] += 1;
                }
            }

            for (int day = 0; day < days; day++)
            {
                var newBorn = newbirths[day];

                foreach (var birthday in births(8, days - day - 1))
                {
                    newbirths[day + 1 + birthday] += newBorn;
                }
            }

            return lanternFishes.Length + newbirths.Sum();
        }
    }

    public class LanternFish
    {
        private int _internalTimer = 0;

        public int Age => _internalTimer;
        public LanternFish()
        {
            _internalTimer = 8;
        }
        public LanternFish(int start)
        {
            _internalTimer = start;
        }

        public LanternFish Spawn()
        {
            if(_internalTimer == 0)
            {
                _internalTimer = 6;
                return new LanternFish();
            }
            else
            {
                _internalTimer--;
            }
            return null;
        }
    }
}
