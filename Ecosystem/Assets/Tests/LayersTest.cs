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
      Assert.IsTrue(Layers.IsPredatorLayer(Layers.WolfLayer));
      Assert.IsTrue(Layers.IsPredatorLayer(Layers.BearLayer));

      Assert.IsFalse(Layers.IsPredatorLayer(Layers.PreyLayer));
      Assert.IsFalse(Layers.IsPredatorLayer(Layers.FoodLayer));
      Assert.IsFalse(Layers.IsPredatorLayer(Layers.WaterLayer));
    }
  }
}