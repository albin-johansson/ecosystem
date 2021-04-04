using Ecosystem.Components;
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

      var consumedFromEntity = GetComponentDataFromEntity<Consumed>(true);
      var localToWorldFromEntity = GetComponentDataFromEntity<LocalToWorld>(true);

      var time = Time;

      Entities.WithAll<Prey, MovingTowardsFood, LocalToWorld>()
              .WithNone<Dead>()
              .WithReadOnly(consumedFromEntity)
              .WithReadOnly(localToWorldFromEntity)
              .ForEach((Entity entity,
                      int entityInQueryIndex,
                      in Prey prey,
                      in MovingTowardsFood movingTowardsFood,
                      in LocalToWorld localToWorld) =>
              {
                var foodEntity = movingTowardsFood.Food;

                if (consumedFromEntity.HasComponent(foodEntity))
                {
                  // The food item got consumed before we reached it, so return to roaming
                  buffer.RemoveComponent<MovingTowardsFood>(entityInQueryIndex, entity);
                  buffer.AddComponent<Roaming>(entityInQueryIndex, entity);
                  return;
                }

                var foodPosition = localToWorldFromEntity[foodEntity].Position;
                if (math.distance(localToWorld.Position, foodPosition) < prey.consumptionDistance)
                {
                  buffer.AddComponent(entityInQueryIndex, foodEntity, new Consumed
                  {
                          when = time.ElapsedTime
                  });

                  buffer.RemoveComponent<MovingTowardsFood>(entityInQueryIndex, entity);
                  buffer.AddComponent<Roaming>(entityInQueryIndex, entity);

                  if (HasComponent<Hunger>(entity))
                  {
                    // Reduce the hunger of the prey that consumed the food
                    var hunger = GetComponent<Hunger>(entity);
                    hunger.value -= 50;
                    hunger.value = math.clamp(hunger.value, 0, hunger.max);
                    buffer.SetComponent(entityInQueryIndex, entity, hunger);
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