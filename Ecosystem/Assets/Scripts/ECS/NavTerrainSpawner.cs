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

    private Entity _prefab;

    private static EntityManager EntityManager => World.DefaultGameObjectInjectionWorld.EntityManager;

    private void Start()
    {
      var query = EntityManager.CreateEntityQuery(typeof(CylinderPrefab));
      _prefab = query.GetSingleton<CylinderPrefab>().Value;
    }

    private void Update()
    {
      if (Input.GetKeyUp(KeyCode.L))
      {
        Spawn();
      }
    }

    private void Spawn()
    {
      var entities = new NativeArray<Entity>(amount, Allocator.Temp);
      EntityManager.Instantiate(_prefab, entities);

      var terrain = Terrain.activeTerrain;

      foreach (var entity in entities)
      {
        AddNavAgent(entity, transform.position);

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
  }
}