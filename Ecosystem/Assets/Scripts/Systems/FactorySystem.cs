using Ecosystem.ECS;
using Reese.Spatial;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

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

      var rabbit = EntityManager.Instantiate(_rabbitPrefab);

      EntityManager.AddComponentData(rabbit, new SpatialTrigger
      {
              Filter = CollisionFilter.Default
      });

      EntityManager.SetComponentData(rabbit, new Translation
      {
              Value = position
      });

      return rabbit;
    }

    private T LoadPrefab<T>() where T : struct, IComponentData
    {
      return EntityManager.CreateEntityQuery(typeof(T)).GetSingleton<T>();
    }
  }
}