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

    private float _nextUpdateTime;
    private float _refreshRateRatio;

    private void Start()
    {
      _refreshRateRatio = 1.0f / refreshRate;
    }

    private void LateUpdate()
    {
      if (Time.unscaledTime > _nextUpdateTime)
      {
        var fps = 1.0f / Time.unscaledDeltaTime;
        text.text = fps.ToString(format);
        _nextUpdateTime = Time.unscaledTime + _refreshRateRatio;
      }
    }
  }
}