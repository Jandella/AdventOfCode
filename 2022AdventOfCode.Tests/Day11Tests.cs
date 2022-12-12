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
            Assert.Equal(79, peeked.WorryLevel);
            var espected = 79 * 19;
            var actual = m.Operation(peeked);
            Assert.Equal(espected, actual);
            Assert.Equal(23, m.Test);
            Assert.Equal(2, m.IfTrue);
            Assert.Equal(3, m.IfFalse);

        }

        [Fact]
        public async Task TestDay11_Solve1()
        {
            var d = new Day11(exampleInput);
            var result = await d.Solve_1();
            Assert.Equal("10605", result);

        }

        [Fact]
        public async Task TestDay11_Solve2()
        {
            var d = new Day11(exampleInput);
            var result = await d.Solve_2();
            Assert.Equal("", result);

        }

        [Fact]
        public async Task TestDay11_Solve1_FileInput()
        {
            var d = new Day11();
            var result = await d.Solve_1();
            Assert.Equal("", result);

        }

        [Fact]
        public async Task TestDay11_Solve2_FileInput()
        {
            var d = new Day11();
            var result = await d.Solve_2();
            Assert.Equal("", result);

        }
    }
}
