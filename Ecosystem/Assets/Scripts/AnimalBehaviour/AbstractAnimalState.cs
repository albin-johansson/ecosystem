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
    private GameObject GetClosestWithFilter(List<Collider> colliders, Func<GameObject, bool> filter)
    {
      GameObject closest = null;
      foreach (var col in colliders.Where(col => filter(col.gameObject)))
      {
        if (closest)
        {
          if (Vector3.Distance(MovementController.GetPosition(), col.gameObject.transform.position) <
              Vector3.Distance(MovementController.GetPosition(), closest.transform.position))
          {
            closest = col.gameObject;
          }
        }
        else
        {
          closest = col.gameObject;
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
      var colliders = GetInVision(mask);
      return GetClosestWithFilter(colliders, o => true);
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

    private List<Collider> GetInVision(LayerMask mask)
    {
      if (!Genome)
      {
        return new List<Collider>(0);
      }

      var colliderArr = new Collider[10];
      var size = Physics.OverlapSphereNonAlloc(MovementController.GetPosition(), Genome.GetVision().Value, colliderArr, mask);
      var colliders = new List<Collider>(size);
      for (var i = 0; i < size; i++)
      {
        colliders.Add(colliderArr[i]);
      }
      return colliders;
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
