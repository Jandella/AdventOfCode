using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2022AdventOfCode.Tests
{
    public class Day11Tests
    {
        public string exampleInput = @"Monkey 0:
  Starting items: 79, 98
  Operation: new = old * 19
  Test: divisible by 23
    If true: throw to monkey 2
    If false: throw to monkey 3

Monkey 1:
  Starting items: 54, 65, 75, 74
  Operation: new = old + 6
  Test: divisible by 19
    If true: throw to monkey 2
    If false: throw to monkey 0

Monkey 2:
  Starting items: 79, 60, 97
  Operation: new = old * old
  Test: divisible by 13
    If true: throw to monkey 1
    If false: throw to monkey 3

Monkey 3:
  Starting items: 74
  Operation: new = old + 3
  Test: divisible by 17
    If true: throw to monkey 0
    If false: throw to monkey 1";

        [Fact]
        public void TestParseMonkey()
        {
            var m = new Monkey(@"Monkey 0:
  Starting items: 79, 98
  Operation: new = old * 19
  Test: divisible by 23
    If true: throw to monkey 2
    If false: throw to monkey 3");
            Assert.Equal(0, m.MonkeyId);
            Assert.Equal(2, m.Stuff.Count);
            var peeked = m.Stuff.Peek();
            Assert.Equal((long)79, peeked.WorryLevel);
            long espected = 79 * 19;
            var actual = m.Operation(peeked);
            Assert.Equal(espected, actual);
            Assert.Equal(23, m.Test);
            Assert.Equal(2, m.IfTrue);
            Assert.Equal(3, m.IfFalse);

        }
        [Fact]
        public void TestDay11_1Round()
        {
            var game = new KeepAwayGame(exampleInput);
            var monkey0 = game.Monkeys[0];
            game.PlayTurn(monkey0);
            Assert.Empty(monkey0.Stuff);
            var monkey1 = game.Monkeys[1];
            game.PlayTurn(monkey1);
            Assert.Empty(monkey1.Stuff);
            var monkey2 = game.Monkeys[2];
            game.PlayTurn(monkey2);
            Assert.Empty(monkey2.Stuff);
            var monkey3 = game.Monkeys[3];
            game.PlayTurn(monkey3);
            Assert.Empty(monkey3.Stuff);
            //after round 1
            //Monkey 0: 20, 23, 27, 26
            //Monkey 1: 2080, 25, 167, 207, 401, 1046
            //Monkey 2: 
            //Monkey 3: 
            Assert.Equal(4, monkey0.Stuff.Count);
            Assert.Equal(6, monkey1.Stuff.Count);
            Assert.Empty(monkey2.Stuff);
            Assert.Empty(monkey3.Stuff);
        }
        [Fact]
        public void TestDay11_PlayRound()
        {
            var game = new KeepAwayGame(exampleInput);
            var monkey0 = game.Monkeys[0];
            var monkey1 = game.Monkeys[1];
            var monkey2 = game.Monkeys[2];
            var monkey3 = game.Monkeys[3];
            game.PlayRound();
            //after round 1
            //Monkey 0: 20, 23, 27, 26
            //Monkey 1: 2080, 25, 167, 207, 401, 1046
            //Monkey 2: 
            //Monkey 3: 
            Assert.Equal(4, monkey0.Stuff.Count);
            Assert.Equal(6, monkey1.Stuff.Count);
            Assert.Empty(monkey2.Stuff);
            Assert.Empty(monkey3.Stuff);
        }
        [Fact]
        public async Task TestDay11_Solve1()
        {
            var d = new Day11(exampleInput);
            var result = await d.Solve_1();
            Assert.Equal("10605", result);

        }

        [Fact]
        public void TestDay11_NoWorry_PlayRound()
        {
            var game = new KeepAwayGame(exampleInput, false);
            var monkey0 = game.Monkeys[0];
            var monkey1 = game.Monkeys[1];
            var monkey2 = game.Monkeys[2];
            var monkey3 = game.Monkeys[3];
            game.PlayRound();
            //after round 1
            //Monkey 0: 20, 23, 27, 26
            //Monkey 1: 2080, 25, 167, 207, 401, 1046
            //Monkey 2: 
            //Monkey 3: 
            Assert.Equal(2, monkey0.Activity);
            Assert.Equal(4, monkey1.Activity);
            Assert.Equal(3, monkey2.Activity);
            Assert.Equal(6, monkey3.Activity);

        }

        [Fact]
        public void TestDay11_NoWorry_Play20Round()
        {
            var game = new KeepAwayGame(exampleInput, false);
            var monkey0 = game.Monkeys[0];
            var monkey1 = game.Monkeys[1];
            var monkey2 = game.Monkeys[2];
            var monkey3 = game.Monkeys[3];
            for (int i = 1; i <= 20; i++)
            {
                game.PlayRound();
            }

            //== After round 20 ==
            //Monkey 0 inspected items 99 times.
            //Monkey 1 inspected items 97 times.
            //Monkey 2 inspected items 8 times.
            //Monkey 3 inspected items 103 times.
            Assert.Equal(99, monkey0.Activity);
            Assert.Equal(97, monkey1.Activity);
            Assert.Equal(8, monkey2.Activity);
            Assert.Equal(103, monkey3.Activity);

        }
        [Fact]
        public async Task TestDay11_Solve2()
        {
            var d = new Day11(exampleInput);
            var result = await d.Solve_2();
            Assert.Equal("2713310158", result);

        }

        [Fact]
        public async Task TestDay11_Solve1_FileInput()
        {
            var d = new Day11();
            var result = await d.Solve_1();
            Assert.Equal("110264", result);

        }

        [Fact]
        public async Task TestDay11_Solve2_FileInput()
        {
            var d = new Day11();
            var result = await d.Solve_2();
            Assert.Equal("23612457316", result);

        }
    }
}
