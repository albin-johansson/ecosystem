using Random = UnityEngine.Random;

namespace Ecosystem.Genes
{
  public sealed class GenomePack
  {
    internal Gene HungerRate;
    internal Gene HungerThreshold;
    internal Gene ThirstRate;
    internal Gene ThirstThreshold;
    internal Gene Vision;
    internal Gene SpeedFactor;
    internal Gene SizeFactor;
    internal Gene DesirabilityFactor;
    internal Gene GestationPeriod;
    internal Gene SexualMaturityTime;

    internal double MutateChance = 0.05;
    internal bool IsMale = Random.value > 0.5;
  }
}