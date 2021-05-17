using UnityEngine;

namespace Ecosystem.UI
{
  public sealed class SunRotation : MonoBehaviour
  {
    [SerializeField] private Light sun;
    [SerializeField] private ParticleSystem particles;

    private bool _isNight;
    private float _rotation;
    private const float CycleSpeed = 0.02f;

    private void Start()
    {
      particles.lights.light.intensity = 0;
      particles.Stop();
    }

    /// <summary>
    /// On each tick the sun is moved across the sky and a variable keeps track of
    /// it´s location to turn on stars when the sun has passed the horizon.
    /// Possible improvement is to make the sun traverse the sky and be visible all the way to the horizon. 
    /// </summary>
    private void Update()
    {
      sun.transform.Rotate(CycleSpeed, 0, 0, Space.Self);
      particles.transform.Rotate(0, 0, CycleSpeed * 0.25f, Space.Self);
      _rotation += CycleSpeed;

      AdjustStarBrightness();

      if (_rotation >= 180 && !_isNight)
      {
        TransformToNight();
      }

      if (_rotation >= 359 && _isNight)
      {
        TransformToDay();
      }

      if (_rotation >= 360)
      {
        _rotation = 0;
      }
    }

    /// <summary>
    /// Dims the brightness of the stars when turning to night and day to smooth the transition.  
    /// </summary>
    private void AdjustStarBrightness()
    {
      var lights = particles.lights.light;
      var intensity = lights.intensity;
      switch (_isNight)
      {
        case true when intensity < 0.4f:
          lights.intensity += 0.01f;
          break;

        case false when intensity > 0:
          lights.intensity -= 0.01f;
          break;
      }
    }

    private void TransformToNight()
    {
      particles.Play();
      _isNight = true;
    }

    private void TransformToDay()
    {
      particles.Clear();
      particles.Stop();
      _isNight = false;
    }
  }
}