using System;
using System.Collections.Generic;
using System.Linq;
using Ecosystem.Genes;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour
{
  public abstract class AbstractAnimalState : IAnimalState
  {
    protected IConsumer Consumer;
    protected WaterConsumer WaterConsumer;
    protected MemoryController MemoryController;
    protected MovementController MovementController;
    protected EcoAnimationController AnimationController;
    protected Reproducer Reproducer;
    protected GameObject Target;
    protected AbstractGenome Genome;


    /// <summary>
    ///   A helper function that returns the closest GameObject of those that return true
    ///   for the filter function, from a list of colliders. If the closest GameObject isn't
    ///   reachable, return null instead.
    /// </summary>
    private GameObject GetClosestWithFilter((Collider[] colliders, int size) colliderArray, Func<GameObject, bool> predicate)
    {
      GameObject closest = null;
      var (colliders, size) = colliderArray;
      for (var i = 0; i < size; ++i)
      {
        var colliderObject = colliders[i].gameObject;
        if (predicate(colliderObject))
        {
          continue;
        }
        if (closest)
        {
          var position = MovementController.GetPosition();
          var firstMag = Vector3.SqrMagnitude(position - closest.transform.position);
          var secondMag = Vector3.SqrMagnitude(position - colliderObject.transform.position);
          if (firstMag > secondMag)
          {
            closest = colliderObject;
          }
        }
        else
        {
          closest = colliderObject;
        }
      }

      if (closest)
      {
        if (MovementController.IsReachable(closest.transform.position))
        {
          return closest;
        }
      }
      return null;
    }

    /// <summary>
    ///   Returns the closest GameObject within the vision radius that lays belong to
    ///   the layers defined by the LayerMask.
    /// </summary>
    protected GameObject GetClosestInVision(LayerMask mask)
    {
      var colliderArray = GetInVision(mask);
      return GetClosestWithFilter(colliderArray, o => true);
    }

    /// <summary>
    ///   Returns the closest GameObject within the vision radius that lays belong to
    ///   the layers defined by the LayerMask and is CompatibleAsParents.
    /// </summary>
    protected GameObject GetClosestMateInVision(LayerMask mask)
    {
      var colliders = GetInVision(mask);
      return GetClosestWithFilter(colliders, Reproducer.CompatibleAsParents);
    }

    private (Collider[] colliders, int size) GetInVision(LayerMask mask)
    {
      var colliders = new Collider[10];
      
      if (!Genome)
      {
        return (colliders, 0);
      }

      var size = Physics.OverlapSphereNonAlloc(MovementController.GetPosition(), Genome.GetVision().Value, colliders, mask);
      return (colliders, size);
    }

    protected GameObject SelectCloser(GameObject first, GameObject second)
    {
      var position = MovementController.GetPosition();
      var firstMag = Vector3.SqrMagnitude(position - first.transform.position);
      var secondMag = Vector3.SqrMagnitude(position - second.transform.position);
      return firstMag < secondMag ? first  : second;
    }


    public abstract AnimalState Type();

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

    public virtual void Begin(GameObject target)
    {
    }

    public virtual GameObject End()
    {
      Consumer.CollideActive = false;
      return Target;
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
  }
}
