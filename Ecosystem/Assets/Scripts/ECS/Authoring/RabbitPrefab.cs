using System;
using JetBrains.Annotations;
using Unity.Entities;

namespace Ecosystem.ECS.Authoring
{
  [Serializable]
  [GenerateAuthoringComponent]
  public struct RabbitPrefab : IComponentData
  {
    [UsedImplicitly] public Entity Value;
  }
}