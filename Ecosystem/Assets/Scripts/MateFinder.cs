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
      Debug.Log("Checks if compatible: " + other.name);
      if (reproducer.CanMate)
      {
        Debug.Log("Reproducer can mate: ");
      }

      if (other.TryGetComponent(out Genome otherGenome1))
      {
        Debug.Log("Found other genome: " + otherGenome1.GetType());
        if (Genome.CompatibleAsParents(genome, otherGenome1))
        {
          Debug.Log("Genomes are compatible");
        }
      }
      return reproducer.CanMate && other.TryGetComponent(out Genome otherGenome) &&
             Genome.CompatibleAsParents(genome, otherGenome);
    }
  }
}