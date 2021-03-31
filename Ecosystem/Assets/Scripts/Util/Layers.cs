using UnityEngine;

namespace Ecosystem.Util
{
  public static class Layers
  {
    // TODO should WolfLayer and BearLayer be merged into a single "PredatorLayer"?
    public static readonly int WolfLayer = LayerMask.NameToLayer("Wolf");
    public static readonly int BearLayer = LayerMask.NameToLayer("Bear");
    public static readonly int WaterLayer = LayerMask.NameToLayer("Water");
    public static readonly int FoodLayer = LayerMask.NameToLayer("Food");
    public static readonly int RabbitLayer = LayerMask.NameToLayer("Rabbit");
    public static readonly int DeerLayer = LayerMask.NameToLayer("Deer");
    public static readonly int PredatorLayer = LayerMask.GetMask("Wolf", "Bear");
    public static readonly int PreyLayer = LayerMask.GetMask("Rabbit", "Deer");

    public static bool IsPredatorLayer(LayerMask layer)
    {
      return layer == WolfLayer || layer == BearLayer;
    }
  }
}