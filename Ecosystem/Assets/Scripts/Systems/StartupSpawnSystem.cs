using Ecosystem.ECS;
using Ecosystem.Util;
using Reese.Nav;
using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ecosystem.Systems
{
  [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
  [UpdateAfter(typeof(NavSurfaceSystem))]
  [UpdateAfter(typeof(NavDestinationSystem))]
  public sealed class StartupSpawnSystem : SystemBase
  {
    public static int InitialRabbitCount;
    public static int InitialWolfCount;

    private FactorySystem _factorySystem;
    private bool _hasSpawned;

    private void OnSceneChanged(Scene current, Scene next)
    {
      Enabled = EcsUtils.IsEcsCapable(next);
      if (Enabled)
      {
        _factorySystem = World.GetOrCreateSystem<FactorySystem>();
      }
    }

    protected override void OnCreate()
    {
      base.OnCreate();
      SceneManager.activeSceneChanged += OnSceneChanged;
    }

    protected override void OnUpdate()
    {
      if (!_hasSpawned)
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
    }
  }
}