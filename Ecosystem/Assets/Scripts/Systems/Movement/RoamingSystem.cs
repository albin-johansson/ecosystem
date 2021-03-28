using Ecosystem.Components;
using Ecosystem.ECS;
using Reese.Nav;
using Reese.Random;
using Unity.Entities;
using Unity.Physics.Systems;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine.SceneManagement;

namespace Ecosystem.Systems.Movement
{
  /// <summary>
  ///   This system is responsible for updating the movement of "roaming" animals, i.e. animals that
  ///   wander randomly in the world. This system iterates all entities with the <c>Roaming</c> component.
  /// </summary>
  [UpdateInGroup(typeof(MovementSystemGroup))]
  public sealed class RoamingSystem : SystemBase
  {
    private EntityCommandBufferSystem _barrier;
    private NavSystem _navSystem;
    private BuildPhysicsWorld _buildPhysicsWorld;
    private RandomSystem _randomSystem;

    private void SceneChanged(Scene current, Scene next)
    {
      Enabled = EcsUtils.IsEcsCapable(next);
      if (Enabled)
      {
        _barrier = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        _navSystem = World.GetOrCreateSystem<NavSystem>();
        _buildPhysicsWorld = World.GetExistingSystem<BuildPhysicsWorld>();
        _randomSystem = World.GetExistingSystem<RandomSystem>();
      }
    }

    protected override void OnCreate()
    {
      base.OnCreate();
      SceneManager.activeSceneChanged += SceneChanged;
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
                var surfaceAabb = renderBoundsFromEntity[surface.Value].Value;
                var randomPoint = NavUtil.GetRandomPointInBounds(ref random, surfaceAabb, 99);

                if (physicsWorld.GetPointOnSurfaceLayer(localToWorld,
                        randomPoint,
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
              .WithBurst()
              .ScheduleParallel();

      _barrier.AddJobHandleForProducer(Dependency);
      _buildPhysicsWorld.AddInputDependency(Dependency);
    }
  }
}