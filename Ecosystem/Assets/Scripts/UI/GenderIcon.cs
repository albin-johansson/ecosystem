using System;
using Ecosystem.Genes;
using UnityEngine;
using UnityEngine.UI;

namespace Ecosystem.UI
{
  public sealed class GenderIcon : MonoBehaviour
  {
    [SerializeField] private Image image;
    [SerializeField] private AbstractGenome genome;
    
    private Sprite _maleSprite;
    private Sprite _femaleSprite;
    private Sprite _femalePregnantSprite;

    private void OnEnable()
    {
      _maleSprite =  Resources.Load<Sprite>("Sprites/male");
      _femaleSprite =  Resources.Load<Sprite>("Sprites/female");
      _femalePregnantSprite = Resources.Load<Sprite>("Sprites/female_pregnant");
    }

    public void SetGenderIcon()
    {
      if (genome.IsMale)
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
      image.sprite = isPregnant ? _femalePregnantSprite : _femaleSprite;
    }
  }
}