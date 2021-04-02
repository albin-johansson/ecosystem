using Ecosystem.Components;
using Reese.Nav;
using Unity.Burst;
using Unity.Entities;
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
  }
}