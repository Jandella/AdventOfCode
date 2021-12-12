using System;

namespace _2021AdventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //Day01 d01 = new Day01();
            //Console.WriteLine("day 1 quiz 1: {0}", d01.ContaMisureQuiz1());
            //Console.WriteLine("day 1 quiz 2: {0}", d01.ContaMisureQuiz2());

            //Day02 d02 = new Day02();
            //Console.WriteLine("day 2 quiz 1: {0}", d02.Quiz1());
            //Console.WriteLine("day 2 quiz 2: {0}", d02.Quiz2());

            Day03 d03 = new Day03();
            Console.WriteLine("day 3 quiz 1: {0}", d03.Quiz1());
            Console.WriteLine("day 3 quiz 2: {0}", d03.Quiz2());
            Console.ReadKey();
        }
    }
}
