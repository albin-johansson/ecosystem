using System.Linq;
using System.Threading;
using Ecosystem.Util;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.RabbitStates
{
  internal sealed class RabbitLookingForFoodState : AbstractAnimalState
  {
    private readonly LayerMask _whatIsFood = LayerMask.GetMask("Food");
    public RabbitLookingForFoodState(RabbitStateData data)
    {
      Consumer = data.Consumer;
      WaterConsumer = data.WaterConsumer;
      MovementController = data.MovementController;
      AnimationController = data.AnimationController;
      MemoryController = data.MemoryController;
      Reproducer = data.Reproducer;
      Genome = data.Genome;
    }

    public override void Begin(GameObject target)
    {
      Target = GetFoodInVision();
      MovementController.StartWander();
      AnimationController.MoveAnimation();
    }

    public override AnimalState Tick()
    {
      if (Target)
      {
        if (Tags.IsPredator(Target))
        {
          return AnimalState.Fleeing;
        }
        else if (Tags.IsFood(Target))
        {
          return AnimalState.RunningTowardsFood;
        }
      }

      if (Consumer.Hunger < WaterConsumer.Thirst && WaterConsumer.IsThirsty())
      {
        return AnimalState.LookingForWater;
      }

      MovementController.UpdateWander();

      return Type();
    }

    public override void OnTriggerEnter(Collider other)
    {
      var otherObject = other.gameObject;
      if (otherObject.CompareTag("Water"))
      {
        MemoryController.SaveToMemory(otherObject);
      }
      else if (Tags.IsFood(otherObject))
      {
        Target = otherObject;
      }
      else if (Tags.IsPredator(otherObject))
      {
        Target = otherObject;
      }
    }

    private GameObject GetFoodInVision()
    {
      return Genome ? GetClosest(GetInVision(MovementController.GetPosition(), Genome.GetVision().Value, _whatIsFood)) : null;
    }

    public override AnimalState Type()
    {
      return AnimalState.LookingForFood;
    }
  }
}