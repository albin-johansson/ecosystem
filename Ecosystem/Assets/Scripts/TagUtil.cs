using UnityEngine;

namespace Ecosystem
{
  public static class TagUtil
  {
    public static bool IsPredator(GameObject animal) => animal.CompareTag("Predator") ||
                                                        animal.CompareTag("Wolf") ||
                                                        animal.CompareTag("Bear");

    public static bool IsPrey(GameObject animal) => animal.CompareTag("Prey") ||
                                                    animal.CompareTag("Rabbit") ||
                                                    animal.CompareTag("Deer");

    public static int CountPredators() => GameObject.FindGameObjectsWithTag("Predator").Length +
                                          GameObject.FindGameObjectsWithTag("Wolf").Length +
                                          GameObject.FindGameObjectsWithTag("Bear").Length;

    public static int CountPrey() => GameObject.FindGameObjectsWithTag("Prey").Length +
                                     GameObject.FindGameObjectsWithTag("Rabbit").Length +
                                     GameObject.FindGameObjectsWithTag("Deer").Length;
  }
}