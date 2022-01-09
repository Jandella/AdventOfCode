using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

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
            TestExplode();
            TestSplit();
            TestReduce();
            TestSums();
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
        private void TestExplode()
        {
            var n1 = new SnailNumber("[[[[[9,8],1],2],3],4]");
            Debug.Assert(n1.Explode());
            Debug.Assert("[[[[0,9],2],3],4]" == n1.ToString());
            Debug.Assert(!n1.Explode());
            var n2 = new SnailNumber(" [7,[6,[5,[4,[3,2]]]]]");
            Debug.Assert(n2.Explode());
            Debug.Assert("[7,[6,[5,[7,0]]]]" == n2.ToString());
            var n3 = new SnailNumber("[[6,[5,[4,[3,2]]]],1]");
            Debug.Assert(n3.Explode());
            Debug.Assert("[[6,[5,[7,0]]],3]" == n3.ToString());
            var n4 = new SnailNumber("[[3,[2,[1,[7,3]]]],[6,[5,[4,[3,2]]]]]");
            Debug.Assert(n4.Explode());
            Debug.Assert("[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]" == n4.ToString());
            Debug.Assert(n4.Explode());
            Debug.Assert("[[3,[2,[8,0]]],[9,[5,[7,0]]]]" == n4.ToString());
            Debug.Assert(!n4.Explode());

        }
        private void TestSplit()
        {
            var n1 = new SnailNumber("[[[[[4,3],4],4],[7,[[8,4],9]]],[1,1]]");
            while (n1.Explode()) ;
            Debug.Assert(n1.Split());
            Debug.Assert("[[[[0,7],4],[[7,8],[0,13]]],[1,1]]" == n1.ToString());
            Debug.Assert(n1.Split());
            Debug.Assert("[[[[0,7],4],[[7,8],[0,[6,7]]]],[1,1]]" == n1.ToString());
            Debug.Assert(!n1.Split());
        }
        private void TestReduce()
        {
            var n5 = new SnailNumber("[[[[[1,1],[2,2]],[3,3]],[4,4]],[5,5]]");
            n5.Reduce();
            Debug.Assert("[[[[3,0],[5,3]],[4,4]],[5,5]]" == n5.ToString());

            var n6 = new SnailNumber("[[[[0,[4,5]],[0,0]],[[[4,5],[2,6]],[9,5]]],[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]]");
            n6.Reduce();
            Debug.Assert("[[[[4,0],[5,4]],[[7,7],[6,0]]],[[8,[7,7]],[[7,9],[5,0]]]]" == n6.ToString());
        }
        private void TestSums()
        {
            var test = @"[1,1]
[2,2]
[3,3]
[4,4]";
            TestSum(test, "[[[[1,1],[2,2]],[3,3]],[4,4]]");

            test = @"[1,1]
[2,2]
[3,3]
[4,4]
[5,5]";
            TestSum(test, "[[[[3,0],[5,3]],[4,4]],[5,5]]");


            TestSum(@"[[[0,[4,5]],[0,0]],[[[4,5],[2,6]],[9,5]]]
[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]", "[[[[4,0],[5,4]],[[7,7],[6,0]]],[[8,[7,7]],[[7,9],[5,0]]]]");

            TestSum(@"[[[[4,0],[5,4]],[[7,7],[6,0]]],[[8,[7,7]],[[7,9],[5,0]]]]
[[2,[[0,8],[3,4]]],[[[6,7],1],[7,[1,6]]]]", "[[[[6,7],[6,7]],[[7,7],[0,7]]],[[[8,7],[7,7]],[[8,8],[8,0]]]]");

            TestSum(@"[[[[6,7],[6,7]],[[7,7],[0,7]]],[[[8,7],[7,7]],[[8,8],[8,0]]]]
[[[[2,4],7],[6,[0,5]]],[[[6,8],[2,8]],[[2,1],[4,5]]]]", "[[[[7,0],[7,7]],[[7,7],[7,8]]],[[[7,7],[8,8]],[[7,7],[8,7]]]]");

            TestSum(@"[[[[7,0],[7,7]],[[7,7],[7,8]]],[[[7,7],[8,8]],[[7,7],[8,7]]]]
[7,[5,[[3,8],[1,4]]]]", "[[[[7,7],[7,8]],[[9,5],[8,7]]],[[[6,8],[0,8]],[[9,9],[9,0]]]]");

            TestSum(@"[[[[7,7],[7,8]],[[9,5],[8,7]]],[[[6,8],[0,8]],[[9,9],[9,0]]]]
[[2,[2,2]],[8,[8,1]]]", "[[[[6,6],[6,6]],[[6,0],[6,7]]],[[[7,7],[8,9]],[8,[8,1]]]]");

            TestSum(@"[[[[6,6],[6,6]],[[6,0],[6,7]]],[[[7,7],[8,9]],[8,[8,1]]]]
[2,9]", "[[[[6,6],[7,7]],[[0,7],[7,7]]],[[[5,5],[5,6]],9]]");

            TestSum(@"[[[[6,6],[7,7]],[[0,7],[7,7]]],[[[5,5],[5,6]],9]]
[1,[[[9,3],9],[[9,0],[0,7]]]]", "[[[[7,8],[6,7]],[[6,8],[0,8]]],[[[7,7],[5,0]],[[5,5],[5,6]]]]");

            TestSum(@"[[[[7,8],[6,7]],[[6,8],[0,8]]],[[[7,7],[5,0]],[[5,5],[5,6]]]]
[[[5,[7,4]],7],1]", "[[[[7,7],[7,7]],[[8,7],[8,7]]],[[[7,0],[7,7]],9]]");

            TestSum(@"[[[[7,7],[7,7]],[[8,7],[8,7]]],[[[7,0],[7,7]],9]]
[[[[4,2],2],6],[8,7]]", "[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]");




            test = @"[[[0,[5,8]],[[1,7],[9,6]]],[[4,[1,2]],[[1,4],2]]]
[[[5,[2,8]],4],[5,[[9,9],0]]]
[6,[[[6,2],[5,6]],[[7,6],[4,7]]]]
[[[6,[0,7]],[0,9]],[4,[9,[9,0]]]]
[[[7,[6,4]],[3,[1,3]]],[[[5,5],1],9]]
[[6,[[7,3],[3,2]]],[[[3,8],[5,7]],4]]
[[[[5,4],[7,7]],8],[[8,3],8]]
[[9,3],[[9,9],[6,[4,9]]]]
[[2,[[7,7],7]],[[5,8],[[9,3],[0,2]]]]
[[[[5,2],5],[8,[3,7]]],[[5,[7,5]],[4,4]]]";
            var sum = TestSum(test, "[[[[6,6],[7,6]],[[7,7],[7,0]]],[[[7,7],[7,7]],[[7,8],[9,9]]]]");
            Debug.Assert(4140 == sum.CalculateMagnitude());

        }


        private SnailNumber TestSum(string input, string expected)
        {
            var addends = ParseInput(input);
            SnailNumber sum = addends.First();
            for (int i = 1; i < addends.Length; i++)
            {
                sum = sum + addends[i];
                sum.Reduce();
            }
            Debug.Assert(expected == sum.ToString());
            return sum;
        }
        private SnailNumber[] ParseInput(string input)
        {
            return input.Split("\n")
                .Select(x => x.Trim())
                .Select(x => new SnailNumber(x))
                .ToArray();
        }
        public int Quiz1()
        {
            var addends = ParseInput(_day18input);
            SnailNumber sum = addends.First();
            for (int i = 1; i < addends.Length; i++)
            {
                sum = sum + addends[i];
                sum.Reduce();
            }
            return sum.CalculateMagnitude();
        }

        public long Quiz2()
        {
            var addends = ParseInput(_day18input);
            long maxVal = 0;
            for (int i = 0; i < addends.Length; i++)
            {
                for (int j = 0; j < addends.Length; j++)
                {
                    if (i == j) break;
                    var sum = addends[i] + addends[j];
                    sum.Reduce();
                    long val = sum.CalculateMagnitude();
                    if (val > maxVal)
                        maxVal = val;
                }
            }
            return maxVal;
        }
    }


    public class SnailNumber
    {
        public int? _value = null;
        public SnailNumber _parent = null;
        public SnailNumber _left = null;
        public SnailNumber _right = null;
        private SnailNumber()
        {

        }
        public SnailNumber(SnailNumber parent, SnailNumber left, SnailNumber right)
        {
            _parent = parent;
            _left = left;
            _right = right;
        }
        public SnailNumber(SnailNumber p, int value)
        {
            _parent = p;
            _value = value;
            _left = null;
            _right = null;
        }

        public SnailNumber(string number)
        {
            var n = Parse(number);
            _parent = null;
            _left = n._left;
            _right = n._right;
        }
        private static SnailNumber Parse(string number)
        {
            SnailNumber current = null;
            Stack<SnailNumber> pairStack = new Stack<SnailNumber>();
            for (int i = 0; i < number.Length; i++)
            {
                var currentChar = number.ElementAt(i);
                var nextChar = char.MinValue;
                if (i < number.Length - 1)
                    nextChar = number.ElementAt(i + 1);
                if (currentChar == '[')
                {
                    var newNode = new SnailNumber();
                    newNode._parent = current;
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
                    if (!current.IsLeaf && current._left == null)
                    {
                        current._left = childNode;
                        childNode._parent = current;
                    }
                    else if (!current.IsLeaf && current._right == null)
                    {
                        current._right = childNode;
                        childNode._parent = current;
                    }
                }
                else if (int.TryParse(currentChar.ToString(), out int x) && nextChar == ',')
                {
                    current._left = new SnailNumber(current, x);
                }
                else if (currentChar == ',' && int.TryParse(nextChar.ToString(), out int y))
                {
                    current._right = new SnailNumber(current, y);
                }
            }
            return current;
        }
        public bool IsLeaf => _value != null;

        public static SnailNumber operator +(SnailNumber a, SnailNumber b)
        {
            var res = new SnailNumber(null, a, b);
            a._parent = res;
            b._parent = res;
            //HACK! I don't know why, if I reduce res I obtain the wrong number
            // but if I do toString and then reduce => I have the right number 
            var number = res.ToString();
            res = new SnailNumber(number);
            return res;
        }
        public void Reduce()
        {
            /*
             * To reduce a snailfish number, you must repeatedly do the first action in this list that applies to the snailfish number:
             * - If any pair is nested inside four pairs, the leftmost such pair explodes.
             * - If any regular number is 10 or greater, the leftmost such regular number splits.
             * During reduction, at most one action applies, after which the process returns to the top of the list of actions. 
             * For example, if split produces a pair that meets the explode criteria, that pair explodes before other splits occur.
             */
            while (Explode() || Split()) // lazy evaluation of || FTW
            {
                while (Explode()) ;//explosion takes precedence over splitting. All possible explosions need to be taken care of first.
                Split();
            }
        }

        public bool Explode()
        {
            /*
             * To explode a pair, the pair's left value is added to the first regular 
             * number to the left of the exploding pair (if any), and the pair's right 
             * value is added to the first regular number to the right of the exploding 
             * pair (if any). Exploding pairs will always consist of two regular numbers.
             * Then, the entire exploding pair is replaced with the regular number 0.
             */
            return ExplodeInternal(0);
        }

        private bool ExplodeInternal(int depth)
        {
            var result = false;
            if (IsLeaf)
                result = false;

            if (depth == 3)
            {
                if (_left != null && !_left.IsLeaf)
                {
                    var leftRegularNumber = _left.FindFirstLeftRegularNumber();
                    if (leftRegularNumber != null)
                        leftRegularNumber._value += _left._left._value;
                    var rightRegularNumber = _left.FindFirstRightRegularNumber();
                    if (rightRegularNumber != null)
                        rightRegularNumber._value += _left._right._value;
                    _left = new SnailNumber(this, 0);
                    result = true;
                }
                if (_right != null && !_right.IsLeaf)
                {
                    var leftRegularNumber = _right.FindFirstLeftRegularNumber();
                    if (leftRegularNumber != null)
                        leftRegularNumber._value += _right._left._value;
                    var rightRegularNumber = _right.FindFirstRightRegularNumber();
                    if (rightRegularNumber != null)
                        rightRegularNumber._value += _right._right._value;
                    _right = new SnailNumber(this, 0);
                    result = true;
                }
                return result;
            }

            if (!result && _left != null)
                result = _left.ExplodeInternal(depth + 1);

            if (!result && _right != null)
                result = _right.ExplodeInternal(depth + 1);

            return result;
        }

        private SnailNumber FindFirstLeftRegularNumber()
        {
            if (_parent == null) return null;

            var currentNode = this;
            var firstLeftRegularNumber = _parent;

            while (firstLeftRegularNumber != null && firstLeftRegularNumber._left == currentNode)
            {
                currentNode = firstLeftRegularNumber;
                firstLeftRegularNumber = firstLeftRegularNumber._parent;
            }

            if (firstLeftRegularNumber == null) return null;

            if (firstLeftRegularNumber.IsLeaf) return firstLeftRegularNumber;

            firstLeftRegularNumber = firstLeftRegularNumber._left;

            while (firstLeftRegularNumber._right != null)
            {
                firstLeftRegularNumber = firstLeftRegularNumber._right;
            }
            return firstLeftRegularNumber;
        }

        private SnailNumber FindFirstRightRegularNumber()
        {
            if (_parent == null) return null;

            var currentNode = this;
            var firstRightRegularNumber = _parent;

            while (firstRightRegularNumber != null && firstRightRegularNumber._right == currentNode)
            {
                currentNode = firstRightRegularNumber;
                firstRightRegularNumber = firstRightRegularNumber._parent;
            }

            if (firstRightRegularNumber == null) return null;

            if (firstRightRegularNumber.IsLeaf) return firstRightRegularNumber;

            firstRightRegularNumber = firstRightRegularNumber._right;

            while (firstRightRegularNumber._left != null)
            {
                firstRightRegularNumber = firstRightRegularNumber._left;
            }
            return firstRightRegularNumber;
        }

        public bool Split()
        {
            /* To split a regular number, replace it with a pair; the left element of the 
             * pair should be the regular number divided by two and rounded down, while 
             * the right element of the pair should be the regular number divided by two and rounded up.
             */
            var res = false;
            //first: split left subtree
            if (_left != null)
            {
                res = _left.Split();
            }
            if (res) return true; //max 1 split each time

            if (IsLeaf && _value >= 10)
            {
                var x = (int)Math.Floor((double)_value / 2.0);
                var y = (int)Math.Ceiling((double)_value / 2.0);
                _value = null;
                _left = new SnailNumber(this, x);
                _right = new SnailNumber(this, y);
                return true;
            }

            //split right subtree
            if (_right != null)
            {
                res = _right.Split() || res;
            }

            if (res) return true;

            return false;
        }

        public int CalculateMagnitude()
        {
            if (_value != null)
                return _value.Value;
            return (3 * _left.CalculateMagnitude()) + (2 * _right.CalculateMagnitude());
        }

        public override string ToString()
        {
            if (_value != null)
            {
                return _value.ToString();
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append('[').Append(_left).Append(',').Append(_right).Append(']');
                return sb.ToString();
            }
        }


    }
}
