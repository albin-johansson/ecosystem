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

    private static readonly Gene HungerRate = new Gene(0.5f, 0.1f, 1f);
    private static readonly Gene HungerThreshold = new Gene(5, 0, 10);
    private static readonly Gene ThirstRate = new Gene(0.5f, 0.1f, 1f);
    private static readonly Gene ThirstThreshold = new Gene(5, 0, 10);
    private static readonly Gene Vision = new Gene(1f, .5f, 1.5f);
    private static readonly Gene SpeedFactor = new Gene(1f, .5f, 1.5f);
    private static readonly Gene SizeFactor = new Gene(1, 0.5f, 1.5f);
    private static readonly Gene DesirabilityFactor = new Gene(0.5f, 0, 1);
    private static readonly Gene GestationPeriod = new Gene(60, 20, 120);
    private static readonly Gene SexualMaturityTime = new Gene(20, 10, 120);

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
      ConvertGenesToAttributes();
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