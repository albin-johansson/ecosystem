namespace Ecosystem.Genes
{
  public static class Genomes
  {
    public static bool CompatibleAsParents(IGenome first, IGenome second)
    {
      return first.GetType() == second.GetType() && first.IsMale != second.IsMale;
    }

    public static GenomePack Merge(GenomePack first, GenomePack second)
    {
      var mutateChance = first.MutateChance;
      return new GenomePack
      {
              HungerRate = Merge(first.HungerRate, second.HungerRate, mutateChance),
              HungerThreshold = Merge(first.HungerThreshold, second.HungerThreshold, mutateChance),
              ThirstRate = Merge(first.ThirstRate, second.ThirstRate, mutateChance),
              ThirstThreshold = Merge(first.ThirstThreshold, second.ThirstThreshold, mutateChance),
              Vision = Merge(first.Vision, second.Vision, mutateChance),
              SpeedFactor = Merge(first.SpeedFactor, second.SpeedFactor, mutateChance),
              SizeFactor = Merge(first.SizeFactor, second.SizeFactor, mutateChance),
              DesirabilityFactor = Merge(first.DesirabilityFactor, second.DesirabilityFactor, mutateChance),
              GestationPeriod = Merge(first.GestationPeriod, second.GestationPeriod, mutateChance),
              SexualMaturityTime = Merge(first.SexualMaturityTime, second.SexualMaturityTime, mutateChance),
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