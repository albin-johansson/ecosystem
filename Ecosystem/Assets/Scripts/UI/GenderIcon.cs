using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ecosystem.UI
{
  public sealed class GenderIcon : MonoBehaviour
  {
    [SerializeField] private Image image;
    private Sprite _maleSprite;
    private Sprite _femaleSprite;

    private void Start()
    {
      _maleSprite =  Resources.Load <Sprite>("Sprites/male");
      _femaleSprite =  Resources.Load <Sprite>("Sprites/female");
    }

    public void SetGenderIcon(bool isMale)
    {
      
      if (isMale)
      {
        image.sprite = _maleSprite;
      }
      else
      {
        image.sprite = _femaleSprite;
      }
      image.color = Color.white;
    }
  }
}