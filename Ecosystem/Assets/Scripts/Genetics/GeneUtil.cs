using System;

namespace Genetics
{
    public static class GeneUtil
    {
        public static double getValidVar(double val, double max, double min)
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

            //return Math.Max(Math.Min(val, max), min);
        }

        private static Random r = new Random();

        public static bool shouldMutate(double percentage)
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
    }
}