using System;

namespace Genetics
{
    public static class GeneUtil
    {
        public static double GetValidVar(double val, double max, double min)
        {
            if (val < min)
            {
                return min;
            }
            else if (val > max)
            {
                return max;
            }
            else
            {
                return val;
            }
        }

        private static Random r = new Random();

        public static bool RandomWithChance(double percentage)
        {
            double roll = r.NextDouble() * 100;
            if (roll < percentage)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public static double MutatedInRange(double max, double min)
        {
            return r.NextDouble() * (max - min) + min;
        }
    }
}