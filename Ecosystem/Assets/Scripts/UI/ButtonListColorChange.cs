using UnityEngine;
using UnityEngine.UI;

namespace Ecosystem.UI
{
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
}