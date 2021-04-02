using Unity.Entities;
using UnityEngine.SceneManagement;

namespace Ecosystem.ECS
{
  /// <summary>
  ///   A utility class for working with the ECS framework.
  /// </summary>
  public static class EcsUtils
  {
    // Extension function for entity managers to obtain a "singleton" component
    public static T GetSingleton<T>(this EntityManager manager) where T : struct, IComponentData
    {
      return manager.CreateEntityQuery(typeof(T)).GetSingleton<T>();
    }

    // Simple utility for checking if a scene is known to support the ECS framework
    public static bool IsEcsCapable(Scene scene)
    {
      return scene.name == "ECSDemo";
    }
  }
}