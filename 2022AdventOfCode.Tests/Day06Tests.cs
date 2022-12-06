using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2022AdventOfCode.Tests
{
    public class Day06Tests
    {
        
        [Fact]
        public async Task TestDay6_Solve1()
        {
            var d = new Day06("mjqjpqmgbljsphdztnvjfqwrcgsmlb");
            var result = await d.Solve_1();
            Assert.Equal("7", result);
            var e = new Day06("bvwbjplbgvbhsrlpgdmjqwftvncz");
            var resultE = await e.Solve_1();
            Assert.Equal("5", resultE);
            var f = new Day06("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg");
            var resultF = await f.Solve_1();
            Assert.Equal("10", resultF);

        }

        [Fact]
        public async Task TestDay6_Solve2()
        {
            var d = new Day06("mjqjpqmgbljsphdztnvjfqwrcgsmlb");
            var result = await d.Solve_2();
            Assert.Equal("19", result);
            var e = new Day06("bvwbjplbgvbhsrlpgdmjqwftvncz");
            var resultE = await e.Solve_2();
            Assert.Equal("23", resultE);
            var f = new Day06("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg");
            var resultF = await f.Solve_2();
            Assert.Equal("29", resultF);

        }

        [Fact]
        public async Task TestDay6_Solve1_FileInput()
        {
            var d = new Day06();
            var result = await d.Solve_1();
            Assert.Equal("1175", result);

        }

        [Fact]
        public async Task TestDay6_Solve2_FileInput()
        {
            var d = new Day06();
            var result = await d.Solve_2();
            Assert.Equal("3217", result);

        }
    }
}
