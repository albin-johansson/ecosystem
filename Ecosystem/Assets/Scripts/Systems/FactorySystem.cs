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
    private Entity _carrotPrefab = Entity.Null;

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

    private void MakeAnimal(Entity entity, float3 position)
    {
      EntityManager.AddComponent<Roaming>(entity);
      SetPosition(entity, position);
    }

    public void MakeRabbit(float3 position)
    {
      Debug.Assert(Enabled);

      if (_rabbitPrefab == Entity.Null)
      {
        _rabbitPrefab = EntityManager.GetSingleton<RabbitPrefab>().Value;
      }

      var rabbit = EntityManager.Instantiate(_rabbitPrefab);
      MakeAnimal(rabbit, position);
    }

    public void MakeWolf(float3 position)
    {
      Debug.Assert(Enabled);

      if (_wolfPrefab == Entity.Null)
      {
        _wolfPrefab = EntityManager.GetSingleton<WolfPrefab>().Value;
      }

      var wolf = EntityManager.Instantiate(_wolfPrefab);
      MakeAnimal(wolf, position);
    }

    public void MakeCarrot(float3 position)
    {
      Debug.Assert(Enabled);

      if (_carrotPrefab == Entity.Null)
      {
        _carrotPrefab = EntityManager.GetSingleton<CarrotPrefab>().Value;
      }

      var carrot = EntityManager.Instantiate(_carrotPrefab);
      SetPosition(carrot, position);
    }
  }
}