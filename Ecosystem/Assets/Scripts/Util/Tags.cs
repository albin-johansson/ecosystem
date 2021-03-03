using UnityEngine;

namespace Ecosystem.Util
{
  public static class Tags
  {
    public static bool IsPredator(GameObject animal) => animal.CompareTag("Predator") ||
                                                        animal.CompareTag("Wolf") ||
                                                        animal.CompareTag("Bear");

    public static bool IsPrey(GameObject animal) => animal.CompareTag("Prey") ||
                                                    animal.CompareTag("Rabbit") ||
                                                    animal.CompareTag("Deer");

    public static int Count(string tag) => GameObject.FindGameObjectsWithTag(tag).Length;

    public static int CountPredators() => Count("Predator") + Count("Wolf") + Count("Bear");

    public static int CountPrey() => Count("Prey") + Count("Rabbit") + Count("Deer");
  }
}