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
