using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace Genetics
{
    public class GeneList
    {
        //list of genes. 
        public const string HungerRate = "hungerRate";
        public const string HungerThreshold = "hungerThreshold";
        public const string ThirstRate = "thirstRate";
        public const string ThirstThreshold = "thirstThreshold";
        public const string Vision = "visionRange";
        public const string SpeedFactor = "speedFactor";
        public const string SizeFactor = "sizeFactor";
        public const string DesireFactor = "desireFactor";

        private List<Gene> genes;
        private static List<string> index = new List<string>();

        static GeneList()
        {
            index.Add(HungerRate);
            index.Add(HungerThreshold);
            index.Add(ThirstRate);
            index.Add(ThirstThreshold);
            index.Add(Vision);
            index.Add(SpeedFactor);
            index.Add(SizeFactor);
            index.Add(DesireFactor);
        }

        private int StringToIndex(string s)
        {
            return index.IndexOf(s);
        }

        public int GetSize()
        {
            return genes.Count;
        }

        public Gene GetGene(string s)
        {
            return genes[StringToIndex(s)];
        }

        public Gene GetGene(int i)
        {
            return genes[i];
        }

        public GeneList(Gene hungerRateGene, Gene hungerThresholdGene, Gene thirstRateGene,
            Gene thirstThresholdGene, Gene visionGene, Gene speedGene, Gene sizeGene, Gene desireGene)
        {
            genes = new List<Gene>();
            genes.Add(hungerRateGene);
            genes.Add(hungerThresholdGene);
            genes.Add(thirstRateGene);
            genes.Add(thirstThresholdGene);
            genes.Add(visionGene);
            genes.Add(speedGene);
            genes.Add(sizeGene);
            genes.Add(desireGene);
        }

        /**
         * Warning, must be ordered. 
         */
        public GeneList(List<Gene> genes)
        {
            this.genes = genes;
        }
    }
}