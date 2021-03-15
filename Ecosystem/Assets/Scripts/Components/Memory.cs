using Unity.Entities;
using UnityEngine;

namespace Ecosystem.Components
{
  [InternalBufferCapacity(5)]
  public struct MemoryElement : IBufferElementData
  {
    public AnimalState State;
    public Vector3 Position;
  }

  public struct MemoryNextIndex : IComponentData
  {
    public int Index;
  }
}