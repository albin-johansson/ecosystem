using Unity.Entities;

namespace Ecosystem.ECS
{
  public static class EcsUtils
  {
    // Extension function for entity managers, less verbose way to initialize a prefab
    public static T LoadPrefab<T>(this EntityManager manager) where T : struct, IComponentData
    {
      return manager.CreateEntityQuery(typeof(T)).GetSingleton<T>();
    }
  }
}