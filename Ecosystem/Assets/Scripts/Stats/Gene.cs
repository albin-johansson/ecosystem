public sealed class Gene
{
  public double Max { get; private set; }
  public double Min { get; private set; }
  public double Value { get; private set; }

  public Gene(double value, double min, double max)
  {
    Max = max;
    Min = min;
    Value = GeneUtil.GetValidVar(value, min, max);
  }

  public Gene(Gene other) : this(other.Value, other.Min, other.Max)
  {
  }

  //Returns value as a number in [0,1]. 
  public double ValueAsDecimal()
  {
    return (Value - Min) / (Max - Min);
  }

  // Returns a new mutated gene based on the current Genes range. 
  public Gene Mutate()
  {
    return new Gene(GeneUtil.MutatedInRange(Min, Max), Min, Max);
  }
}