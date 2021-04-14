namespace Ecosystem.Genes
{
  public static class Genomes
  {
    public static bool CompatibleAsParents(IGenome first, IGenome second)
    {
      return first.GetType() == second.GetType() && first.IsMale != second.IsMale;
    }

    public static GenomeData Merge(IGenome firstGenome, IGenome secondGenome)
    {
      var first = firstGenome.GetGenes();
      var second = secondGenome.GetGenes();
      var mutateChance = first.MutateChance;
      return new GenomeData
      {
        HungerRate = Gene.Merge(first.HungerRate, second.HungerRate, mutateChance),
        HungerThreshold = Gene.Merge(first.HungerThreshold, second.HungerThreshold, mutateChance),
        ThirstRate = Gene.Merge(first.ThirstRate, second.ThirstRate, mutateChance),
        ThirstThreshold = Gene.Merge(first.ThirstThreshold, second.ThirstThreshold, mutateChance),
        Vision = Gene.Merge(first.Vision, second.Vision, mutateChance),
        Speed = Gene.Merge(first.Speed, second.Speed, mutateChance),
        SizeFactor = Gene.Merge(first.SizeFactor, second.SizeFactor, mutateChance),
        DesirabilityScore = Gene.Merge(first.DesirabilityScore, second.DesirabilityScore, mutateChance),
        GestationPeriod = Gene.Merge(first.GestationPeriod, second.GestationPeriod, mutateChance),
        SexualMaturityTime = Gene.Merge(first.SexualMaturityTime, second.SexualMaturityTime, mutateChance),
        MutateChance = mutateChance
      };
    }
  }
}