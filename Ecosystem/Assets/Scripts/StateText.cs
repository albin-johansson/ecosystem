using UnityEngine;

namespace Ecosystem
{
  public class StateText : MonoBehaviour
  {
    [SerializeField] private TextMesh _textMesh;

    public void SetText(string Text)
    {
      _textMesh.text = Text;
    }
  }
}