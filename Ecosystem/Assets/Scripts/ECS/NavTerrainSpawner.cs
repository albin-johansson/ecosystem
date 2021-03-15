using Ecosystem.Authoring;
using Ecosystem.Util;
using Reese.Nav;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Ecosystem.ECS
{
  public sealed class NavTerrainSpawner : MonoBehaviour
  {
    [SerializeField] private int amount = 100;
    [SerializeField] private float3 spawnOffset = new float3(0, 1, 0);
    [SerializeField] private float3 destination;

    private Entity _prefab;

    private static EntityManager EntityManager => World.DefaultGameObjectInjectionWorld.EntityManager;

    private void Start()
    {
      _prefab = EntityManager.CreateEntityQuery(typeof(CylinderPrefab)).GetSingleton<CylinderPrefab>().Value;
    }

    private void Update()
    {
      if (Input.GetKeyUp(KeyCode.L))
      {
        Spawn();
      }
    }

    private static void AddNavAgent(Entity entity, float3 position)
    {
      EntityManager.AddComponentData(entity, new NavAgent
      {
              TranslationSpeed = 20 + 5 * Random.value,
              RotationSpeed = 0.3f,
              TypeID = NavUtil.GetAgentType(NavConstants.HUMANOID),
              Offset = new float3(0, 1, 0)
      });

      EntityManager.AddComponentData(entity, new Translation
      {
              Value = position
      });

      EntityManager.AddComponent<LocalToWorld>(entity);
      EntityManager.AddComponent<Parent>(entity);
      EntityManager.AddComponent<LocalToParent>(entity);
      EntityManager.AddComponent<NavNeedsSurface>(entity);
      EntityManager.AddComponent<NavTerrainCapable>(entity);
    }

    private void Spawn()
    {
      var entities = new NativeArray<Entity>(amount, Allocator.Temp);

      EntityManager.Instantiate(_prefab, entities);

      foreach (var entity in entities)
      {
        AddNavAgent(entity, transform.position);

        // EntityManager.AddComponentData(entity, new NavDestination
        // {
        // Teleport = false,
        // Tolerance = 10f,
        // CustomLerp = false,
        // WorldPoint = destination
        // });

        var terrain = Terrain.activeTerrain;
        if (Terrains.RandomWalkablePosition(terrain, out var position))
        {
          EntityManager.AddComponentData(entity, new NavDestination
          {
                  Teleport = false,
                  Tolerance = 10f,
                  CustomLerp = false,
                  WorldPoint = position
          });
        }
      }

      entities.Dispose();
    }
  }
}