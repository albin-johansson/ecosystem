using Ecosystem.Util;
using NUnit.Framework;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

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
    public void IsPredatorTest()
    {
      Assert.IsTrue(Tags.IsPredator(_wolf));
      Assert.IsTrue(Tags.IsPredator(_bear));

      Assert.IsFalse(Tags.IsPredator(_rabbit));
      Assert.IsFalse(Tags.IsPredator(_deer));
      Assert.IsFalse(Tags.IsPredator(_carrot));
    }

    [Test]
    public void IsPreyTest()
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
    public void IsAnimalTest()
    {
      Assert.IsTrue(Tags.IsAnimal(_wolf));
      Assert.IsTrue(Tags.IsAnimal(_bear));
      Assert.IsTrue(Tags.IsAnimal(_rabbit));
      Assert.IsTrue(Tags.IsAnimal(_deer));

      Assert.IsFalse(Tags.IsAnimal(_carrot));
    }

    [Test]
    public void CountTest()
    {
      Assert.IsTrue(Tags.Count("Rabbit") > 0);
      Assert.IsTrue(Tags.Count("Deer") > 0);
      Assert.IsTrue(Tags.Count("Wolf") > 0);
      Assert.IsTrue(Tags.Count("Bear") > 0);
    }

    [Test]
    public void CountPredatorsTest()
    {
      Assert.AreEqual(Tags.Count("Wolf") + Tags.Count("Bear"), Tags.CountPredators());
    }

    [Test]
    public void CountPreyTest()
    {
      Assert.AreEqual(Tags.Count("Rabbit") + Tags.Count("Deer"), Tags.CountPrey());
    }

    [Test]
    public void CountFoodTest()
    {
      // We only have carrots as the as food items, for now at least
      Assert.AreEqual(Tags.Count("Carrot"), Tags.CountFood());
    }
  }
}