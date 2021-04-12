using System;
using Unity.Entities;

namespace Ecosystem.Components
{
  [Serializable]
  [GenerateAuthoringComponent]
  public struct Hunger : IComponentData
  {
    public float value;
    public float rate;
    public float max;
  }
}