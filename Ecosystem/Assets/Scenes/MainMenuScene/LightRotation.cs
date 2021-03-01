using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class NewBehaviourScript : MonoBehaviour
{
  private DirectionalLight sun;
  private float ypos;
  private float zpos;
  private float wpos;

  private void Start()
  {
    ypos = sun.orientation.y;
    zpos = sun.orientation.z;
    wpos = sun.orientation.w;
  }

  // Update is called once per frame
  void Update()
  {
    sun.orientation.Set(sun.orientation.x + 1, ypos, zpos, wpos);
    if (sun.orientation.x > 360)
    {
      sun.orientation.Set(0, ypos, zpos, wpos);
    }
  }
}