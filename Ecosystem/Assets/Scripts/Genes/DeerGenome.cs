using System.Collections.Generic;

namespace Ecosystem.Genes
{
  public sealed class DeerGenome : AbstractGenome
  {
    private static float _mutateChance = 0.05f;
    private static Dictionary<GeneType, Preset> _preset;

    public static readonly Dictionary<GeneType, Preset> DefaultSet = new Dictionary<GeneType, Preset>()
    {
      {GeneType.HungerRate, new Preset(0.2f, 0.5f, new[] {0.3f})},
      {GeneType.HungerThreshold, new Preset(30, 40, new[] {30f, 35f, 40f})},
      {GeneType.ThirstRate, new Preset(0.3f, 0.5f, new[] {0.3f})},
      {GeneType.ThirstThreshold, new Preset(25, 40, new[] {25f, 35f, 40f})},
      {GeneType.Vision, new Preset(5, 15, new[] {7f, 9f, 11f, 13f})},
      {GeneType.Speed, new Preset(1, 4, new[] {2f, 3f, 4f})},
      {GeneType.GestationPeriod, new Preset(10, 120, new[] {12f, 20f, 60f, 90f, 120f})},
      {GeneType.SexualMaturityTime, new Preset(10, 50, new[] {10f, 20f, 30f, 40f, 50f})}
    };

    public static readonly Dictionary<GeneType, Preset> DefaultSingular = new Dictionary<GeneType, Preset>()
    {
      {GeneType.HungerRate, new Preset(0.2f, 0.5f, new[] {0.3f})},
      {GeneType.HungerThreshold, new Preset(30, 40, new[] {35f})},
      {GeneType.ThirstRate, new Preset(0.3f, 0.5f, new[] {0.4f})},
      {GeneType.ThirstThreshold, new Preset(25, 40, new[] {30f})},
      {GeneType.Vision, new Preset(10, 15, new[] {12.5f})},
      {GeneType.Speed, new Preset(2, 4, new[] {3f})},
      {GeneType.GestationPeriod, new Preset(10, 120, new[] {40f})},
      {GeneType.SexualMaturityTime, new Preset(10, 150, new[] {40f})}
    };

    static DeerGenome()
    {
      _preset = DefaultSet;
    }


    protected override void Initialize()
    {
      key = GenerateKey(10);
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