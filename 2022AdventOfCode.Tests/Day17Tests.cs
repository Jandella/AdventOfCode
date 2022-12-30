using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2022AdventOfCode.Tests
{
    public class Day17Tests
    {
        public string exampleInput = ">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>";
        [Fact]
        public async Task TestDay17_Solve1()
        {
            var d = new Day17(exampleInput);
            var result = await d.Solve_1();
            Assert.Equal("3068", result);

        }

        [Fact]
        public async Task TestDay17_Solve2()
        {
            var d = new Day17(exampleInput);
            var result = await d.Solve_2();
            Assert.Equal("", result);

        }

        [Fact]
        public async Task TestDay17_Solve1_FileInput()
        {
            var d = new Day17();
            var result = await d.Solve_1();
            Assert.Equal("", result);

        }

        [Fact]
        public async Task TestDay17_Solve2_FileInput()
        {
            var d = new Day17();
            var result = await d.Solve_2();
            Assert.Equal("", result);

        }
    }
}
