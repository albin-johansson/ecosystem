using UnityEngine;
using Random = System.Random;

namespace Ecosystem.Genes
{
  public static class GeneUtil
  {
    private static readonly Random Random = new Random();

    public static float GetValidVar(float value, float min, float max)
    {
      return Mathf.Clamp(value, min, max);
    }

    public static bool RandomWithChance(double percentage)
    {
      return Random.NextDouble() * 100 < percentage;
    }

    public static float Mutate(float min, float max)
    {
      return (float) Random.NextDouble() * (max - min) + min;
    }
  }
}