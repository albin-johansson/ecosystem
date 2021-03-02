namespace Ecosystem
{
  /// <summary>
  ///   A delegate class used by animal behaviour scripts, which implements the common behaviour.
  ///   Note, this class is intentionally not a <c>MonoBehaviour</c>.
  /// </summary>
  public sealed class AnimalDelegate
  {
    public MemoryController MemoryController { get; set; }
    public TargetTracker TargetTracker { get; set; }
    public WaterConsumer WaterConsumer { get; set; }
    public IConsumer Consumer { get; set; } // The "food" consumer

    public Desire Desire { get; private set; } = Desire.Idle;

    //Checks the memory for objects that matches the prioritised desire
    private void CheckMemory()
    {
      if (!TargetTracker.HasTarget && Desire != Desire.Idle)
      {
        var (match, vector3) = MemoryController.GetFromMemory(Desire);
        if (match)
        {
          TargetTracker.SetTarget(vector3, Desire);
        }
      }
    }

    //Sets priority, will set priority of what is currently most needed.
    private void UpdatePriority()
    {
      // TODO in the future, we might want to also check for Desire.Mate here
      if (Consumer.Hunger > WaterConsumer.Thirst && Consumer.IsHungry())
      {
        Desire = Desire.Prey;
      }
      else if (WaterConsumer.Thirst > Consumer.Hunger && WaterConsumer.IsThirsty())
      {
        Desire = Desire.Water;
      }
      else
      {
        Desire = Desire.Idle;
      }
    }

    public void Update()
    {
      UpdatePriority();
      CheckMemory();
    }
  }
}