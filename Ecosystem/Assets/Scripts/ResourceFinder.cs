using Ecosystem.Genes;
using Ecosystem.Util;
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
    [SerializeField] private Genome genome;

    private AnimalBehaviourDelegate _delegate;

    private void Start()
    {
      _delegate = new AnimalBehaviourDelegate
      {
              MemoryController = memoryController,
              TargetTracker = targetTracker,
              WaterConsumer = waterConsumer,
              Consumer = foodConsumer
      };
      var sp = GetComponent<SphereCollider>();
      sp.radius = (sp.radius / sp.transform.lossyScale.magnitude) * (float) genome.GetVisionFactor();
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
      if (Layers.IsPredatorLayer(other.gameObject.layer))
      {
        targetTracker.FleeFromPredator(other.gameObject);
        return;
      }

      memoryController.SaveToMemory(other.gameObject);

      if (!targetTracker.HasTarget)
      {
        var state = _delegate.AnimalState;
        if (state == AnimalState.LookingForFood && other.gameObject.layer == Layers.FoodLayer ||
            state == AnimalState.LookingForWater && other.gameObject.layer == Layers.WaterLayer ||
            other.CompareTag("Reproducer") && state == AnimalState.Idle && mateFinder.CompatibleAsParents(other.gameObject))
        {
          targetTracker.SetTarget(other.gameObject.transform.position, state);
        }
      }
    }
  }
}