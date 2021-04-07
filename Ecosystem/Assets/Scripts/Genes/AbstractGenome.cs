using UnityEngine;
using UnityEngine.AI;

namespace Ecosystem.Genes
{
  public abstract class AbstractGenome : MonoBehaviour, IGenome
  {
    internal GenomeData Data;

    private const float MetabolismFactor = 1.495f; // 1.15 (Vision) * 1.30 (Speed)
    private const float ChildFoodConsumtionFactor = 4f / 3f;

    private void Awake()
    {
      Initialize();
    }

    protected abstract void Initialize();

    public void Initialize(IGenome first, IGenome second)
    {
      Data = Genomes.Merge(first, second);
      ConvertGenesToAttributes();
    }

    public bool IsMale => Data.IsMale;
    
    public float GetChildFoodConsumtionFactor() => ChildFoodConsumtionFactor;

    public float WalkingSpeed => GetSpeed().Value * GetSizeFactor().Value;

    public float Metabolism => GetHungerRate().Value * GetSizeFactor().Value * (1 + MetabolismFactor * (GetVision().ValueAsDecimal() + GetSpeed().ValueAsDecimal()));

    public float Attractiveness => GetDesirabilityScore().Value;
    
    protected void ConvertGenesToAttributes()
    {
      if (gameObject.TryGetComponent(out NavMeshAgent navMeshAgent))
      {
        navMeshAgent.speed *= GetSpeed().Value;
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