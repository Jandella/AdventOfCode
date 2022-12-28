using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2022AdventOfCode.Tests
{
    public class Day15Tests
    {
        public string exampleInput = @"Sensor at x=2, y=18: closest beacon is at x=-2, y=15
Sensor at x=9, y=16: closest beacon is at x=10, y=16
Sensor at x=13, y=2: closest beacon is at x=15, y=3
Sensor at x=12, y=14: closest beacon is at x=10, y=16
Sensor at x=10, y=20: closest beacon is at x=10, y=16
Sensor at x=14, y=17: closest beacon is at x=10, y=16
Sensor at x=8, y=7: closest beacon is at x=2, y=10
Sensor at x=2, y=0: closest beacon is at x=2, y=10
Sensor at x=0, y=11: closest beacon is at x=2, y=10
Sensor at x=20, y=14: closest beacon is at x=25, y=17
Sensor at x=17, y=20: closest beacon is at x=21, y=22
Sensor at x=16, y=7: closest beacon is at x=15, y=3
Sensor at x=14, y=3: closest beacon is at x=15, y=3
Sensor at x=20, y=1: closest beacon is at x=15, y=3";
        [Fact]
        public async Task TestDay15_Solve1()
        {
            var d = new Day15(new Day15Parameters
            {
                Input= exampleInput,
                Y = 10
            });
            var result = await d.Solve_1();
            Assert.Equal("26", result);

        }

        [Fact]
        public void TestPerimeter()
        {
            var p = new Pair(new Coordinate(8, 7), new Coordinate(2,10));
            var perimeter = p.OutsidePerimeter();
            Assert.Equal(40, perimeter.Count);
            Assert.Equal(new Coordinate(8,-3), perimeter.Last());
        }

        [Fact]
        public async Task TestDay15_Solve2()
        {
            var d = new Day15(new Day15Parameters
            {
                Input = exampleInput,
                Y = 10, 
                MinDistressBeaconCoordinate = 0,
                MaxDistressBeaconCoordinate = 20
            });
            var result = await d.Solve_2();
            Assert.Equal("56000011", result);

        }

        [Fact]
        public async Task TestDay15_Solve1_FileInput()
        {
            var d = new Day15(new Day15Parameters
            {
                Input = null,
                Y = 2000000
            });
            var result = await d.Solve_1();
            Assert.Equal("4724228", result);

        }

        [Fact]
        public async Task TestDay15_Solve2_FileInput()
        {
            var d = new Day15(new Day15Parameters
            {
                Input = null,
                Y = 2000000,
                MinDistressBeaconCoordinate = 0,
                MaxDistressBeaconCoordinate = 4000000
            });
            var result = await d.Solve_2();
            Assert.Equal("13622251246513", result);

        }
    }
}
