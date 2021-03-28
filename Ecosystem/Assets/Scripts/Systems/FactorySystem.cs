using Ecosystem.Components;
using Ecosystem.ECS;
using Ecosystem.ECS.Authoring;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Ecosystem.Systems
{
  public sealed class FactorySystem : AbstractSystem
  {
    private Entity _rabbitPrefab = Entity.Null;
    private Entity _wolfPrefab = Entity.Null;

    protected override void OnUpdate()
    {
      // Do nothing
    }

    private void SetPosition(Entity entity, float3 position)
    {
      EntityManager.SetComponentData(entity, new Translation
      {
              Value = position
      });
    }

    public void MakeRabbit(float3 position)
    {
      Debug.Assert(Enabled);

      if (_rabbitPrefab == Entity.Null)
      {
        _rabbitPrefab = EntityManager.LoadPrefab<RabbitPrefab>().Value;
      }

      var rabbit = EntityManager.Instantiate(_rabbitPrefab);

      EntityManager.AddComponent<Roaming>(rabbit);

      SetPosition(rabbit, position);
    }

    public void MakeWolf(float3 position)
    {
      Debug.Assert(Enabled);

      if (_wolfPrefab == Entity.Null)
      {
        _wolfPrefab = EntityManager.LoadPrefab<WolfPrefab>().Value;
      }

      var wolf = EntityManager.Instantiate(_wolfPrefab);

      EntityManager.AddComponent<Roaming>(wolf);

      SetPosition(wolf, position);
    }
  }
}