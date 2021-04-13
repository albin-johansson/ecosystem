using UnityEngine;

namespace Ecosystem.Util
{
  public static class Layers
  {
    public static readonly int RabbitMask = LayerMask.GetMask("Rabbit");
    public static readonly int DeerMask = LayerMask.GetMask("Deer");
    public static readonly int WolfMask = LayerMask.GetMask("Wolf");
    public static readonly int BearMask = LayerMask.GetMask("Bear");

    public static readonly int PredatorMask = WolfMask | BearMask;
    public static readonly int PreyMask = RabbitMask | DeerMask;

    public static readonly int AnimalMask = PredatorMask | PreyMask;

    public static readonly int WaterMask = LayerMask.GetMask("Water");
    public static readonly int FoodMask = LayerMask.GetMask("Food");
  }
}