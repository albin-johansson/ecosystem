using UnityEngine;
using Random = System.Random;


public sealed class GeneUtil
{
  //Clamps
  public static double GetValidVar(double value, double min, double max)
  {
    return Mathf.Clamp((float) value, (float) min, (float) max);
  }

  private static Random r = new Random();

  public static bool RandomWithChance(double percentage)
  {
    double roll = r.NextDouble() * 100;
    if (roll < percentage)
    {
      return true;
    }
    else
    {
      return false;
    }
  }


  public static double MutatedInRange(double min, double max)
  {
    return r.NextDouble() * (max - min) + min;
  }
}