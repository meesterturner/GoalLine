using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalLine.Data
{
    class Utils
    {
        private Random rng;

        public Utils()
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
    }
}
