using Ecosystem.Genes;
using UnityEngine;

namespace Ecosystem
{
  public sealed class MateFinder : MonoBehaviour
  {
    [SerializeField] private AbstractGenome genome;
    [SerializeField] private Reproducer reproducer;

    public bool CompatibleAsParents(GameObject other)
    {
      return reproducer.CanMate && other.TryGetComponent(out AbstractGenome otherGenome) &&
             Genomes.CompatibleAsParents(genome, otherGenome);
    }
  }
}