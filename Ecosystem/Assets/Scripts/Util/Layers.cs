using UnityEngine;

namespace Ecosystem.Util
{
  public static class Layers
  {
    public static readonly int WolfLayer = LayerMask.NameToLayer("Wolf");
    public static readonly int BearLayer = LayerMask.NameToLayer("Bear");
    public static readonly int WaterLayer = LayerMask.NameToLayer("Water");
    public static readonly int FoodLayer = LayerMask.NameToLayer("Food");
    public static readonly int PreyLayer = LayerMask.NameToLayer("Prey");

    public static bool IsPredatorLayer(LayerMask layer)
    {
      return layer == WolfLayer || layer == BearLayer;
    }
  }
}