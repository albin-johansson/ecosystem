using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonListColorChange : MonoBehaviour
{
  [SerializeField] private Image buttonBackground;
  [SerializeField] private Text headline;

  public void ChangeButtonListColor(Color color)
  {
    buttonBackground.color = color;
    headline.color = color;
  }
}