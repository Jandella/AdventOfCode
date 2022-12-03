using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2022AdventOfCode.Tests
{
    public class Day02Tests
    {
        [Fact]
        public async Task TestDay2_Solve1()
        {
            string input = @"A Y
B X
C Z";
            var d = new Day02(input);
            var result = await d.Solve_1();
            Assert.Equal("15", result);

        }

        [Fact]
        public async Task TestDay2_Solve2()
        {
            string input = @"A Y
B X
C Z";
            var d = new Day02(input);
            var result = await d.Solve_2();
            Assert.Equal("12", result);

        }

        [Fact]
        public async Task TestDay2_Solve1_FileInput()
        {
            var d = new Day02();
            var result = await d.Solve_1();
            Assert.Equal("11906", result);

        }

        [Fact]
        public async Task TestDay2_Solve2_FileInput()
        {
            var d = new Day02();
            var result = await d.Solve_2();
            Assert.Equal("11186", result);

        }
    }
}
