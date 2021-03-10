using UnityEngine.Analytics;

namespace Ecosystem.Util
{
  public static class SessionTime
  {
    /// <summary>
    ///   Returns the duration of the current session, in milliseconds.
    /// </summary>
    /// <returns>the time since the session started, in milliseconds.</returns>
    public static long Now() => AnalyticsSessionInfo.sessionElapsedTime;
  }
}