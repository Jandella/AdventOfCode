using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2022AdventOfCode.Tests
{
    public class Day12Tests
    {
        public string exampleInput = @"Sabqponm
abcryxxl
accszExk
acctuvwj
abdefghi";
        [Fact]
        public async Task TestDay12_Solve1()
        {
            var d = new Day12(exampleInput);
            var result = await d.Solve_1();
            Assert.Equal("31", result);

        }

        [Fact]
        public async Task TestDay12_Solve2()
        {
            var d = new Day12(exampleInput);
            var result = await d.Solve_2();
            Assert.Equal("29", result);

        }

        [Fact]
        public async Task TestDay12_Solve1_FileInput()
        {
            var d = new Day12();
            var result = await d.Solve_1();
            Assert.Equal("497", result);

        }

        [Fact]
        public async Task TestDay12_Solve2_FileInput()
        {
            var d = new Day12();
            var result = await d.Solve_2();
            Assert.Equal("492", result);

        }
    }
}
