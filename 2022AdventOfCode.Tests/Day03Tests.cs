using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2022AdventOfCode.Tests
{
    public class Day03Tests
    {
        private static string exampleInput = @"vJrwpWtwJgWrhcsFMMfFFhFp
jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL
PmmdzqPrVvPwwTWBwg
wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn
ttgJtRGJQctTZtZT
CrZsJsPPZsGzwwsLwLmpwMDw";
        [Fact]
        public void TestDay3_Compartments()
        {
            var d = new Day03(exampleInput);
            var res = d.GetCompartments("vJrwpWtwJgWrhcsFMMfFFhFp");
            Assert.Equal("vJrwpWtwJgWr", res[0]);
            Assert.Equal("hcsFMMfFFhFp", res[1]);
            var res2 = d.GetCompartments("jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL");
            Assert.Equal("jqHRNqRjqzjGDLGL", res2[0]);
            Assert.Equal("rsFMfFZSrLrFZsSL", res2[1]);
        }
        [Fact]
        public void TestDay3_FindSame()
        {
            var d = new Day03(exampleInput);
            var res = d.FindSame("vJrwpWtwJgWr", "hcsFMMfFFhFp");
            Assert.Equal('p', res);
            res = d.FindSame("jqHRNqRjqzjGDLGL", "rsFMfFZSrLrFZsSL");
            Assert.Equal('L', res);
        }
        [Fact]
        public async Task TestDay3_Solve1()
        {
            
            var d = new Day03(exampleInput);
            var result = await d.Solve_1();
            Assert.Equal("157", result);

        }

        [Fact]
        public void TestDay3_FindBadge()
        {
            var d = new Day03(exampleInput);
            var res = d.FindBadge(new List<string>
            {
                "vJrwpWtwJgWrhcsFMMfFFhFp",
                "jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL",
                "PmmdzqPrVvPwwTWBwg"
            });
            Assert.Equal('r', res);
            res = d.FindBadge(new List<string>
            {
                "wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn",
                "ttgJtRGJQctTZtZT",
                "CrZsJsPPZsGzwwsLwLmpwMDw"
            });
            Assert.Equal('Z', res);
        }

        [Fact]
        public async Task TestDay3_Solve2()
        {
            
            var d = new Day03(exampleInput);
            var result = await d.Solve_2();
            Assert.Equal("70", result);

        }

        [Fact]
        public async Task TestDay3_Solve1_FileInput()
        {
            var d = new Day03();
            var result = await d.Solve_1();
            Assert.Equal("7821", result);

        }

        [Fact]
        public async Task TestDay3_Solve2_FileInput()
        {
            var d = new Day03();
            var result = await d.Solve_2();
            Assert.Equal("2752", result);

        }
    }
}
