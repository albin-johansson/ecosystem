using UnityEngine;

namespace Ecosystem.Consumers
{
  /// <summary>
  ///   An interface for food consumers, although not necessarily non-meat food.
  /// </summary>
  public interface IConsumer
  {
    float Hunger { get; }

    bool ColliderActive { get; set; }

    GameObject EatingFromGameObject { get; set; }

    bool IsHungry();

    bool IsConsuming { get; set; }

    void SetSaturation(float value);
  }
}