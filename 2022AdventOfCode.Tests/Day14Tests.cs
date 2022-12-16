using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2022AdventOfCode.Tests
{
    public class Day14Tests
    {
        public string exampleInput = @"498,4 -> 498,6 -> 496,6
503,4 -> 502,4 -> 502,9 -> 494,9";
        [Fact]
        public void TestDay14_Parse()
        {
            var d = new Day14(exampleInput);
            var parsed = d.ParseScan();
            Assert.NotEmpty(parsed);
            Assert.Equal(10, parsed.Count);
        }
        [Fact]
        public async Task TestDay14_Solve1()
        {
            var d = new Day14(exampleInput);
            var result = await d.Solve_1();
            Assert.Equal("24", result);

        }

        [Fact]
        public void TestDay14_Floor()
        {
            var d = new Day14(exampleInput);
            var cave = new CaveSystem();
            cave.Rocks = d.ParseScan();
            cave.UpdateFloorValue();
            Assert.Equal(11, cave.FloorY);

        }
        [Fact]
        public async Task TestDay14_Solve2()
        {
            var d = new Day14(exampleInput);
            var result = await d.Solve_2();
            Assert.Equal("93", result);

        }

        [Fact]
        public async Task TestDay14_Solve1_FileInput()
        {
            var d = new Day14();
            var result = await d.Solve_1();
            Assert.Equal("592", result);

        }

        [Fact]
        public async Task TestDay14_Solve2_FileInput()
        {
            var d = new Day14();
            var result = await d.Solve_2();
            Assert.Equal("30367", result);

        }
    }
}
