using UnityEngine;
using UnityEngine.UI;

namespace Ecosystem.UI
{
  /// <summary>
  /// A simple script for monitoring the current frame rate and displaying
  /// it via a text label.
  /// </summary>
  public sealed class FPSCounter : MonoBehaviour
  {
    [SerializeField] private Text text;
    [SerializeField] private string format = "0.0";

    [SerializeField, Tooltip("How many times the label is updated, per second")]
    private float refreshRate = 3;

    private static readonly float[] History = new float[100];

    private float _nextUpdateTime;
    private float _refreshRateRatio;
    private int _index;

    private void Start()
    {
      _refreshRateRatio = 1.0f / refreshRate;
    }

    private void LateUpdate()
    {
      if (Time.unscaledTime > _nextUpdateTime)
      {
        var fps = 1.0f / Time.unscaledDeltaTime;

        ++_index;
        _index %= History.Length;
        History[_index] = fps;

        text.text = fps.ToString(format);
        _nextUpdateTime = Time.unscaledTime + _refreshRateRatio;
      }
    }

    private void OnApplicationQuit()
    {
      Debug.Log($"Min FPS: {GetMinimumFPS()}");
      Debug.Log($"Max FPS: {GetMaximumFPS()}");
      Debug.Log($"Average FPS: {GetAverageFPS()}");
    }

    /// Returns the minimum FPS recorded in the frame rate history.
    public static float GetMinimumFPS()
    {
      var min = History[0];

      foreach (var fps in History)
      {
        min = Mathf.Min(min, fps);
      }

      return min;
    }

    /// Returns the maximum FPS recorded in the frame rate history.
    public static float GetMaximumFPS()
    {
      var max = History[0];

      foreach (var fps in History)
      {
        max = Mathf.Max(max, fps);
      }

      return max;
    }

    /// Returns the average FPS of the frame rate history.
    public static float GetAverageFPS()
    {
      float sum = 0;

      foreach (var fps in History)
      {
        sum += fps;
      }

      return sum / History.Length;
    }
  }
}