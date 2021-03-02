using UnityEngine;
using UnityEngine.UI;

namespace Ecosystem.UI
{
  public class ButtonListColor : MonoBehaviour
  {
    [SerializeField] private Image buttonBackground;
    [SerializeField] private Text headline;

    /// <summary>
    /// Turns the background of the scene buttons and headline into selected color
    /// </summary>
    /// <param name="color"></param>
    public void ChangeButtonListColor(Color color)
    {
      buttonBackground.color = color;
      headline.color = color;
    }
  }
}