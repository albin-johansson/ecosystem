using Ecosystem.Genes;
using UnityEngine;

namespace Ecosystem
{
  public sealed class Reproducer : MonoBehaviour
  {
    [SerializeField] private Genome genome;
    [SerializeField] private GameObject prefab;
    [SerializeField] private double gestationPeriod;

    private bool IsPregnant { get; set; }

    private double _pregnancyElapsedTime;
    private Genome _mateGenome;

    private void Update()
    {
      if (IsPregnant)
      {
        _pregnancyElapsedTime += Time.deltaTime;
        if (_pregnancyElapsedTime >= gestationPeriod)
        {
          GiveBirth();
        }
      }
    }

    private void GiveBirth()
    {
      var currentTransform = transform;

      IsPregnant = false;
      _pregnancyElapsedTime = 0;

      var child = Instantiate(prefab, currentTransform.position, currentTransform.rotation);
      var childGenome = child.GetComponent<Genome>();
      childGenome.Initialize(genome, _mateGenome);
    }

    private void StartPregnancy(Genome mateGenome)
    {
      IsPregnant = true;
      _mateGenome = mateGenome;
    }

    private void OnTriggerEnter(Collider other)
    {
      if (other.CompareTag("Body") &&
          other.TryGetComponent(out Genome otherGenome) &&
          Genome.CompatibleAsParents(genome, otherGenome))
      {
        var otherReproducer = other.gameObject.GetComponent<Reproducer>();
        if (genome.IsMale && !otherReproducer.IsPregnant)
        {
          otherReproducer.StartPregnancy(genome);
        }
        else if (!IsPregnant)
        {
          StartPregnancy(otherGenome);
        }
      }
    }
  }
}