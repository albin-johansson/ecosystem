using Ecosystem.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using ParallelBuffer = Unity.Entities.EntityCommandBuffer.ParallelWriter;

namespace Ecosystem.Systems.Collisions
{
  /// <summary>
  ///   This system is responsible for checking that animals that are moving towards food and water resources consume
  ///   the resources when they close enough. 
  /// </summary>
  /// <remarks>
  ///   This system doesn't make use of colliders, instead the raw distance between entities is used. This is because of
  ///   an apparent limitation with the third-party collision library, which prevents multiple overlapping colliders on
  ///   a single animal (or rather, it is *extremely* slow).
  /// </remarks>
  [UpdateInGroup(typeof(CollisionSystemGroup))]
  [UpdateAfter(typeof(RoamingPreyCollisionSystem))]
  public sealed class AnimalMovingTowardsResourceCollisionSystem : AbstractSystem
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
        .WithAll<MovingTowardsResource, Animal, LocalToWorld>()
        .WithAll<Hunger, Thirst>()
        .WithNone<Dead>()
        .WithReadOnly(localToWorldFromEntity)
        .WithReadOnly(waterFromEntity)
        .WithReadOnly(carrotFromEntity)
        .ForEach((Entity entity,
          int entityInQueryIndex,
          in MovingTowardsResource movingTowardsResource,
          in Animal animal,
          in LocalToWorld localToWorld,
          in Hunger hunger,
          in Thirst thirst) =>
        {
          var resourceEntity = movingTowardsResource.Resource;
          var resourcePosition = localToWorldFromEntity[resourceEntity].Position;

          if (math.distance(localToWorld.Position, resourcePosition) <= animal.resourceConsumptionDistance)
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
        .WithName("AnimalMovingTowardsResourceCollisionSystemJob")
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