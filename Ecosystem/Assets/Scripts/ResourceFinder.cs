using UnityEngine;
using Random = UnityEngine.Random;

public sealed class ResourceFinder : MonoBehaviour
{
  [SerializeField] private FoodConsumer foodConsumer;
  [SerializeField] private WaterConsumer waterConsumer;

  [SerializeField] private MemoryController memoryController;
  [SerializeField] private TargetTracker targetTracker;

  private bool _hasTarget = false;

  private MemoryController.Desire _priority = MemoryController.Desire.Idle;

  private void Update()
  {
    UpdatePriority();
    CheckMemory();
  }

  //Public function to access the _hasTarget boolean. 
  public void SetHasTarget(bool hasTarget)
  {
    _hasTarget = hasTarget;
  }

  //Checks MemoryController for objects that matches the priority Desire
  private void CheckMemory()
  {
    if (!_hasTarget)
    {
      _hasTarget = true;
      var gameObjects = memoryController.GetFromMemory(_priority);

      //Selects a random object that matches priority, if non exist set hasTarget to false again.
      if (gameObjects.Count > 0)
      {
        var target = gameObjects[Random.Range(0, gameObjects.Count - 1)];
        targetTracker.SetTarget(target);
      }
      else
      {
        _hasTarget = false;
      }
    }
  }

  //Sets priority, needs to be worked on to get a better flow
  private void UpdatePriority()
  {
    // Hunger has implicit priority
    if (foodConsumer.IsHungry())
    {
      _priority = MemoryController.Desire.Food;
    }
    else if (waterConsumer.IsThirsty())
    {
      _priority = MemoryController.Desire.Water;
    }
    else
    {
      _priority = MemoryController.Desire.Idle;
    }
  }
  
  /// <summary>
  /// When colliding with an object that object is saved to MemoryController and then set as a target in TargetTracker if the priority matches.
  /// Might be an improvment to only save the object and not set it as a target.
  /// </summary>
  private void OnTriggerEnter(Collider other)
  {
    memoryController.SaveToMemory(other.gameObject);
    if (!_hasTarget)
    {
      if (_priority == MemoryController.Desire.Food && other.GetComponent<Food>() != null ||
          _priority == MemoryController.Desire.Water && other.GetComponent<Water>() != null)
      {
        _hasTarget = true;
        targetTracker.SetTarget(other.gameObject);
      }
    }
  }
}