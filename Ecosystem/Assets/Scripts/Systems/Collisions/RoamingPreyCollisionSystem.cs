using Ecosystem.Components;
using Reese.Nav;
using Reese.Spatial;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;

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
      var consumedFromEntity = GetComponentDataFromEntity<Consumed>(true);
      var localToWorldFromEntity = GetComponentDataFromEntity<LocalToWorld>(true);

      Entities.WithAll<Prey, SpatialTrigger, PhysicsCollider>()
              .WithAll<Roaming>()
              .WithNone<MovingTowardsFood, Dead>()
              .WithChangeFilter<SpatialEntry>()
              .WithReadOnly(carrotFromEntity)
              .WithReadOnly(consumedFromEntity)
              .WithReadOnly(localToWorldFromEntity)
              .ForEach((Entity entity, int entityInQueryIndex, in DynamicBuffer<SpatialEntry> entries) =>
              {
                // Traverse from the end of the buffer for performance reasons.
                for (var i = entries.Length - 1; i >= 0; --i)
                {
                  var activator = entries[i].Value.Activator;

                  if (carrotFromEntity.HasComponent(activator) &&
                      !consumedFromEntity.HasComponent(activator) &&
                      localToWorldFromEntity.HasComponent(activator))
                  {
                    // Stop roaming and target the detected carrot
                    buffer.RemoveComponent<Roaming>(entityInQueryIndex, entity);

                    buffer.AddComponent(entityInQueryIndex, entity, new MovingTowardsFood
                    {
                            Food = activator
                    });

                    buffer.AddComponent(entityInQueryIndex, entity, new NavDestination
                    {
                            Teleport = false,
                            Tolerance = 5.0f,
                            CustomLerp = false,
                            WorldPoint = localToWorldFromEntity[activator].Position
                    });

                    break;
                  }
                }
              })
              .WithName("RoamingPreyCollisionSystemJob")
              .WithBurst()
              .ScheduleParallel();

      _barrier.AddJobHandleForProducer(Dependency);
    }
  }
}