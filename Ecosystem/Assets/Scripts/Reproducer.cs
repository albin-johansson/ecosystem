using System;
using Ecosystem.Genes;
using UnityEngine;

namespace Ecosystem
{
  public sealed class Reproducer : MonoBehaviour
  {
    [SerializeField] private Genome genome;
    [SerializeField] private GameObject prefab;

    private double gestationPeriod;
    private double SexualMaturityTime;
    private bool IsPregnant { get; set; }
    private bool IsSexuallyMature { get; set; }

    private double _pregnancyElapsedTime;
    private double _maturityElapsedTime;
    private Genome _mateGenome;

    void Start()
    {
      SexualMaturityTime = genome.GetSexualMaturityTime();
      gestationPeriod = genome.GetGestationPeriod();
      Debug.Log("Gestation period:" + genome.GetGestationPeriod());
    }

    private void Update()
    {
      if (!IsSexuallyMature)
      {
        _maturityElapsedTime += Time.deltaTime;
        if (_maturityElapsedTime >= SexualMaturityTime)
        {
          IsSexuallyMature = true;
        }
      }

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
      if (other.CompareTag("Reproducer") &&
          other.TryGetComponent(out Reproducer otherReproducer) &&
          Genome.CompatibleAsParents(genome, otherReproducer.genome) &&
          otherReproducer.IsSexuallyMature)
      {
        Debug.Log("Try Mating");
        if (genome.IsMale && !otherReproducer.IsPregnant)
        {
          otherReproducer.StartPregnancy(genome);
        }
        else if (!IsPregnant)
        {
          StartPregnancy(otherReproducer.genome);
        }
      }
    }

    public bool CanMate()
    {
      return IsPregnant && IsSexuallyMature;
    }
  }
}