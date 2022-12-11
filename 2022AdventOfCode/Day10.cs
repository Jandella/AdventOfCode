using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2022AdventOfCode
{
    public class Day10 : AoCHelper.BaseDay
    {
        private string _input;
        public Day10()
        {
            _input = System.IO.File.ReadAllText(InputFilePath);
        }
        public Day10(string input)
        {
            _input = input;
        }

        public override ValueTask<string> Solve_1()
        {
            var sum = 0;
            using (StringReader r = new StringReader(_input))
            {
                var steps = 20;
                var max = 220;
                var cpu = new DeviceCpu();
                var line = r.ReadLine() ?? "";
                cpu.ReadInstruction(line);
                while (cpu.Cycle <= max)
                {
                    if (cpu.CurrentInstruction == null)
                    {
                        if (r.Peek() == -1)
                            break;

                        line = r.ReadLine() ?? "";
                        cpu.ReadInstruction(line);
                    }

                    if (cpu.Cycle % steps == 0)
                    {
                        sum += cpu.Cycle * cpu.Value;
                        steps += 40;
                    }

                    cpu.PerformCycle();
                }
            }
            return new ValueTask<string>(sum.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var device = new Device();
            using (StringReader r = new StringReader(_input))
            {
                var max = 240;
                
                var line = r.ReadLine() ?? "";
                device.Cpu.ReadInstruction(line);
                while (device.Cpu.Cycle <= max)
                {
                    if (device.Cpu.CurrentInstruction == null)
                    {
                        if (r.Peek() == -1)
                            break;

                        line = r.ReadLine() ?? "";
                        device.Cpu.ReadInstruction(line);
                    }

                    

                    device.PerformCycle();
                }
            }
            return new ValueTask<string>(device.Crt.Screen);
        }
    }

    public class DeviceCpu
    {
        public int Cycle { get; set; } = 1;
        public int Value { get; set; } = 1;

        public CpuInstruction? CurrentInstruction { get; private set; }
        public void PerformCycle()
        {
            if (CurrentInstruction == null)
                return;  //or cycle +1 (?)
            Cycle++;
            if (CurrentInstruction.EndingCycle == Cycle)
            {
                Value += CurrentInstruction.ValueToAdd;
                CurrentInstruction = null;
            }
        }
        public void ReadInstruction(string instruction)
        {
            CurrentInstruction = new CpuInstruction(Cycle, instruction);
        }
    }

    public class CpuInstruction
    {
        private string _toString = "";
        public CpuInstruction(int startingCycle, string instruction)
        {
            _toString = instruction;
            StartingCycle = startingCycle;
            var splitted = instruction.Split(" ");
            if (splitted[0] == "addx")
            {
                ValueToAdd = int.Parse(splitted[1]);
                EndingCycle = startingCycle + 2;
            }
            else
            {
                EndingCycle = startingCycle + 1;
                ValueToAdd = 0;
            }
        }
        public int StartingCycle { get; private set; }
        public int EndingCycle { get; private set; }
        public int ValueToAdd { get; private set; }

        public override string ToString()
        {
            return _toString;
        }
    }
    public class CrtPixelPosition
    {
        public CrtPixelPosition()
        {

        }
        public CrtPixelPosition(int row, int col)
        {
            Row = row;
            Col = col;
        }
        public int Row { get; set; }
        public int Col { get; set; }
    }
    public class DeviceCrt
    {
        public DeviceCrt()
        {
            Screen = "";

            for (int r = 0; r < 6; r++)
            {
                for (int c = 0; c < 40; c++)
                {
                    ScreenChar[r, c] = '.';
                    Screen += ".";
                }
                Screen += Environment.NewLine;
            }

        }
        private char[,] ScreenChar { get; set; } = new char[6, 40];
        public string Screen { get; private set; }
        public int SpritePosition { get; private set; } = 1;
        public CrtPixelPosition DrawPosition { get; set; } = new CrtPixelPosition(0, 0);
        public void Draw(int cycle)
        {
            if(SpritePosition == DrawPosition.Col || SpritePosition + 1 == DrawPosition.Col
                || SpritePosition -1 == DrawPosition.Col)
            {
                ScreenChar[DrawPosition.Row,DrawPosition.Col] = '#';
            }
            else
            {
                ScreenChar[DrawPosition.Row, DrawPosition.Col] = '.';
            }
            if (cycle % 40 == 0)
            {
                DrawPosition.Col = 0;
                DrawPosition.Row++;
            }
            else
            {
                DrawPosition.Col++;
            }
            UpdateScreen();
        }
        public void UpdateSprite(int value)
        {
            SpritePosition += value;
        }
        private void UpdateScreen()
        {
            Screen = "";

            for (int r = 0; r < 6; r++)
            {
                for (int c = 0; c < 40; c++)
                {
                    Screen += ScreenChar[r, c];
                }
                if(r < 5)
                {
                    Screen += Environment.NewLine;
                }
            }
        }
    }

    public class Device
    {
        public DeviceCpu Cpu { get; set; } = new DeviceCpu();
        public DeviceCrt Crt { get; set; } = new DeviceCrt();

        public void PerformCycle()
        {
            Crt.Draw(Cpu.Cycle);
            var valueToAdd = 0;
            if(Cpu.CurrentInstruction != null)
            {
                valueToAdd = Cpu.CurrentInstruction.ValueToAdd;
            }
            
            Cpu.PerformCycle();
            if(Cpu.CurrentInstruction== null)
            {
                Crt.UpdateSprite(valueToAdd);
            }
        }
    }
}
