using Ecosystem.Components;
using Reese.Nav;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using ParallelBuffer = Unity.Entities.EntityCommandBuffer.ParallelWriter;

namespace Ecosystem.ECS
{
  public static class Nav
  {
    [BurstCompile]
    public static void StopChaseAndRoam(ref ParallelBuffer buffer, int index, in Entity entity)
    {
      buffer.RemoveComponent<NavFollow>(index, entity);
      buffer.AddComponent<NavStop>(index, entity);
      buffer.AddComponent<Roaming>(index, entity);
    }

    [BurstCompile]
    public static void SetDestination(ref ParallelBuffer buffer, int index, in Entity entity, in float3 destination)
    {
      buffer.AddComponent(index, entity, new NavDestination
      {
        Teleport = false,
        Tolerance = 2.0f,
        CustomLerp = false,
        WorldPoint = destination
      });
    }
  }
}