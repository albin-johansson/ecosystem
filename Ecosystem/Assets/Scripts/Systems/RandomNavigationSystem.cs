using Ecosystem.Components;
using Reese.Nav;
using Reese.Random;
using Unity.Entities;
using Unity.Physics.Systems;
using Unity.Rendering;
using Unity.Transforms;

namespace Ecosystem.Systems
{
  [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
  [UpdateBefore(typeof(BuildPhysicsWorld))]
  public sealed class RandomNavigationSystem : SystemBase
  {
    private NavSystem NavSystem => World.GetOrCreateSystem<NavSystem>();

    private BuildPhysicsWorld BuildPhysicsWorldSys => World.GetExistingSystem<BuildPhysicsWorld>();

    private EntityCommandBufferSystem BufferSystem =>
            World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();

    protected override void OnUpdate()
    {
      var physicsWorld = BuildPhysicsWorldSys.PhysicsWorld;
      var settings = NavSystem.Settings;
      var commandBuffer = BufferSystem.CreateCommandBuffer().AsParallelWriter();
      var jumpableBufferFromEntity = GetBufferFromEntity<NavJumpableBufferElement>(true);
      var renderBoundsFromEntity = GetComponentDataFromEntity<RenderBounds>(true);
      var randomArray = World.GetExistingSystem<RandomSystem>().RandomArray;

      Entities.WithNone<NavProblem, NavDestination, NavPlanning>()
              .WithAll<Roaming>()
              .WithReadOnly(jumpableBufferFromEntity)
              .WithReadOnly(renderBoundsFromEntity)
              .WithReadOnly(physicsWorld)
              .WithNativeDisableParallelForRestriction(randomArray)
              .ForEach((Entity entity,
                      int entityInQueryIndex,
                      int nativeThreadIndex,
                      ref NavAgent agent,
                      in Parent surface,
                      in LocalToWorld localToWorld) =>
              {
                if (surface.Value.Equals(Entity.Null) || !jumpableBufferFromEntity.HasComponent(surface.Value))
                {
                  return;
                }

                var random = randomArray[nativeThreadIndex];

                if (physicsWorld.GetPointOnSurfaceLayer(
                        localToWorld,
                        NavUtil.GetRandomPointInBounds(ref random, renderBoundsFromEntity[surface.Value].Value, 99),
                        out var validDestination,
                        settings.ObstacleRaycastDistanceMax,
                        settings.ColliderLayer,
                        settings.SurfaceLayer))
                {
                  commandBuffer.AddComponent(entityInQueryIndex, entity, new NavDestination
                  {
                          WorldPoint = validDestination
                  });
                }

                randomArray[nativeThreadIndex] = random;
              })
              .WithName("RandomNavigationSystemJob")
              .ScheduleParallel();

      BufferSystem.AddJobHandleForProducer(Dependency);
      BuildPhysicsWorldSys.AddInputDependency(Dependency);
    }
  }
}