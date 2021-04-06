using Ecosystem.Components;
using Ecosystem.ECS;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Ecosystem.Systems.Collisions
{
  [UpdateInGroup(typeof(CollisionSystemGroup))]
  [UpdateAfter(typeof(RoamingPreyCollisionSystem))]
  public sealed class PreyMovingTowardsFoodCollisionSystem : AbstractSystem
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
      var time = Time;

      Entities
        .WithAll<Prey, MovingTowardsFood, LocalToWorld>()
        .WithNone<Dead>()
        .WithReadOnly(localToWorldFromEntity)
        .ForEach((Entity entity,
          int entityInQueryIndex,
          in Prey prey,
          in MovingTowardsFood movingTowardsFood,
          in LocalToWorld localToWorld) =>
        {
          var foodEntity = movingTowardsFood.Food;

          var foodPosition = localToWorldFromEntity[foodEntity].Position;
          if (math.distance(localToWorld.Position, foodPosition) < prey.consumptionDistance)
          {
            PreyUtils.RegisterConsumption(ref buffer, entityInQueryIndex, time.ElapsedTime);
            PreyUtils.StopGoingForFoodAndRoam(ref buffer, entityInQueryIndex, entity);

            if (HasComponent<Hunger>(entity))
            {
              var hunger = GetComponent<Hunger>(entity);
              PreyUtils.ConsumeFood(ref buffer, entityInQueryIndex, entity, hunger);
            }
          }
        })
        .WithName("PreyMovingTowardsFoodCollisionSystemJob")
        .WithBurst()
        .ScheduleParallel();

      _barrier.AddJobHandleForProducer(Dependency);
    }
  }
}