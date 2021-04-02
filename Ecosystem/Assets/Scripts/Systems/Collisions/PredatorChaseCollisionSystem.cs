using Ecosystem.Components;
using Ecosystem.ECS;
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
  ///   and the predator subsequently returns to roaming. Furthermore, if a predator is too far away from the chased
  ///   prey, the case is aborted.
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
      var deadFromEntity = GetComponentDataFromEntity<Dead>(true);
      var time = Time;

      Entities.WithAll<Predator, NavFollow, LocalToWorld>()
              .WithNone<Dead>()
              .WithReadOnly(localToWorldFromEntity)
              .WithReadOnly(deadFromEntity)
              .ForEach((Entity entity,
                      int entityInQueryIndex,
                      in Predator predator,
                      in NavFollow follow,
                      in LocalToWorld localToWorld) =>
              {
                var preyEntity = follow.Target;

                if (deadFromEntity.HasComponent(preyEntity))
                {
                  // The chased animal died from something else, so abort the chase
                  Nav.StopChaseAndRoam(ref buffer, entityInQueryIndex, entity);
                  return;
                }

                var predatorPosition = localToWorld.Position;
                var preyPosition = localToWorldFromEntity[preyEntity].Position;

                var distance = math.distance(predatorPosition, preyPosition);

                if (distance <= predator.attackDistance)
                {
                  // The predator is close enough to kill the chased prey
                  EcsUtils.Kill(ref buffer, entityInQueryIndex, preyEntity, time.ElapsedTime, CauseOfDeath.Eaten);
                  Nav.StopChaseAndRoam(ref buffer, entityInQueryIndex, entity);
                }
                else if (distance > predator.maxChaseDistance)
                {
                  // The predator is too far away from the chased animal, so abort the chase
                  Nav.StopChaseAndRoam(ref buffer, entityInQueryIndex, entity);
                }
              })
              .WithName("PredatorChaseCollisionSystemJob")
              .WithBurst()
              .ScheduleParallel();

      _barrier.AddJobHandleForProducer(Dependency);
    }
  }
}