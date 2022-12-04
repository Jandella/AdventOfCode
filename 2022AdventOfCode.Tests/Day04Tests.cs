using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2022AdventOfCode.Tests
{
    public class Day04Tests
    {
        private static string exampleInput = @"2-4,6-8
2-3,4-5
5-7,7-9
2-8,3-7
6-6,4-6
2-6,4-8";
        [Fact]
        public void TestDay4_SectionContains()
        {
            Section a = new Section("6-6");
            Section b = new Section("4-6");
            Assert.True(b.Contains(a));
            Assert.False(a.Contains(b));

            Section c = new Section("2-6");
            Section d = new Section("4-8");
            Assert.False(c.Contains(d));
            Assert.False(d.Contains(c));

            Section e = new Section("2-8");
            Section f = new Section("3-7");
            Assert.True(e.Contains(f));
            Assert.False(f.Contains(e));
        }

        [Fact]
        public async Task TestDay4_Solve1()
        {
            var d = new Day04(exampleInput);
            var result = await d.Solve_1();
            Assert.Equal("2", result);

        }

        [Fact]
        public void TestDay4_SectionOverlaps()
        {
            Section a = new Section("6-6");
            Section b = new Section("4-6");
            Assert.True(b.Overlaps(a));
            Assert.True(a.Overlaps(b));

            Section c = new Section("2-6");
            Section d = new Section("4-8");
            Assert.True(c.Overlaps(d));
            Assert.True(d.Overlaps(c));

            Section e = new Section("2-8");
            Section f = new Section("3-7");
            Assert.True(e.Overlaps(f));
            Assert.True(f.Overlaps(e));

            Section g = new Section("2-4");
            Section h = new Section("6-8");
            Assert.False(g.Overlaps(h));
            Assert.False(h.Overlaps(g));
        }
        [Fact]
        public async Task TestDay4_Solve2()
        {
            var d = new Day04(exampleInput);
            var result = await d.Solve_2();
            Assert.Equal("4", result);

        }

        [Fact]
        public async Task TestDay4_Solve1_FileInput()
        {
            var d = new Day04();
            var result = await d.Solve_1();
            Assert.Equal("490", result);

        }

        [Fact]
        public async Task TestDay4_Solve2_FileInput()
        {
            var d = new Day04();
            var result = await d.Solve_2();
            Assert.Equal("921", result);

        }
    }
}
