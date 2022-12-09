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
        private List<DeviceDirectory> ParseInput()
        {
            var dirs = new List<DeviceDirectory>();
            DeviceDirectory? currentNode = null;
            using (StringReader r = new StringReader(_input))
            {
                while (r.Peek() != -1)
                {
                    string line = r.ReadLine() ?? "-";
                    if (line.StartsWith("$"))
                    {
                        if (line.Equals("$ ls"))
                            continue;
                        if (!line.Equals("$ cd .."))
                        {
                            string nodeName = line.Split(' ')[2];
                            if (currentNode == null)
                            {
                                var node = new DeviceDirectory
                                {
                                    Name = nodeName,
                                    Parent = currentNode
                                };

                                currentNode = node;

                                dirs.Add(node);
                            }
                            else
                            {
                                var child = currentNode.SubDirs
                                    .Where(x => x.Name == nodeName).FirstOrDefault();
                                if (child != null)
                                {
                                    child.Parent = currentNode;
                                    currentNode = child;
                                }
                            }

                        }
                        else
                        {
                            if(currentNode != null)
                            {
                                int size = currentNode.Size;

                                currentNode = currentNode.Parent;

                                if (currentNode != null)
                                    currentNode.Size += size;
                            }
                            

                        }
                        continue;
                    }
                    else if (line.StartsWith("dir"))
                    {
                        var child = new DeviceDirectory
                        {
                            Name = line.Split(' ')[1]
                        };
                        if (currentNode != null)
                        {
                            currentNode.SubDirs.Add(child);
                        }
                        dirs.Add(child);
                    }
                    else
                    {
                        int values = int.Parse(line.Split(' ')[0]);
                        if (currentNode != null)
                        {
                            currentNode.Size += values;
                        }
                    }
                }
            }
            //go back to sum last folder size to parent
            while (currentNode != null)
            {
                int size = currentNode.Size;

                currentNode = currentNode.Parent;

                if (currentNode != null)
                    currentNode.Size += size;
            }
            return dirs;
        }
        public override ValueTask<string> Solve_1()
        {

            var dirs = ParseInput();
            var total = dirs.Select(directory => directory.Size).Where(fileSize => fileSize <= 100000).Sum();
            return new ValueTask<string>(total.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var dirs = ParseInput();
            var totalDiskSpace = 70000000;
            var unusedSpaceNeeded = 30000000;
            var root = dirs.First(x => x.Name == "/");
            var total = root.Size;
            var unusedSpace = totalDiskSpace - total;
            var minSizeNeeded = unusedSpaceNeeded - unusedSpace;
            var minSize = dirs.Where(x => x.Size >= minSizeNeeded).Min(x => x.Size);
            return new ValueTask<string>(minSize.ToString());
        }
    }

    public class DeviceDirectory
    {
        public DeviceDirectory? Parent { get; set; }
        public string? Name { get; set; }
        public List<DeviceDirectory> SubDirs { get; set; } = new List<DeviceDirectory>();
        public int Size { get; set; }
        public override string ToString()
        {
            return $"{Name} ({Size})";
        }
    }
}
