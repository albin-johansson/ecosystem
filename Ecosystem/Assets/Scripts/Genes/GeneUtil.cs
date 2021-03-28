using UnityEngine;

namespace Ecosystem.Genes
{
  public static class GeneUtil
  {
    public static bool RandomWithChance(double percentage)
    {
      return Random.value * 100 < percentage;
    }

    public static float Mutate(float min, float max)
    {
      return Random.value * (max - min) + min;
    }

    public static Gene CreateGeneFromPreset(Preset preset)
    {
      return new Gene(preset.values[Random.Range(0, preset.values.Length)], preset.min, preset.max);
    }
  }
}