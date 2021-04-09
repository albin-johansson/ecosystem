using System;
using Ecosystem.Genes;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour
{
  public abstract class AbstractAnimalState : IAnimalState
  {
    protected StaminaController StaminaController;
    protected IConsumer Consumer;
    protected WaterConsumer WaterConsumer;
    protected MemoryController MemoryController;
    protected MovementController MovementController;
    protected EcoAnimationController AnimationController;
    protected Reproducer Reproducer;
    protected GameObject Target;
    protected AbstractGenome Genome;

    // This is used to avoid repeated allocations
    private readonly Collider[] _colliderBuffer = new Collider[10];

    public virtual void Begin(GameObject target)
    {
    }

    public virtual GameObject End()
    {
      Consumer.CollideActive = false;
      return Target;
    }

    public virtual AnimalState Tick()
    {
      if (Consumer.Hunger > WaterConsumer.Thirst && Consumer.IsHungry())
      {
        return AnimalState.LookingForFood;
      }
      else if (WaterConsumer.Thirst > Consumer.Hunger && WaterConsumer.IsThirsty())
      {
        return AnimalState.LookingForWater;
      }
      else if (Reproducer.IsFertile)
      {
        return AnimalState.LookingForMate;
      }
      else
      {
        return AnimalState.Idle;
      }
    }

    public virtual void OnTriggerEnter(Collider other)
    {
      if (other.gameObject.CompareTag("Water"))
      {
        MemoryController.SaveToMemory(other.gameObject);
      }
    }

    public virtual void OnTriggerExit(Collider other)
    {
    }

    public abstract AnimalState Type();

    protected GameObject SelectCloser(GameObject first, GameObject second)
    {
      var position = MovementController.GetPosition();
      var firstMag = Vector3.SqrMagnitude(position - first.transform.position);
      var secondMag = Vector3.SqrMagnitude(position - second.transform.position);
      return firstMag < secondMag ? first : second;
    }

    /// <summary>
    ///   Returns the closest GameObject within the vision radius that lays belong to
    ///   the layers defined by the LayerMask.
    /// </summary>
    protected GameObject GetClosestInVision(LayerMask mask)
    {
      return GetClosest(mask);
    }

    /// <summary>
    ///   Returns the closest GameObject within the vision radius that lays belong to
    ///   the layers defined by the LayerMask and is CompatibleAsParents.
    /// </summary>
    protected GameObject GetClosestMateInVision(LayerMask mask)
    {
      return GetClosest(mask, Reproducer.CompatibleAsParents);
    }

    /// <summary>
    ///   Updates the collider buffer with currently overlapping colliders.
    /// </summary>
    /// <param name="mask">the layer mask used when checking for overlaps.</param>
    /// <returns>the number of elements in the buffer that were the result from overlaps.</returns>
    private int UpdateCollisionBuffer(LayerMask mask)
    {
      if (!Genome)
      {
        return 0;
      }

      var position = MovementController.GetPosition();
      return Physics.OverlapSphereNonAlloc(position, Genome.GetVision().Value, _colliderBuffer, mask);
    }

    /// <summary>
    ///   Returns the currently closest GameObject. An optional predicate can be used to filter the colliders. If the
    ///   closest GameObject isn't reachable, this function returns null.
    /// </summary>
    /// <remarks>
    ///   This function is frequently called, so efforts have been made into making it as fast as possible, which is why
    ///   the code is somewhat "low-level". These efforts mainly include avoiding repeated allocation of lists and
    ///   buffers.
    /// </remarks>
    private GameObject GetClosest(LayerMask mask, Func<GameObject, bool> predicate = null)
    {
      GameObject closest = null;

      var nColliders = UpdateCollisionBuffer(mask);
      var position = MovementController.GetPosition();

      for (var index = 0; index < nColliders; ++index)
      {
        var colliderGameObject = _colliderBuffer[index].gameObject;
        if (predicate != null && !predicate(colliderGameObject))
        {
          continue;
        }

        if (closest)
        {
          if (Vector3.Distance(position, colliderGameObject.transform.position) <
              Vector3.Distance(position, closest.transform.position))
          {
            closest = colliderGameObject;
          }
        }
        else
        {
          closest = colliderGameObject;
        }
      }

      return closest && MovementController.IsReachable(closest.transform.position) ? closest : null;
    }
  }
}