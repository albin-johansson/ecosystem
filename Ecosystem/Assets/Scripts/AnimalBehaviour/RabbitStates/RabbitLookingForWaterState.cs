using Ecosystem.Util;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Ecosystem.AnimalBehaviour.RabbitStates
{
  internal sealed class RabbitLookingForWaterState : AbstractAnimalState
  {
    private readonly LayerMask _whatIsWater = LayerMask.GetMask("Water"); 
    public RabbitLookingForWaterState(RabbitStateData data)
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
      Target = MemoryController.GetClosestInMemory(Tags.IsWater, MovementController.GetPosition());
      if (!Target)
      {
        Target = GetWaterInVision();
      }
      MovementController.StartWander();
      AnimationController.MoveAnimation();
    }

    public override AnimalState Tick()
    {
      MovementController.UpdateWander();
      if (Target)
      {
        if (Tags.IsPredator(Target))
        {
          return AnimalState.Fleeing;
        }
        else if (Tags.IsWater(Target))
        {
          return AnimalState.RunningTowardsWater;
        }
      }
      return base.Tick();
    }

    public override void OnTriggerEnter(Collider other)
    {
      var otherObject = other.gameObject;

      if (Tags.IsWater(otherObject))
      {
        MemoryController.SaveToMemory(otherObject);
        Target = otherObject;
      }
      else if (Tags.IsPredator(otherObject))
      {
        Target = other.gameObject;
      }
    }

    private GameObject GetWaterInVision()
    {
      return Genome ? GetClosest(GetInVision(MovementController.GetPosition(), Genome.GetVision().Value, _whatIsWater)) : null;
    }

    public override AnimalState Type()
    {
      return AnimalState.LookingForWater;
    }
  }
}
