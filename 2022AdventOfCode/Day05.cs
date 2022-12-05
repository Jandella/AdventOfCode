using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace _2022AdventOfCode
{
    public class Day05 : AoCHelper.BaseDay
    {
        private string _input;
        public Day05()
        {
            _input = System.IO.File.ReadAllText(InputFilePath);
        }
        public Day05(string input)
        {
            _input = input;
        }
        public override ValueTask<string> Solve_1()
        {
            var inputParts = _input.Split(Environment.NewLine+Environment.NewLine);
            var crates = new StackOfCrates(inputParts[0]);
            var instructionsSplitted = inputParts[1].Split("\n");
            
            foreach (var item in instructionsSplitted)
            {
                var currentInstruction = new CraneInstruction(item);
                var indexToMove = currentInstruction.From - 1; //0 based
                var destIndex = currentInstruction.To- 1;
                for (int i = 0; i < currentInstruction.Move; i++)
                {
                    var crate = crates.Crates[indexToMove].Pop();
                    crates.Crates[destIndex].Push(crate);
                }
            }
            string res = "";
            foreach (var item in crates.Crates)
            {
                res += item.Peek();
            }
            return new ValueTask<string>(res);
        }

        public override ValueTask<string> Solve_2()
        {
            throw new NotImplementedException();
        }
    }

    public class StackOfCrates
    {
        public StackOfCrates(int size)
        {
            Crates = new Stack<char>[size];
        }
        public StackOfCrates(string input)
        {
            var lines = input.Split("\n");
            var indexesLine = lines.Last();
            var indexes = indexesLine.Split(" ").Where(x => !string.IsNullOrEmpty(x)).Select(x => int.Parse(x));
            Crates = new Stack<char>[indexes.Count()];
            for(int i = lines.Length - 2; i >= 0; i--)
            {
                for (int j = 0; j < Crates.Length; j++)
                {
                    if (Crates[j] == null)
                    {
                        Crates[j] = new Stack<char>();
                    }
                    var currentChar = lines[i][(j *4) + 1];
                    if (char.IsLetter(currentChar))
                    {
                        Crates[j].Push(currentChar);
                    }
                }
            }
        }
        public Stack<char>[] Crates { get; set; }
        
    }
    public class CraneInstruction
    {
        public CraneInstruction()
        {

        }
        public CraneInstruction(string input)
        {
            //example string move 1 from 2 to 1
            var splitted = input.Split(" ");
            Move = int.Parse(splitted[1]);
            From = int.Parse(splitted[3]);
            To = int.Parse(splitted[5]);
        }
        public int Move { get; set; }
        public int From { get; set; }
        public int To { get; set; }
    }
}
