using Ecosystem.Components;
using Ecosystem.Logging;
using Reese.Nav;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
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
    [BurstCompile]
    public static void AddSingleton<T>(this EntityManager manager, in T singleton) where T : struct, IComponentData
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
    [BurstCompile]
    public static T GetSingleton<T>(this EntityManager manager) where T : struct, IComponentData
    {
      return manager.CreateEntityQuery(typeof(T)).GetSingleton<T>();
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

    /// <summary>
    ///   Sets the destination of an entity.
    /// </summary>
    /// <param name="buffer">the parallel entity command buffer that will be used.</param>
    /// <param name="index">the index associated with the operation, should be <c>entityInQueryIndex</c>.</param>
    /// <param name="entity">the entity that will have its destination set.</param>
    /// <param name="destination">the world position that will be set as the destination.</param>
    [BurstCompile]
    public static void SetDestination(ref ParallelBuffer buffer, int index, in Entity entity, in float3 destination)
    {
      buffer.AddComponent(index, entity, new NavDestination
      {
        Teleport = false,
        Tolerance = 2.0f,
        CustomLerp = false,
        WorldPoint = destination
      });
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
    ///   Indicates whether or not an entity with the supplied thirst component is thirsty.
    /// </summary>
    /// <param name="thirst">the current thirst state.</param>
    /// <returns><c>true</c> if the associated entity is thirsty; <c>false</c> otherwise.</returns>
    [BurstCompile]
    public static bool IsThirsty(in Thirst thirst)
    {
      return thirst.value >= 0.05 * thirst.max;
    }

    /// <summary>
    ///   Indicates whether or not an entity with the supplied hunger component is hungry.
    /// </summary>
    /// <param name="hunger">the current hunger state.</param>
    /// <returns><c>true</c> if the associated entity is hungry; <c>false</c> otherwise.</returns>
    [BurstCompile]
    public static bool IsHungry(in Hunger hunger)
    {
      return hunger.value >= 0.05 * hunger.max;
    }
  }
}