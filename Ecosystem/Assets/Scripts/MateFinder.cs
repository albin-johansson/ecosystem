using Ecosystem.Genes;
using UnityEngine;

namespace Ecosystem
{
  public sealed class MateFinder : MonoBehaviour
  {
    [SerializeField] private TargetTracker targetTracker;
    [SerializeField] private Genome genome;
    [SerializeField] private Reproducer reproducer;
    
    private void OnTriggerEnter(Collider other)
    {
      if (reproducer.CanMate() && other.TryGetComponent(out Genome otherGenome) && Genome.CompatibleAsParents(genome, otherGenome))
      {
        Debug.Log("Going to mate");
        targetTracker.SetTarget(other.gameObject);
      }
    }
  }
}