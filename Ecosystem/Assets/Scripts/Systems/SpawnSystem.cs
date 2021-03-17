using Ecosystem.Util;
using Reese.Nav;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Ecosystem.Systems
{
  [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
  [UpdateAfter(typeof(NavSurfaceSystem))] 
  [UpdateAfter(typeof(NavDestinationSystem))]
  public sealed class SpawnSystem : SystemBase
  {
    private bool _keyDown;

    private static World InjectionWorld => World.DefaultGameObjectInjectionWorld;

    private static FactorySystem FactorySystem => InjectionWorld.GetOrCreateSystem<FactorySystem>();
    private static TrackingSystem TrackingSystem => InjectionWorld.GetOrCreateSystem<TrackingSystem>();

    protected override void OnUpdate()
    {
      switch (Input.GetKey(KeyCode.L))
      {
        case true when !_keyDown:
          var entity = FactorySystem.MakeRabbit(new float3(645, 0, 475));

          var terrain = Terrain.activeTerrain;
          if (Terrains.RandomWalkablePosition(terrain, out var position))
          {
            TrackingSystem.SetDestination(entity, position);
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