using Ecosystem.Consumer;
using Ecosystem.Genes;
using Ecosystem.Spawning;
using Ecosystem.UI;
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
    [SerializeField] private GameObject animalModel;
    [SerializeField] private GenderIcon _genderIcon;

    private Transform _directoryOfAnimal;
    private IGenome _mateGenome;
    private float _gestationPeriod;
    private float _sexualMaturityTime;
    private float _pregnancyElapsedTime;
    private float _maturityElapsedTime;
    private float _childSaturation;
    private float _childHydration;
    private bool _isSexuallyMature;
    private static readonly Vector3 ChildSize = new Vector3(0.7f, 0.7f, 0.7f);

    public bool IsWilling { get; set; }

    public bool IsPregnant { get; private set; }

    public bool IsFertile => !IsPregnant && _isSexuallyMature;

    public bool CanMate => IsFertile && IsWilling;

    private void Start()
    {
      _sexualMaturityTime = genome.GetSexualMaturityTime().Value;
      _gestationPeriod = genome.GetGestationPeriod().Value;
      _directoryOfAnimal = gameObject.transform.parent.parent;
      animalModel.transform.localScale = ChildSize;
    }

    private void Update()
    {
      if (!_isSexuallyMature)
      {
        _maturityElapsedTime += Time.deltaTime;
        if (_maturityElapsedTime >= _sexualMaturityTime && !_isSexuallyMature)
        {
          _isSexuallyMature = true;
          animalModel.transform.localScale = Vector3.one;
        }
      }

      if (IsPregnant)
      {
        _childSaturation += genome.Metabolism * AbstractGenome.ChildFoodConsumptionFactor * Time.deltaTime * 4;
        _childHydration += genome.GetThirstRate().Value * Time.deltaTime * 4;
        _pregnancyElapsedTime += Time.deltaTime;
        if (_pregnancyElapsedTime >= _gestationPeriod)
        {
          GiveBirth();
          _childSaturation = 0;
          _childHydration = 0;
        }
      }
    }

    public bool CompatibleAsParents(GameObject other)
    {
      return other.transform.parent.GetChild(0).TryGetComponent(out Reproducer otherReproducer) &&
             otherReproducer.CanMate &&
             Genomes.CompatibleAsParents(genome, otherReproducer.genome);
    }

    private void GiveBirth()
    {
      var currentTransform = transform;

      IsPregnant = false;
      _pregnancyElapsedTime = 0;

      _genderIcon.SetPregnancyIcon(false);

      var child = ObjectPoolHandler.Instance.Construct(keyToPool);
      var childTransform = child.transform;

      childTransform.position = currentTransform.position;
      childTransform.rotation = currentTransform.rotation;
      childTransform.parent = _directoryOfAnimal;
      child.SetActive(true);

      if (TryGetComponent(out AbstractGenome childGenome))
      {
        childGenome.Initialize(genome, _mateGenome);
      }

      var childConsumer = child.GetComponentInChildren<IConsumer>();
      var childWaterConsumer = child.GetComponentInChildren<WaterConsumer>();

      childWaterConsumer.SetHydration(_childHydration);
      childConsumer.SetSaturation(_childSaturation);

      OnBirth?.Invoke(child);
    }

    private void StartPregnancy(IGenome mateGenome)
    {
      IsPregnant = true;
      _mateGenome = mateGenome;

      _genderIcon.SetPregnancyIcon(true);

      OnMating?.Invoke(gameObject.transform.position, prefab.tag, mateGenome, genome);
    }

    private void OnTriggerEnter(Collider other)
    {
      if (!genome.IsMale &&
          CanMate &&
          other.TryGetComponent(out Reproducer otherReproducer) &&
          Genomes.CompatibleAsParents(genome, otherReproducer.genome) &&
          otherReproducer.CanMate
      )
      {
        StartPregnancy(otherReproducer.genome);
      }
    }
  }
}