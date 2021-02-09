using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace Tests // Yes, this should be called Editor but that clashes with the assets
{
  public sealed class DummyTest
  {
    [Test]
    public void SillyTest()
    {
      Assert.AreEqual(2, 1 + 1);
    }
  }
}