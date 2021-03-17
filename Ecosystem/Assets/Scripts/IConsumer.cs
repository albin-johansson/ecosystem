namespace Ecosystem
{
  /// <summary>
  ///   An interface for food consumers, although not necessarily non-meat food.
  /// </summary>
  public interface IConsumer
  {
    double Hunger { get; }

    bool IsHungry();

    void SetSaturation(float value);
  }
}