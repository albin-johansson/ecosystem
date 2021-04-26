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
    private Sprite _femalePregnantSprite;

    private void Start()
    {
      _maleSprite =  Resources.Load <Sprite>("Sprites/male");
      _femaleSprite =  Resources.Load <Sprite>("Sprites/female");
      _femalePregnantSprite = Resources.Load <Sprite>("Sprites/female_pregnant");
        
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

    public void SetPregnancyIcon(bool isPregnant)
    {
      if (isPregnant)
      {
        image.sprite = _femalePregnantSprite;
      }
      else
      {
        image.sprite = _femaleSprite;
      }
    }
  }
}