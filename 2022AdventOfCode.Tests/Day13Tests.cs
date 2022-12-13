using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2022AdventOfCode.Tests
{
    public class Day13Tests
    {
        public string exampleInput = @"[1,1,3,1,1]
[1,1,5,1,1]

[[1],[2,3,4]]
[[1],4]

[9]
[[8,7,6]]

[[4,4],4,4]
[[4,4],4,4,4]

[7,7,7,7]
[7,7,7]

[]
[3]

[[[]]]
[[]]

[1,[2,[3,[4,[5,6,7]]]],8,9]
[1,[2,[3,[4,[5,6,0]]]],8,9]";
        [Fact]
        public async Task TestDay13_Solve1()
        {
            var d = new Day13(exampleInput);
            var result = await d.Solve_1();
            Assert.Equal("13", result);

        }

        [Fact]
        public async Task TestDay13_Solve2()
        {
            var d = new Day13(exampleInput);
            var result = await d.Solve_2();
            Assert.Equal("140", result);

        }

        [Fact]
        public async Task TestDay13_Solve1_FileInput()
        {
            var d = new Day13();
            var result = await d.Solve_1();
            Assert.Equal("5806", result);

        }

        [Fact]
        public async Task TestDay13_Solve2_FileInput()
        {
            var d = new Day13();
            var result = await d.Solve_2();
            Assert.Equal("23600", result);

        }
    }
}
