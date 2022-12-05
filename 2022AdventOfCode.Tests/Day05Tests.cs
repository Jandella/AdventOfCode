using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2022AdventOfCode.Tests
{
    public class Day05Tests
    {
        private static string exampleInput = 
@"    [D]    
[N] [C]    
[Z] [M] [P]
 1   2   3 

move 1 from 2 to 1
move 3 from 1 to 3
move 2 from 2 to 1
move 1 from 1 to 2";
        [Fact]
        public void TestDay5_ParseStackOfCrates()
        {
            var a = new StackOfCrates(@"    [D]    
[N] [C]    
[Z] [M] [P]
 1   2   3");
            Assert.Equal(3, a.Crates.Length);
            Assert.Equal('N', a.Crates[0].Peek());
            Assert.Equal('D', a.Crates[1].Peek());
            Assert.Equal('P', a.Crates[2].Peek());
        }
        [Fact]
        public void TestDay5_ParseMoves()
        {
            var a = new CraneInstruction("move 1 from 2 to 1");
            Assert.Equal(1, a.Move);
            Assert.Equal(2, a.From);
            Assert.Equal(1, a.To);
        }
        [Fact]
        public async Task TestDay5_Solve1()
        {
            var d = new Day05(exampleInput);
            var result = await d.Solve_1();
            Assert.Equal("CMZ", result);

        }

        [Fact]
        public async Task TestDay5_Solve2()
        {
            var d = new Day05(exampleInput);
            var result = await d.Solve_2();
            Assert.Equal("12", result);

        }

        [Fact]
        public async Task TestDay5_Solve1_FileInput()
        {
            var d = new Day05();
            var result = await d.Solve_1();
            Assert.Equal("TGWSMRBPN", result);

        }

        [Fact]
        public async Task TestDay5_Solve2_FileInput()
        {
            var d = new Day05();
            var result = await d.Solve_2();
            Assert.Equal("", result);

        }
    }
}
