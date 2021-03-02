using UnityEngine;
using UnityEngine.UI;

public class LightRotation : MonoBehaviour
{
  [SerializeField] private Light sun;
  [SerializeField] private new ParticleSystem particleSystem;
  [SerializeField] private ButtonListColorChange buttonListColorChange;
  
  private float _rotation;
  private const float CycleSpeed = 0.04f;

  public bool IsNight { get; private set; }


  private void Start()
  {
    particleSystem.lights.light.intensity = 0;
    particleSystem.Stop();
  }

  void Update()
  {
    sun.transform.Rotate(CycleSpeed, 0, 0, Space.Self);
    particleSystem.transform.Rotate(0, 0, CycleSpeed * 0.5f, Space.Self);
    _rotation += CycleSpeed;

    AdjustStarBrightness();

    if (_rotation >= 180 && !IsNight)
    {
      TransformToNight();
    }

    if (_rotation >= 359 && IsNight)
    {
      TransformToDay();
    }

    if (_rotation >= 360)
    {
      _rotation = 0;
    }
  }

  private void AdjustStarBrightness()
  {
    if (IsNight && particleSystem.lights.light.intensity < 0.4f)
    {
      particleSystem.lights.light.intensity += 0.01f;
    }
    else if (!IsNight && particleSystem.lights.light.intensity > 0)
    {
      particleSystem.lights.light.intensity -= 0.01f;
    }
  }

  private void TransformToNight()
  {
    buttonListColorChange.ChangeButtonListColor(Color.white);
    particleSystem.Play();
    IsNight = true;
  }

  private void TransformToDay()
  {
    buttonListColorChange.ChangeButtonListColor(Color.black);
    particleSystem.Clear();
    particleSystem.Stop();
    IsNight = false;
  }
}