using System.Collections.Generic;

namespace Ecosystem.Genes
{
  public interface IGenome
  {
    void Initialize(IGenome first, IGenome second);

    float Speed { get; }

    float Metabolism { get; }

    float Attractiveness { get; }

    bool IsMale { get; }

    Gene GetHungerRate();

    Gene GetHungerThreshold();

    Gene GetThirstRate();

    Gene GetThirstThreshold();

    Gene GetVision();

    Gene GetSpeedFactor();

    Gene GetSizeFactor();

    Gene GetDesirabilityScore();

    Gene GetGestationPeriod();

    Gene GetSexualMaturityTime();

    GenomePack GetGenes();

    Dictionary<GeneType, Gene> GetInitialGenes();
  }
}