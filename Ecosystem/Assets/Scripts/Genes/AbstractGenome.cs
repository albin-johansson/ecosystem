using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

namespace Ecosystem.Genes
{
  public abstract class AbstractGenome : MonoBehaviour, IGenome
  {
    internal GenomeData Data;

    private const float MetabolismFactor = 1.495f; // 1.15 (Vision) * 1.30 (Speed)
    private const float ChildFoodConsumtionFactor = 4f / 3f;
    public string key;
    private static Random _random = new Random();
    private const string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    protected static string GenerateKey(int length)
    {
      return new string(Enumerable.Repeat(_chars, length)
        .Select(s => s[_random.Next(s.Length)]).ToArray());
    }

    private void Awake()
    {
      Initialize();
    }

    protected abstract void Initialize();

    public void Initialize(IGenome first, IGenome second)
    {
      key = GenerateKey(10);
      Data = Genomes.Merge(first, second);
      ConvertGenesToAttributes();
    }

    public bool IsMale => Data.IsMale;

    public float GetChildFoodConsumtionFactor() => ChildFoodConsumtionFactor;

    public float WalkingSpeed => GetSpeed().Value * GetSizeFactor().Value;


    public float Metabolism => GetHungerRate().Value * GetSizeFactor().Value *
                               (1 + MetabolismFactor * (GetVision().ValueAsDecimal() + GetSpeed().ValueAsDecimal()));

    public float Attractiveness => GetDesirabilityScore().Value;

    protected void ConvertGenesToAttributes()
    {
      if (gameObject.TryGetComponent(out NavMeshAgent navMeshAgent))
      {
        navMeshAgent.speed = GetSpeed().Value;
      }

      gameObject.transform.localScale *= GetSizeFactor().Value;
    }

    public Gene GetHungerRate() => Data.HungerRate;

    public Gene GetHungerThreshold() => Data.HungerThreshold;

    public Gene GetThirstRate() => Data.ThirstRate;

    public Gene GetThirstThreshold() => Data.ThirstThreshold;

    public Gene GetVision() => Data.Vision;

    public Gene GetSpeed() => Data.Speed;

    public Gene GetSizeFactor() => Data.SizeFactor;

    public Gene GetDesirabilityScore() => Data.DesirabilityScore;

    public Gene GetGestationPeriod() => Data.GestationPeriod;

    public Gene GetSexualMaturityTime() => Data.SexualMaturityTime;

    public GenomeData GetGenes() => Data;
  }
}