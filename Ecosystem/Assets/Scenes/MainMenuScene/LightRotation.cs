using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class LightRotation : MonoBehaviour
{
  [SerializeField] private Light sun;
  [SerializeField] private Camera cam;
  [SerializeField] private ParticleSystem particleSystem;
  private bool isNight;
  private float rotation;
  private float intesityTemp;
  

  private void Start()
  {
    particleSystem.Stop();
  }

  // Update is called once per frame
  void Update()
  {
    sun.transform.Rotate(0.02f,0,0, Space.Self);
    particleSystem.transform.Rotate(0, 0, 0.02f, Space.Self);
    rotation = rotation + 0.02f;
    if (isNight && particleSystem.lights.light.intensity < 1)
    {
      
    }
    if (rotation >= 170 && !isNight)
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
    particleSystem.Play();
    isNight = true;
  }

  private void TransformToDay()
  {
    particleSystem.Clear();
    particleSystem.Stop();
    isNight = false;
  }
}