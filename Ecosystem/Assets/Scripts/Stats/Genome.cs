using System.Collections.Generic;
using UnityEngine;

namespace Ecosystem.Stats
{
  public class Genome : MonoBehaviour
  {
    private static readonly Gene HungerRate = new Gene(1, 0.5, 10);
    private static readonly Gene HungerThreshold = new Gene(5, 0, 10);
    private static readonly Gene ThirstRate = new Gene(1, 0.5, 10);
    private static readonly Gene ThirstThreshold = new Gene(5, 0, 10);
    private static readonly Gene Vision = new Gene(25, 1, 50);
    private static readonly Gene SpeedFactor = new Gene(1.5, 1, 2);
    private static readonly Gene SizeFactor = new Gene(0.5, 0.1, 1);
    private static readonly Gene DesirabilityFactor = new Gene(1, 1, 10);

    protected double MutateChance;
    protected Dictionary<GeneType, Gene> Genes = new Dictionary<GeneType, Gene>();

    public void Awake()
    {
      Initialize();
    }

    /// <summary>
    /// Generate a new (possibly mutated) genome based on the two parents' genome
    /// </summary>
    /// <param name="first"> First parent </param>
    /// <param name="second"> Second parent </param>
    public void Initialize(Genome first, Genome second)
    {
      var newGenes = new Dictionary<GeneType, Gene>();

      foreach (var otherGene in first.Genes)
      {
        Gene gene;
        if (GeneUtil.RandomWithChance(first.MutateChance))
        {
          //mutate
          gene = otherGene.Value.Mutate();
        }
        else
        {
          //pick parent
          gene = GeneUtil.RandomWithChance(50) ? new Gene(otherGene.Value) : new Gene(second.Genes[otherGene.Key]);
        }

        newGenes[otherGene.Key] = gene;
      }

      Genes = newGenes;
      MutateChance = first.MutateChance;
    }

    protected virtual void Initialize()
    {
      Initialize(0.05);
    }

    protected virtual void Initialize(double mutateChance)
    {
      MutateChance = mutateChance;
      Genes[GeneType.HungerRate] = HungerRate;
      Genes[GeneType.HungerThreshold] = HungerThreshold;
      Genes[GeneType.ThirstRate] = ThirstRate;
      Genes[GeneType.ThirstThreshold] = ThirstThreshold;
      Genes[GeneType.Vision] = Vision;
      Genes[GeneType.SpeedFactor] = SpeedFactor;
      Genes[GeneType.SizeFactor] = SizeFactor;
      Genes[GeneType.DesirabilityScore] = DesirabilityFactor;
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
}