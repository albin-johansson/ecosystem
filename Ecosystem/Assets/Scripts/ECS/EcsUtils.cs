using Unity.Entities;
using UnityEngine.SceneManagement;

namespace Ecosystem.ECS
{
  public static class EcsUtils
  {
    // Extension function for entity managers, less verbose way to initialize a prefab
    public static T LoadPrefab<T>(this EntityManager manager) where T : struct, IComponentData
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