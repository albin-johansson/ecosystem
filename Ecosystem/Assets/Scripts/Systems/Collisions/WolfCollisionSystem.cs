using Ecosystem.Components;
using Ecosystem.ECS;
using Reese.Nav;
using Reese.Spatial;
using Unity.Entities;
using Unity.Physics;
using UnityEngine.SceneManagement;

namespace Ecosystem.Systems.Collisions
{
  [UpdateInGroup(typeof(CollisionSystemGroup))]
  public sealed class WolfCollisionSystem : SystemBase
  {
    private EntityCommandBufferSystem _barrier;

    private void OnSceneChanged(Scene current, Scene next)
    {
      Enabled = EcsUtils.IsEcsCapable(next);
      if (Enabled)
      {
        _barrier = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
      }
    }

    protected override void OnCreate()
    {
      base.OnCreate();
      SceneManager.activeSceneChanged += OnSceneChanged;
    }

    protected override void OnUpdate()
    {
      var buffer = _barrier.CreateCommandBuffer().AsParallelWriter();

      Entities.WithAll<Wolf, SpatialTrigger, PhysicsCollider>()
              .WithNone<NavFollow, Dead>()
              .WithChangeFilter<SpatialEntry>() // Note that we only observe collision entries
              .ForEach((Entity wolf, in DynamicBuffer<SpatialEntry> entries) =>
              {
                // Traverse from the end of the buffer for performance reasons.
                for (var i = entries.Length - 1; i >= 0; --i)
                {
                  var activator = entries[i].Value.Activator;
                  if (HasComponent<Rabbit>(activator) && !HasComponent<Dead>(activator))
                  {
                    // Stop roaming and pursue the rabbit
                    buffer.RemoveComponent<Roaming>(wolf.Index, wolf);
                    buffer.AddComponent(wolf.Index, wolf, new NavFollow
                    {
                            Target = activator,
                            MaxDistance = 50,
                            MinDistance = 0
                    });
                  }
                }
              }).WithName("WolfCollisionSystemEntryJob")
              .WithBurst()
              .ScheduleParallel();

      _barrier.AddJobHandleForProducer(Dependency);
    }
  }
}