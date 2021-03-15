using System;
using Unity.Entities;

namespace Ecosystem.Authoring
{
  [Serializable]
  [GenerateAuthoringComponent]
  public struct CylinderPrefab : IComponentData
  {
    public Entity Value;
  }
}