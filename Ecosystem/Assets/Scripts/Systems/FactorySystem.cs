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

      EntityManager.SetComponentData(rabbit, new Translation
      {
              Value = position
      });
    }
  }
}