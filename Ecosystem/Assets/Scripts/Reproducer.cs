using Ecosystem.Genes;
using UnityEngine;

namespace Ecosystem
{
  public sealed class Reproducer : MonoBehaviour
  {
    [SerializeField] private Genome genome;
    [SerializeField] private GameObject prefab;

    private bool _isPregnant;
    private bool _isSexuallyMature;
    private double _gestationPeriod;
    private double _sexualMaturityTime;
    private double _pregnancyElapsedTime;
    private double _maturityElapsedTime;
    private Genome _mateGenome;

    public bool CanMate => !_isPregnant && _isSexuallyMature;

    private void Start()
    {
      _sexualMaturityTime = genome.GetSexualMaturityTime();
      _gestationPeriod = genome.GetGestationPeriod();
    }

    private void Update()
    {
      if (!_isSexuallyMature)
      {
        _maturityElapsedTime += Time.deltaTime;
        if (_maturityElapsedTime >= _sexualMaturityTime)
        {
          _isSexuallyMature = true;
        }
      }

      if (_isPregnant)
      {
        _pregnancyElapsedTime += Time.deltaTime;
        if (_pregnancyElapsedTime >= _gestationPeriod)
        {
          GiveBirth();
        }
      }
    }

    private void GiveBirth()
    {
      var currentTransform = transform;

      _isPregnant = false;
      _pregnancyElapsedTime = 0;

      var child = Instantiate(prefab, currentTransform.position, currentTransform.rotation);
      var childGenome = child.GetComponent<Genome>();
      childGenome.Initialize(genome, _mateGenome);
    }

    private void StartPregnancy(Genome mateGenome)
    {
      _isPregnant = true;
      _mateGenome = mateGenome;
    }

    private void OnTriggerEnter(Collider other)
    {
      if (other.CompareTag("Reproducer") &&
          other.TryGetComponent(out Reproducer otherReproducer) &&
          Genome.CompatibleAsParents(genome, otherReproducer.genome) &&
          otherReproducer.CanMate && CanMate)
      {
        if (genome.IsMale && !otherReproducer._isPregnant)
        {
          otherReproducer.StartPregnancy(genome);
        }
        else if (!_isPregnant)
        {
          StartPregnancy(otherReproducer.genome);
        }
      }
    }

    public bool CompatibleAsParents(GameObject other)
    {
      return CanMate && other.TryGetComponent(out Reproducer reproducer) &&
             Genome.CompatibleAsParents(genome, reproducer.genome);
    }
    
  }
}