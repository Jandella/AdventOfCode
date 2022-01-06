using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
namespace _2021AdventOfCode
{

    /// <summary>
    /// --- Day 3: Binary Diagnostic ---
    /// The submarine has been making some odd creaking noises, so you ask it to produce a diagnostic report just in case.
    /// The diagnostic report(your puzzle input) consists of a list of binary numbers which, when decoded properly, can tell you many useful things about the conditions of the submarine.The first parameter to check is the power consumption.
    /// You need to use the binary numbers in the diagnostic report to generate two new binary numbers(called the gamma rate and the epsilon rate). The power consumption can then be found by multiplying the gamma rate by the epsilon rate.
    /// </summary>
    public class Day03
    {
        private string _day3Input = "";

        public Day03()
        {
            _day3Input = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Inputs", "Day03Input.txt");
        }

        private byte[][] ParseInput()
        {
            return _day3Input
                .Split('\n')
                .Select(x => ToByteArray(x.Trim()))
                .ToArray();
        }

        private byte[] ToByteArray(string s)
        {
            var byteArray = new byte[s.Length];
            int i = 0;
            foreach (var character in s)
            {
                if(character == '1')
                {
                    byteArray[i] = 1;
                }
                else
                {
                    byteArray[i] = 0;
                }
                i++;
            }
            return byteArray;
        }
        public int Quiz1()
        {
            var input = ParseInput();
            var gammaRate = 0;
            var epsilonRate = 0;
            int pos = 0;
            var size = input.First().Length;
            for (int col = size -1; col >= 0; col--)
            {
                int countOnes = 0;
                int countZeroes = 0;
                for (int row = 0; row < input.Length; row++)
                {
                    var b = input[row][col];
                    if(b == 0)
                    {
                        countZeroes++;
                    }
                    else
                    {
                        countOnes++;
                    }
                }

                byte nextGammaRateBit;
                byte nextEpsilonRateBit;
                if (countOnes > countZeroes)
                {
                    nextGammaRateBit = 1;
                    nextEpsilonRateBit = 0;
                }
                else
                {
                    nextGammaRateBit = 0;
                    nextEpsilonRateBit = 1;
                }
                gammaRate = gammaRate + ((int)nextGammaRateBit << pos);
                epsilonRate = epsilonRate + ((int)nextEpsilonRateBit << pos);
                pos++;
            }
            return gammaRate * epsilonRate;
        }
        public int Quiz2()
        {
            var input = ParseInput();
            var oxygenGeneratorRate = 0;
            var co2scrubRate = 0;
            int pos = 0;
            var size = input.First().Length;
            var oxigen = input;
            var co2 = input;
            int i = 0;
            while(i < size)
            {
                oxigen = FilterOxygen(oxigen, i);
                co2 = FilterCo2(co2, i);
                i++;
            }
            
            for (int j = size - 1; j >= 0; j--)
            {
                oxygenGeneratorRate = oxygenGeneratorRate + ((int)oxigen.Single()[j] << pos);
                co2scrubRate = co2scrubRate + ((int)co2.Single()[j] << pos);
                pos++;
            }
            
            return oxygenGeneratorRate * co2scrubRate;
        }

        private byte[][] FilterOxygen(byte[][] input, int index)
        {
            if(input.Length == 1)
            {
                return input.ToArray();
            }
            var onePos = new List<int>();
            var zeroPos = new List<int>();
            for (int row = 0; row < input.Length; row++)
            {
                if (input[row][index] == 0)
                {
                    
                    zeroPos.Add(row);
                }
                else
                {
                    onePos.Add(row);
                }
            }
            var res = new List<byte[]>();
            if(onePos.Count >= zeroPos.Count)
            {
                foreach (var item in onePos)
                {
                    res.Add(input[item]);
                }
            }
            else
            {
                foreach (var item in zeroPos)
                {
                    res.Add(input[item]);
                }
            }
            return res.ToArray();
        }
        private byte[][] FilterCo2(byte[][] input, int index)
        {
            if (input.Length == 1)
            {
                return input.ToArray();
            }
            var onePos = new List<int>();
            var zeroPos = new List<int>();
            for (int row = 0; row < input.Length; row++)
            {
                if (input[row][index] == 0)
                {

                    zeroPos.Add(row);
                }
                else
                {
                    onePos.Add(row);
                }
            }
            var res = new List<byte[]>();
            if (onePos.Count < zeroPos.Count)
            {
                foreach (var item in onePos)
                {
                    res.Add(input[item]);
                }
            }
            else
            {
                foreach (var item in zeroPos)
                {
                    res.Add(input[item]);
                }
            }
            return res.ToArray();
        }
    }

    
}
