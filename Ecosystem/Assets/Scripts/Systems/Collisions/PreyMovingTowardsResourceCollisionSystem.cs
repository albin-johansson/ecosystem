using Ecosystem.Components;
using Ecosystem.ECS;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

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
              PreyUtils.RegisterFoodConsumption(ref buffer, entityInQueryIndex, time.ElapsedTime);
              PreyUtils.ConsumeFood(ref buffer, entityInQueryIndex, entity, hunger);
            }
            else if (waterFromEntity.HasComponent(resourceEntity))
            {
              PreyUtils.ConsumeWater(ref buffer, entityInQueryIndex, entity, thirst);
            }

            PreyUtils.StopGoingForResourceAndRoam(ref buffer, entityInQueryIndex, entity);
          }
        })
        .WithName("PreyMovingTowardsResourceCollisionSystemJob")
        .WithBurst()
        .ScheduleParallel();

      _barrier.AddJobHandleForProducer(Dependency);
    }
  }
}