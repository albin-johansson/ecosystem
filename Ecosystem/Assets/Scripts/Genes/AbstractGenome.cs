using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Ecosystem.Genes
{
  public abstract class AbstractGenome : MonoBehaviour, IGenome
  {
    internal GenomeData Data;
    private const float MetabolismFactor = 1.495f;
    public const float ChildFoodConsumptionFactor = 4f / 3f;

    private static readonly Dictionary<GeneType, Preset> DefaultPresets = new Dictionary<GeneType, Preset>()
    {
      {GeneType.HungerRate, new Preset(0, 10, new[] {1f, 5f, 7f})},
      {GeneType.HungerThreshold, new Preset(0, 10, new[] {1f, 5f, 7f})},
      {GeneType.ThirstRate, new Preset(0, 10, new[] {0.5f, 3f, 6f, 9f})},
      {GeneType.ThirstThreshold, new Preset(0, 10, new[] {1f, 5f, 7f})},
      {GeneType.Vision, new Preset(1, 50, new[] {5f, 10f, 25f, 40f, 45f})},
      {GeneType.Speed, new Preset(1, 2, new[] {1f, 1.5f, 2f})},
      {GeneType.GestationPeriod, new Preset(10, 120, new[] {12f, 20f, 50f, 70f, 90f, 110f})},
      {GeneType.SexualMaturityTime, new Preset(10, 150, new[] {20f, 50f, 90f, 140f})}
    };

    private void Awake()
    {
      Initialize();
    }


    /// <summary>
    /// Default for creating genes for new animals without
    /// its presets already defined.
    /// </summary>
    protected Dictionary<GeneType, Gene> CreateGenes()
    {
      return CreateGenes(DefaultPresets);
    }

    private static Dictionary<GeneType, Gene> CreateGenes(Dictionary<GeneType, Preset> presets)
    {
      var hungerRate = GeneUtil.CreateGeneFromPreset(presets[GeneType.HungerRate]);
      var hungerThreshold = GeneUtil.CreateGeneFromPreset(presets[GeneType.HungerThreshold]);
      var thirstRate = GeneUtil.CreateGeneFromPreset(presets[GeneType.ThirstRate]);
      var thirstThreshold = GeneUtil.CreateGeneFromPreset(presets[GeneType.ThirstThreshold]);
      var vision = GeneUtil.CreateGeneFromPreset(presets[GeneType.Vision]);
      var speedFactor = GeneUtil.CreateGeneFromPreset(presets[GeneType.Speed]);
      var gestationPeriod = GeneUtil.CreateGeneFromPreset(presets[GeneType.GestationPeriod]);
      var sexualMaturityTime = GeneUtil.CreateGeneFromPreset(presets[GeneType.SexualMaturityTime]);

      return new Dictionary<GeneType, Gene>
      {
        {GeneType.HungerRate, hungerRate},
        {GeneType.HungerThreshold, hungerThreshold},
        {GeneType.ThirstRate, thirstRate},
        {GeneType.ThirstThreshold, thirstThreshold},
        {GeneType.Vision, vision},
        {GeneType.Speed, speedFactor},
        {GeneType.GestationPeriod, gestationPeriod},
        {GeneType.SexualMaturityTime, sexualMaturityTime}
      };
    }

    protected abstract void Initialize();
    protected abstract Dictionary<GeneType, Preset> GetPresets();
    protected abstract float GetClassMutateChance();

    protected GenomeData CreateData()
    {
      return GenomeData.Create(CreateGenes(GetPresets()), GetClassMutateChance());
    }

    public void Initialize(IGenome first, IGenome second)
    {
      Data = Genomes.Merge(first, second);
      ConvertGenesToAttributes();
    }

    public bool IsMale => Data.IsMale;

    public float WalkingSpeed => GetSpeed().Value;

    public float Metabolism => GetHungerRate().Value *
                               (1 + MetabolismFactor * (GetVision().ValueAsDecimal() + GetSpeed().ValueAsDecimal()));

    protected void ConvertGenesToAttributes()
    {
      if (gameObject.TryGetComponent(out NavMeshAgent navMeshAgent))
      {
        navMeshAgent.speed = GetSpeed().Value;
      }
    }

    public Gene GetHungerRate() => Data.HungerRate;

    public Gene GetHungerThreshold() => Data.HungerThreshold;

    public Gene GetThirstRate() => Data.ThirstRate;

    public Gene GetThirstThreshold() => Data.ThirstThreshold;

    public Gene GetVision() => Data.Vision;

    public Gene GetSpeed() => Data.Speed;

    public Gene GetGestationPeriod() => Data.GestationPeriod;

    public Gene GetSexualMaturityTime() => Data.SexualMaturityTime;

    public GenomeData GetGenes() => Data;
  }
}