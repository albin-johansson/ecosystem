using UnityEngine;

namespace Ecosystem.Util
{
  public static class Nutrition
  {
    public static float GetNutritionalValue(GameObject animal)
    {
      if (Tags.IsRabbit(animal))
      {
        return 50;
      }
      else if (Tags.IsDeer(animal))
      {
        return 300;
      }
      else if (Tags.IsWolf(animal))
      {
        return 200;
      }
      else if (Tags.IsBear(animal))
      {
        return 400;
      }
      else
      {
        return 100;
      }
    }
  }
}