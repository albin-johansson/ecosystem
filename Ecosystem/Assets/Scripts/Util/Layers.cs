using UnityEngine;

namespace Ecosystem.Util
{
  public static class Layers
  {
    public static readonly int WolfMask = LayerMask.GetMask("Wolf");
    public static readonly int BearMask = LayerMask.GetMask("Bear");
    public static readonly int WaterMask = LayerMask.GetMask("Water");
    public static readonly int FoodMask = LayerMask.GetMask("Food");
    public static readonly int PreyMask = LayerMask.GetMask("Prey");
    public static readonly int PredatorMask = LayerMask.GetMask("Wolf", "Bear");

    public static bool IsPredatorLayer(LayerMask layer)
    {
      return layer == WolfMask || layer == BearMask;
    }
  }
}