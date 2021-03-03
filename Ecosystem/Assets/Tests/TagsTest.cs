using Ecosystem.Util;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
  public sealed class TagsTest
  {
    private readonly GameObject _wolf = new GameObject {tag = "Wolf"};
    private readonly GameObject _bear = new GameObject {tag = "Bear"};
    private readonly GameObject _rabbit = new GameObject {tag = "Rabbit"};
    private readonly GameObject _deer = new GameObject {tag = "Deer"};
    private readonly GameObject _carrot = new GameObject {tag = "Carrot"};

    [Test]
    public void IsPredator()
    {
      Assert.IsTrue(Tags.IsPredator(_wolf));
      Assert.IsTrue(Tags.IsPredator(_bear));

      Assert.IsFalse(Tags.IsPredator(_rabbit));
      Assert.IsFalse(Tags.IsPredator(_deer));
      Assert.IsFalse(Tags.IsPredator(_carrot));
    }

    [Test]
    public void IsPrey()
    {
      Assert.IsTrue(Tags.IsPrey(_rabbit));
      Assert.IsTrue(Tags.IsPrey(_deer));

      Assert.IsFalse(Tags.IsPrey(_wolf));
      Assert.IsFalse(Tags.IsPrey(_bear));
      Assert.IsFalse(Tags.IsPrey(_carrot));
    }

    [Test]
    public void IsFoodTest()
    {
      Assert.IsTrue(Tags.IsFood(_carrot));

      Assert.IsFalse(Tags.IsFood(_rabbit));
      Assert.IsFalse(Tags.IsFood(_deer));
      Assert.IsFalse(Tags.IsFood(_wolf));
      Assert.IsFalse(Tags.IsFood(_bear));
    }

    [Test]
    public void CountTest()
    {
      Assert.IsTrue(Tags.Count("Rabbit") > 0);
    }
  }
}