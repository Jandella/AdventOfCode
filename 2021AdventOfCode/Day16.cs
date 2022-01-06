using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021AdventOfCode
{
    /// <summary>
    /// --- Day 16: Packet Decoder ---
    /// As you leave the cave and reach open waters, you receive a transmission from the Elves back on the ship.
    /// The transmission was sent using the Buoyancy Interchange Transmission System(BITS), a method of packing numeric expressions into a binary sequence.Your submarine's computer has saved the transmission in hexadecimal (your puzzle input).
    /// </summary>
    public class Day16
    {
        private string _day16input = @"";
        private Dictionary<char, byte[]> _encoding = new Dictionary<char, byte[]>
        {
            ['0'] = new byte[] { 0, 0, 0, 0 },
            ['1'] = new byte[] { 0, 0, 0, 1 },
            ['2'] = new byte[] { 0, 0, 1, 0 },
            ['3'] = new byte[] { 0, 0, 1, 1 },
            ['4'] = new byte[] { 0, 1, 0, 0 },
            ['5'] = new byte[] { 0, 1, 0, 1 },
            ['6'] = new byte[] { 0, 1, 1, 0 },
            ['7'] = new byte[] { 0, 1, 1, 1 },
            ['8'] = new byte[] { 1, 0, 0, 0 },
            ['9'] = new byte[] { 1, 0, 0, 1 },
            ['A'] = new byte[] { 1, 0, 1, 0 },
            ['B'] = new byte[] { 1, 0, 1, 1 },
            ['C'] = new byte[] { 1, 1, 0, 0 },
            ['D'] = new byte[] { 1, 1, 0, 1 },
            ['E'] = new byte[] { 1, 1, 1, 0 },
            ['F'] = new byte[] { 1, 1, 1, 1 }
        };
        public Day16()
        {
            _day16input = System.IO.File.ReadAllText(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Inputs", "Day16Input.txt"));
        }
        public int Quiz1()
        {
            var code1 = Decode("8A004A801A8002F478");
            var sumVersions1 = code1.SumVersions();
            System.Diagnostics.Debug.Assert(sumVersions1 == 16, $"First test fail ({sumVersions1} != 16)");
            var code2 = Decode("620080001611562C8802118E34");
            var sumVersions2 = code2.SumVersions();
            System.Diagnostics.Debug.Assert(sumVersions2 == 12, $"Second test fail ({sumVersions2} != 12)");
            var code3 = Decode("C0015000016115A2E0802F182340");
            var sumVersions3 = code3.SumVersions();
            System.Diagnostics.Debug.Assert(sumVersions3 == 23, $"Third test fail ({sumVersions3} != 23)");
            var code4 = Decode("A0016C880162017C3686B18A3D4780");
            var sumVersions4 = code4.SumVersions();
            System.Diagnostics.Debug.Assert(sumVersions4 == 31, $"Fourth test fail ({sumVersions4} != 31)");

            var code = Decode(_day16input);
            return code.SumVersions();
        }

        public BitArray ToCodedBits(string input)
        {
            var res = new BitArray(input.Length * 4);
            int i = 0;
            foreach (var item in input)
            {
                var bits = _encoding[item];
                for (int j = 0; j < bits.Length; j++)
                {
                    res[i + j] = bits[j] == 1;
                }
                i = i + bits.Length;
            }
            return res;
        }

        private Packet Decode(string input)
        {
            var bits = ToCodedBits(input);
            var res = DecodePacket(bits, 0, bits.Length);
            return res.Item2;

        }

        private (int?, Packet) DecodePacket(BitArray array, int start, int end)
        {
            if (start >= end)
                return (default, null);
            int typeIndex = start + 3;
            int packetIndex = typeIndex + 3;
            if (typeIndex >= end)
                return (default, null);
            if (packetIndex >= end)
                return (default, null);

            var res = new Packet();
            res.Version = (int)GetUlongFromBitArray(array, start, typeIndex);
            res.Type = (int)GetUlongFromBitArray(array, typeIndex, packetIndex);
            int index = end;
            if (res.IsOperator)
            {
                int subPacketIndex = packetIndex + 1;
                int subpacketStartIndex;
                if (array[packetIndex])
                {
                    subpacketStartIndex = subPacketIndex + 11;
                    int numbersOfPackets = (int)GetUlongFromBitArray(array, subPacketIndex, subpacketStartIndex);
                    int bitsIterator = subpacketStartIndex;
                    for (int i = 0; i < numbersOfPackets; i++)
                    {
                        if (bitsIterator >= end)
                            break;

                        var sub = DecodePacket(array, bitsIterator, end);
                        if (sub.Item1 != null && sub.Item2 != null)
                        {
                            res.Subpackets.Add(sub.Item2);
                            bitsIterator = sub.Item1.Value;
                            index = sub.Item1.Value;
                        }
                    }
                }
                else
                {
                    subpacketStartIndex = subPacketIndex + 15;
                    var subpacketLength = (int)GetUlongFromBitArray(array, subPacketIndex, subpacketStartIndex);
                    int? bitsIterator = subpacketStartIndex;
                    while (bitsIterator != null && bitsIterator < (subpacketStartIndex + subpacketLength))
                    {
                        var sub = DecodePacket(array, bitsIterator.Value, end);
                        if (sub.Item2 != null)
                        {
                            res.Subpackets.Add(sub.Item2);
                            index = sub.Item1.Value;
                        }

                        bitsIterator = sub.Item1;
                    }
                }
            }
            else
            {
                var decodedLiteralValue = GetLiteralValue(array, packetIndex);
                res.LiteralValue = decodedLiteralValue.LiteralValue;
                index = decodedLiteralValue.Index;
            }

            return (index, res);
        }

        private ulong GetUlongFromBitArray(BitArray bitArray, int startIndex, int endIndex)
        {
            ulong value = 0;
            int size = endIndex - startIndex;
            //MSB
            int shift = size - 1;
            for (int i = 0; i < size; i++)
            {
                if (bitArray[i + startIndex])
                    value += 1UL << shift;
                shift--;
            }

            return value;
        }

        private LiteralResult GetLiteralValue(BitArray bitArray, int start)
        {
            int countBits = 0;
            List<BitArray> b = new List<BitArray>();
            BitArray current = new BitArray(4);
            bool isLast = false;
            bool stop = false;
            int i = start;
            while (!stop && i < bitArray.Count)
            {
                if (countBits == 0 && !isLast)
                {
                    //check group
                    if (!bitArray[i])
                    {
                        //last packet
                        isLast = true;
                    }
                    current = new BitArray(4);
                    b.Add(current);
                }
                else if (countBits < 5)
                {
                    current[countBits - 1] = bitArray[i];
                }
                countBits++;
                if (countBits >= 5 && !isLast)
                    countBits = 0;
                if (countBits >= 5 && isLast)
                {
                    //last read, end while
                    stop = true;
                }
                i++;
            }
            var literal = new BitArray(b.Count * 4);
            int k = 0;
            foreach (var c in b)
            {
                for (int j = 0; j < 4; j++)
                {
                    literal[k + j] = c[j];
                }
                k = k + c.Length;
            }
            var res = new LiteralResult
            {
                LiteralValue = GetUlongFromBitArray(literal, 0, literal.Count),
                Index = i,
            };
            
            return res;
        }


        public ulong Quiz2()
        {
            var code1 = Decode("C200B40A82");
            var value1 = code1.CalculateValue();
            System.Diagnostics.Debug.Assert(value1 == 3, $"Test fail ({value1} != 3)");
            var code2 = Decode("04005AC33890");
            var value2 = code2.CalculateValue();
            System.Diagnostics.Debug.Assert(value2 == 54, $"Test fail ({value1} != 54)");
            var code3 = Decode("880086C3E88112");
            var value3 = code3.CalculateValue();
            System.Diagnostics.Debug.Assert(value3 == 7, $"Test fail ({value1} != 7)");
            
            var code4 = Decode("CE00C43D881120");
            var value4 = code4.CalculateValue();
            System.Diagnostics.Debug.Assert(value4 == 9, $"Test fail ({value1} != 9)");

            var code5 = Decode("D8005AC2A8F0");
            var value5 = code5.CalculateValue();
            System.Diagnostics.Debug.Assert(value5 == 1, $"(1)Test fail ({value1} != 1)");
            var code6 = Decode("F600BC2D8F");
            var value6 = code6.CalculateValue();
            System.Diagnostics.Debug.Assert(value6 == 0, $"(2)Test fail ({value1} != 0)");
            var code7 = Decode("9C005AC2F8F0");
            var value7 = code7.CalculateValue();
            System.Diagnostics.Debug.Assert(value7 == 0, $"(3)Test fail ({value1} != 0)");
            var code8 = Decode("9C0141080250320F1802104A08");
            var value8 = code8.CalculateValue();
            System.Diagnostics.Debug.Assert(value8 == 1, $"(4)Test fail ({value1} != 1)");

            var code = Decode(_day16input);
            var res = code.CalculateValue();
            return res;
        }
    }
    public class LiteralResult
    {
        public ulong LiteralValue { get; set; }
        /// <summary>
        /// The first empty bit (bit that is not into the literal value bits)
        /// </summary>
        public int Index { get; set; }
    }
    public class Packet
    {
        public int Version { get; set; }
        public int Type { get; set; }

        public bool IsOperator => Type != 0b100;
        public ulong? LiteralValue { get; set; }

        public List<Packet> Subpackets { get; set; } = new List<Packet>();


        public int SumVersions()
        {
            if (Subpackets == null || !Subpackets.Any())
                return Version;
            var sum = Version;
            foreach (var item in Subpackets)
            {
                sum += item.SumVersions();
            }
            return sum;
        }

        public ulong CalculateValue()
        {
            

            if (!IsOperator)
                return LiteralValue ?? 0;

            if (Subpackets == null || !Subpackets.Any())
                return LiteralValue ?? 0;

            ulong res;
            switch (Type)
            {
                case 0: //Packets with type ID 0 are sum packets
                    res = 0;
                    foreach (var item in Subpackets)
                    {
                        res += item.CalculateValue();
                    }
                    break;
                case 1: //Packets with type ID 1 are product packets
                    res = 1;
                    foreach (var item in Subpackets)
                    {
                        res *= item.CalculateValue();
                    }
                    break;
                case 2: //Packets with type ID 2 are minimum packets
                    var listMinValues = new List<ulong>();
                    foreach (var item in Subpackets)
                    {
                        listMinValues.Add(item.CalculateValue());
                    }
                    res = listMinValues.Min();
                    break;
                case 3: //Packets with type ID 3 are maximum packets
                    var listMaxValues = new List<ulong>();
                    foreach (var item in Subpackets)
                    {
                        listMaxValues.Add(item.CalculateValue());
                    }
                    res = listMaxValues.Max();
                    break;
                case 5: //Packets with type ID 5 are greater than packets
                    /* their value is 1 if the value of the first sub-packet is greater than the value of the second sub-packet; 
                     * otherwise, their value is 0. These packets always have exactly two sub-packets.
                     */
                    if(Subpackets[0].CalculateValue() > Subpackets[1].CalculateValue())
                    {
                        res = 1;
                    }
                    else
                    {
                        res = 0;
                    }
                    break;
                case 6: // Packets with type ID 6 are less than packets
                    /*
                     * their value is 1 if the value of the first sub-packet is less than the value of the second sub-packet; 
                     * otherwise, their value is 0. These packets always have exactly two sub-packets.
                     */
                    if (Subpackets[0].CalculateValue() < Subpackets[1].CalculateValue())
                    {
                        res = 1;
                    }
                    else
                    {
                        res = 0;
                    }
                    break;
                case 7: //Packets with type ID 7 are equal to packets
                    /*
                     * their value is 1 if the value of the first sub-packet is equal to the value of the second sub-packet; 
                     * otherwise, their value is 0. These packets always have exactly two sub-packets.
                     */
                    if (Subpackets[0].CalculateValue() == Subpackets[1].CalculateValue())
                    {
                        res = 1;
                    }
                    else
                    {
                        res = 0;
                    }
                    break;
                default:
                    res = LiteralValue ?? 0;
                    break;
            }
            return res;
        }
    }

}
