namespace Ecosystem
{
  /// <summary>
  ///   A delegate class used by animal behaviour scripts, which implements the common behaviour.
  ///   Note, this class is intentionally not a <c>MonoBehaviour</c>.
  /// </summary>
  public sealed class AnimalBehaviourDelegate
  {
    public MemoryController MemoryController { get; set; }
    public TargetTracker TargetTracker { get; set; }
    public WaterConsumer WaterConsumer { get; set; }
    public IConsumer Consumer { get; set; } // The "food" consumer

    public AnimalState AnimalState { get; private set; } = AnimalState.Idle;

    //Checks the memory for objects that matches the prioritised desire
    private void CheckMemory()
    {
      if (!TargetTracker.HasTarget && AnimalState != AnimalState.Idle)
      {
        var (match, vector3) = MemoryController.GetFromMemory(AnimalState);
        if (match)
        {
          TargetTracker.SetTarget(vector3, AnimalState);
        }
      }
    }

    //Sets priority, will set priority of what is currently most needed.
    private void UpdatePriority()
    {
      // TODO in the future, we might want to also check for Desire.Mate here
      if (TargetTracker.IsChased)
      {
        AnimalState = AnimalState.Fleeing;
        WaterConsumer.StopDrinking();
        TargetTracker.ResumeTracking();
        return;
      }

      if (WaterConsumer.IsDrinking)
      {
        AnimalState = AnimalState.Drinking;
        TargetTracker.StopTracking();
        return;
      }
      else
      {
        TargetTracker.ResumeTracking();
      }


      if (Consumer.Hunger > WaterConsumer.Thirst && Consumer.IsHungry())
      {
        AnimalState = AnimalState.LookingForFood;
      }
      else if (WaterConsumer.Thirst > Consumer.Hunger && WaterConsumer.IsThirsty())
      {
        AnimalState = AnimalState.LookingForWater;
      }
      else
      {
        AnimalState = AnimalState.Idle;
      }
    }

    public void Update()
    {
      UpdatePriority();
      CheckMemory();
    }
  }
}