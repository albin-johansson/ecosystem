using System.Collections.Generic;

namespace Ecosystem.Genes
{
  public sealed class BearGenome : AbstractGenome
  {
    private static readonly Gene HungerRate = new Gene(1.2f, 0.5f, 10);
    private static readonly Gene HungerThreshold = new Gene(4, 0, 10);
    private static readonly Gene ThirstRate = new Gene(1.5f, 0.5f, 10);
    private static readonly Gene ThirstThreshold = new Gene(3, 0, 10);
    private static readonly Gene Vision = new Gene(22, 1, 50);
    private static readonly Gene SpeedFactor = new Gene(15, 1, 25);
    private static readonly Gene SizeFactor = new Gene(0.5f, 0.1f, 1);
    private static readonly Gene DesirabilityFactor = new Gene(1, 1, 10);
    private static readonly Gene GestationPeriod = new Gene(14, 10, 120);
    private static readonly Gene SexualMaturityTime = new Gene(15, 10, 150);

    public static readonly Dictionary<GeneType, Gene> DefaultGenes = new Dictionary<GeneType, Gene>
    {
      {GeneType.HungerRate, HungerRate},
      {GeneType.HungerThreshold, HungerThreshold},
      {GeneType.ThirstRate, ThirstRate},
      {GeneType.ThirstThreshold, ThirstThreshold},
      {GeneType.Vision, Vision},
      {GeneType.SpeedFactor, SpeedFactor},
      {GeneType.SizeFactor, SizeFactor},
      {GeneType.DesirabilityScore, DesirabilityFactor},
      {GeneType.GestationPeriod, GestationPeriod},
      {GeneType.SexualMaturityTime, SexualMaturityTime},
    };


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

    public static void SetPreset(Dictionary<GeneType, Preset> presets, float mutateChance = 0.05f)
    {
      preset = presets;
      _mutateChance = mutateChance;
    }

    protected override void Initialize()
    {
      //Data = CreateData();
      Data = GenomeData.Create(DefaultGenes);
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