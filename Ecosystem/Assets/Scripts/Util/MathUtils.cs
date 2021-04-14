using UnityEngine;

namespace Ecosystem.Util
{
  public static class MathUtils
  {
    /// <summary>
    ///   Rotates a vector by the specified amount in the Y-plane
    /// </summary>
    /// <param name="direction">the original direction vector that will be rotated.</param>
    /// <param name="amount">the amount to rotate the vector by.</param>
    /// <returns>the rotated direction vector.</returns>
    public static Vector3 RotateDirectionY(in Vector3 direction, float amount)
    {
      return Quaternion.Euler(0, amount, 0) * direction;
    }
  }
}