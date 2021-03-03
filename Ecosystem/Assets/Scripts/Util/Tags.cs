using UnityEngine;

namespace Ecosystem.Util
{
  public static class Tags
  {
    public static bool IsPredator(GameObject animal) => animal.CompareTag("Wolf") ||
                                                        animal.CompareTag("Bear");

    public static bool IsPrey(GameObject animal) => animal.CompareTag("Rabbit") ||
                                                    animal.CompareTag("Deer");

    public static bool IsFood(GameObject gameObject) => gameObject.CompareTag("Carrot");

    public static int Count(string tag) => GameObject.FindGameObjectsWithTag(tag).Length;

    public static int CountPredators() => Count("Wolf") + Count("Bear");

    public static int CountPrey() => Count("Rabbit") + Count("Deer");

    public static int CountFood() => Count("Carrot");
  }
}