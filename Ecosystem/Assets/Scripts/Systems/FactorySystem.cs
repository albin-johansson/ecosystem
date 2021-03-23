using Ecosystem.Components;
using Ecosystem.ECS;
using Ecosystem.ECS.Authoring;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Ecosystem.Systems
{
  public sealed class FactorySystem : SystemBase
  {
    private Entity _rabbitPrefab;
    private Entity _wolfPrefab;

    protected override void OnUpdate()
    {
      // Do nothing
    }

    public void MakeRabbit(float3 position)
    {
      if (_rabbitPrefab == Entity.Null)
      {
        _rabbitPrefab = EntityManager.LoadPrefab<RabbitPrefab>().Value;
      }

      var rabbit = EntityManager.Instantiate(_rabbitPrefab);

      EntityManager.AddComponent<Roaming>(rabbit);
      EntityManager.SetComponentData(rabbit, new Translation
      {
              Value = position
      });
    }

    public void MakeWolf(float3 position)
    {
      if (_wolfPrefab == Entity.Null)
      {
        _wolfPrefab = EntityManager.LoadPrefab<WolfPrefab>().Value;
      }
      
      var wolf = EntityManager.Instantiate(_wolfPrefab);
      EntityManager.AddComponent<Idle>(wolf);
      
      EntityManager.SetComponentData(wolf, new Translation
      {
              Value = position
      });
    }
  }
}