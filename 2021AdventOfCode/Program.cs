using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace _2021AdventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of code!");
            var stopWatch = new Stopwatch();
            //Day01 d01 = new Day01();
            //Console.WriteLine("day 1 quiz 1: {0}", d01.ContaMisureQuiz1());
            //Console.WriteLine("day 1 quiz 2: {0}", d01.ContaMisureQuiz2());

            //Day02 d02 = new Day02();
            //Console.WriteLine("day 2 quiz 1: {0}", d02.Quiz1());
            //Console.WriteLine("day 2 quiz 2: {0}", d02.Quiz2());

            //Day03 d03 = new Day03();
            //Console.WriteLine("day 3 quiz 1: {0}", d03.Quiz1());
            //Console.WriteLine("day 3 quiz 2: {0}", d03.Quiz2());

            //Day04 d04 = new Day04();
            //Console.WriteLine("day 4 quiz 1: {0}", d04.Quiz1());
            //Console.WriteLine("day 4 quiz 2: {0}", d04.Quiz2());

            //Day05 d05 = new Day05();
            //Console.WriteLine("day 5 quiz 1: {0}", d05.Quiz1());
            //Console.WriteLine("day 5 quiz 2: {0}", d05.Quiz2());
            //Day06 d06 = new Day06();
            //stopWatch.Restart();
            //var day06quiz1res = d06.Quiz1();
            //stopWatch.Stop();
            //Console.WriteLine("day 6 quiz 1: {0} - elapsed time: {1}", day06quiz1res, stopWatch.Elapsed);

            //stopWatch.Restart();
            //var day06quiz2res = d06.Quiz2();
            //stopWatch.Stop();
            //Console.WriteLine("day 6 quiz 2: {0} - elapsed time: {1}", day06quiz2res, stopWatch.Elapsed);

            //Day07 d07 = new Day07();
            //Console.WriteLine("day 7 quiz 1: {0}", d07.Quiz1());
            //Console.WriteLine("day 7 quiz 2: {0}", d07.Quiz2());

            //Day08 d08 = new Day08();
            //Console.WriteLine("day 8 quiz 1: {0}", d08.Quiz1());
            //Console.WriteLine("day 8 quiz 2: {0}", d08.Quiz2());
            Day09 d09 = new Day09();
            Console.WriteLine("day 9 quiz 1: {0}", d09.Quiz1());
            Console.WriteLine("day 9 quiz 2: {0}", d09.Quiz2());

            Console.ReadKey();
        }
    }
}
