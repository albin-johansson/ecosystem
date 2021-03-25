using Ecosystem.Components;
using Reese.Nav;
using Reese.Spatial;
using Unity.Entities;
using Unity.Physics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace Ecosystem.Systems
{
  [UpdateInGroup(typeof(SimulationSystemGroup))]
  [UpdateAfter(typeof(SpatialStartSystem))]
  [UpdateBefore(typeof(SpatialEndSystem))]
  public sealed class CollisionSystem : SystemBase
  {
    private EntityCommandBufferSystem _barrier;

    protected override void OnCreate()
    {
      base.OnCreate();
      if (SceneManager.GetActiveScene().name != "ECSDemo")
      {
        Enabled = false;
      }
      else
      {
        _barrier = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
      }
    }

    protected override void OnUpdate()
    {
      var buffer = _barrier.CreateCommandBuffer().AsParallelWriter();

      Entities.WithAll<Wolf, SpatialTrigger, PhysicsCollider>()
              .WithNone<Dead>()
              .WithChangeFilter<SpatialEntry>()
              .ForEach((in DynamicBuffer<SpatialEntry> entries) =>
              {
                // Traverse from the end of the buffer for performance reasons.
                for (var i = entries.Length - 1; i >= 0; --i)
                {
                  var entity = entries[i].Value.Activator;
                  if (HasComponent<Rabbit>(entity) && !HasComponent<Dead>(entity))
                  {
                    buffer.AddComponent<Dead>(entity.Index, entity);
                  }
                }
              })
              .WithName("CollisionSystemEntryJob")
              .ScheduleParallel();

      _barrier.AddJobHandleForProducer(Dependency);
    }
  }
}