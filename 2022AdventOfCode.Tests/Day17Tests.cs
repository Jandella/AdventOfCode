using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2022AdventOfCode.Tests
{
    public class Day17Tests
    {
        public string exampleInput = ">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>";

        [Fact]
        public void TestDay17_TallNarrowCamber_1()
        {
            var cave = new TallNarrowChamber(exampleInput);
            var res = cave.SimulateFalling(1);
            Assert.Equal(1, res.Height);
            Assert.Single(res.FallenRocks);
        }
        [Fact]
        public void TestDay17_TallNarrowCamber_2()
        {
            var cave = new TallNarrowChamber(exampleInput);
            var res = cave.SimulateFalling(2);
            Assert.Equal(4, res.Height);
            Assert.Equal(2, res.FallenRocks.Count);
        }
        [Fact]
        public void TestDay17_TallNarrowCamber_3()
        {
            var cave = new TallNarrowChamber(exampleInput);
            var res = cave.SimulateFalling(3);
            Debug.WriteLine(cave.PrintStatus(res.FallenRocks, res.Height, null));
            Assert.Equal(6, res.Height);
            Assert.Equal(3, res.FallenRocks.Count);

        }

        [Fact]
        public void TestDay17_TallNarrowCamber_5()
        {
            var cave = new TallNarrowChamber(exampleInput);
            var res = cave.SimulateFalling(5);
            Debug.WriteLine(cave.PrintStatus(res.FallenRocks, res.Height, null));
            Assert.Equal(9, res.Height);
            Assert.Equal(5, res.FallenRocks.Count);
        }

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
            Assert.Equal("1514285714288", result);

        }

        [Fact]
        public async Task TestDay17_Solve1_FileInput()
        {
            var d = new Day17();
            var result = await d.Solve_1();
            Assert.Equal("3188", result);

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
