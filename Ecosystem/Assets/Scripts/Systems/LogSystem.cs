using System;
using System.Collections.Generic;
using Ecosystem.Components;
using Ecosystem.ECS;
using Ecosystem.Logging;
using JetBrains.Annotations;
using Unity.Entities;

namespace Ecosystem.Systems
{
  public sealed class LogSystem : AbstractSystem
  {
    [Serializable]
    private struct SimulationEvent
    {
      [UsedImplicitly] public double time;
      [UsedImplicitly] public int deathIndex;
    }

    [Serializable]
    private struct DeathEvent
    {
      [UsedImplicitly] public CauseOfDeath cause;
    }

    [Serializable]
    private class JsonData
    {
      [UsedImplicitly] public double duration;

      [UsedImplicitly] public int initialRabbitCount;
      [UsedImplicitly] public int initialWolfCount;

      [UsedImplicitly] public int rabbitCount;
      [UsedImplicitly] public int wolfCount;

      public List<SimulationEvent> events = new List<SimulationEvent>();
      public List<DeathEvent> deaths = new List<DeathEvent>();
    }

    protected override void OnDestroy()
    {
      base.OnDestroy();

      var initialData = EntityManager.GetSingleton<InitialSimulationData>();
      var data = new JsonData
      {
              duration = Time.ElapsedTime,
              initialRabbitCount = initialData.initialRabbitCount,
              initialWolfCount = initialData.initialWolfCount
      };

      ProcessDeaths(data);
      ProcessSurvivors(data);

      // Sort the event array according to the event times, which makes life easier in the visualisation scripts
      data.events.Sort((lhs, rhs) => lhs.time.CompareTo(rhs.time));

      LogFileWriter.SaveEcs(data);
    }

    private void ProcessDeaths(JsonData data)
    {
      Entities.WithAll<Dead>()
              .ForEach((Entity entity, in Dead dead) =>
              {
                data.events.Add(new SimulationEvent
                {
                        time = dead.when,
                        deathIndex = data.deaths.Count
                });

                data.deaths.Add(new DeathEvent
                {
                        cause = dead.cause
                });
              })
              .WithName("LogSystemProcessDeathsJob")
              .WithoutBurst()
              .Run();
    }

    private void ProcessSurvivors(JsonData data)
    {
      Entities.WithAny<Predator, Prey>()
              .WithNone<Dead>()
              .ForEach((Entity entity) =>
              {
                if (HasComponent<Rabbit>(entity))
                {
                  ++data.rabbitCount;
                }
                else if (HasComponent<Wolf>(entity))
                {
                  ++data.wolfCount;
                }
              })
              .WithName("LogSystemCountAnimalsJob")
              .WithoutBurst()
              .Run();
    }
  }
}