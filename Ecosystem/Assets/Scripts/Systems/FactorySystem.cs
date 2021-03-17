using Ecosystem.Components;
using Ecosystem.ECS;
using Reese.Nav;
using Reese.Spatial;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using Random = UnityEngine.Random;

namespace Ecosystem.Systems
{
  public sealed class FactorySystem : SystemBase
  {
    private Entity _rabbitPrefab;

    protected override void OnUpdate()
    {
      // Do nothing
    }

    public Entity MakeRabbit(float3 position)
    {
      if (_rabbitPrefab == Entity.Null)
      {
        _rabbitPrefab = LoadPrefab<RabbitPrefab>().Value;
      }

      var entity = EntityManager.Instantiate(_rabbitPrefab);

      EntityManager.AddComponent<Rabbit>(entity);

      AddNavAgent(entity, position);
      
      EntityManager.AddComponent<PhysicsCollider>(entity);
      EntityManager.AddComponent<SpatialActivator>(entity);
      
      EntityManager.AddComponent(entity, typeof(SpatialTag));
      var buffer = EntityManager.AddBuffer<SpatialTag>(entity);
      buffer.Add("Rabbit");

      return entity;
    }

    private T LoadPrefab<T>() where T : struct, IComponentData
    {
      return EntityManager.CreateEntityQuery(typeof(T)).GetSingleton<T>();
    }

    private void AddNavAgent(Entity entity, float3 position)
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