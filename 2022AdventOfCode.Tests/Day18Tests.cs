using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2022AdventOfCode.Tests
{
    public class Day18Tests
    {
        public string exampleInput = @"2,2,2
1,2,2
3,2,2
2,1,2
2,3,2
2,2,1
2,2,3
2,2,4
2,2,6
1,2,5
3,2,5
2,1,5
2,3,5";
        [Fact]
        public async Task TestDay18_Solve1_SmallInput()
        {
            string smallInput = @"1,1,1
2,1,1";
            var d = new Day18(smallInput);
            var result = await d.Solve_1();
            Assert.Equal("10", result);

        }
        [Fact]
        public async Task TestDay18_Solve1()
        {
            var d = new Day18(exampleInput);
            var result = await d.Solve_1();
            Assert.Equal("64", result);

        }

        [Fact]
        public async Task TestDay18_Solve2()
        {
            var d = new Day18(exampleInput);
            var result = await d.Solve_2();
            Assert.Equal("58", result);

        }

        [Fact]
        public async Task TestDay18_Solve1_FileInput()
        {
            var d = new Day18();
            var result = await d.Solve_1();
            Assert.Equal("3542", result);

        }

        [Fact]
        public async Task TestDay18_Solve2_FileInput()
        {
            var d = new Day18();
            var result = await d.Solve_2();
            Assert.Equal("", result);

        }
    }
}
