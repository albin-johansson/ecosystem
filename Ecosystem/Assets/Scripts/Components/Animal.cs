using System;
using Unity.Entities;

namespace Ecosystem.Components
{
  [Serializable]
  [GenerateAuthoringComponent]
  public struct Animal : IComponentData
  {
    public float resourceConsumptionDistance;
  }
}