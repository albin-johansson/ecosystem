using System;
using System.Collections.Generic;
using Ecosystem.Components;
using Ecosystem.ECS;
using Ecosystem.Logging;
using Ecosystem.UI;
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
      [UsedImplicitly] public string type;
      [UsedImplicitly] public string tag;
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

      [UsedImplicitly] public int initialAliveCount;
      [UsedImplicitly] public int initialAlivePreyCount;
      [UsedImplicitly] public int initialAlivePredatorCount;
      [UsedImplicitly] public int initialAliveRabbitsCount;
      [UsedImplicitly] public int initialAliveDeerCount;
      [UsedImplicitly] public int initialAliveWolvesCount;
      [UsedImplicitly] public int initialAliveBearsCount;
      [UsedImplicitly] public int initialFoodCount;

      [UsedImplicitly] public int aliveCount;
      [UsedImplicitly] public int deadCount;
      [UsedImplicitly] public int preyConsumedCount;
      [UsedImplicitly] public int birthCount;
      [UsedImplicitly] public int matingCount;

      [UsedImplicitly] public float minimumFps;
      [UsedImplicitly] public float maximumFps;
      [UsedImplicitly] public float averageFps;

      public List<SimulationEvent> events = new List<SimulationEvent>();
      public List<DeathEvent> deaths = new List<DeathEvent>();
    }

    private double _startTime;

    protected override void OnStartRunning()
    {
      base.OnStartRunning();
      _startTime = Time.ElapsedTime;
    }

    protected override void OnDestroy()
    {
      base.OnDestroy();
      if (!Enabled)
      {
        return;
      }

      var initialData = EntityManager.GetSingleton<InitialSimulationData>();
      var data = new JsonData
      {
        duration = (Time.ElapsedTime - _startTime) * 1000.0,

        initialAliveCount = initialData.initialRabbitCount + initialData.initialDeerCount +
                            initialData.initialWolfCount + initialData.initialBearCount,
        initialAlivePreyCount = initialData.initialRabbitCount + initialData.initialDeerCount,
        initialAlivePredatorCount = initialData.initialWolfCount + initialData.initialBearCount,
        initialAliveRabbitsCount = initialData.initialRabbitCount,
        initialAliveDeerCount = initialData.initialDeerCount,
        initialAliveWolvesCount = initialData.initialWolfCount,
        initialAliveBearsCount = initialData.initialBearCount,
        initialFoodCount = initialData.initialCarrotCount,

        minimumFps = FPSCounter.GetMinimumFPS(),
        maximumFps = FPSCounter.GetMaximumFPS(),
        averageFps = FPSCounter.GetAverageFPS()
      };

      ProcessDeaths(data);
      ProcessFoodConsumption(data);
      ProcessSurvivors(data);

      // Sort the event array according to the event times, which makes life easier in the visualisation scripts
      data.events.Sort((lhs, rhs) => lhs.time.CompareTo(rhs.time));

      LogFileWriter.SaveEcs(data);
    }

    private void ProcessDeaths(JsonData data)
    {
      Entities
        .WithAll<Dead>()
        .ForEach((Entity entity, in Dead dead) =>
        {
          var tag = "";
          if (HasComponent<Rabbit>(entity))
          {
            tag = "Rabbit";
          }
          else if (HasComponent<Deer>(entity))
          {
            tag = "Deer";
          }
          else if (HasComponent<Wolf>(entity))
          {
            tag = "Wolf";
          }
          else if (HasComponent<Bear>(entity))
          {
            tag = "Bear";
          }

          data.events.Add(new SimulationEvent
          {
            time = dead.when * 1000,
            type = "death",
            tag = tag,
            deathIndex = data.deaths.Count
          });

          data.deaths.Add(new DeathEvent
          {
            cause = dead.cause
          });

          ++data.deadCount;

          if (dead.cause == CauseOfDeath.Eaten)
          {
            ++data.preyConsumedCount;
          }
        })
        .WithName("LogSystemProcessDeathsJob")
        .WithoutBurst()
        .Run();
    }

    private void ProcessFoodConsumption(JsonData data)
    {
      Entities
        .WithAll<Consumption>()
        .ForEach((Entity entity, in Consumption consumed) =>
        {
          data.events.Add(new SimulationEvent
          {
            time = consumed.when * 1000,
            type = "consumption",
            tag = "Carrot",
            deathIndex = -1
          });
        })
        .WithName("LogSystemProcessFoodConsumptionJob")
        .WithoutBurst()
        .Run();
    }

    private void ProcessSurvivors(JsonData data)
    {
      Entities
        .WithAny<Predator, Prey>()
        .WithNone<Dead>()
        .ForEach((Entity entity) => ++data.aliveCount)
        .WithName("LogSystemCountAnimalsJob")
        .WithoutBurst()
        .Run();
    }
  }
}