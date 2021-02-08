using UnityEngine;
using Random = UnityEngine.Random;

public sealed class ResourceFinder : MonoBehaviour
{
  [SerializeField] private FoodConsumer foodConsumer;
  [SerializeField] private WaterConsumer waterConsumer;

  [SerializeField] private MemoryController memoryController;
  [SerializeField] private TargetTracker targetTracker;

  private Desire _priority = Desire.Idle;

  private void Update()
  {
    UpdatePriority();
    CheckMemory();
  }

  //Checks MemoryController for objects that matches the priority Desire
  private void CheckMemory()
  {
    if (!targetTracker.HasTarget)
    {
      var gameObjects = memoryController.GetFromMemory(_priority);

      //Selects a random object that matches priority, if non exist set hasTarget to false again.
      if (gameObjects.Count > 0)
      {
        var target = gameObjects[Random.Range(0, gameObjects.Count - 1)];
        targetTracker.SetTarget(target);
      }
    }
  }

  //Sets priority, OBS. needs to be worked on to get a better flow
  private void UpdatePriority()
  {
    // Hunger has implicit priority
    if (foodConsumer.IsHungry())
    {
      _priority = Desire.Food;
    }
    else if (waterConsumer.IsThirsty())
    {
      _priority = Desire.Water;
    }
    else
    {
      _priority = Desire.Idle;
    }
  }

  /// <summary>
  /// When colliding with an object that object is saved to MemoryController and then set as a target in TargetTracker if the priority matches.
  /// Might be an improvment to only save the object and not set it as a target.
  /// </summary>
  private void OnTriggerEnter(Collider other)
  {
    memoryController.SaveToMemory(other.gameObject);
    if (!targetTracker.HasTarget)
    {
      if (_priority == Desire.Food && other.GetComponent<Food>() != null ||
          _priority == Desire.Water && other.GetComponent<Water>() != null)
      {
        targetTracker.SetTarget(other.gameObject);
      }
    }
  }
}