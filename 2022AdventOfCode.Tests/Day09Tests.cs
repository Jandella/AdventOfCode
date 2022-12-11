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
            var d = new Day09(exampleInput, 10);
            var result = await d.Solve_2();
            Assert.Equal("1", result);

        }

        [Fact]
        public async Task TestDay9_Solve2_Example2()
        {
            var d = new Day09(@"R 5
U 8
L 8
D 3
R 17
D 10
L 25
U 20", 10);
            var result = await d.Solve_2();
            Assert.Equal("36", result);

        }

        [Fact]
        public async Task TestDay9_Solve1_FileInput()
        {
            var d = new Day09();
            var result = await d.Solve_1();
            Assert.Equal("6011", result);

        }

        [Fact]
        public async Task TestDay9_Solve2_FileInput()
        {
            var d = new Day09(10);
            var result = await d.Solve_2();
            Assert.Equal("2419", result);

        }
    }
}
