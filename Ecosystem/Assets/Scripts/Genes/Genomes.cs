namespace Ecosystem.Genes
{
  public static class Genomes
  {
    public static bool CompatibleAsParents(IGenome first, IGenome second)
    {
      return first.GetType() == second.GetType() && first.IsMale != second.IsMale;
    }

    public static GenomeData Merge(IGenome first, IGenome second)
    {
      var firstData = first.GetGenes();
      var secondData = second.GetGenes();
      var mutateChance = firstData.MutateChance;
      return new GenomeData
      {
              HungerRate = Merge(firstData.HungerRate, secondData.HungerRate, mutateChance),
              HungerThreshold = Merge(firstData.HungerThreshold, secondData.HungerThreshold, mutateChance),
              ThirstRate = Merge(firstData.ThirstRate, secondData.ThirstRate, mutateChance),
              ThirstThreshold = Merge(firstData.ThirstThreshold, secondData.ThirstThreshold, mutateChance),
              Vision = Merge(firstData.Vision, secondData.Vision, mutateChance),
              SpeedFactor = Merge(firstData.SpeedFactor, secondData.SpeedFactor, mutateChance),
              SizeFactor = Merge(firstData.SizeFactor, secondData.SizeFactor, mutateChance),
              DesirabilityScore = Merge(firstData.DesirabilityScore, secondData.DesirabilityScore, mutateChance),
              GestationPeriod = Merge(firstData.GestationPeriod, secondData.GestationPeriod, mutateChance),
              SexualMaturityTime = Merge(firstData.SexualMaturityTime, secondData.SexualMaturityTime, mutateChance),
              MutateChance = mutateChance
      };
    }

    private static Gene Merge(Gene first, Gene second, double mutateChance)
    {
      if (GeneUtil.RandomWithChance(mutateChance))
      {
        return first.Mutate();
      }
      else
      {
        return GeneUtil.RandomWithChance(50)
                ? new Gene(first)
                : new Gene(second);
      }
    }
  }
}