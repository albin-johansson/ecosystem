using Ecosystem.Genes;
using Ecosystem.Spawning;
using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem
{
  public sealed class Reproducer : MonoBehaviour
  {
    /// <summary>
    ///   The signature of birth event handlers.
    /// </summary>
    /// <param name="animal">the game object associated with the animal that was born.</param>
    public delegate void BirthEvent(GameObject animal);

    /// <summary>
    ///   The signature of mating event handlers.
    /// </summary>
    /// <param name="position">the position of the mating.</param>
    /// <param name="animalTag">the tag associated with the animals that mated.</param>
    /// <param name="male">the genome associated with the male.</param>
    /// <param name="female">the genome associated with the female.</param>
    public delegate void MatingEvent(Vector3 position, string animalTag, IGenome male, IGenome female);

    /// <summary>
    ///   This event is emitted every time an animal is born.
    /// </summary>
    public static event BirthEvent OnBirth;

    /// <summary>
    ///   This event is emitted every time two animals mate.
    /// </summary>
    public static event MatingEvent OnMating;

    [SerializeField] private AbstractGenome genome;
    [SerializeField] private GameObject prefab;
    [SerializeField] private string keyToPool;

    private Transform _directoryOfAnimal;
    public bool IsPregnant { get; private set; }
    private bool _isSexuallyMature;
    private double _gestationPeriod;
    private double _sexualMaturityTime;
    private double _pregnancyElapsedTime;
    private double _maturityElapsedTime;
    private float _childSaturation;
    private IConsumer _consumer;
    private IGenome _mateGenome;
    public bool CanMate => !IsPregnant && _isSexuallyMature;

    private void Start()
    {
      _sexualMaturityTime = genome.GetSexualMaturityTime().Value;
      _gestationPeriod = genome.GetGestationPeriod().Value;
      _directoryOfAnimal = gameObject.transform.parent.parent;
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

      if (IsPregnant)
      {
        _childSaturation += (genome.Metabolism * genome.GetChildFoodConsumtionFactor()) * Time.deltaTime;
        _pregnancyElapsedTime += Time.deltaTime;
        if (_pregnancyElapsedTime >= _gestationPeriod)
        {
          GiveBirth();
          _childSaturation = 0;
        }
      }
    }

    private void GiveBirth()
    {
      Debug.Log("Give Birth");
      var currentTransform = transform;

      IsPregnant = false;
      _pregnancyElapsedTime = 0;
      var child = ObjectPoolHandler.instance.GetFromPool(keyToPool);
      child.transform.position = currentTransform.position;
      child.transform.rotation = currentTransform.rotation;
      child.transform.parent = _directoryOfAnimal;
      child.SetActive(true);
      if (TryGetComponent<AbstractGenome>(out var childGenome))
      {
        childGenome.Initialize(genome, _mateGenome);
      }

      var childConsumer = child.GetComponentInChildren<IConsumer>();
      childConsumer.SetSaturation(_childSaturation);

      OnBirth?.Invoke(child);
    }

    private void StartPregnancy(IGenome mateGenome)
    {
      IsPregnant = true;
      _mateGenome = mateGenome;

      OnMating?.Invoke(gameObject.transform.position, prefab.tag, mateGenome, genome);
    }

    private void OnTriggerEnter(Collider other)
    {
      if (other.CompareTag("Reproducer") &&
          other.TryGetComponent(out Reproducer otherReproducer) &&
          Genomes.CompatibleAsParents(genome, otherReproducer.genome) &&
          otherReproducer.CanMate && CanMate)
      {
        if (Random.value >= (genome.Attractiveness + otherReproducer.genome.Attractiveness) / 2)
        {
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
    }

    public bool CompatibleAsParents(GameObject other)
    {
      return CanMate && other.TryGetComponent(out Reproducer reproducer) &&
             Genomes.CompatibleAsParents(genome, reproducer.genome);
    }
  }
}