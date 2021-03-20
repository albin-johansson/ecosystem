using System.Collections.Generic;

namespace Ecosystem.Genes
{
  public sealed class RabbitGenome : AbstractGenome
  {
    internal static Dictionary<GeneType, Preset> preset = new Dictionary<GeneType, Preset>()
    {
      {GeneType.HungerRate, new Preset(0, 1, new[] {0.5f})},
      {GeneType.HungerThreshold, new Preset(0, 1, new[] {0.5f})},
      {GeneType.ThirstRate, new Preset(0, 1, new[] {0.5f})},
      {GeneType.ThirstThreshold, new Preset(0, 1, new[] {0.5f})},
      {GeneType.Vision, new Preset(0, 1, new[] {0.5f})},
      {GeneType.SpeedFactor, new Preset(0, 1, new[] {0.5f})},
      {GeneType.SizeFactor, new Preset(0, 1, new[] {0.5f})},
      {GeneType.DesirabilityScore, new Preset(0, 1, new[] {0.5f})},
      {GeneType.GestationPeriod, new Preset(0, 1, new[] {0.5f})},
      {GeneType.SexualMaturityTime, new Preset(0, 1, new[] {0.5f})}
    };

    internal static float chancee = 0.05f;

    public static void SetPreset(Dictionary<GeneType, Preset> presets, float mutateChance = 0.05f)
    {
      preset = presets;
      chancee = mutateChance;
    }

    protected override void Initialize()
    {
      Data = GenomeData.Create(CreateGenes(preset), chancee);
      //Data = GenomeData.Create(DefaultGenes);
    }
  }
}