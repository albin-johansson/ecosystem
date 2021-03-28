using System;
using Unity.Entities;

namespace Ecosystem.Components
{
  [Serializable]
  [GenerateAuthoringComponent]
  public struct Predator : IComponentData
  {
    public float attackDistance;
  }
}