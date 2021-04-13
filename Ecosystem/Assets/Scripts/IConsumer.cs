using UnityEngine;

namespace Ecosystem
{
  /// <summary>
  ///   An interface for food consumers, although not necessarily non-meat food.
  /// </summary>
  public interface IConsumer
  {
    double Hunger { get; }

    bool CollideActive { get; set; }
    
    GameObject EatingFromGameObject { get; set; }

    bool IsHungry();

    bool IsConsuming { get; set; }

    void SetSaturation(float value);
  }
}