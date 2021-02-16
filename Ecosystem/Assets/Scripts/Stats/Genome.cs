using System.Collections.Generic;
using UnityEngine;


public class Genome : MonoBehaviour
{
  protected double mutateChance;
  protected Dictionary<GeneType, Gene> genes = new Dictionary<GeneType, Gene>();

  /// <summary>
  /// Generate a new (possibly mutated) genome based on the two parents' genome
  /// </summary>
  /// <param name="g1"></param>
  /// <param name="g2"></param>
  public Genome(Genome g1, Genome g2)
  {
    var newGenes = new Dictionary<GeneType, Gene>();

    foreach (var gene in g1.genes)
    {
      Gene g;
      if (GeneUtil.RandomWithChance(g1.mutateChance))
      {
        //mutate
        g = gene.Value.MutatedGene();
      }
      else
      {
        //pick parent
        g = GeneUtil.RandomWithChance(50) ? new Gene(gene.Value) : new Gene(g2.genes[gene.Key]);
      }

      newGenes.Add(gene.Key, g);
    }

    genes = newGenes;
    mutateChance = g1.mutateChance;
  }

  //default genome, should only be used in development before an animal has its own default.  
  public Genome()
  {
    genes.Add(GeneType.HungerRate, new Gene(2, 1, 3));
    genes.Add(GeneType.HungerThreshold, new Gene(10, 0, 50));
    genes.Add(GeneType.ThirstRate, new Gene(3, 2, 5));
    genes.Add(GeneType.ThirstThreshold, new Gene(10, 0, 50));
    genes.Add(GeneType.Vision, new Gene(5, 1, 10));
    genes.Add(GeneType.SpeedFactor, new Gene(2, 1, 3));
    genes.Add(GeneType.SizeFactor, new Gene(2, 1, 3));
    genes.Add(GeneType.DesirabilityScore, new Gene(1, 1, 10));
  }


  /// <summary>
  /// Speed depends on the metabolism (HungerRate), the speed (SpeedFactor),
  /// and the size (SizeFactor)
  /// </summary>
  /// <returns></returns>
  public double GetSpeed()
  {
    return genes[GeneType.HungerRate].Value
           * genes[GeneType.SpeedFactor].Value *
           genes[GeneType.SizeFactor].ValueAsDecimal();
  }

  public double GetVisionRange()
  {
    return genes[GeneType.Vision].Value;
  }

  /// <summary>
  /// The metabolism depends on the rate it self and the size. 
  /// </summary>
  /// <returns></returns>
  public double GetHungerRate()
  {
    return genes[GeneType.HungerRate].Value * genes[GeneType.SizeFactor].Value;
  }

  public double GetHungerThreshold()
  {
    return genes[GeneType.HungerThreshold].Value;
  }

  public double GetThirstRate()
  {
    return genes[GeneType.ThirstRate].Value;
  }

  public double GetThirstThreshold()
  {
    return genes[GeneType.ThirstThreshold].Value;
  }

  /// <summary>
  /// Attractiveness depends solely on the desirability gene.
  /// Could depend on more in the future. 
  /// </summary>
  /// <returns></returns>
  public double GetDesirability()
  {
    return genes[GeneType.DesirabilityScore].Value;
  }
}