using Ecosystem.Util;
using NUnit.Framework;
using Assert = UnityEngine.Assertions.Assert;

namespace Tests
{
  public sealed class LayersTest
  {
    [Test]
    public void IsPredatorLayerTest()
    {
      Assert.IsTrue(Layers.IsPredatorLayer(Layers.WolfMask));
      Assert.IsTrue(Layers.IsPredatorLayer(Layers.BearMask));

      Assert.IsFalse(Layers.IsPredatorLayer(Layers.PreyMask));
      Assert.IsFalse(Layers.IsPredatorLayer(Layers.FoodMask));
      Assert.IsFalse(Layers.IsPredatorLayer(Layers.WaterMask));
    }
  }
}