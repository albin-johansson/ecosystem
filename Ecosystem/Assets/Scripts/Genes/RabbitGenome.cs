using System.Collections.Generic;

namespace Ecosystem.Genes
{
  public sealed class RabbitGenome : AbstractGenome
  {
    private static GenomePreset _preset;

    static RabbitGenome()
    {
      _preset.tmp = new Dictionary<GeneType, Preset>()
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
    }

    public static Dictionary<GeneType, Gene> DefaultGenes = GenerateNewDictionary();

    private static Dictionary<GeneType, Gene> GenerateNewDictionary()
    {
      Gene HungerRate = GeneUtil.NewGeneFromList(_preset.tmp[GeneType.HungerRate].min,
        _preset.tmp[GeneType.HungerRate].max, _preset.tmp[GeneType.HungerRate].values);
      Gene HungerThreshold = GeneUtil.NewGeneFromList(_preset.tmp[GeneType.HungerThreshold].min,
        _preset.tmp[GeneType.HungerThreshold].max, _preset.tmp[GeneType.HungerThreshold].values);
      Gene ThirstRate = GeneUtil.NewGeneFromList(_preset.tmp[GeneType.ThirstRate].min,
        _preset.tmp[GeneType.ThirstRate].max, _preset.tmp[GeneType.ThirstRate].values);
      Gene ThirstThreshold = GeneUtil.NewGeneFromList(_preset.tmp[GeneType.ThirstThreshold].min,
        _preset.tmp[GeneType.ThirstThreshold].max, _preset.tmp[GeneType.ThirstThreshold].values);
      Gene Vision = GeneUtil.NewGeneFromList(_preset.tmp[GeneType.Vision].min, _preset.tmp[GeneType.Vision].max,
        _preset.tmp[GeneType.Vision].values);
      Gene SpeedFactor = GeneUtil.NewGeneFromList(_preset.tmp[GeneType.SpeedFactor].min,
        _preset.tmp[GeneType.SpeedFactor].max, _preset.tmp[GeneType.SpeedFactor].values);
      Gene SizeFactor = GeneUtil.NewGeneFromList(_preset.tmp[GeneType.SizeFactor].min,
        _preset.tmp[GeneType.SizeFactor].max, _preset.tmp[GeneType.SizeFactor].values);
      Gene DesirabilityFactor = GeneUtil.NewGeneFromList(_preset.tmp[GeneType.DesirabilityScore].min,
        _preset.tmp[GeneType.DesirabilityScore].max, _preset.tmp[GeneType.DesirabilityScore].values);
      Gene GestationPeriod = GeneUtil.NewGeneFromList(_preset.tmp[GeneType.GestationPeriod].min,
        _preset.tmp[GeneType.GestationPeriod].max, _preset.tmp[GeneType.GestationPeriod].values);
      Gene SexualMaturityTime = GeneUtil.NewGeneFromList(_preset.tmp[GeneType.SexualMaturityTime].min,
        _preset.tmp[GeneType.SexualMaturityTime].max, _preset.tmp[GeneType.SexualMaturityTime].values);
      return DefaultGenes = new Dictionary<GeneType, Gene>
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
        },
      };
    }

    protected override void Initialize()
    {
      Data = GenomeData.Create(DefaultGenes);
    }
  }
}