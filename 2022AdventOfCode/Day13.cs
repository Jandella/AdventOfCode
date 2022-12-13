using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace _2022AdventOfCode
{
    public class Day13 : AoCHelper.BaseDay
    {
        private string _input;
        public Day13()
        {
            _input = System.IO.File.ReadAllText(InputFilePath);
        }
        public Day13(string input)
        {
            _input = input;
        }

        public override ValueTask<string> Solve_1()
        {
            var packetCouples = _input.Split(Environment.NewLine + Environment.NewLine);
            int index = 1;
            var correct = new List<int>();
            var comparer = new PacketComparer();
            foreach (var item in packetCouples)
            {
                var packetString = item.Split(Environment.NewLine);
                var a = Packet.Parse(packetString[0]);
                var b = Packet.Parse(packetString[1]);
                var res = comparer.Compare(a, b);
                if(res <= 0)
                {
                    correct.Add(index);
                }
                index++;
            }
            var sum = correct.Sum();
            return new ValueTask<string>(sum.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var packetCouples = _input.Split(Environment.NewLine + Environment.NewLine);
            var comparer = new PacketComparer();
            var listOfPackets = new List<Packet>();
            foreach (var item in packetCouples)
            {
                var packetString = item.Split(Environment.NewLine);
                var a = Packet.Parse(packetString[0]);
                var b = Packet.Parse(packetString[1]);
                listOfPackets.Add(a);
                listOfPackets.Add(b);
            }
            var dividerPacket2 = Packet.Parse("[[2]]");
            var dividerPacket6 = Packet.Parse("[[6]]");
            listOfPackets.Add(dividerPacket2);//first divider packet
            listOfPackets.Add(dividerPacket6);//second divider packet
            listOfPackets.Sort(comparer);
            int index = 1;
            var correct = new List<int>();
            foreach (var packet in listOfPackets)
            {
                if(comparer.Compare(packet, dividerPacket2) == 0
                    || comparer.Compare(packet, dividerPacket6) == 0)
                {
                    correct.Add(index);
                }
                index++;
            }
            var m = correct.Aggregate((a, b) => a * b);
            return new ValueTask<string>(m.ToString());
        }
    }
    
    public abstract class Packet
    {
        public static Packet Parse(string line)
        {
            if (string.IsNullOrEmpty(line))
            {
                return new ListPacket(new List<Packet>());
            }
            var trimmed = line.Trim();
            JsonElement json = JsonSerializer.Deserialize<JsonElement>(trimmed);
            return DeserializeRecursive(json);
        }

        private static Packet DeserializeRecursive(JsonElement json)
        {
            if (json.ValueKind == JsonValueKind.Array)
            {
                return new ListPacket(json.EnumerateArray().Select(DeserializeRecursive));
            }
            else if(json.ValueKind == JsonValueKind.Number)
            {
                return new NumberPacket(json.GetInt32());
            }
            throw new ArgumentException("Unsupported type");
        }
    }

    public class NumberPacket : Packet
    {
        public NumberPacket(int value)
        {
            Value = value;
        }
        public int Value { get; set; }
        public override string ToString()
        {
            return Value.ToString();
        }
    }
    public class ListPacket : Packet
    {
        public ListPacket(IEnumerable<Packet> values)
        {
            Values = new Packet[values.Count()];
            int i = 0;
            foreach (var item in values)
            {
                Values[i] = item;
                i++;
            }
        }
        public Packet[] Values { get; set; }
        public override string ToString()
        {
            return $"[{string.Join(",", Values.Select(x => x.ToString()))}]"; ;
        }
    }

    public class PacketComparer : IComparer<Packet>
    {
        public int Compare(Packet? x, Packet? y)
        {
            if (x == null)
            {
                throw new ArgumentException(nameof(x));
            }
            if(y == null)
            {
                throw new ArgumentException(nameof(y));
            }
            
            //first one: are both numbers?
            if(x is NumberPacket xNumber && y is NumberPacket yNumber)
            {
                //both are nubmers, go with int.CompareTo(int)
                return xNumber.Value.CompareTo(yNumber.Value);
            }

            //normalizing to list
            var xList = x as ListPacket;
            if (xList == null)
            {
                xList = new ListPacket(new List<Packet>() { x });
            }
            var yList = y as ListPacket;
            if(yList == null)
            {
                yList = new ListPacket(new List<Packet>() { y });
            }
            var minSize = Math.Min(xList.Values.Length, yList.Values.Length);
            if(xList.Values.Length == yList.Values.Length && minSize == 0) 
            {
                //both list are empty
                return 0;
            }
            for (int i = 0; i < minSize; i++)
            {
                var res = Compare(xList.Values[i], yList.Values[i]);
                if(res != 0)
                {
                    //one of the element is different, return res
                    return res;
                }
            }
            //iteration stopped because end of shortest list reached.
            //comparing lenght
            return xList.Values.Length.CompareTo(yList.Values.Length);
        }

        
    }
}
