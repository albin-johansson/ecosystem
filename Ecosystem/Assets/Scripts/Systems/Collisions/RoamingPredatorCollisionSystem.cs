using Ecosystem.Components;
using Ecosystem.ECS;
using Reese.Nav;
using Reese.Spatial;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;

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
      var waterFromEntity = GetComponentDataFromEntity<Water>(true);
      var localToWorldFromEntity = GetComponentDataFromEntity<LocalToWorld>(true);

      Entities
        .WithAll<Predator, SpatialTrigger, PhysicsCollider>()
        .WithAll<Roaming, Hunger, Thirst>()
        .WithNone<NavFollow, Dead>()
        .WithChangeFilter<SpatialEntry>()
        .WithReadOnly(preyFromEntity)
        .WithReadOnly(deadFromEntity)
        .WithReadOnly(waterFromEntity)
        .WithReadOnly(localToWorldFromEntity)
        .ForEach((Entity entity,
          int entityInQueryIndex,
          in Predator predator,
          in Hunger hunger,
          in Thirst thirst,
          in DynamicBuffer<SpatialEntry> entries) =>
        {
          // Traverse from the end of the buffer for performance reasons.
          for (var i = entries.Length - 1; i >= 0; --i)
          {
            var activator = entries[i].Value.Activator;

            // Only chase prey that isn't dead
            if (preyFromEntity.HasComponent(activator) && !deadFromEntity.HasComponent(activator) &&
                EcsUtils.IsHungry(hunger))
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
            else if (waterFromEntity.HasComponent(activator) && EcsUtils.IsThirsty(thirst))
            {
              buffer.RemoveComponent<Roaming>(entityInQueryIndex, entity);

              buffer.AddComponent(entityInQueryIndex, entity, new MovingTowardsResource
              {
                Resource = activator
              });

              var position = localToWorldFromEntity[activator].Position;
              EcsUtils.SetDestination(ref buffer, entityInQueryIndex, entity, position);

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