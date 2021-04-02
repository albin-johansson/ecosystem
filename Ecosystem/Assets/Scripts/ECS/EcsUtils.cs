using Ecosystem.Components;
using Ecosystem.Logging;
using Reese.Nav;
using Unity.Burst;
using Unity.Entities;
using UnityEngine.SceneManagement;
using ParallelBuffer = Unity.Entities.EntityCommandBuffer.ParallelWriter;

namespace Ecosystem.ECS
{
  /// <summary>
  ///   A utility class for working with the ECS framework.
  /// </summary>
  public static class EcsUtils
  {
    public static void AddSingleton<T>(this EntityManager manager, T singleton) where T : struct, IComponentData
    {
      manager.AddComponentData(manager.CreateEntity(), singleton);
    }

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

    [BurstCompile]
    public static void Kill(ref ParallelBuffer buffer, int index, in Entity entity, double time, CauseOfDeath cause)
    {
      buffer.AddComponent(index, entity, new Dead
      {
              when = time,
              cause = cause
      });
      buffer.AddComponent<NavStop>(index, entity);
    }
  }
}