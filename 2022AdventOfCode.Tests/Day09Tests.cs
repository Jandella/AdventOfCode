using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2022AdventOfCode.Tests
{
    public class Day09Tests
    {
        public string exampleInput = @"R 4
U 4
L 3
D 1
R 4
D 1
L 5
R 2";
        [Fact]
        public async Task TestDay9_Solve1()
        {
            var d = new Day09(exampleInput);
            var result = await d.Solve_1();
            Assert.Equal("13", result);

        }

        [Fact]
        public async Task TestDay9_Solve2()
        {
            var d = new Day09(exampleInput);
            var result = await d.Solve_2();
            Assert.Equal("", result);

        }

        [Fact]
        public async Task TestDay9_Solve1_FileInput()
        {
            var d = new Day09();
            var result = await d.Solve_1();
            Assert.Equal("", result);

        }

        [Fact]
        public async Task TestDay9_Solve2_FileInput()
        {
            var d = new Day09();
            var result = await d.Solve_2();
            Assert.Equal("", result);

        }
    }
}
