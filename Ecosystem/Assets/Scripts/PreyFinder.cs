using Ecosystem.Genes;
using UnityEngine;

namespace Ecosystem
{
  public sealed class PreyFinder : MonoBehaviour // TODO rename to PredatorBehaviour?
  {
    [SerializeField] private PreyConsumer preyConsumer;
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
              Consumer = preyConsumer
      };
      var sp = GetComponent<SphereCollider>();
      Debug.Log("Radius before: " + sp.radius);
      sp.radius = (sp.radius / sp.transform.lossyScale.magnitude) * (float) genome.GetVisionRange();
      Debug.Log("Radius after: " + sp.radius);
    }

    private void Update()
    {
      _delegate.Update();
    }

    /// <summary>
    /// When colliding with an object, that object is saved to the animals memory, and subsequently set as a target if the
    /// priority matches.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
      memoryController.SaveToMemory(other.gameObject);

      if (!targetTracker.HasTarget)
      {
        var state = _delegate.AnimalState;
        if (state == AnimalState.LookingForFood && other.gameObject.layer == LayerUtil.PreyLayer ||
            state == AnimalState.LookingForWater && other.gameObject.layer == LayerUtil.WaterLayer ||
            state == AnimalState.Idle && mateFinder.CompatibleAsParents(other.gameObject))
        {
          targetTracker.SetTarget(other.gameObject.transform.position, state);
        }
      }
    }
  }
}
