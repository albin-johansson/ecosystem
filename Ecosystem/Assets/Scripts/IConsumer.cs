namespace Ecosystem
{
  /// <summary>
  ///   An interface for food consumers, although not necessarily non-meat food.
  /// </summary>
  public interface IConsumer
  {
    double Hunger { get; }

    bool CollideActive { get; set; }

    bool IsHungry();

    bool IsAttacking { get; set; }

    void SetSaturation(float value);
  }
}