using System.Collections.Generic;
using UnityEngine;

public class Genome : MonoBehaviour
{
  private double _mutateChance = 0.05;
  protected Dictionary<GeneType, Gene> Genes;

  private void Awake()
  {
    Genes = new Dictionary<GeneType, Gene>
    {
            [GeneType.HungerRate] = new Gene(2, 1, 3),
            [GeneType.HungerThreshold] = new Gene(10, 0, 50),
            [GeneType.ThirstRate] = new Gene(3, 2, 5),
            [GeneType.ThirstThreshold] = new Gene(10, 0, 50),
            [GeneType.Vision] = new Gene(5, 1, 10),
            [GeneType.SpeedFactor] = new Gene(2, 1, 3),
            [GeneType.SizeFactor] = new Gene(2, 1, 3),
            [GeneType.DesirabilityScore] = new Gene(1, 1, 10)
    };
  }

  /// <summary>
  /// Changes the genome and potentially mutates it based on two parent genomes.
  /// </summary>
  public void Merge(Genome first, Genome second)
  {
    var newGenes = new Dictionary<GeneType, Gene>();

    foreach (var otherGene in first.Genes)
    {
      Gene gene;

      if (GeneUtil.RandomWithChance(first._mutateChance))
      {
        gene = otherGene.Value.Mutate();
      }
      else
      {
        gene = GeneUtil.RandomWithChance(50)
                ? new Gene(otherGene.Value)
                : new Gene(second.Genes[otherGene.Key]);
      }

      newGenes[otherGene.Key] = gene;
    }

    Genes = newGenes;
    _mutateChance = first._mutateChance;
  }

  /// <summary>
  /// Speed depends on the metabolism (HungerRate), the speed (SpeedFactor),
  /// and the size (SizeFactor)
  /// </summary>
  public double GetSpeed()
  {
    return Genes[GeneType.HungerRate].Value
           * Genes[GeneType.SpeedFactor].Value *
           Genes[GeneType.SizeFactor].ValueAsDecimal();
  }

  public double GetVisionRange()
  {
    return Genes[GeneType.Vision].Value;
  }

  /// <summary>
  /// The metabolism depends on the rate it self and the size. 
  /// </summary>
  /// <returns>
  /// The rate at which hunger should increase. 
  /// </returns>
  public double GetHungerRate()
  {
    return Genes[GeneType.HungerRate].Value * Genes[GeneType.SizeFactor].Value;
  }

  public double GetHungerThreshold()
  {
    return Genes[GeneType.HungerThreshold].Value;
  }

  public double GetThirstRate()
  {
    return Genes[GeneType.ThirstRate].Value;
  }

  public double GetThirstThreshold()
  {
    return Genes[GeneType.ThirstThreshold].Value;
  }

  /// <summary>
  /// Attractiveness depends solely on the desirability gene.
  /// Could depend on more in the future. 
  /// </summary>
  /// <returns>
  /// A score for desirability. 
  /// </returns>
  public double GetDesirability()
  {
    return Genes[GeneType.DesirabilityScore].Value;
  }
}