using System.Collections.Generic;
using UnityEngine;

public class Genome : MonoBehaviour
{
  public void Awake()
  {
    Initialize();
  }

  protected double MutateChance;
  protected Dictionary<GeneType, Gene> Genes = new Dictionary<GeneType, Gene>();

  /// <summary>
  /// Generate a new (possibly mutated) genome based on the two parents' genome
  /// </summary>
  /// <param name="g1"> First parent </param>
  /// <param name="g2"> Second parent </param>
  public void Initialize(Genome g1, Genome g2)
  {
    var newGenes = new Dictionary<GeneType, Gene>();

    foreach (var gene in g1.Genes)
    {
      Gene g;
      if (GeneUtil.RandomWithChance(g1.MutateChance))
      {
        //mutate
        g = gene.Value.Mutate();
      }
      else
      {
        //pick parent
        g = GeneUtil.RandomWithChance(50) ? new Gene(gene.Value) : new Gene(g2.Genes[gene.Key]);
      }

      newGenes.Add(gene.Key, g);
    }

    Genes = newGenes;
    MutateChance = g1.MutateChance;
  }


  public virtual void Initialize()
  {
    Initialize(0.05);
  }

  //default genome, should only be used in development before an animal has its own default.  
  public virtual void Initialize(double mutateChance)
  {
    MutateChance = mutateChance;
    Genes.Add(GeneType.HungerRate, new Gene(2, 1, 3));
    Genes.Add(GeneType.HungerThreshold, new Gene(10, 0, 50));
    Genes.Add(GeneType.ThirstRate, new Gene(3, 2, 5));
    Genes.Add(GeneType.ThirstThreshold, new Gene(10, 0, 50));
    Genes.Add(GeneType.Vision, new Gene(5, 1, 10));
    Genes.Add(GeneType.SpeedFactor, new Gene(2, 1, 3));
    Genes.Add(GeneType.SizeFactor, new Gene(2, 1, 3));
    Genes.Add(GeneType.DesirabilityScore, new Gene(1, 1, 10));
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
    Debug.Log("TR: " + Genes[GeneType.ThirstRate].Value);
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