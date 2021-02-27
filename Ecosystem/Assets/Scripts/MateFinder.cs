using Ecosystem.Genes;
using UnityEngine;

namespace Ecosystem
{
  public sealed class MateFinder : MonoBehaviour
  {
    [SerializeField] private TargetTracker targetTracker;
    [SerializeField] private Genome genome;

    private void OnTriggerEnter(Collider other)
    {
      if (other.TryGetComponent(out Genome otherGenome) && Genome.CompatibleAsParents(genome, otherGenome))
      {
        targetTracker.SetTarget(other.gameObject);
      }
    }
  }
}