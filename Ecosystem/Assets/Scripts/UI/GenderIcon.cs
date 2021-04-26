using UnityEngine;
using UnityEngine.UI;

namespace Ecosystem.UI
{
  public sealed class GenderIcon : MonoBehaviour
  {
    private Sprite _sprite;
    
    public void SetGenderIcon(bool isMale)
    {
      var image = gameObject.GetComponent<Image>();
      if (isMale)
      {
        _sprite =  Resources.Load <Sprite>("Sprites/male");
        image.sprite = _sprite;
      }
      else
      {
        _sprite =  Resources.Load <Sprite>("Sprites/female");
        image.sprite = _sprite;
      }
      image.color = Color.white;
    }
  }
}