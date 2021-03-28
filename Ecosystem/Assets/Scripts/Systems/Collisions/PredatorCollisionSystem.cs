using Ecosystem.Components;
using Reese.Nav;
using Reese.Spatial;
using Unity.Entities;
using Unity.Physics;

namespace Ecosystem.Systems.Collisions
{
  [UpdateInGroup(typeof(CollisionSystemGroup))]
  public sealed class PredatorCollisionSystem : AbstractSystem
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
              .WithNone<NavFollow, Dead>()
              .WithChangeFilter<SpatialEntry>() // Note that we only observe collision entries
              .WithReadOnly(preyFromEntity)
              .WithReadOnly(deadFromEntity)
              .ForEach((Entity predatorEntity, in DynamicBuffer<SpatialEntry> entries) =>
              {
                // Traverse from the end of the buffer for performance reasons.
                for (var i = entries.Length - 1; i >= 0; --i)
                {
                  var activator = entries[i].Value.Activator;

                  if (preyFromEntity.HasComponent(activator) && !deadFromEntity.HasComponent(activator))
                  {
                    // Stop roaming and pursue the detected prey
                    buffer.RemoveComponent<Roaming>(predatorEntity.Index, predatorEntity);
                    buffer.AddComponent(predatorEntity.Index, predatorEntity, new NavFollow
                    {
                            Target = activator,
                            MaxDistance = 20, // TODO make this a property of predators? Or a separate comp
                            MinDistance = 0
                    });
                  }
                }
              }).WithName("PredatorCollisionSystemEntryJob")
              .WithBurst()
              .ScheduleParallel();

      _barrier.AddJobHandleForProducer(Dependency);
    }
  }
}