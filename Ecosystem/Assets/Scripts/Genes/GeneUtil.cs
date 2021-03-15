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

    public static Gene NewGeneFromList(float min, float max, float[] vals)
    {
      return new Gene(vals[Random.Range(0, vals.Length)], min, max);
    }
  }
}