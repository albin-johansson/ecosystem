using Ecosystem.Genes;
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

    private Transform _directoryOfAnimal;
    private bool _isPregnant;
    private bool _isSexuallyMature;
    private double _gestationPeriod;
    private double _sexualMaturityTime;
    private double _pregnancyElapsedTime;
    private double _maturityElapsedTime;
    private float _childSaturation;
    private IConsumer _consumer;
    private IGenome _mateGenome;
    private readonly float _childFoodConsumtionFraction = 1/3;

    public bool CanMate => !_isPregnant && _isSexuallyMature;

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

      if (_isPregnant)
      {
        if (_consumer == null)
        {
          _consumer = transform.root.GetComponentInChildren<IConsumer>();
        }
        _consumer.Hunger += (genome.Metabolism * _childFoodConsumtionFraction) * Time.deltaTime;
        _childSaturation += (genome.Metabolism * _childFoodConsumtionFraction) *  Time.deltaTime;
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
      var currentTransform = transform;

      _isPregnant = false;
      _pregnancyElapsedTime = 0;

      var child = Instantiate(prefab, currentTransform.position, currentTransform.rotation, _directoryOfAnimal);
      var childGenome = child.GetComponent<AbstractGenome>();
      childGenome.Initialize(genome, _mateGenome);
      
      var childConsumer = child.GetComponentInChildren<IConsumer>();
      childConsumer.SetSaturation(_childSaturation);

      OnBirth?.Invoke(child);
    }

    private void StartPregnancy(IGenome mateGenome)
    {
      _isPregnant = true;
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
        if (Random.value >= (genome.Attractiveness + otherReproducer.genome.Attractiveness)/2)
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
    }

    public bool CompatibleAsParents(GameObject other)
    {
      return CanMate && other.TryGetComponent(out Reproducer reproducer) &&
             Genomes.CompatibleAsParents(genome, reproducer.genome);
    }
    
  }
}