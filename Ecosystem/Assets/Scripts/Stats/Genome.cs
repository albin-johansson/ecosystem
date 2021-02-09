using System.Collections.Generic;

namespace Stats
{
  public class Genome
  {
    protected double mutateChance;

    protected GeneList genes;

    public Genome(Genome g1, Genome g2)
    {
      List<Gene> list = new List<Gene>();
      for (int i = 0; i < g1.genes.GetSize(); i++)
      {
        Gene g;
        if (GeneUtil.RandomWithChance(g1.mutateChance))
        {
          //mutate
          g = new Gene(g1.genes.GetGene(i).GetMax(), g1.genes.GetGene(i).GetMin());
        }
        else
        {
          //pick parent
          if (GeneUtil.RandomWithChance(50))
          {
            g = g1.genes.GetGene(i);
          }
          else
          {
            g = g2.genes.GetGene(i);
          }
        }

        list.Add(g);
      }
    }

    public Genome()
    {
    }

    public Genome(GeneList geneList, double percentage)
    {
      genes = geneList;
      mutateChance = percentage;
    }

    public double GetSpeed()
    {
      double speed = genes.GetGene(GeneList.HungerRate).GetValue() * genes.GetGene(GeneList.SpeedFactor).GetValue() *
                     ((genes.GetGene(GeneList.SizeFactor).GetMax() - genes.GetGene(GeneList.SizeFactor).GetMin()) -
                      genes.GetGene(GeneList.SizeFactor).GetValue());
      return speed;
    }

    public double GetVisionRange()
    {
      return genes.GetGene(GeneList.Vision).GetValue();
    }

    public double GetHungerRate()
    {
      return genes.GetGene(GeneList.HungerRate).GetValue() * genes.GetGene(GeneList.SizeFactor).GetValue();
    }

    public double GetHungerThreshold()
    {
      return genes.GetGene(GeneList.HungerThreshold).GetValue();
    }

    public double GetThirstRate()
    {
      return genes.GetGene(GeneList.ThirstRate).GetValue();
    }

    public double GetThirstThreshold()
    {
      return genes.GetGene(GeneList.ThirstThreshold).GetValue();
    }

    public double GetDesirability()
    {
      return genes.GetGene(GeneList.DesireFactor).GetValue();
    }
  }
}