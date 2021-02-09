using UnityEngine;

public sealed class AnimStatesController : MonoBehaviour
{
 private static State _animstate = State.Idle;

 public static State Animstate { get; set; }
}
