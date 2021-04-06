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
          return 50;
        case "Deer":
          return 300;
        case "Wolf":
          return 200;
        case "Bear":
          return 400;
        default:
          return 100;
      }
    }
  }
}