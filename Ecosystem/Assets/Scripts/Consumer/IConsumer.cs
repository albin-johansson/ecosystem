using UnityEngine;

namespace Ecosystem.Consumer
{
  /// <summary>
  ///   An interface for food consumers, although not necessarily non-meat food.
  /// </summary>
  public interface IConsumer
  {
    float Hunger { get; }

    bool ColliderActive { get; set; }

    bool IsConsuming { get; set; }

    GameObject EatingFromGameObject { get; set; }

    bool IsHungry();

    void SetSaturation(float value);

    void CheckLastCollision();
  }
}