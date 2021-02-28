using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ecosystem
{
  public static class LayerUtil
  {
    public static readonly int WolfLayer = LayerMask.NameToLayer("Wolf"); // Change to "Wolf"
    public static readonly int WaterLayer = LayerMask.NameToLayer("Water");
    public static readonly int FoodLayer = LayerMask.NameToLayer("Food");
    public static readonly int PreyLayer = LayerMask.NameToLayer("Prey");

    public static bool IsPredatorLayer(LayerMask layer)
    {
      return layer == WolfLayer; // Add more checks here if we add more predators...
    }
  }
}