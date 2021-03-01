using Ecosystem.Genes;
using UnityEngine;

namespace Ecosystem
{
  public sealed class Reproducer : MonoBehaviour
  {
    [SerializeField] private Genome genome;
    [SerializeField] private GameObject prefab;
    
    private bool _IsPregnant { get; set; }
    private bool _IsSexuallyMature { get; set; }

    private double _gestationPeriod;
    private double _SexualMaturityTime;
    private double _pregnancyElapsedTime;
    private double _maturityElapsedTime;
    private Genome _mateGenome;

    private void Start()
    {
      _SexualMaturityTime = genome.GetSexualMaturityTime();
      _gestationPeriod = genome.GetGestationPeriod();
      Debug.Log("Gestation period:" + genome.GetGestationPeriod());
    }

    private void Update()
    {
      if (!_IsSexuallyMature)
      {
        _maturityElapsedTime += Time.deltaTime;
        if (_maturityElapsedTime >= _SexualMaturityTime)
        {
          _IsSexuallyMature = true;
        }
      }

      if (_IsPregnant)
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

      _IsPregnant = false;
      _pregnancyElapsedTime = 0;

      var child = Instantiate(prefab, currentTransform.position, currentTransform.rotation);
      var childGenome = child.GetComponent<Genome>();
      childGenome.Initialize(genome, _mateGenome);
    }

    private void StartPregnancy(Genome mateGenome)
    {
      _IsPregnant = true;
      _mateGenome = mateGenome;
    }

    private void OnTriggerEnter(Collider other)
    {
      if (other.CompareTag("Reproducer") &&
          other.TryGetComponent(out Reproducer otherReproducer) &&
          Genome.CompatibleAsParents(genome, otherReproducer.genome) &&
          otherReproducer._IsSexuallyMature)
      {
        Debug.Log("Try Mating");
        if (genome.IsMale && !otherReproducer._IsPregnant)
        {
          otherReproducer.StartPregnancy(genome);
        }
        else if (!_IsPregnant)
        {
          StartPregnancy(otherReproducer.genome);
        }
      }
    }

    public bool CanMate()
    {
      return _IsPregnant && _IsSexuallyMature;
    }
  }
}