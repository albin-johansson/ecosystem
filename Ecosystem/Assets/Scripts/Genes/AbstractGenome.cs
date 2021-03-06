using System.Collections.Generic;
using UnityEngine;

namespace Ecosystem.Genes
{
  public abstract class AbstractGenome : MonoBehaviour, IGenome
  {
    internal GenomePack Pack;

    private void Awake()
    {
      Initialize();
    }

    protected abstract void Initialize();

    public abstract Dictionary<GeneType, Gene> GetInitialGenes();

    public void Initialize(IGenome first, IGenome second)
    {
      Pack = Genomes.Merge(first.GetGenes(), second.GetGenes());
    }

    public bool IsMale => Pack.IsMale;

    public float Speed => GetHungerRate().Value *
                          GetSpeedFactor().Value *
                          GetSizeFactor().ValueAsDecimal();

    public float Metabolism => GetHungerRate().Value * GetSizeFactor().Value;

    public float Attractiveness => GetDesirabilityScore().Value;

    public Gene GetHungerRate() => Pack.HungerRate;

    public Gene GetHungerThreshold() => Pack.HungerThreshold;

    public Gene GetThirstRate() => Pack.ThirstRate;

    public Gene GetThirstThreshold() => Pack.ThirstThreshold;

    public Gene GetVision() => Pack.Vision;

    public Gene GetSpeedFactor() => Pack.SpeedFactor;

    public Gene GetSizeFactor() => Pack.SizeFactor;

    public Gene GetDesirabilityScore() => Pack.DesirabilityFactor;

    public Gene GetGestationPeriod() => Pack.GestationPeriod;

    public Gene GetSexualMaturityTime() => Pack.SexualMaturityTime;

    public GenomePack GetGenes() => Pack;
  }
}