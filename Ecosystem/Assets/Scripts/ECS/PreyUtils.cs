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
    public static void StopGoingForResourceAndRoam(ref ParallelBuffer buffer, int index, in Entity entity)
    {
      buffer.RemoveComponent<MovingTowardsResource>(index, entity);
      buffer.AddComponent<Roaming>(index, entity);
    }

    [BurstCompile]
    public static void RegisterFoodConsumption(ref ParallelBuffer buffer, int index, double when)
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

    [BurstCompile]
    public static void ConsumeWater(ref ParallelBuffer buffer, int index, in Entity entity, in Thirst thirst)
    {
      var newThirst = thirst;
      newThirst.value -= 0.75f * newThirst.max;
      newThirst.value = math.clamp(newThirst.value, 0, newThirst.max);
      buffer.SetComponent(index, entity, newThirst);
    }
  }
}