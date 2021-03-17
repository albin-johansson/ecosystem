using System;
using Unity.Entities;

namespace Ecosystem.ECS
{
  [Serializable]
  [GenerateAuthoringComponent]
  public struct RabbitPrefab : IComponentData
  {
    public Entity Value;
  }
}