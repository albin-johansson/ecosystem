using UnityEngine;
using Random = System.Random;

namespace Ecosystem.Genes
{
  public sealed class GeneUtil
  {
    private static readonly Random Random = new Random();

    //Clamps
    public static double GetValidVar(double value, double min, double max)
    {
      return Mathf.Clamp((float) value, (float) min, (float) max);
    }

    public static bool RandomWithChance(double percentage)
    {
      return Random.NextDouble() * 100 < percentage;
    }

    public static double MutatedInRange(double min, double max)
    {
      return Random.NextDouble() * (max - min) + min;
    }
  }
}