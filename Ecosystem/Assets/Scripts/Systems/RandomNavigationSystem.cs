using Ecosystem.Util;
using Reese.Nav;
using Unity.Entities;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;

namespace Ecosystem.Systems
{
  [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
  [UpdateBefore(typeof(BuildPhysicsWorld))]
  public sealed class RandomNavigationSystem : SystemBase
  {
    private EntityCommandBufferSystem BufferSystem => World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();

    protected override void OnUpdate()
    {
      var commandBuffer = BufferSystem.CreateCommandBuffer();
      var terrain = Terrain.activeTerrain;

      Entities.WithNone<NavProblem, NavDestination, NavPlanning>()
              .ForEach((Entity entity, in Parent surface) =>
              {
                if (surface.Value.Equals(Entity.Null))
                {
                  return;
                }

                if (Terrains.RandomWalkablePosition(terrain, out var position))
                {
                  commandBuffer.AddComponent(entity, new NavDestination
                  {
                          Teleport = false,
                          Tolerance = 10f,
                          CustomLerp = false,
                          WorldPoint = position
                  });
                }
              }).WithName("RandomNavigationSystemJob").WithoutBurst().Run();
    }
  }
}