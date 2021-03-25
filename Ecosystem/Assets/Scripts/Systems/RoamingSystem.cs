using Ecosystem.Components;
using Reese.Nav;
using Reese.Random;
using Unity.Entities;
using Unity.Physics.Systems;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine.SceneManagement;

namespace Ecosystem.Systems
{
  [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
  [UpdateBefore(typeof(BuildPhysicsWorld))]
  public sealed class RoamingSystem : SystemBase
  {
    private EntityCommandBufferSystem _barrier;
    private NavSystem _navSystem;
    private BuildPhysicsWorld _buildPhysicsWorld;
    private RandomSystem _randomSystem;

    protected override void OnCreate()
    {
      base.OnCreate();
      if (SceneManager.GetActiveScene().name != "ECSDemo")
      {
        Enabled = false;
      }
      else
      {
        _barrier = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        _navSystem = World.GetOrCreateSystem<NavSystem>();
        _buildPhysicsWorld = World.GetExistingSystem<BuildPhysicsWorld>();
        _randomSystem = World.GetExistingSystem<RandomSystem>();
      }
    }

    protected override void OnUpdate()
    {
      var buffer = _barrier.CreateCommandBuffer().AsParallelWriter();
      
      var jumpableBufferFromEntity = GetBufferFromEntity<NavJumpableBufferElement>(true);
      var renderBoundsFromEntity = GetComponentDataFromEntity<RenderBounds>(true);
      
      var physicsWorld = _buildPhysicsWorld.PhysicsWorld;
      var randomArray = _randomSystem.RandomArray;
      var settings = _navSystem.Settings;

      Entities.WithNone<NavProblem, NavDestination, NavPlanning>()
              .WithNone<Dead>()
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
                  buffer.AddComponent(entityInQueryIndex, entity, new NavDestination
                  {
                          WorldPoint = validDestination
                  });
                }

                randomArray[nativeThreadIndex] = random;
              })
              .WithName("RoamingSystemJob")
              .ScheduleParallel();

      _barrier.AddJobHandleForProducer(Dependency);
      _buildPhysicsWorld.AddInputDependency(Dependency);
    }
  }
}