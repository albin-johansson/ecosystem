using Ecosystem.Components;
using Ecosystem.ECS;
using Reese.Nav;
using Reese.Spatial;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using ParallelBuffer = Unity.Entities.EntityCommandBuffer.ParallelWriter;

namespace Ecosystem.Systems.Collisions
{
  [UpdateInGroup(typeof(CollisionSystemGroup))]
  public sealed class RoamingPreyCollisionSystem : AbstractSystem
  {
    private EntityCommandBufferSystem _barrier;

    protected override void Initialize()
    {
      _barrier = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
      var buffer = _barrier.CreateCommandBuffer().AsParallelWriter();

      var carrotFromEntity = GetComponentDataFromEntity<Carrot>(true);
      var waterFromEntity = GetComponentDataFromEntity<Water>(true);
      var localToWorldFromEntity = GetComponentDataFromEntity<LocalToWorld>(true);

      Entities
        .WithAll<Prey, SpatialTrigger, PhysicsCollider>()
        .WithAll<Roaming, Hunger, Thirst>()
        .WithNone<Dead, MovingTowardsResource>()
        .WithChangeFilter<SpatialEntry>()
        .WithReadOnly(carrotFromEntity)
        .WithReadOnly(waterFromEntity)
        .WithReadOnly(localToWorldFromEntity)
        .ForEach((Entity entity,
          int entityInQueryIndex,
          in Hunger hunger,
          in Thirst thirst,
          in DynamicBuffer<SpatialEntry> entries) =>
        {
          // Traverse from the end of the buffer for performance reasons.
          for (var i = entries.Length - 1; i >= 0; --i)
          {
            var activator = entries[i].Value.Activator;

            if (!localToWorldFromEntity.HasComponent(activator))
            {
              continue;
            }

            if (carrotFromEntity.HasComponent(activator) && hunger.value >= 0.05 * hunger.max ||
                waterFromEntity.HasComponent(activator) && thirst.value >= 0.05 * thirst.max)
            {
              buffer.RemoveComponent<Roaming>(entityInQueryIndex, entity);

              buffer.AddComponent(entityInQueryIndex, entity, new MovingTowardsResource
              {
                Resource = activator
              });

              var position = localToWorldFromEntity[activator].Position;
              SetDestination(ref buffer, entityInQueryIndex, entity, position);

              break;
            }
          }
        })
        .WithName("RoamingPreyCollisionSystemJob")
        .WithBurst()
        .ScheduleParallel();

      _barrier.AddJobHandleForProducer(Dependency);
    }

    [BurstCompile]
    private static void SetDestination(ref ParallelBuffer buffer, int index, in Entity entity, in float3 destination)
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