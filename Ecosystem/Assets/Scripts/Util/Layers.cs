using UnityEngine;

namespace Ecosystem.Util
{
  public static class Layers
  {
    public static readonly int AnimalMask = LayerMask.GetMask("Bear", "Wolf", "Prey");
    public static readonly int WolfMask = LayerMask.GetMask("Wolf");
    public static readonly int BearMask = LayerMask.GetMask("Bear");
    public static readonly int PredatorMask = LayerMask.GetMask("Wolf", "Bear");
    public static readonly int PreyMask = LayerMask.GetMask("Prey");
    public static readonly int WaterMask = LayerMask.GetMask("Water");
    public static readonly int FoodMask = LayerMask.GetMask("Food");
  }
}