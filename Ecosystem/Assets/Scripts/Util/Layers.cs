using UnityEngine;

namespace Ecosystem.Util
{
  public static class Layers
  {
    // TODO should WolfLayer and BearLayer be merged into a single "PredatorLayer"?
    public static readonly int WolfLayer = LayerMask.GetMask("Wolf");
    public static readonly int BearLayer = LayerMask.GetMask("Bear");
    public static readonly int WaterLayer = LayerMask.GetMask("Water");
    public static readonly int FoodLayer = LayerMask.GetMask("Food");
    public static readonly int PreyLayer = LayerMask.GetMask("Prey");
    public static readonly int PredatorLayer = LayerMask.GetMask("Wolf", "Bear");

    public static bool IsPredatorLayer(LayerMask layer)
    {
      return layer == WolfLayer || layer == BearLayer;
    }
  }
}