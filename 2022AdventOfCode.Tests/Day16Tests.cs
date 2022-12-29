using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2022AdventOfCode.Tests
{
    public class Day16Tests
    {
        public string exampleInput = @"Valve AA has flow rate=0; tunnels lead to valves DD, II, BB
Valve BB has flow rate=13; tunnels lead to valves CC, AA
Valve CC has flow rate=2; tunnels lead to valves DD, BB
Valve DD has flow rate=20; tunnels lead to valves CC, AA, EE
Valve EE has flow rate=3; tunnels lead to valves FF, DD
Valve FF has flow rate=0; tunnels lead to valves EE, GG
Valve GG has flow rate=0; tunnels lead to valves FF, HH
Valve HH has flow rate=22; tunnel leads to valve GG
Valve II has flow rate=0; tunnels lead to valves AA, JJ
Valve JJ has flow rate=21; tunnel leads to valve II";

        [Fact]
        public void TestDay16_Parse()
        {
            var d = new Day16(exampleInput);
            var res = d.ParseInput();
            Assert.True(res.ContainsKey("DD"));
            var testItem = res["DD"];
            Assert.NotNull(testItem);
            Assert.Equal(20, testItem.Flow);
            Assert.Contains<string>("AA", testItem.ConnectedTo);
            Assert.Contains<string>("CC", testItem.ConnectedTo);
            Assert.Contains<string>("EE", testItem.ConnectedTo);
        }

        [Fact]
        public async Task TestDay16_Solve1()
        {
            var d = new Day16(exampleInput);
            var result = await d.Solve_1();
            Assert.Equal("1651", result);

        }

        [Fact]
        public async Task TestDay16_Solve2()
        {
            var d = new Day16(exampleInput);
            var result = await d.Solve_2();
            Assert.Equal("1707", result);

        }

        [Fact]
        public async Task TestDay16_Solve1_FileInput()
        {
            var d = new Day16();
            var result = await d.Solve_1();
            Assert.Equal("1940", result);

        }

        [Fact]
        public async Task TestDay16_Solve2_FileInput()
        {
            var d = new Day16();
            var result = await d.Solve_2();
            Assert.Equal("2469", result);

        }
    }
}
