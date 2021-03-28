using System.Collections.Generic;

namespace Ecosystem.Genes
{
  public sealed class BearGenome : AbstractGenome
  {
    private static float _mutateChance = 0.05f;
    private static Dictionary<GeneType, Preset> _preset;

    public static readonly Dictionary<GeneType, Preset> DefaultSet = new Dictionary<GeneType, Preset>()
    {
      {GeneType.HungerRate, new Preset(0.1f, 1, new[] {0.1f, 0.5f, 0.9f})},
      {GeneType.HungerThreshold, new Preset(0, 10f, new[] {1f, 5f, 9f})},
      {GeneType.ThirstRate, new Preset(0.1f, 1, new[] {0.1f, 0.5f, 0.9f})},
      {GeneType.ThirstThreshold, new Preset(0, 10, new[] {1f, 5f, 7f})},
      {GeneType.Vision, new Preset(1, 50, new[] {5f, 10f, 25f, 45f, 50f})},
      {GeneType.SpeedFactor, new Preset(1, 2, new[] {1f, 1.5f, 1.9f})},
      {GeneType.SizeFactor, new Preset(0.5f, 1.5f, new[] {0.5f, 1f, 1.5f})},
      {GeneType.DesirabilityScore, new Preset(1, 10, new[] {1f, 5f, 9f})},
      {GeneType.GestationPeriod, new Preset(20, 120, new[] {20f, 60f, 90f, 120f})},
      {GeneType.SexualMaturityTime, new Preset(10, 150, new[] {20f, 40f, 60f, 90f, 120f, 150f})}
    };

    public static readonly Dictionary<GeneType, Preset> DefaultSingular = new Dictionary<GeneType, Preset>()
    {
      {GeneType.HungerRate, new Preset(0, 1, new[] {0.5f})},
      {GeneType.HungerThreshold, new Preset(0, 1f, new[] {0.5f})},
      {GeneType.ThirstRate, new Preset(0, 1, new[] {0.5f})},
      {GeneType.ThirstThreshold, new Preset(0, 10, new[] {5f})},
      {GeneType.Vision, new Preset(1, 50, new[] {30f})},
      {GeneType.SpeedFactor, new Preset(1, 2, new[] {1.5f})},
      {GeneType.SizeFactor, new Preset(0.5f, 1.5f, new[] {1f})},
      {GeneType.DesirabilityScore, new Preset(1, 10, new[] {5f})},
      {GeneType.GestationPeriod, new Preset(10, 120, new[] {40f})},
      {GeneType.SexualMaturityTime, new Preset(10, 150, new[] {40f})}
    };

    static BearGenome()
    {
      _preset = DefaultSet;
    }


    protected override void Initialize()
    {
      Data = CreateData();
      ConvertGenesToAttributes();
    }

    public static void SetPreset(Dictionary<GeneType, Preset> presets, float mutateChance = 0.05f)
    {
      _preset = presets;
      _mutateChance = mutateChance;
    }


    protected override Dictionary<GeneType, Preset> GetPresets()
    {
      return _preset;
    }

    protected override float GetClassMutateChance()
    {
      return _mutateChance;
    }
  }
}