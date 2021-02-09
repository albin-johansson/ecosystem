﻿namespace Stats
{
  public class RabbitGenome : Genome
  {
    private static double defaultMutateChance;
    private static GeneList defaultGenelist;

    /// <summary>
    /// Default values, constant ranges. 
    /// </summary>
    static RabbitGenome()
    {
      defaultMutateChance = 0.05;
      Gene hungerRateGene = new Gene(1, 10, 0.5);
      Gene hungerThresholdGene = new Gene(5, 10, 0);
      Gene thirstRateGene = new Gene(1, 10, 0.5);
      Gene thirstThresholdGene = new Gene(5, 10, 0);
      Gene visionGene = new Gene(25, 50, 1);
      Gene speedGene = new Gene(1.5, 2, 1);
      Gene sizeGene = new Gene(0.5, 1, 0.1);
      Gene desireGene = new Gene(1, 10, 1);
      defaultGenelist = new GeneList(hungerRateGene, hungerThresholdGene,
        thirstRateGene, thirstThresholdGene, visionGene, speedGene, sizeGene,
        desireGene);
    }

    public RabbitGenome() : base(defaultGenelist.Copy(), defaultMutateChance)
    {
    }
  }
}