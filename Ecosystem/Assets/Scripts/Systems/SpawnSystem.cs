using Ecosystem.Util;
using Reese.Nav;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Ecosystem.Systems
{
  [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
  [UpdateAfter(typeof(NavSurfaceSystem))]
  [UpdateAfter(typeof(NavDestinationSystem))]
  [UpdateAfter(typeof(RandomNavigationSystem))]
  public sealed class SpawnSystem : SystemBase
  {
    private bool _keyDown;
    private bool _hasSpawned;

    private static World InjectionWorld => World.DefaultGameObjectInjectionWorld;

    private static FactorySystem FactorySystem => InjectionWorld.GetOrCreateSystem<FactorySystem>();
    private static TrackingSystem TrackingSystem => InjectionWorld.GetOrCreateSystem<TrackingSystem>();

    protected override void OnUpdate()
    {
      if (!_hasSpawned)
      {
        var terrain = Terrain.activeTerrain;
        for (var i = 0; i < 1000; ++i)
        {
          if (Terrains.RandomWalkablePosition(terrain, out var position))
          {
            if (Random.value > 0.5)
            {
              FactorySystem.MakeRabbit(position);
            }
            else
            {
              FactorySystem.MakeWolf(position);
            }
          }
        }

        _hasSpawned = true;
      }

      switch (Input.GetKey(KeyCode.L))
      {
        case true when !_keyDown:
          var terrain = Terrain.activeTerrain;
          if (Terrains.RandomWalkablePosition(terrain, out var position))
          {
            FactorySystem.MakeRabbit(position);
          }

          _keyDown = true;
          break;

        case false when _keyDown:
          _keyDown = false;
          break;
      }
    }
  }
}