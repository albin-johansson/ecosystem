﻿using System.Collections.Generic;

namespace Ecosystem.Genes
{
  public sealed class RabbitGenome : AbstractGenome
  {
    public static List<RabbitGenome> InitalGenomes = new List<RabbitGenome>();

    private static float _mutateChance = 0.05f;
    private static Dictionary<GeneType, Preset> _preset;

    public static readonly Dictionary<GeneType, Preset> DefaultSet = new Dictionary<GeneType, Preset>()
    {
      {GeneType.HungerRate, new Preset(0, 10, new[] {1f, 5f, 7f})},
      {GeneType.HungerThreshold, new Preset(0, 10, new[] {1f, 5f, 7f})},
      {GeneType.ThirstRate, new Preset(0, 10, new[] {0.5f, 3f, 6f, 9f})},
      {GeneType.ThirstThreshold, new Preset(0, 10, new[] {1f, 5f, 7f})},
      {GeneType.Vision, new Preset(1, 50, new[] {5f, 10f, 25f, 40f, 45f})},
      {GeneType.Speed, new Preset(1, 2, new[] {1f, 1.5f, 2f})},
      {GeneType.SizeFactor, new Preset(0.5f, 1.5f, new[] {0.5f, 1f, 1.5f})},
      {GeneType.DesirabilityScore, new Preset(1, 10, new[] {1f, 5f, 10f})},
      {GeneType.GestationPeriod, new Preset(10, 120, new[] {12f, 20f, 50f, 70f, 90f, 110f})},
      {GeneType.SexualMaturityTime, new Preset(10, 150, new[] {20f, 50f, 90f, 140f})}
    };

    public static readonly Dictionary<GeneType, Preset> DefaultSingular = new Dictionary<GeneType, Preset>()
    {
      {GeneType.HungerRate, new Preset(0, 10, new[] {1f})},
      {GeneType.HungerThreshold, new Preset(0, 10, new[] {10f})},
      {GeneType.ThirstRate, new Preset(0, 10, new[] {0.5f})},
      {GeneType.ThirstThreshold, new Preset(0, 10, new[] {10f})},
      {GeneType.Vision, new Preset(1, 50, new[] {25f})},
      {GeneType.Speed, new Preset(1, 2, new[] {1.5f})},
      {GeneType.SizeFactor, new Preset(0.5f, 1.5f, new[] {1f})},
      {GeneType.DesirabilityScore, new Preset(1, 10, new[] {1f})},
      {GeneType.GestationPeriod, new Preset(10, 120, new[] {11f})},
      {GeneType.SexualMaturityTime, new Preset(10, 150, new[] {11f})}
    };

    static RabbitGenome()
    {
      _preset = DefaultSet;
    }

    protected override void Initialize()
    {
      key = GenerateKey(10);
      Data = CreateData();
      ConvertGenesToAttributes();
      InitalGenomes.Add(this);
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