using System;
using Unity.Entities;

namespace Ecosystem.Components
{
  [Serializable]
  public struct Consumption : IComponentData
  {
    public double when;
  }
}