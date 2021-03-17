using UnityEngine;
using UnityEngine.AI;

namespace Ecosystem.Genes
{
  public abstract class AbstractGenome : MonoBehaviour, IGenome
  {
    internal GenomeData Data;

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
    
    public float GetChildFoodConsumtionFactor() => Data.ChildFoodConsumtionFactor;

    public float Speed => GetHungerRate().Value *
                          GetSpeedFactor().Value *
                          GetSizeFactor().ValueAsDecimal();

    public float Metabolism => GetHungerRate().Value * GetSizeFactor().Value * (1 + Data.MetabolismFactor * ((GetVision().Value - 1) + (GetSpeedFactor().Value - 1)));

    public float Attractiveness => GetDesirabilityScore().Value;
    
    protected void ConvertGenesToAttributes()
    {
      if (gameObject.TryGetComponent(out NavMeshAgent navMeshAgent))
      {
        navMeshAgent.speed *= Speed;
      }
      
      gameObject.transform.localScale *= GetSizeFactor().Value;
    }

    public Gene GetHungerRate() => Data.HungerRate;

    public Gene GetHungerThreshold() => Data.HungerThreshold;

    public Gene GetThirstRate() => Data.ThirstRate;

    public Gene GetThirstThreshold() => Data.ThirstThreshold;

    public Gene GetVision() => Data.Vision;

    public Gene GetSpeedFactor() => Data.SpeedFactor;

    public Gene GetSizeFactor() => Data.SizeFactor;

    public Gene GetDesirabilityScore() => Data.DesirabilityScore;

    public Gene GetGestationPeriod() => Data.GestationPeriod;

    public Gene GetSexualMaturityTime() => Data.SexualMaturityTime;

    public GenomeData GetGenes() => Data;
  }
}