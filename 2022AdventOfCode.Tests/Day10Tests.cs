using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2022AdventOfCode.Tests
{
    public class Day10Tests
    {
        public string exampleInput = @"addx 15
addx -11
addx 6
addx -3
addx 5
addx -1
addx -8
addx 13
addx 4
noop
addx -1
addx 5
addx -1
addx 5
addx -1
addx 5
addx -1
addx 5
addx -1
addx -35
addx 1
addx 24
addx -19
addx 1
addx 16
addx -11
noop
noop
addx 21
addx -15
noop
noop
addx -3
addx 9
addx 1
addx -3
addx 8
addx 1
addx 5
noop
noop
noop
noop
noop
addx -36
noop
addx 1
addx 7
noop
noop
noop
addx 2
addx 6
noop
noop
noop
noop
noop
addx 1
noop
noop
addx 7
addx 1
noop
addx -13
addx 13
addx 7
noop
addx 1
addx -33
noop
noop
noop
addx 2
noop
noop
noop
addx 8
noop
addx -1
addx 2
addx 1
noop
addx 17
addx -9
addx 1
addx 1
addx -3
addx 11
noop
noop
addx 1
noop
addx 1
noop
noop
addx -13
addx -19
addx 1
addx 3
addx 26
addx -30
addx 12
addx -1
addx 3
addx 1
noop
noop
noop
addx -9
addx 18
addx 1
addx 2
noop
noop
addx 9
noop
noop
noop
addx -1
addx 2
addx -37
addx 1
addx 3
noop
addx 15
addx -21
addx 22
addx -6
addx 1
noop
addx 2
addx 1
noop
addx -10
noop
noop
addx 20
addx 1
addx 2
addx 2
addx -6
addx -11
noop
noop
noop";
        [Fact]
        public async Task TestDay10_BaseInput()
        {
            var d = new Day10(@"noop
addx 3
addx -5");
            var result = await d.Solve_1();
            Assert.Equal("0", result);
        }
        [Fact]
        public async Task TestDay10_Solve1()
        {
            var d = new Day10(exampleInput);
            var result = await d.Solve_1();
            Assert.Equal("13140", result);

        }

        [Fact]
        public async Task TestDay10_Solve2()
        {
            var d = new Day10(exampleInput);
            var result = await d.Solve_2();
            Assert.Equal(@"##..##..##..##..##..##..##..##..##..##..
###...###...###...###...###...###...###.
####....####....####....####....####....
#####.....#####.....#####.....#####.....
######......######......######......####
#######.......#######.......#######.....", result);

        }

        [Fact]
        public async Task TestDay10_Solve1_FileInput()
        {
            var d = new Day10();
            var result = await d.Solve_1();
            Assert.Equal("17380", result);

        }

        [Fact]
        public async Task TestDay10_Solve2_FileInput()
        {
            var d = new Day10();
            var result = await d.Solve_2();
            Assert.Equal(
@"####..##...##..#..#.####.###..####..##..
#....#..#.#..#.#..#....#.#..#.#....#..#.
###..#....#....#..#...#..#..#.###..#....
#....#.##.#....#..#..#...###..#....#....
#....#..#.#..#.#..#.#....#.#..#....#..#.
#.....###..##...##..####.#..#.####..##..", result);

        }
    }
}
