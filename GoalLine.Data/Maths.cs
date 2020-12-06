using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalLine.Data
{
    public class Maths
    {
        private Random rng;
        private double? gRandom2 = null; // Set & reset by GaussianRandom()

        public Maths()
        {
            rng = new Random();
        }

        /// <summary>
        /// Helper due to .Net's own RNG routine returning one LESS than the maximum value
        /// </summary>
        /// <param name="From">Lowest possible value</param>
        /// <param name="To">Highest possible value</param>
        /// <returns></returns>
        public int RandomInclusive(int From, int To)
        {
            return rng.Next(From, To + 1);
        }

        /// <summary>
        /// Generate a Gauss-distributed (pseudo)random number. 
        /// Concept borrowed from https://github.com/kashifsoofi/bygfoot/blob/master/src/maths.c
        /// </summary>
        /// <returns></returns>
        public double GaussianRandom()
        {
            double retVal;

            if(gRandom2 == null)
            {
                double V1;
                double V2;
                double S;

                do
                {
                    double U1 = rng.NextDouble();
                    double U2 = rng.NextDouble();
                    

                    V1 = 2 * U1 - 1;
                    V2 = 2 * U2 - 1;
                    S = V1 * V1 + V2 * V2;
                } while (S >= 1 || S == 0);

                retVal = V1 * Math.Sqrt(-2 * Math.Log(S) / S);
                gRandom2 = V2 * Math.Sqrt(-2 * Math.Log(S) / S);
            }
            else
            {
                retVal = (double)gRandom2;
                gRandom2 = null;
            }

            return retVal;
        }

        /// <summary>
        /// Returns a Generate a Gauss-distributed random number within given boundaries.
        /// Expectation value of the distribution is (upper + lower) / 2,
        /// the variance is so that the number is between the boundaries with probability
        /// 99,7 %. If the number isn't between the boundaries, we cut off.
        /// Concept borrowed from https://github.com/kashifsoofi/bygfoot/blob/master/src/maths.c
        /// </summary>
        /// <param name="Lower">Lower boundary</param>
        /// <param name="Upper">Upper boundary</param>
        /// <returns>Random number</returns>
        public double GaussianDistributedRandom(double Lower, double Upper)
        {
            double retVal = (Upper - Lower) / 6 * GaussianRandom() + (Upper + Lower) / 2;

            if (retVal < Lower)
                retVal = Lower;

            if (retVal > Upper)
                retVal = Upper;

            return retVal;
        }

        public int GaussianDistributedRandom_Int(double Lower, double Upper)
        {
            return Convert.ToInt32(GaussianDistributedRandom(Lower, Upper));
        }

        /// <summary>
        /// Returns an age based on the given DOB, and the current game date.
        /// </summary>
        /// <param name="dob">Date of Birth</param>
        /// <returns>Age</returns>
        public int CalculateAgeInGame(DateTime dob)
        {
            WorldAdapter wa = new WorldAdapter();

            DateTime today = wa.CurrentDate;
            int age = today.Year - dob.Year;
            if(dob.Date > today.AddYears(0 - age))
            {
                age--;
            }

            return age;
        }
    }
}
