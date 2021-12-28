using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021AdventOfCode
{
    public class Day16
    {
        private string _day16input = @"005473C9244483004B001F79A9CE75FF9065446725685F1223600542661B7A9F4D001428C01D8C30C61210021F0663043A20042616C75868800BAC9CB59F4BC3A40232680220008542D89B114401886F1EA2DCF16CFE3BE6281060104B00C9994B83C13200AD3C0169B85FA7D3BE0A91356004824A32E6C94803A1D005E6701B2B49D76A1257EC7310C2015E7C0151006E0843F8D000086C4284910A47518CF7DD04380553C2F2D4BFEE67350DE2C9331FEFAFAD24CB282004F328C73F4E8B49C34AF094802B2B004E76762F9D9D8BA500653EEA4016CD802126B72D8F004C5F9975200C924B5065C00686467E58919F960C017F00466BB3B6B4B135D9DB5A5A93C2210050B32A9400A9497D524BEA660084EEA8EF600849E21EFB7C9F07E5C34C014C009067794BCC527794BCC424F12A67DCBC905C01B97BF8DE5ED9F7C865A4051F50024F9B9EAFA93ECE1A49A2C2E20128E4CA30037100042612C6F8B600084C1C8850BC400B8DAA01547197D6370BC8422C4A72051291E2A0803B0E2094D4BB5FDBEF6A0094F3CCC9A0002FD38E1350E7500C01A1006E3CC24884200C46389312C401F8551C63D4CC9D08035293FD6FCAFF1468B0056780A45D0C01498FBED0039925B82CCDCA7F4E20021A692CC012B00440010B8691761E0002190E21244C98EE0B0C0139297660B401A80002150E20A43C1006A0E44582A400C04A81CD994B9A1004BB1625D0648CE440E49DC402D8612BB6C9F5E97A5AC193F589A100505800ABCF5205138BD2EB527EA130008611167331AEA9B8BDCC4752B78165B39DAA1004C906740139EB0148D3CEC80662B801E60041015EE6006801364E007B801C003F1A801880350100BEC002A3000920E0079801CA00500046A800C0A001A73DFE9830059D29B5E8A51865777DCA1A2820040E4C7A49F88028B9F92DF80292E592B6B840";
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

        public int Quiz1()
        {
            var a = Decode("D2FE28");
            throw new NotImplementedException();
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
            var res = new Packet();
            res.Version = GetIntFromBitArray(bits, 0, 3);
            res.Type = GetIntFromBitArray(bits, 3, 6);
            if (res.IsOperator)
            {
                if (bits[6])
                {
                    var subpacketLength = GetIntFromBitArray(bits, 7, 7 + 15);
                }
                else
                {
                    var numbersOfPackets = GetIntFromBitArray(bits, 7, 7 + 11);
                }
            }
            else
            {
                res.LiteralValue = GetLiteralValue(bits, 6, bits.Length);
            }
            return res;

        }

        private int GetIntFromBitArray(BitArray bitArray, int startIndex, int endIndex)
        {
            int value = 0;
            int size = endIndex - startIndex;
            //MSB
            int shift = size - 1;
            for (int i = 0; i < size; i++)
            {
                if (bitArray[i + startIndex])
                    value += 1 << shift;
                shift--;
            }

            return value;
        }

        private int GetLiteralValue(BitArray bitArray, int start, int end)
        {
            int countBits = 0;
            List<BitArray> b = new List<BitArray>();
            BitArray current = new BitArray(4);
            bool isLast = false;
            for (int i = start; i < end; i++)
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
            return GetIntFromBitArray(literal, 0, literal.Count);
        }
        public int Quiz2()
        {
            throw new NotImplementedException();
        }
    }

    public class Packet
    {
        public int Version { get; set; }
        public int Type { get; set; }

        public bool IsOperator => Type != 0b100;
        public int? LiteralValue { get; set; }

        public List<Packet> Subpackets { get; set; } = new List<Packet>();
    }
    
}
