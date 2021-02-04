using System;

namespace Genetics
{
    public class SizeGene
    {
        private static double max = 1;
        private static double min = 0.1;
        private double size;

        public SizeGene() : this(0.5)
        {
        }

        public SizeGene(double s)
        {
            size = GeneUtil.getValidVar(s, max, min);
        }

        public double getSize()
        {
            return size;
        }
    }
}