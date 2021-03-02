using UnityEngine;
using UnityEngine.UI;

namespace Ecosystem.UI
{
  public sealed class MenuColorManager : MonoBehaviour
  {
    [SerializeField] private Image buttonBackground;
    [SerializeField] private Text headline;

    /// <summary>
    /// Turns the background of the scene buttons and headline into selected color
    /// </summary>
    public void ChangeColor(Color color)
    {
      buttonBackground.color = color;
      headline.color = color;
    }
  }
}