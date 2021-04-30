using UnityEngine.Analytics;

namespace Ecosystem.Util
{
  public static class SessionTime
  {
    private static long _sceneStartTime;

    /// Returns the duration of the current session, in milliseconds.
    public static long Now() => AnalyticsSessionInfo.sessionElapsedTime;

    /// Returns the duration of the session in a scene, in milliseconds.
    public static long NowSinceSceneStart() => Now() - _sceneStartTime;

    public static void SetSceneStart(long startTime)
    {
      _sceneStartTime = startTime;
    }
  }
}