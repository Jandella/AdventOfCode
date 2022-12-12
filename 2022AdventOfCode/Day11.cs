using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2022AdventOfCode
{
    public class Day11 : AoCHelper.BaseDay
    {
        private string _input;
        public Day11()
        {
            _input = System.IO.File.ReadAllText(InputFilePath);
        }
        public Day11(string input)
        {
            _input = input;
        }

        public override ValueTask<string> Solve_1()
        {
            var game = new KeepAwayGame(_input);
            for (int round = 1; round <= 20; round++)
            {
                foreach (var m in game.Monkeys)
                {
                    game.PlayTurn(m.Value);
                }
            }
            var topTwoActive = game.Monkeys.OrderByDescending(x => x.Value.Activity).Take(2).ToList();
            var total = topTwoActive[0].Value.Activity * topTwoActive[1].Value.Activity;
            return new ValueTask<string>(total.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            throw new NotImplementedException();
        }
    }

    public class KeepAwayGame
    {
        public KeepAwayGame(string input)
        {
            var splitted = input.Split(Environment.NewLine + Environment.NewLine);
            Monkeys = new Dictionary<int, Monkey>();
            foreach (var item in splitted)
            {
                var m = new Monkey(item);
                Monkeys[m.MonkeyId] = m;
            }
        }
        public Dictionary<int, Monkey> Monkeys { get; set; }

        public void PlayTurn(Monkey m)
        {
            while (m.Stuff.Any())
            {
                var item = m.Stuff.Dequeue();
                //inspect
                item.WorryLevel = m.Operation(item);
                m.Activity++;
                //monkeys get bored
                item.WorryLevel = (long)Math.Floor((item.WorryLevel / 3.0));
                //test worry
                int monkeyToThrow = 0;
                if(item.WorryLevel % m.Test == 0)
                {
                    monkeyToThrow = m.IfTrue;
                }
                else
                {
                    monkeyToThrow = m.IfFalse;
                }
                //throw to the other monkey
                Monkeys[monkeyToThrow].Stuff.Enqueue(item);
            }
        }
    }

    public class Monkey
    {
        private string _operation;
        public Monkey(string monkeyDesc)
        {
            /*
             * Monkey 0:
  Starting items: 79, 98
  Operation: new = old * 19
  Test: divisible by 23
    If true: throw to monkey 2
    If false: throw to monkey 3
             */
            Stuff = new Queue<Item>();
            var splitted = monkeyDesc.Split(Environment.NewLine);
            MonkeyId = ParseMonkeyId(splitted[0]);
            Stuff = ParseItems(splitted[1]);
            _operation = splitted[2];
            Operation = ParseOperation(splitted[2]);
            Test = ParseLastInt(splitted[3]);
            IfTrue = ParseLastInt(splitted[4]);
            IfFalse = ParseLastInt(splitted[5]);
        }
        private int ParseMonkeyId(string line)
        {
            //Monkey 0:
            var splitted = line.Trim().Split(' ');
            var numberAsString = splitted[1][..splitted[1].IndexOf(':')];
            return int.Parse(numberAsString);
        }
        private Queue<Item> ParseItems(string line)
        {
            //Starting items: 79, 98
            var firstSplit = line.Trim().Split(':');
            var secondSplit = firstSplit[1].Split(',');
            var items = new Queue<Item>();
            foreach (var item in secondSplit)
            {
                items.Enqueue(new Item
                {
                    WorryLevel = long.Parse(item.Trim())
                });
            }
            return items;
        }
        private Func<Item,long> ParseOperation(string line)
        {
            //Operation: new = old * 19
            //[0] Operation:
            //[1] new
            //[2] =
            //[3] old
            //[4] *
            //[5] 19
            var splitted = line.Trim().Split(' ');
            long? worryFactor = null;
            long parsedWorryFactor = 0;
            if(long.TryParse(splitted[5], out parsedWorryFactor))
            {
                worryFactor = parsedWorryFactor;
            }
            Func<Item, long> res;
            switch (splitted[4])
            {
                case "+":
                    res = (i) =>
                    {
                        
                        return i.WorryLevel + (worryFactor ?? i.WorryLevel);
                    };
                    break;
                case "*":
                    res = (i) =>
                    {
                        return i.WorryLevel * (worryFactor ?? i.WorryLevel);
                    };
                    break;
                default:
                    res = (i) =>
                    {
                        return i.WorryLevel;;
                    };
                    break;
            }
            return res;

        }
        private int ParseLastInt(string line)
        {
            //Test: divisible by 23
            //If true: throw to monkey 2
            //If false: throw to monkey 3
            var splitted = line.Trim().Split(' ');
            return int.Parse(splitted.Last());
        }

        public int MonkeyId { get; set; }
        public Queue<Item> Stuff { get; set; }
        public Func<Item, long> Operation { get; set; }
        public int Test { get; set; }
        public int IfTrue { get; set; }
        public int IfFalse { get; set; }
        public long Activity { get; set; }
        public override string ToString()
        {
            var s = @"Monkey {0}:
  Items: {1}
{2}
  Test: divisible by {3}
    If true: throw to monkey {4}
    If false: throw to monkey {5}
  Activity: {6}";
            return string.Format(s, MonkeyId, 
                string.Join(", ", Stuff.Select(x => x.WorryLevel)), 
                _operation,
                Test, IfTrue, IfFalse, Activity);
        }
    }

    public class Item
    {
        public long WorryLevel { get; set; }
    }
}
