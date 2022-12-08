using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2022AdventOfCode
{
    public class Day07 : AoCHelper.BaseDay
    {
        private string _input;
        public Day07()
        {
            _input = System.IO.File.ReadAllText(InputFilePath);
        }
        public Day07(string input)
        {
            _input = input;
        }

        public override ValueTask<string> Solve_1()
        {
            int total = 0;
            var stack = new Stack<DeviceDirectory>();
            var dirs = new List<DeviceDirectory>();
            using (StringReader r = new(_input))
            {
                while (r.Peek() != -1)
                {
                    string line = r.ReadLine() ?? "-";
                    if (line.StartsWith("$ cd"))
                    {
                        var dirName = line.Substring("$cd ".Length + 1);
                        if(dirName == "..")
                        {
                            //go back
                            var poppedDir = stack.Pop();
                            if(poppedDir.Size <= 100000)
                            {
                                if(stack.Count > 0)
                                {
                                    var current = stack.Peek();
                                    current.Size += poppedDir.Size;
                                }
                                //when popped, we should have the right size
                                total+= poppedDir.Size;
                            }

                        }
                        else
                        {
                            var newDir = new DeviceDirectory()
                            {
                                Name = dirName,
                            };
                            dirs.Add(newDir);
                            if(stack.Count > 0)
                            {
                                // Add new directory as sub directory of current directory
                                var current = stack.Peek();
                                current.SubDirs.Add(newDir);
                            }
                            stack.Push(newDir);
                        }
                    }
                    else if(!line.StartsWith("$ ls"))
                    {
                        //it's either a directory or a file
                        var splitted = line.Split(' ');
                        var current = stack.Peek();
                        if (splitted[0] == "dir")
                        {   
                            var newItem = new DeviceDirectory()
                            {
                                Name = splitted[1],
                            };
                        }
                        else
                        {
                            current.Size += int.Parse(splitted[0]);
                        }
                    }
                    
                }
            }
            var aaaa = dirs.Select(directory => directory.Size).Where(fileSize => fileSize <= 100000).Sum();
            return new ValueTask<string>(total.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            throw new NotImplementedException();
        }
    }

    public class DeviceDirectory
    {
        public string? Name { get; set; }
        public List<DeviceDirectory> SubDirs { get; set; } = new List<DeviceDirectory>();
        public List<string> Files { get; set; } = new List<string>();
        public int Size { get; set; }
        public override string ToString()
        {
            return $"{Name} ({Size})";
        }
    }
}
