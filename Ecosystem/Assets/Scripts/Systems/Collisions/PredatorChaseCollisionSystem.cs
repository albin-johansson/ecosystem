using Ecosystem.Components;
using Ecosystem.Logging;
using Reese.Nav;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Ecosystem.Systems.Collisions
{
  /// <summary>
  ///   This system is responsible for checking for predators that get within striking distance of the prey that they
  ///   are chasing. When a predator is within striking distance, the prey simply dies in the current implementation,
  ///   and the predator subsequently returns to roaming.
  /// </summary>
  /// <remarks>
  ///   This system doesn't make use of colliders, instead the raw distance between entities is used. This is because of
  ///   an apparent limitation with the third-party collision library, which prevents multiple overlapping colliders on
  ///   a single animal (or rather, it is *extremely* slow).
  /// </remarks>
  [UpdateInGroup(typeof(CollisionSystemGroup))]
  [UpdateAfter(typeof(PredatorCollisionSystem))]
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
      var preyFromEntity = GetComponentDataFromEntity<Prey>(true);
      var deadFromEntity = GetComponentDataFromEntity<Dead>(true);

      Entities.WithAll<Predator, NavFollow, LocalToWorld>()
              .WithNone<Dead>()
              .WithReadOnly(localToWorldFromEntity)
              .WithReadOnly(preyFromEntity)
              .WithReadOnly(deadFromEntity)
              .ForEach((Entity predatorEntity, int entityInQueryIndex, in Predator predator, in NavFollow follow) =>
              {
                var preyEntity = follow.Target;

                if (deadFromEntity.HasComponent(preyEntity))
                {
                  // If the target prey died during our chase, return to roaming
                  buffer.RemoveComponent<NavFollow>(entityInQueryIndex, predatorEntity);
                  buffer.RemoveComponent<NavDestination>(entityInQueryIndex, predatorEntity);
                  buffer.RemoveComponent<NavPlanning>(entityInQueryIndex, predatorEntity);
                  buffer.RemoveComponent<NavProblem>(entityInQueryIndex, predatorEntity);
                  buffer.AddComponent<Roaming>(entityInQueryIndex, predatorEntity);
                  return;
                }

                if (!localToWorldFromEntity.HasComponent(preyEntity) ||
                    !preyFromEntity.HasComponent(preyEntity))
                {
                  return;
                }

                var predatorPosition = localToWorldFromEntity[predatorEntity].Position;
                var preyPosition = localToWorldFromEntity[preyEntity].Position;

                if (math.distance(predatorPosition, preyPosition) <= predator.attackDistance)
                {
                  // Mark the pursued animal as dead
                  buffer.AddComponent(preyEntity.Index, preyEntity, new Dead
                  {
                          cause = CauseOfDeath.Eaten
                  });

                  // Make sure that the dead animal stops
                  buffer.RemoveComponent<NavAgent>(preyEntity.Index, preyEntity);
                  buffer.RemoveComponent<NavDestination>(preyEntity.Index, preyEntity);
                  buffer.RemoveComponent<NavPlanning>(preyEntity.Index, preyEntity);

                  // Make the predator stop following the consumed animal
                  buffer.RemoveComponent<NavFollow>(entityInQueryIndex, predatorEntity);
                  buffer.RemoveComponent<NavDestination>(entityInQueryIndex, predatorEntity);
                  buffer.RemoveComponent<NavPlanning>(entityInQueryIndex, predatorEntity);
                  buffer.RemoveComponent<NavProblem>(entityInQueryIndex, predatorEntity);

                  // Make the predator return to roaming
                  buffer.AddComponent<Roaming>(entityInQueryIndex, predatorEntity);
                }
              })
              .WithName("PredatorChaseSystemJob")
              .WithBurst()
              .ScheduleParallel();

      _barrier.AddJobHandleForProducer(Dependency);
    }
  }
}