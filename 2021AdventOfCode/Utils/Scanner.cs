using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021AdventOfCode.Utils
{
    public class Scanner
    {
        // min overlapping beacons between two scanners if they overlap
        const int MIN_MATCHES = 12;
        public List<Coordinate3D> Beacons { get; set; } = new List<Coordinate3D>();
        public Coordinate3D RelativePosition { get; set; } = new Coordinate3D();
        public Scanner()
        {

        }
        /// <summary>
        /// The string containing the list of beacons in the following format:
        /// "686,422,578
        /// 605,423,415
        /// 515,917,-361
        /// -336,658,858
        /// 95,138,22
        /// -476,619,847
        /// -340,-569,-846"
        /// </summary>
        /// <param name="input"></param>
        public Scanner(string input)
        {
            var splittedCoordinates = input.Split("\n").Select(x => x.Trim());
            RelativePosition = new Coordinate3D();
            foreach (var coord in splittedCoordinates)
            {
                Beacons.Add(new Coordinate3D(coord));
            }

        }
        public int ManhattanDistance(Scanner other)
        {
            var x = Math.Abs(RelativePosition.X - other.RelativePosition.X);
            var y = Math.Abs(RelativePosition.Y - other.RelativePosition.Y);
            var z = Math.Abs(RelativePosition.Z - other.RelativePosition.Z);

            return x + y + z;
        }

        private Scanner RotateScanner(string order)
        {
            var res = new Scanner();
            var tokens = order.Split(",");
            foreach (var beacon in Beacons)
            {
                var rotatedCoord = new Coordinate3D();
                foreach (var token in tokens)
                {
                    switch (token)
                    {
                        case "-x":
                            rotatedCoord.X = -1 * beacon.X;
                            break;
                        case "x":
                            rotatedCoord.X = beacon.X;
                            break;
                        case "-y":
                            rotatedCoord.Y = -1 * beacon.Y;
                            break;
                        case "y":
                            rotatedCoord.Y = beacon.Y;
                            break;
                        case "-z":
                            rotatedCoord.Z = -1 * beacon.Z;
                            break;
                        case "z":
                            rotatedCoord.Z = beacon.Z;
                            break;
                        default:
                            break;
                    }
                }
                res.Beacons.Add(rotatedCoord);
                
            }
            res.RelativePosition = new Coordinate3D()
            {
                X = 0,
                Y = 0,
                Z = 0
            };
            return res;
        }

        private Scanner OffsetScanner(Coordinate3D offset)
        {
            var res = new Scanner();
            foreach (var beacon in Beacons)
            {
                res.Beacons.Add(new Coordinate3D
                {
                    X = beacon.X + offset.X,
                    Y = beacon.Y + offset.Y,
                    Z = beacon.Z + offset.Z
                });
            }
            res.RelativePosition = new Coordinate3D
            {
                X = 0,
                Y = 0,
                Z = 0
            };
            return res;
        }
        public Scanner MatchBeacons(Scanner obj)
        {
            var orders = GetAllCoordinatePermutations();

            foreach (var order in orders)
            {
                var rotatedScanner = obj.RotateScanner(order);
                foreach (var beaconPos in Beacons)
                {
                    foreach (var otherBeaconPos in obj.Beacons)
                    {
                        var offset = beaconPos - otherBeaconPos;
                        var temp = rotatedScanner.OffsetScanner(offset);
                        if (MinBeaconsMatch(temp))
                        {
                            temp.RelativePosition = offset;
                            return temp;
                        }
                    }
                }
            }
            return null;
        }

        private bool MinBeaconsMatch(Scanner other)
        {
            var intersection = new List<Coordinate3D>();

            for (int i = 0; i < other.Beacons.Count; i++)
            {
                if (Beacons.Contains(other.Beacons[i]))
                {
                    intersection.Add(other.Beacons[i]);
                }
            }

            if (intersection.Count >= MIN_MATCHES)
            {
                return true;
            }

            return false;
        }

        private static List<string> GetAllCoordinatePermutations()
        {
            var x = new string[] { "x", "-x" };
            var y = new string[] { "y", "-y" };
            var z = new string[] { "z", "-z" };

            List<string> orders = new List<string>();

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        string order = x[i] + "," + y[j] + "," + z[k];
                        orders.Add(order);
                    }
                }

                for (int j = 0; j < 2; j++)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        string order = x[i] + "," + z[j] + "," + y[k];
                        orders.Add(order);
                    }
                }
            }

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        string order = y[i] + "," + x[j] + "," + z[k];
                        orders.Add(order);
                    }
                }

                for (int j = 0; j < 2; j++)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        string order = y[i] + "," + z[j] + "," + x[k];
                        orders.Add(order);
                    }
                }
            }

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        string order = z[i] + "," + x[j] + "," + y[k];
                        orders.Add(order);
                    }
                }

                for (int j = 0; j < 2; j++)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        string order = z[i] + "," + y[j] + "," + x[k];
                        orders.Add(order);
                    }
                }
            }

            //to remove configurations that are not rotations but mirror. Example: x,y,-z is a mirror of x,y,z and not a rotation. Hence it's invalid.
            return orders.Where(s => IsDeterminantOne(s)).ToList();
        }

        private static bool IsDeterminantOne(string order)
        {
            int[][] mat = new int[3][];

            string[] parts = order.Split(",");

            for (int i = 0; i < 3; i++)
            {
                if (parts[i] == "x" || parts[i] == "-x")
                {
                    if (parts[i][0] == '-')
                    {
                        mat[i] = new int[] { -1, 0, 0 };
                    }
                    else
                    {
                        mat[i] = new int[] { 1, 0, 0 };
                    }
                }
                else if (parts[i] == "y" || parts[i] == "-y")
                {
                    if (parts[i][0] == '-')
                    {
                        mat[i] = new int[] { 0, -1, 0 };
                    }
                    else
                    {
                        mat[i] = new int[] { 0, 1, 0 };
                    }
                }
                else
                {
                    if (parts[i][0] == '-')
                    {
                        mat[i] = new int[] { 0, 0, -1 };
                    }
                    else
                    {
                        mat[i] = new int[] { 0, 0, 1 };
                    }
                }
            }

            /* Example: mat for (x,z,y) is
            *  [1,0,0]
            *  [0,0,1]
            *  [0,1,0]
            */

            int determinant = mat[0][0] * (mat[1][1] * mat[2][2] - mat[1][2] * mat[2][1]);
            determinant -= mat[0][1] * (mat[1][0] * mat[2][2] - mat[1][2] * mat[2][0]);
            determinant += mat[0][2] * (mat[1][0] * mat[2][1] - mat[1][1] * mat[2][0]);

            // determinant of rotational matrices are 1 (as cos^2(x)+sin^(x) = 1) (check rotational matrix online). For reflections determinant is -1.
            return determinant == 1;
        }
    }
}
