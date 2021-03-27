using System.Collections.Generic;

namespace Ecosystem.Genes
{
  public sealed class RabbitGenome : AbstractGenome
  {
    internal static Dictionary<GeneType, Preset> preset = new Dictionary<GeneType, Preset>()
    {
      {GeneType.HungerRate, new Preset(0, 10, new[] {1f})},
      {GeneType.HungerThreshold, new Preset(0, 10, new[] {5f})},
      {GeneType.ThirstRate, new Preset(0, 10, new[] {0.5f})},
      {GeneType.ThirstThreshold, new Preset(0, 10, new[] {5f})},
      {GeneType.Vision, new Preset(1, 50, new[] {25f})},
      {GeneType.SpeedFactor, new Preset(1, 2, new[] {1.5f})},
      {GeneType.SizeFactor, new Preset(0.1f, 1, new[] {0.5f})},
      {GeneType.DesirabilityScore, new Preset(1, 10, new[] {1f})},
      {GeneType.GestationPeriod, new Preset(10, 120, new[] {12f})},
      {GeneType.SexualMaturityTime, new Preset(10, 150, new[] {20f})}
    };

    internal static float _mutateChance = 0.05f;


    public static Dictionary<GeneType, Preset> defaultSet = new Dictionary<GeneType, Preset>()
    {
      {GeneType.HungerRate, new Preset(0, 2, new[] {0.5f, 1f})},
      {GeneType.HungerThreshold, new Preset(0, 2, new[] {0.5f, 1f})},
      {GeneType.ThirstRate, new Preset(0, 2, new[] {0.5f, 1f})},
      {GeneType.ThirstThreshold, new Preset(0, 2, new[] {0.5f, 1f})},
      {GeneType.Vision, new Preset(0, 2, new[] {0.5f, 1f})},
      {GeneType.SpeedFactor, new Preset(0, 2, new[] {0.5f, 1f})},
      {GeneType.SizeFactor, new Preset(0, 2, new[] {0.5f, 1f})},
      {GeneType.DesirabilityScore, new Preset(0, 2, new[] {0.5f, 1f})},
      {GeneType.GestationPeriod, new Preset(0, 2, new[] {0.5f, 1f})},
      {GeneType.SexualMaturityTime, new Preset(0, 2, new[] {0.5f, 1f})}
    };

    //TODO: select which should be the default! set that to preset. 
    public static Dictionary<GeneType, Preset> defaultSingular = new Dictionary<GeneType, Preset>()
    {
      {GeneType.HungerRate, new Preset(0, 10, new[] {1f})},
      {GeneType.HungerThreshold, new Preset(0, 10, new[] {5f})},
      {GeneType.ThirstRate, new Preset(0, 10, new[] {0.5f})},
      {GeneType.ThirstThreshold, new Preset(0, 10, new[] {5f})},
      {GeneType.Vision, new Preset(1, 50, new[] {25f})},
      {GeneType.SpeedFactor, new Preset(1, 2, new[] {1.5f})},
      {GeneType.SizeFactor, new Preset(0.1f, 1, new[] {0.5f})},
      {GeneType.DesirabilityScore, new Preset(1, 10, new[] {1f})},
      {GeneType.GestationPeriod, new Preset(10, 120, new[] {12f})},
      {GeneType.SexualMaturityTime, new Preset(10, 150, new[] {20f})}
    };


    public static void SetPreset(Dictionary<GeneType, Preset> presets, float mutateChance = 0.05f)
    {
      preset = presets;
      _mutateChance = mutateChance;
    }

    protected override void Initialize()
    {
      Data = CreateData();
    }

    protected override Dictionary<GeneType, Preset> GetPresets()
    {
      return preset;
    }

    protected override float GetClassMutateChance()
    {
      return _mutateChance;
    }
  }
}