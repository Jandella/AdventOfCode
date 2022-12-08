using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2022AdventOfCode.Tests
{
    public class Day07Tests
    {
        private static string exampleInput =
@"$ cd /
$ ls
dir a
14848514 b.txt
8504156 c.dat
dir d
$ cd a
$ ls
dir e
29116 f
2557 g
62596 h.lst
$ cd e
$ ls
584 i
$ cd ..
$ cd ..
$ cd d
$ ls
4060174 j
8033020 d.log
5626152 d.ext
7214296 k";
        
        [Fact]
        public async Task TestDay7_Solve1()
        {
            var d = new Day07(exampleInput);
            var result = await d.Solve_1();
            Assert.Equal("1428881", result);

        }

        [Fact]
        public async Task TestDay7_Solve2()
        {
            var d = new Day07(exampleInput);
            var result = await d.Solve_2();
            Assert.Equal("", result);

        }

        [Fact]
        public async Task TestDay7_Solve1_FileInput()
        {
            var d = new Day07();
            var result = await d.Solve_1();
            Assert.Equal("", result);

        }

        [Fact]
        public async Task TestDay7_Solve2_FileInput()
        {
            var d = new Day07();
            var result = await d.Solve_2();
            Assert.Equal("10475598", result);

        }
    }
}
