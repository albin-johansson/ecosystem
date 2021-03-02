using UnityEngine;

namespace Ecosystem.UI
{
  public class SunRotation : MonoBehaviour
  {
    [SerializeField] private Light sun;
    [SerializeField] private new ParticleSystem particleSystem;
    [SerializeField] private ButtonListColor buttonListColor;

    private bool _isNight;
    private float _rotation;
    private const float CycleSpeed = 0.04f;

    private void Start()
    {
      particleSystem.lights.light.intensity = 0;
      particleSystem.Stop();
    }

    /// <summary>
    /// On each tick the sun is moved across the sky and a variable keeps track of
    /// it´s location to turn on stars when the sun has passed the horizon.
    /// Possible improvement is to make the sun traverse the sky and be visible all the way to the horizon. 
    /// </summary>
    void Update()
    {
      sun.transform.Rotate(CycleSpeed, 0, 0, Space.Self);
      particleSystem.transform.Rotate(0, 0, CycleSpeed * 0.5f, Space.Self);
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
      if (_isNight && particleSystem.lights.light.intensity < 0.4f)
      {
        particleSystem.lights.light.intensity += 0.01f;
      }
      else if (!_isNight && particleSystem.lights.light.intensity > 0)
      {
        particleSystem.lights.light.intensity -= 0.01f;
      }
    }

    private void TransformToNight()
    {
      buttonListColor.ChangeButtonListColor(Color.white);
      particleSystem.Play();
      _isNight = true;
    }

    private void TransformToDay()
    {
      buttonListColor.ChangeButtonListColor(Color.black);
      particleSystem.Clear();
      particleSystem.Stop();
      _isNight = false;
    }
  }
}