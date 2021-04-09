using Ecosystem.Components;
using Ecosystem.ECS;
using Ecosystem.Logging;
using Reese.Nav;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using ParallelBuffer = Unity.Entities.EntityCommandBuffer.ParallelWriter;

namespace Ecosystem.Systems.Collisions
{
  /// <summary>
  ///   This system is responsible for checking for predators that get within striking distance of the prey that they
  ///   are chasing. When a predator is within striking distance, the prey simply dies in the current implementation,
  ///   and the predator subsequently returns to roaming. Furthermore, if a predator is too far away from the chased
  ///   prey, the chase is aborted.
  /// </summary>
  /// <remarks>
  ///   This system doesn't make use of colliders, instead the raw distance between entities is used. This is because of
  ///   an apparent limitation with the third-party collision library, which prevents multiple overlapping colliders on
  ///   a single animal (or rather, it is *extremely* slow).
  /// </remarks>
  [UpdateInGroup(typeof(CollisionSystemGroup))]
  [UpdateAfter(typeof(RoamingPredatorCollisionSystem))]
  public sealed class PredatorChaseCollisionSystem : AbstractSystem
  {
    private EntityCommandBufferSystem _barrier;

    protected override void Initialize()
    {
      _barrier = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
      var buffer = _barrier.CreateCommandBuffer().AsParallelWriter();

      var localToWorldFromEntity = GetComponentDataFromEntity<LocalToWorld>(true);
      var deadFromEntity = GetComponentDataFromEntity<Dead>(true);

      var time = Time;

      Entities.WithAll<Predator, NavFollow, LocalToWorld>()
        .WithNone<Dead>()
        .WithReadOnly(localToWorldFromEntity)
        .WithReadOnly(deadFromEntity)
        .ForEach((Entity entity,
          int entityInQueryIndex,
          in Predator predator,
          in NavFollow follow) =>
        {
          var preyEntity = follow.Target;

          if (deadFromEntity.HasComponent(preyEntity))
          {
            // The chased animal died from something else, so abort the chase
            StopChaseAndRoam(ref buffer, entityInQueryIndex, entity);
            return;
          }

          var predatorPosition = localToWorldFromEntity[entity].Position;
          var preyPosition = localToWorldFromEntity[preyEntity].Position;

          var distance = math.distance(predatorPosition, preyPosition);

          if (distance <= predator.attackDistance)
          {
            // The predator is close enough to kill the chased prey
            EcsUtils.Kill(ref buffer, entityInQueryIndex, preyEntity, time.ElapsedTime, CauseOfDeath.Eaten);
            StopChaseAndRoam(ref buffer, entityInQueryIndex, entity);

            if (HasComponent<Hunger>(entity))
            {
              // Reduce the hunger of the predator that consumed the prey
              var hunger = GetComponent<Hunger>(entity);
              hunger.value -= 50;
              hunger.value = math.clamp(hunger.value, 0, hunger.max);
              buffer.SetComponent(entityInQueryIndex, entity, hunger);
            }
          }
          else if (distance > predator.maxChaseDistance)
          {
            // The predator is too far away from the chased animal, so abort the chase
            StopChaseAndRoam(ref buffer, entityInQueryIndex, entity);
          }
        })
        .WithName("PredatorChaseCollisionSystemJob")
        .WithBurst()
        .ScheduleParallel();

      _barrier.AddJobHandleForProducer(Dependency);
    }

    [BurstCompile]
    private static void StopChaseAndRoam(ref ParallelBuffer buffer, int index, in Entity entity)
    {
      buffer.RemoveComponent<NavFollow>(index, entity);
      buffer.AddComponent<Roaming>(index, entity);
    }
  }
}