using Ecosystem.Genes;
using UnityEngine;

namespace Ecosystem
{
  public sealed class MateFinder : MonoBehaviour
  {
    [SerializeField] private Genome genome;
    [SerializeField] private Reproducer reproducer;

    public bool CompatibleAsParents(GameObject other)
    {
      return reproducer.CompatibleAsParents(other);
    }
  }
}