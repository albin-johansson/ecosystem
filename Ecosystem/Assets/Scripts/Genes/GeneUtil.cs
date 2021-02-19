using Random = System.Random;

namespace Ecosystem.Genes
{
  public static class GeneUtil
  {
    private static readonly Random Random = new Random();

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