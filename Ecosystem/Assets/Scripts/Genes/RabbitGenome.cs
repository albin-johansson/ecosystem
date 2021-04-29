﻿using System.Collections.Generic;

namespace Ecosystem.Genes
{
  public sealed class RabbitGenome : AbstractGenome
  {
    private static float _mutateChance = 0.05f;
    private static Dictionary<GeneType, Preset> _preset;

    public static readonly Dictionary<GeneType, Preset> DefaultSet = new Dictionary<GeneType, Preset>()
    {
      {GeneType.HungerRate, new Preset(0.2f, 0.5f, new[] {0.2f, 0.3f, 0.4f, 0.5f})},
      {GeneType.HungerThreshold, new Preset(30, 40, new[] {30f, 35f, 40f})},
      {GeneType.ThirstRate, new Preset(0.3f, 0.5f, new[] {0.3f, 0.4f, 0.5f})},
      {GeneType.ThirstThreshold, new Preset(25, 40, new[] {25f, 35f, 40f})},
      {GeneType.Vision, new Preset(5, 10, new[] {5f, 6f, 7.5f, 9f, 10f})},
      {GeneType.Speed, new Preset(2, 4, new[] {2f, 3f, 4f})},
      {GeneType.GestationPeriod, new Preset(10, 120, new[] {12f, 20f, 50f, 70f, 90f, 110f})},
      {GeneType.SexualMaturityTime, new Preset(10, 50, new[] {10f, 20f, 30f, 40f, 50f})}
    };

    public static readonly Dictionary<GeneType, Preset> DefaultSingular = new Dictionary<GeneType, Preset>()
    {
      {GeneType.HungerRate, new Preset(0.2f, 0.5f, new[] {0.3f})},
      {GeneType.HungerThreshold, new Preset(30, 40, new[] {35f})},
      {GeneType.ThirstRate, new Preset(0.3f, 0.5f, new[] {0.4f})},
      {GeneType.ThirstThreshold, new Preset(25, 40, new[] {30f})},
      {GeneType.Vision, new Preset(5, 10, new[] {7.5f})},
      {GeneType.Speed, new Preset(2, 4, new[] {3f})},
      {GeneType.GestationPeriod, new Preset(10, 120, new[] {12f})},
      {GeneType.SexualMaturityTime, new Preset(10, 150, new[] {20f})}
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