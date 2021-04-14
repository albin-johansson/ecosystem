using UnityEngine;

namespace Ecosystem.UI
{
  public sealed class StateText : MonoBehaviour
  {
    [SerializeField] private TextMesh textMesh;

    public void SetText(string text)
    {
      textMesh.text = text;
    }
  }
}