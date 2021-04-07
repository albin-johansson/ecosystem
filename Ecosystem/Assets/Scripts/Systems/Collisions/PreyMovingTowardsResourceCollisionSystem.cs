using Ecosystem.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using ParallelBuffer = Unity.Entities.EntityCommandBuffer.ParallelWriter;

namespace Ecosystem.Systems.Collisions
{
  [UpdateInGroup(typeof(CollisionSystemGroup))]
  [UpdateAfter(typeof(RoamingPreyCollisionSystem))]
  public sealed class PreyMovingTowardsResourceCollisionSystem : AbstractSystem
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
      var waterFromEntity = GetComponentDataFromEntity<Water>(true);
      var carrotFromEntity = GetComponentDataFromEntity<Carrot>(true);
      var time = Time;

      Entities
        .WithAll<Prey, LocalToWorld, MovingTowardsResource>()
        .WithAll<Hunger, Thirst>()
        .WithNone<Dead>()
        .WithReadOnly(localToWorldFromEntity)
        .WithReadOnly(waterFromEntity)
        .WithReadOnly(carrotFromEntity)
        .ForEach((Entity entity,
          int entityInQueryIndex,
          in Prey prey,
          in LocalToWorld localToWorld,
          in MovingTowardsResource movingTowardsResource,
          in Hunger hunger,
          in Thirst thirst) =>
        {
          var resourceEntity = movingTowardsResource.Resource;
          var resourcePosition = localToWorldFromEntity[resourceEntity].Position;

          if (math.distance(localToWorld.Position, resourcePosition) < prey.consumptionDistance)
          {
            if (carrotFromEntity.HasComponent(resourceEntity))
            {
              buffer.AddComponent(entityInQueryIndex, buffer.CreateEntity(entityInQueryIndex), new Consumption
              {
                when = time.ElapsedTime
              });
              ConsumeFood(ref buffer, entityInQueryIndex, entity, hunger);
            }
            else if (waterFromEntity.HasComponent(resourceEntity))
            {
              ConsumeWater(ref buffer, entityInQueryIndex, entity, thirst);
            }

            buffer.RemoveComponent<MovingTowardsResource>(entityInQueryIndex, entity);
            buffer.AddComponent<Roaming>(entityInQueryIndex, entity);
          }
        })
        .WithName("PreyMovingTowardsResourceCollisionSystemJob")
        .WithBurst()
        .ScheduleParallel();

      _barrier.AddJobHandleForProducer(Dependency);
    }

    [BurstCompile]
    private static void ConsumeFood(ref ParallelBuffer buffer, int index, in Entity entity, in Hunger hunger)
    {
      var newHunger = hunger;
      newHunger.value -= 0.75f * newHunger.max;
      newHunger.value = math.clamp(newHunger.value, 0, newHunger.max);
      buffer.SetComponent(index, entity, newHunger);
    }

    [BurstCompile]
    private static void ConsumeWater(ref ParallelBuffer buffer, int index, in Entity entity, in Thirst thirst)
    {
      var newThirst = thirst;
      newThirst.value -= 0.75f * newThirst.max;
      newThirst.value = math.clamp(newThirst.value, 0, newThirst.max);
      buffer.SetComponent(index, entity, newThirst);
    }
  }
}