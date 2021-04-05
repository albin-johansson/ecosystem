using System;
using UnityEngine;

namespace Ecosystem.Util
{
  public static class Nutrition
  {
    public static Double getNutrition(GameObject animal)
    {
      switch (animal.tag)
      {
        case "Rabbit":
          return 100;
        case "Deer":
          return 500;
        default:
          return 100;
      }
    }
  }
}