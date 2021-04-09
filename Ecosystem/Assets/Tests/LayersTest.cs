using Ecosystem.Util;
using NUnit.Framework;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace Tests
{
  public sealed class LayersTest
  {
    [Test]
    public void PredatorMask()
    {
      Assert.AreEqual(Layers.PredatorMask, LayerMask.GetMask("Bear", "Wolf"));
    }
  }
}