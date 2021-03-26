using System;
using System.Collections.Generic;
using System.Linq;
using Ecosystem.Genes;
using UnityEngine;
using UnityEngine.UIElements;

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

    private GameObject GetClosestWithFilter(List<Collider> colliders, Func<GameObject, bool> filter)
    {
      GameObject closest = null;
      foreach (var col in colliders.Where(col => filter(col.gameObject)))
      {
        if (closest)
        {
          if(Vector3.Distance(MovementController.GetPosition(), col.gameObject.transform.position)<
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
      return closest;
    }

    protected GameObject GetClosest(List<Collider> colliders)
    {
      return GetClosestWithFilter(colliders, o => true);
    }

    protected List<Collider> GetInVision(Vector3 position, float visionRange, LayerMask mask)
    {
      var colliderArr = new Collider[10];
      var size = Physics.OverlapSphereNonAlloc(position, visionRange, colliderArr, mask);
      
      var colliders = new List<Collider>(size);
      for (var i = 0; i < size; i++)
      {
        colliders.Add(colliderArr[i]);
      }
      return colliders;
    }

    protected GameObject GetClosestMate(List<Collider> colliders)
    {
      return GetClosestWithFilter(colliders, Reproducer.CompatibleAsParents);
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

    public virtual void Begin(GameObject target) { }

    public virtual GameObject End()
    {
      Consumer.CollideActive = false;
      return Target;
    }

    public virtual void OnTriggerEnter(Collider other)
    { 
      if (other.gameObject.CompareTag("Water"))
      {
      }
    }

    public virtual void OnTriggerExit(Collider other)
    {
    }
  }
}