using Ecosystem.Systems;
using Ecosystem.Util;
using Unity.Entities;
using UnityEngine;

namespace Ecosystem.ECS
{
  public sealed class EcsDemoSpawner : MonoBehaviour
  {
    private static World InjectionWorld => World.DefaultGameObjectInjectionWorld;
    private static FactorySystem FactorySystem => InjectionWorld.GetOrCreateSystem<FactorySystem>();
    private static TrackingSystem TrackingSystem => InjectionWorld.GetOrCreateSystem<TrackingSystem>();

    private void Update()
    {
      if (Input.GetKeyUp(KeyCode.L))
      {
        Spawn();
      }
    }

    private void Spawn()
    {
      var entity = FactorySystem.MakeRabbit(transform.position);

      var terrain = Terrain.activeTerrain;
      if (Terrains.RandomWalkablePosition(terrain, out var position))
      {
        TrackingSystem.SetDestination(entity, position);
      }
    }
  }
}