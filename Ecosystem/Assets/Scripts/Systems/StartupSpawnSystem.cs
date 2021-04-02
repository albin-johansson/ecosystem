using Ecosystem.Components;
using Ecosystem.ECS;
using Ecosystem.Util;
using Reese.Nav;
using Unity.Entities;
using UnityEngine;

namespace Ecosystem.Systems
{
  [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
  [UpdateAfter(typeof(NavSurfaceSystem))]
  [UpdateAfter(typeof(NavDestinationSystem))]
  public sealed class StartupSpawnSystem : AbstractSystem
  {
    public static int InitialRabbitCount;
    public static int InitialWolfCount;

    private FactorySystem _factorySystem;
    private bool _hasSpawned;
    private int _count; // We use this to wait a frame before we spawn entities, to let prefabs instantiate

    protected override void Initialize()
    {
      _factorySystem = World.GetOrCreateSystem<FactorySystem>();
      EntityManager.AddSingleton(new InitialSimulationData
      {
              initialRabbitCount = InitialRabbitCount,
              initialWolfCount = InitialWolfCount
      });
    }

    protected override void OnUpdate()
    {
      if (_count > 1 && !_hasSpawned)
      {
        var terrain = Terrain.activeTerrain;

        for (var i = 0; i < InitialRabbitCount; ++i)
        {
          if (Terrains.RandomWalkablePosition(terrain, out var position))
          {
            _factorySystem.MakeRabbit(position);
          }
        }

        for (var i = 0; i < InitialWolfCount; ++i)
        {
          if (Terrains.RandomWalkablePosition(terrain, out var position))
          {
            _factorySystem.MakeWolf(position);
          }
        }

        _hasSpawned = true;
      }

      ++_count;
    }
  }
}