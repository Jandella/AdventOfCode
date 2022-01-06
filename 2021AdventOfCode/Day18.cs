using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021AdventOfCode
{
    /// <summary>
    /// --- Day 18: Snailfish ---
    /// You descend into the ocean trench and encounter some snailfish.They say they saw the sleigh keys! 
    /// They'll even tell you which direction the keys went if you help one of the smaller snailfish with his math homework.
    /// </summary>
    public class Day18
    {
        private string _day18input = "";
        public Day18()
        {
            _day18input = System.IO.File.ReadAllText(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Inputs", "Day18Input.txt"));
        }

        public void Test()
        {
            TestParse();
            TestMagnitude();
        }

        private void TestParse()
        {
            var n1 = new SnailNumber("[1,2]");
            Debug.Assert("[1,2]" == n1.ToString());
            var n2 = new SnailNumber("[[1,2],3]");
            Debug.Assert("[[1,2],3]" == n2.ToString());
            var n3 = new SnailNumber("[9,[8,7]]");
            Debug.Assert("[9,[8,7]]" == n3.ToString());
            var n4 = new SnailNumber("[[1,9],[8,5]]");
            Debug.Assert("[[1,9],[8,5]]" == n4.ToString());
            var n5 = new SnailNumber("[[[[1,2],[3,4]],[[5,6],[7,8]]],9]");
            Debug.Assert("[[[[1,2],[3,4]],[[5,6],[7,8]]],9]" == n5.ToString());
            var n6 = new SnailNumber("[[[9,[3,8]],[[0,9],6]],[[[3,7],[4,9]],3]]");
            Debug.Assert("[[[9,[3,8]],[[0,9],6]],[[[3,7],[4,9]],3]]" == n6.ToString());
            var n7 = new SnailNumber("[[[[1,3],[5,3]],[[1,3],[8,7]]],[[[4,9],[6,9]],[[8,2],[7,3]]]]");
            Debug.Assert("[[[[1,3],[5,3]],[[1,3],[8,7]]],[[[4,9],[6,9]],[[8,2],[7,3]]]]" == n7.ToString());
        }
        private void TestMagnitude()
        {
            var n1 = new SnailNumber("[[1,2],[[3,4],5]]");
            Debug.Assert(143 == n1.CalculateMagnitude());
            var n2 = new SnailNumber("[[[[0,7],4],[[7,8],[6,0]]],[8,1]]");
            Debug.Assert(1384 == n2.CalculateMagnitude());
            var n3 = new SnailNumber("[[[[1,1],[2,2]],[3,3]],[4,4]]");
            Debug.Assert(445 == n3.CalculateMagnitude());
            var n4 = new SnailNumber("[[[[3,0],[5,3]],[4,4]],[5,5]]");
            Debug.Assert(791 == n4.CalculateMagnitude());
            var n5 = new SnailNumber("[[[[5,0],[7,4]],[5,5]],[6,6]]");
            Debug.Assert(1137 == n5.CalculateMagnitude());
            var n6 = new SnailNumber("[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]");
            Debug.Assert(3488 == n6.CalculateMagnitude());
        }

        public int Quiz1()
        {
            throw new NotImplementedException();
        }

        public int Quiz2()
        {
            throw new NotImplementedException();
        }
    }


    public class SnailNumber
    {
        private bool _isLiteral;
        private int _literal;
        private SnailNumber[] _values = new SnailNumber[0];

        public SnailNumber()
        {
            _values = new SnailNumber[] { null, null };
            _isLiteral = false;
        }
        private SnailNumber(int x)
        {
            _isLiteral = true;
            _literal = x;
        }
        public SnailNumber(int x, int y)
        {
            _isLiteral = false;
            _values = new SnailNumber[] { new SnailNumber(x), new SnailNumber(y) };
        }
        public SnailNumber(SnailNumber x, SnailNumber y)
        {
            _isLiteral = false;
            _values = new SnailNumber[] { x, y };
        }

        public SnailNumber(string number)
        {
            _isLiteral = false;
            _values = new SnailNumber[] { null, null };
            var tmp = Parse(number);
            _values[0] = tmp._values[0];
            _values[1] = tmp._values[1];
        }
        private static SnailNumber Parse(string number)
        {
            SnailNumber current = null;
            Stack<SnailNumber> pairStack = new Stack<SnailNumber>();
            for (int i = 0; i < number.Length; i++)
            {
                var currentChar = number.ElementAt(i);
                var nextChar = char.MinValue;
                if(i < number.Length - 1)
                    nextChar = number.ElementAt(i + 1);
                if(currentChar == '[')
                {
                    var newNode = new SnailNumber();
                    current = newNode;
                    pairStack.Push(current);
                }
                else if (currentChar == ']')
                {
                    var childNode = pairStack.Pop();
                    if (!pairStack.Any())
                    {
                        current = childNode;
                        break;
                    }

                    current = pairStack.Peek();
                    if (!current._isLiteral && current._values[0] == null)
                    {
                        current._values[0] = childNode;
                    }
                    else if (!current._isLiteral && current._values[1] == null)
                    {
                        current._values[1] = childNode;
                    }
                }
                else if (int.TryParse(currentChar.ToString(), out int x) && nextChar == ',')
                {
                    current._values[0] = new SnailNumber(x);
                }
                else if(currentChar == ',' && int.TryParse(nextChar.ToString(), out int y))
                {
                    current._values[1] = new SnailNumber(y);
                }
            }
            return current;
        }

        public void Reduce()
        {
            var t = FindExplosion(0);
            if (t != null)
            {
                //todo: explode
                Reduce();
                return;
            }
        }

        private void Explode()
        {
            
        }

        private SnailNumber FindExplosion(int depth)
        {
            if (_isLiteral) return null;

            if(depth == 3)
            {
                if (!_values[0]._isLiteral)
                {
                    var numberToExplode = _values[0];
                    _values[0] = new SnailNumber(0);

                    return _values[0];
                }
                if (!_values[1]._isLiteral)
                {
                    return _values[1];
                }
                return null;
            }

            var leftExplosion = _values[0].FindExplosion(depth + 1);
            if (leftExplosion != null)
                return leftExplosion;
            return _values[1].FindExplosion(depth + 1);
        }

        private void ExecuteExplosion(SnailNumber parent, SnailNumber exploded)
        {
            if(parent._values[0].ToString() == exploded.ToString())
            {
                parent._values[0] = new SnailNumber(0);
                parent._values[1]._literal += exploded._literal;
            }
            else if (parent._values[1].ToString() == exploded.ToString())
            {
                parent._values[0]._literal += exploded._literal;
                parent._values[1] = new SnailNumber(0);
            }
        }

        private void Split()
        {

        }

        public int CalculateMagnitude()
        {
            if (_isLiteral)
                return _literal;
            return (3 * _values[0].CalculateMagnitude()) + (2 * _values[1].CalculateMagnitude());
        }

        public override string ToString()
        {
            if (_isLiteral)
            {
                return _literal.ToString();
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append('[').Append(_values[0]).Append(',').Append(_values[1]).Append(']');
                return sb.ToString();
            }
        }
    }
}
