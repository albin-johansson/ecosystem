using UnityEngine;

namespace Ecosystem.Util
{
  /// <summary>
  ///   A utility class related to game object tags. An important use case for tags is counting
  ///   the amount of different kinds of game objects, such as prey, predators and food.
  /// </summary>
  public static class Tags
  {
    public static bool IsPredator(GameObject animal) => animal.CompareTag("Wolf") ||
                                                        animal.CompareTag("Bear");

    public static bool IsPrey(GameObject animal) => animal.CompareTag("Rabbit") ||
                                                    animal.CompareTag("Deer");

    /// <summary>
    ///   Indicates whether or not the supplied game object has a tag that indicates that it's a food item.
    /// </summary>
    /// <param name="gameObject">the game object that will be checked</param>
    /// <returns><c>true</c> if the game object is a food item; <c>false</c> otherwise.</returns>
    public static bool IsFood(GameObject gameObject) => gameObject.CompareTag("Carrot");

    /// <summary>
    ///   Counts the current amount of game objects with the specified tag.
    /// </summary>
    /// <remarks>
    ///   The supplied string must be a valid tag.
    /// </remarks>
    /// <param name="tag">the tag that will be counted.</param>
    /// <returns>the current amount of game objects with the specified tag.</returns>
    public static int Count(string tag) => GameObject.FindGameObjectsWithTag(tag).Length;

    /// <summary>
    ///   Returns the current amount of predator game objects.
    /// </summary>
    /// <returns>the current amount of predators.</returns>
    public static int CountPredators() => Count("Wolf") + Count("Bear");

    /// <summary>
    ///   Returns the current amount of prey game objects.
    /// </summary>
    /// <returns>the current amount of prey.</returns>
    public static int CountPrey() => Count("Rabbit") + Count("Deer");

    /// <summary>
    ///   Returns the current amount of food item game objects.
    /// </summary>
    /// <returns>the current amount of food items.</returns>
    public static int CountFood() => Count("Carrot");
  }
}