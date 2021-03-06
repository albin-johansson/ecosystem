using UnityEngine;

namespace Ecosystem.Genes
{
  public sealed class Gene
  {
    public float Min { get; }
    public float Max { get; }
    public float Value { get; }

    public Gene(float value, float min, float max)
    {
      Max = max;
      Min = min;
      Value = Mathf.Clamp(value, min, max);
    }

    public Gene(Gene other) : this(other.Value, other.Min, other.Max)
    {
    }

    //Returns value as a number in [0,1]. 
    public float ValueAsDecimal()
    {
      return (Value - Min) / (Max - Min);
    }

    // Returns a new mutated gene based on the current Genes range. 
    public Gene Mutate()
    {
      return new Gene(GeneUtil.Mutate(Min, Max), Min, Max);
    }
  }
}