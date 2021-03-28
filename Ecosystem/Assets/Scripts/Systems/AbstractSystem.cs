using Ecosystem.ECS;
using Unity.Entities;
using UnityEngine.SceneManagement;

namespace Ecosystem.Systems
{
  // This class helps with making sure that systems are only enabled in ECS-compatible scenes 
  public abstract class AbstractSystem : SystemBase
  {
    protected virtual void Initialize()
    {
      // Do nothing by default, systems can override this to do initialize their data
    }

    private void OnSceneChanged(Scene current, Scene next)
    {
      Enabled = EcsUtils.IsEcsCapable(next);
      if (Enabled)
      {
        Initialize();
      }
    }

    protected override void OnCreate()
    {
      base.OnCreate();
      SceneManager.activeSceneChanged += OnSceneChanged;
    }
  }
}