using NUnit.Framework;
using Assert = UnityEngine.Assertions.Assert;

namespace Editor
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