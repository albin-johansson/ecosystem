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
    /// <summary>
    ///   Adds a singleton component to the entity manager.
    /// </summary>
    /// <param name="manager">the source entity manager.</param>
    /// <param name="singleton">the component that will be added to the entity manager as a singleton.</param>
    /// <typeparam name="T">the type of the singleton component.</typeparam>
    public static void AddSingleton<T>(this EntityManager manager, T singleton) where T : struct, IComponentData
    {
      manager.AddComponentData(manager.CreateEntity(), singleton);
    }

    /// <summary>
    ///   Returns a singleton component, i.e. a component that there only exists a single instance of in the the entity
    ///   manager.
    /// </summary>
    /// <param name="manager">the source entity manager.</param>
    /// <typeparam name="T">the type of the singleton component.</typeparam>
    /// <returns>the singleton component.</returns>
    public static T GetSingleton<T>(this EntityManager manager) where T : struct, IComponentData
    {
      return manager.CreateEntityQuery(typeof(T)).GetSingleton<T>();
    }

    /// <summary>
    ///   Indicates whether or not a scene supports the ECS simulation framework.
    /// </summary>
    /// <param name="scene">the scene that will be checked.</param>
    /// <returns><c>true</c> if the scene supports the ECS framework; <c>false</c> otherwise.</returns>
    public static bool IsEcsCapable(Scene scene)
    {
      return scene.name == "ECSDemo";
    }

    /// <summary>
    ///   Marks an animal entity as dead and stops any potential movement of the animal.
    /// </summary>
    /// <param name="buffer">the parallel entity command buffer that will be used.</param>
    /// <param name="index">the index associated with the operation, should be <c>entityInQueryIndex</c>.</param>
    /// <param name="entity">the entity that will be killed.</param>
    /// <param name="time">the time point that will be associated with the death of the entity.</param>
    /// <param name="cause">the reason that the animal died.</param>
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