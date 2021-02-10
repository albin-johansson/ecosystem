using System.Collections.Generic;
using UnityEngine;


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
        g = new Gene(g1.genes.GetGene(i).Max, g1.genes.GetGene(i).Min);
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

    genes = new GeneList(list);
    mutateChance = g1.mutateChance;
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
    double speed = genes.GetGene(GeneList.HungerRate).Value * genes.GetGene(GeneList.SpeedFactor).Value *
                   ((genes.GetGene(GeneList.SizeFactor).Max - genes.GetGene(GeneList.SizeFactor).Min) -
                    genes.GetGene(GeneList.SizeFactor).Value);
    return speed;
  }

  public double GetVisionRange()
  {
    return genes.GetGene(GeneList.Vision).Value;
  }

  /// <summary>
  /// The metabolism depends on the rate it self and the size. 
  /// </summary>
  /// <returns></returns>
  public double GetHungerRate()
  {
    return genes.GetGene(GeneList.HungerRate).Value * genes.GetGene(GeneList.SizeFactor).Value;
  }

  public double GetHungerThreshold()
  {
    return genes.GetGene(GeneList.HungerThreshold).Value;
  }

  public double GetThirstRate()
  {
    return genes.GetGene(GeneList.ThirstRate).Value;
  }

  public double GetThirstThreshold()
  {
    return genes.GetGene(GeneList.ThirstThreshold).Value;
  }

  /// <summary>
  /// Attractiveness depends solely on the desirability gene.
  /// Could depend on more in the future. 
  /// </summary>
  /// <returns></returns>
  public double GetDesirability()
  {
    return genes.GetGene(GeneList.DesireFactor).Value;
  }
}