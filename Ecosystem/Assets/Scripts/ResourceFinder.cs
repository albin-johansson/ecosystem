using UnityEngine;

namespace Ecosystem
{
  public sealed class ResourceFinder : MonoBehaviour // TODO rename to PreyBehaviour?
  {
    [SerializeField] private FoodConsumer foodConsumer;
    [SerializeField] private WaterConsumer waterConsumer;
    [SerializeField] private MateFinder mateFinder;
    [SerializeField] private MemoryController memoryController;
    [SerializeField] private TargetTracker targetTracker;

    private AnimalDelegate _delegate;

    private void Start()
    {
      _delegate = new AnimalDelegate
      {
              MemoryController = memoryController,
              TargetTracker = targetTracker,
              WaterConsumer = waterConsumer,
              Consumer = foodConsumer
      };
    }

    private void Update()
    {
      _delegate.Update();
    }

    /// <summary>
    ///   When colliding with an object, that object is saved to the animals memory, and
    ///   subsequently set as a target if the priority matches. If a predator is within
    ///   field of view the animal will flee.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
      if (LayerUtil.IsPredatorLayer(other.gameObject.layer))
      {
        targetTracker.FleeFromPredator(other.gameObject);
        return;
      }

      memoryController.SaveToMemory(other.gameObject);

      if (!targetTracker.HasTarget)
      {
        var desire = _delegate.Desire;
        if (desire == Desire.Food && other.gameObject.layer == LayerUtil.FoodLayer ||
            desire == Desire.Water && other.gameObject.layer == LayerUtil.WaterLayer ||
            desire == Desire.Idle && mateFinder.CompatibleAsParents(other.gameObject))
        {
          targetTracker.SetTarget(other.gameObject.transform.position, desire);
        }
      }
    }
  }
}