using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace _2021AdventOfCode
{
    /// <summary>
    /// --- Day 2: Dive! ---
    // Now, you need to figure out how to pilot this thing.
    /// It seems like the submarine can take a series of commands like forward 1, down 2, or up 3:
    ///    forward X increases the horizontal position by X units.
    ///    down X increases the depth by X units.
    ///    up X decreases the depth by X units.
    /// Note that since you're on a submarine, down and up affect your depth, and so they have the opposite result of what you might expect.
    /// The submarine seems to already have a planned course (your puzzle input). You should probably figure out where it's going.
    /// </summary>
    public class Day02
    {
        private readonly string _day2Input = "";

        public Day02()
        {
            _day2Input = System.IO.File.ReadAllText(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Inputs", "Day02Input.txt"));
        }
        private SubCommand[] ParseInput()
        {
            return _day2Input
                .Split('\n').Select(x => x.Trim())
                .Select(x => new SubCommand(x)).ToArray();
        }
        public int Quiz1()
        {
            int horizontalPosition = 0;
            int depthPosition = 0;
            var course = ParseInput();
            foreach (var cmd in course)
            {
                switch (cmd.Movement)
                {
                    case "forward":
                        horizontalPosition = horizontalPosition + cmd.Units;
                        break;
                    case "down":
                        depthPosition = depthPosition + cmd.Units;
                        break;
                    case "up":
                        depthPosition = depthPosition - cmd.Units;
                        break;
                    default:
                        break;
                }
            }

            return horizontalPosition * depthPosition;
        }

        public int Quiz2()
        {
            int horizontalPosition = 0;
            int depthPosition = 0;
            int aim = 0;
            var course = ParseInput();
            foreach (var cmd in course)
            {
                switch (cmd.Movement)
                {
                    case "forward":
                        horizontalPosition = horizontalPosition + cmd.Units;
                        depthPosition = depthPosition + cmd.Units * aim;
                        break;
                    case "down":
                        aim = aim + cmd.Units;
                        break;
                    case "up":
                        aim = aim - cmd.Units;
                        break;
                    default:
                        break;
                }
            }

            return horizontalPosition * depthPosition;
        }
    }

    public class SubCommand
    {
        public SubCommand(string cmd)
        {
            var data = cmd.Split(' ');
            Movement = data[0];
            Units = int.Parse(data[1]);
        }

        public int Units { get; set; }
        public string Movement { get; set; }

    }
}
