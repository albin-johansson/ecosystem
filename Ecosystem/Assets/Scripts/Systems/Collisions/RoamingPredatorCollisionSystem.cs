using Ecosystem.Components;
using Reese.Nav;
using Reese.Spatial;
using Unity.Entities;
using Unity.Physics;

namespace Ecosystem.Systems.Collisions
{
  /// <summary>
  ///   This system goes through entry collisions for all currently roaming predators. The predators will
  ///   start chasing any colliding prey that isn't dead.
  /// </summary>
  [UpdateInGroup(typeof(CollisionSystemGroup))]
  public sealed class RoamingPredatorCollisionSystem : AbstractSystem
  {
    private EntityCommandBufferSystem _barrier;

    protected override void Initialize()
    {
      _barrier = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
      var buffer = _barrier.CreateCommandBuffer().AsParallelWriter();

      var preyFromEntity = GetComponentDataFromEntity<Prey>(true);
      var deadFromEntity = GetComponentDataFromEntity<Dead>(true);

      Entities.WithAll<Predator, SpatialTrigger, PhysicsCollider>()
              .WithAll<Roaming>()
              .WithNone<NavFollow, Dead>()
              .WithChangeFilter<SpatialEntry>()
              .WithReadOnly(preyFromEntity)
              .WithReadOnly(deadFromEntity)
              .ForEach((Entity entity,
                      int entityInQueryIndex,
                      in Predator predator,
                      in DynamicBuffer<SpatialEntry> entries) =>
              {
                // Traverse from the end of the buffer for performance reasons.
                for (var i = entries.Length - 1; i >= 0; --i)
                {
                  var activator = entries[i].Value.Activator;

                  // Only chase prey that isn't dead
                  if (preyFromEntity.HasComponent(activator) && !deadFromEntity.HasComponent(activator))
                  {
                    // Stop roaming and pursue the detected prey
                    buffer.RemoveComponent<Roaming>(entityInQueryIndex, entity);
                    buffer.AddComponent(entityInQueryIndex, entity, new NavStop());
                    buffer.AddComponent(entityInQueryIndex, entity, new NavFollow
                    {
                            Target = activator,
                            MinDistance = 0,
                            MaxDistance = predator.maxChaseDistance
                    });

                    break;
                  }
                }
              })
              .WithName("RoamingPredatorCollisionSystemJob")
              .WithBurst()
              .ScheduleParallel();

      _barrier.AddJobHandleForProducer(Dependency);
    }
  }
}