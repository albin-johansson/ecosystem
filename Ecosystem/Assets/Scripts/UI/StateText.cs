using TMPro;
using UnityEngine;

namespace Ecosystem.UI
{
  public sealed class StateText : MonoBehaviour
  {
    [SerializeField] private TMP_Text textMesh;

    public void SetText(string text)
    {
      textMesh.text = text;
    }
  }
}