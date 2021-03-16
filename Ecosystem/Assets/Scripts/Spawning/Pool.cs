using System;
using UnityEngine;

namespace Ecosystem.Spawning
{
  [Serializable]
  public sealed class Pool
  {
    public string key;
    public GameObject prefab;
    public int size;
  }
}