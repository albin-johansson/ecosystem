using Ecosystem.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using ParallelBuffer = Unity.Entities.EntityCommandBuffer.ParallelWriter;

namespace Ecosystem.ECS
{
  public static class PreyUtils
  {
    [BurstCompile]
    public static void StopGoingForFoodAndRoam(ref ParallelBuffer buffer, int index, in Entity entity)
    {
      buffer.RemoveComponent<MovingTowardsFood>(index, entity);
      buffer.AddComponent<Roaming>(index, entity);
    }

    [BurstCompile]
    public static void RegisterConsumption(ref ParallelBuffer buffer, int index, double when)
    {
      var entity = buffer.CreateEntity(index);
      buffer.AddComponent(index, entity, new Consumption
      {
              when = when
      });
    }

    [BurstCompile]
    public static void ConsumeFood(ref ParallelBuffer buffer, int index, in Entity entity, in Hunger hunger)
    {
      var newHunger = hunger;
      newHunger.value -= 0.75f * newHunger.max;
      newHunger.value = math.clamp(newHunger.value, 0, newHunger.max);
      buffer.SetComponent(index, entity, newHunger);
    }
  }
}