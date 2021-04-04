using Ecosystem.Components;
using Ecosystem.ECS;
using Ecosystem.Logging;
using Unity.Entities;

namespace Ecosystem.Systems
{
  [UpdateInGroup(typeof(SimulationSystemGroup))]
  public sealed class ThirstSystem : AbstractSystem
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

      Entities.WithAll<Thirst>()
              .WithNone<Dead>()
              .ForEach((Entity entity, int entityInQueryIndex, ref Thirst thirst) =>
              {
                thirst.value += thirst.rate * time.DeltaTime;
                if (thirst.value > thirst.max)
                {
                  EcsUtils.Kill(ref buffer, entityInQueryIndex, entity, time.ElapsedTime, CauseOfDeath.Dehydration);
                }
              })
              .WithName("ThirstSystemJob")
              .WithBurst()
              .ScheduleParallel();

      _barrier.AddJobHandleForProducer(Dependency);
    }
  }
}