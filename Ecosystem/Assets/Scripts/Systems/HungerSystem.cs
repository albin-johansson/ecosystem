using Ecosystem.Components;
using Ecosystem.ECS;
using Ecosystem.Logging;
using Unity.Entities;

namespace Ecosystem.Systems
{
  [UpdateInGroup(typeof(SimulationSystemGroup))]
  public sealed class HungerSystem : AbstractSystem
  {
    private EntityCommandBufferSystem _barrier;

    protected override void Initialize()
    {
      _barrier = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
      var buffer = _barrier.CreateCommandBuffer().AsParallelWriter();
      var time = Time;

      Entities.WithAll<Hunger>()
              .ForEach((Entity entity, int entityInQueryIndex, ref Hunger hunger) =>
              {
                hunger.value += hunger.rate * time.DeltaTime;
                if (hunger.value > hunger.max)
                {
                  EcsUtils.Kill(ref buffer, entityInQueryIndex, entity, time.ElapsedTime, CauseOfDeath.Starvation);
                }
              })
              .WithName("HungerSystemJob")
              .WithBurst()
              .ScheduleParallel();

      _barrier.AddJobHandleForProducer(Dependency);
    }
  }
}