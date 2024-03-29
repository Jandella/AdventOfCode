﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace _2021AdventOfCode
{
    /// <summary>
    /// --- Day 10: Syntax Scoring ---
    //You ask the submarine to determine the best route out of the deep-sea cave, but it only replies:
    ///Syntax error in navigation subsystem on line: all of them
    ///All of them?! The damage is worse than you thought.You bring up a copy of the navigation subsystem (your puzzle input).
    /// </summary>
    public class Day10
    {
        private string _day10input = "";

        private string _example = @"[({(<(())[]>[[{[]{<()<>>
[(()[<>])]({[<{<<[]>>(
{([(<{}[<>[]}>{[]{[(<()>
(((({<>}<{<{<>}{[]{[]{}
[[<[([]))<([[{}[[()]]]
[{[{({}]{}}([{[{{{}}([]
{<[[]]>}<{[{[{[]{()[[[]
[<(<(<(<{}))><([]([]()
<{([([[(<>()){}]>(<<{{
<{([{{}}[<[[[<>{}]]]>[]]";
        private List<char> open = new List<char> { '(', '[', '{', '<' };
        private List<char> closed = new List<char> { ')', ']', '}', '>' };
        private Dictionary<char, int> _score = new Dictionary<char, int>
        {
            [')'] = 3,
            [']'] = 57,
            ['}'] = 1197,
            ['>'] = 25137,
        };
        private Dictionary<char, int> _quiz2score = new Dictionary<char, int>
        {
            [')'] = 1,
            [']'] = 2,
            ['}'] = 3,
            ['>'] = 4,
        };
        public Day10()
        {
            _day10input = System.IO.File.ReadAllText(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Inputs", "Day10Input.txt"));
        }
        private List<string> ParseInput(string input)
        {
            return input.Split($"\r\n", StringSplitOptions.RemoveEmptyEntries).ToList();
        }
        public int Quiz1()
        {
            var lines = ParseInput(_day10input);
            var stack = new List<char>();
            int illegalScore = 0;
            foreach (var line in lines)
            {
                foreach (var c in line)
                {
                    if (open.Contains(c))
                        stack.Add(c);
                    if (closed.Contains(c))
                    {
                        var lastChar = stack.Last();
                        var expected = closed[open.IndexOf(lastChar)];
                        if(c == expected)
                        {
                            stack.RemoveAt(stack.Count - 1);
                            continue;
                        }
                        else
                        {
                            illegalScore += _score[c];
                            break;
                        }
                    }
                }
                stack.Clear();
            }
            return illegalScore;
        }

        public long Quiz2()
        {
            var lines = ParseInput(_day10input);
            var stack = new List<char>();
            var scores = new List<long>();
            foreach (var line in lines)
            {
                bool invalidSequence = false;
                foreach (var c in line)
                {
                    if (open.Contains(c))
                        stack.Add(c);
                    if (closed.Contains(c))
                    {
                        var lastChar = stack.Last();
                        var expected = closed[open.IndexOf(lastChar)];
                        if (c == expected)
                        {
                            stack.RemoveAt(stack.Count - 1);
                            continue;
                        }
                        else
                        {
                            invalidSequence = true;
                            break;
                        }
                    }
                }
                if (!invalidSequence && stack.Count > 0)
                {
                    var complete = new List<char>();
                    for (int i = stack.Count - 1; i >= 0; i--)
                    {
                        complete.Add(closed[open.IndexOf(stack[i])]);
                    }
                    long ac = 0;
                    foreach (var c in complete)
                    {
                        ac = (ac * 5) + _quiz2score[c];
                    }
                    scores.Add(ac);
                }
                stack.Clear();
            }
            scores.Sort();
            return scores[scores.Count / 2];
        }
    }
}
