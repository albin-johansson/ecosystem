using System;

namespace Stats
{
  public class GeneUtil
  {
    /// <summary>
    /// Checks that the value is in the given range. If it is outside the range,
    /// set it to the closest value. 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="max"></param>
    /// <param name="min"></param>
    /// <returns></returns>
    public static double GetValidVar(double value, double max, double min)
    {
      if (value < min)
      {
        return min;
      }
      else if (value > max)
      {
        return max;
      }
      else
      {
        return value;
      }
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


    public static double MutatedInRange(double max, double min)
    {
      return r.NextDouble() * (max - min) + min;
    }
  }
}