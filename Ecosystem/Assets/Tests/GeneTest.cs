using Ecosystem.Genes;
using NUnit.Framework;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace Tests
{
  public sealed class GeneTest
  {
    [Test]
    public void ConstructorTest()
    {
      const float min = 0.1f;
      const float max = 1.3f;

      {
        const float value = 0.75f; // Valid gene value

        var gene = new Gene(value, min, max);
        Assert.AreEqual(value, gene.Value);
      }

      {
        const float value = min - 0.1f; // Underflow

        var gene = new Gene(value, min, max);
        Assert.AreEqual(min, gene.Value);
      }

      {
        const float value = max + 0.1f; // Overflow

        var gene = new Gene(value, min, max);
        Assert.AreEqual(max, gene.Value);
      }
    }

    [Test]
    public void CopyConstructorTest()
    {
      var original = new Gene(0.4f, 0.2f, 0.9f);
      var copy = new Gene(original);

      Assert.AreEqual(original.Value, copy.Value);
      Assert.AreEqual(original.Min, copy.Min);
      Assert.AreEqual(original.Max, copy.Max);
    }
  }
}