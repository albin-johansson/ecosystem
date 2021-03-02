namespace Ecosystem
{
  /// <summary>
  ///   An interface for food consumers, although not necessarily non-meat food.
  /// </summary>
  public interface IConsumer
  {
    public double Hunger { get; }

    public bool IsHungry();
  }
}