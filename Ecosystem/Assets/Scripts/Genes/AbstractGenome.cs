using System.Collections.Generic;
using UnityEngine;

namespace Ecosystem.Genes
{
  public abstract class AbstractGenome : MonoBehaviour, IGenome
  {
    internal GenomeData Data;

    internal Dictionary<GeneType, Preset> _preset = new Dictionary<GeneType, Preset>()
    {
      {GeneType.HungerRate, new Preset(0, 1, new[] {0f, 1f})},
      {GeneType.HungerThreshold, new Preset(0, 1, new[] {0f, 1f})},
      {GeneType.ThirstRate, new Preset(0, 1, new[] {0f, 1f})},
      {GeneType.ThirstThreshold, new Preset(0, 1, new[] {0f, 1f})},
      {GeneType.Vision, new Preset(0, 1, new[] {0f, 1f})},
      {GeneType.SpeedFactor, new Preset(0, 1, new[] {0f, 1f})},
      {GeneType.SizeFactor, new Preset(0, 1, new[] {0f, 1f})},
      {GeneType.DesirabilityScore, new Preset(0, 1, new[] {0f, 1f})},
      {GeneType.GestationPeriod, new Preset(0, 1, new[] {0f, 1f})},
      {GeneType.SexualMaturityTime, new Preset(0, 1, new[] {0f, 1f})}
    };

    private void Awake()
    {
      Initialize();
    }

    protected Dictionary<GeneType, Gene> CreateGenes()
    {
      Gene HungerRate = GeneUtil.NewGeneFromList(_preset[GeneType.HungerRate].min,
        _preset[GeneType.HungerRate].max, _preset[GeneType.HungerRate].values);
      Gene HungerThreshold = GeneUtil.NewGeneFromList(_preset[GeneType.HungerThreshold].min,
        _preset[GeneType.HungerThreshold].max, _preset[GeneType.HungerThreshold].values);
      Gene ThirstRate = GeneUtil.NewGeneFromList(_preset[GeneType.ThirstRate].min,
        _preset[GeneType.ThirstRate].max, _preset[GeneType.ThirstRate].values);
      Gene ThirstThreshold = GeneUtil.NewGeneFromList(_preset[GeneType.ThirstThreshold].min,
        _preset[GeneType.ThirstThreshold].max, _preset[GeneType.ThirstThreshold].values);
      Gene Vision = GeneUtil.NewGeneFromList(_preset[GeneType.Vision].min, _preset[GeneType.Vision].max,
        _preset[GeneType.Vision].values);
      Gene SpeedFactor = GeneUtil.NewGeneFromList(_preset[GeneType.SpeedFactor].min,
        _preset[GeneType.SpeedFactor].max, _preset[GeneType.SpeedFactor].values);
      Gene SizeFactor = GeneUtil.NewGeneFromList(_preset[GeneType.SizeFactor].min,
        _preset[GeneType.SizeFactor].max, _preset[GeneType.SizeFactor].values);
      Gene DesirabilityFactor = GeneUtil.NewGeneFromList(_preset[GeneType.DesirabilityScore].min,
        _preset[GeneType.DesirabilityScore].max, _preset[GeneType.DesirabilityScore].values);
      Gene GestationPeriod = GeneUtil.NewGeneFromList(_preset[GeneType.GestationPeriod].min,
        _preset[GeneType.GestationPeriod].max, _preset[GeneType.GestationPeriod].values);
      Gene SexualMaturityTime = GeneUtil.NewGeneFromList(_preset[GeneType.SexualMaturityTime].min,
        _preset[GeneType.SexualMaturityTime].max, _preset[GeneType.SexualMaturityTime].values);
      Dictionary<GeneType, Gene> genes = new Dictionary<GeneType, Gene>
      {
        {
          GeneType.HungerRate, HungerRate
        },
        {
          GeneType.HungerThreshold, HungerThreshold
        },
        {
          GeneType.ThirstRate, ThirstRate
        },
        {
          GeneType.ThirstThreshold, ThirstThreshold
        },
        {
          GeneType.Vision, Vision
        },
        {
          GeneType.SpeedFactor, SpeedFactor
        },
        {
          GeneType.SizeFactor, SizeFactor
        },
        {
          GeneType.DesirabilityScore, DesirabilityFactor
        },
        {
          GeneType.GestationPeriod, GestationPeriod
        },
        {
          GeneType.SexualMaturityTime, SexualMaturityTime
        }
      };
      return genes;
    }

    protected abstract void Initialize();

    public void Initialize(IGenome first, IGenome second)
    {
      Data = Genomes.Merge(first, second);
    }

    public bool IsMale => Data.IsMale;

    public float Speed => GetHungerRate().Value *
                          GetSpeedFactor().Value *
                          GetSizeFactor().ValueAsDecimal();

    public float Metabolism => GetHungerRate().Value * GetSizeFactor().Value;

    public float Attractiveness => GetDesirabilityScore().Value;

    public Gene GetHungerRate() => Data.HungerRate;

    public Gene GetHungerThreshold() => Data.HungerThreshold;

    public Gene GetThirstRate() => Data.ThirstRate;

    public Gene GetThirstThreshold() => Data.ThirstThreshold;

    public Gene GetVision() => Data.Vision;

    public Gene GetSpeedFactor() => Data.SpeedFactor;

    public Gene GetSizeFactor() => Data.SizeFactor;

    public Gene GetDesirabilityScore() => Data.DesirabilityScore;

    public Gene GetGestationPeriod() => Data.GestationPeriod;

    public Gene GetSexualMaturityTime() => Data.SexualMaturityTime;

    public GenomeData GetGenes() => Data;
  }
}