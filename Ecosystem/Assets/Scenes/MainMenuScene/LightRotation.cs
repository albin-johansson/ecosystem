using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class LightRotation : MonoBehaviour
{
  [SerializeField] private Light sun;
  [SerializeField] private Camera cam;
  [SerializeField] private new ParticleSystem particleSystem;
  [SerializeField] private Text mainText;
  private bool isNight;
  private float rotation;
  private const float CycleSpeed = 0.04f;
  

  private void Start()
  {
    particleSystem.lights.light.intensity = 0;
    particleSystem.Stop();
  }

  // Update is called once per frame
  void Update()
  {
    sun.transform.Rotate(CycleSpeed,0,0, Space.Self);
    particleSystem.transform.Rotate(0, 0, CycleSpeed, Space.Self);
    rotation = rotation + CycleSpeed;
    if (isNight && particleSystem.lights.light.intensity < 0.4f)
    {
      particleSystem.lights.light.intensity += 0.01f;
    } else if (!isNight && particleSystem.lights.light.intensity > 0)
    {
      particleSystem.lights.light.intensity -= 0.01f;
    }
    if (rotation >= 175 && !isNight)
    {
      print("Night");
      TransformToNight();
    }

    if (rotation >= 340 && isNight)
    {
      print("daytime");
      TransformToDay();
    }

    if (rotation >= 360)
    {
      rotation = 0;
    }
  }

  private void TransformToNight()
  {
    mainText.color = Color.white;
    particleSystem.Play();
    isNight = true;
  }

  private void TransformToDay()
  {
    mainText.color = Color.black;
    particleSystem.Clear();
    particleSystem.Stop();
    isNight = false;
  }
}