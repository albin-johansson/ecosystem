using UnityEngine;

namespace Ecosystem.UI
{
  public class StateText : MonoBehaviour
  {
    [SerializeField] private TextMesh textMesh;

    public void SetText(string Text)
    {
      textMesh.text = Text;
    }
  }
}