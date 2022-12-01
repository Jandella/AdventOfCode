namespace _2022AdventOfCode.Tests
{
    public class DaysTests
    {
        [Fact]
        public async Task TestDay1_Solve1()
        {
            string input = @"1000
2000
3000

4000

5000
6000

7000
8000
9000

10000";
            var d = new Day01(input);
            var result = await d.Solve_1();
            Assert.Equal("24000", result);

        }
        [Fact]
        public async Task TestDay1_Solve2()
        {
            string input = @"1000
2000
3000

4000

5000
6000

7000
8000
9000

10000";
            var d = new Day01(input);
            var result = await d.Solve_2();
            Assert.Equal("45000", result);
        }


    }
}