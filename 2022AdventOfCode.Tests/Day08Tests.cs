using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2022AdventOfCode.Tests
{
    public class Day08Tests
    {
        public string exampleInput = @"30373
25512
65332
33549
35390";
        [Fact]
        public async Task TestDay8_Solve1()
        {
            var d = new Day08(exampleInput);
            var result = await d.Solve_1();
            Assert.Equal("21", result);

        }

        [Fact]
        public async Task TestDay8_Solve2()
        {
            var d = new Day08(exampleInput);
            var result = await d.Solve_2();
            Assert.Equal("", result);

        }

        [Fact]
        public async Task TestDay8_Solve1_FileInput()
        {
            var d = new Day08();
            var result = await d.Solve_1();
            Assert.Equal("", result);

        }

        [Fact]
        public async Task TestDay8_Solve2_FileInput()
        {
            var d = new Day08();
            var result = await d.Solve_2();
            Assert.Equal("", result);

        }
    }
}
