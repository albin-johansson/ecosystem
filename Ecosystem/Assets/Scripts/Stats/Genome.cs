using System.Collections.Generic;
using UnityEngine;

namespace Stats
{
  public class Genome : MonoBehaviour
  {
    protected double mutateChance;
    protected GeneList genes;

    /// <summary>
    /// Generate a new (possibly mutated) genome based on the two parents' genome
    /// </summary>
    /// <param name="g1"></param>
    /// <param name="g2"></param>
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

      this.genes = new GeneList(list);
      this.mutateChance = g1.mutateChance;
    }

    public Genome()
    {
    }

    public Genome(GeneList geneList, double percentage)
    {
      genes = geneList;
      mutateChance = percentage;
    }

    /// <summary>
    /// Speed depends on the metabolism (HungerRate), the speed (SpeedFactor),
    /// and the size (SizeFactor)
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// The metabolism depends on the rate it self and the size. 
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Attractiveness depends solely on the desirability gene.
    /// Could depend on more in the future. 
    /// </summary>
    /// <returns></returns>
    public double GetDesirability()
    {
      return genes.GetGene(GeneList.DesireFactor).GetValue();
    }
  }
}