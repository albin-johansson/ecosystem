using System;
using System.IO;
using UnityEngine;

namespace Ecosystem.Logging
{
  /// <summary>
  ///   Used to write logged simulation data to a JSON file.
  /// </summary>
  public static class LogFileWriter
  {
    /// <summary>
    ///   The directory to which the logs are saved.
    /// </summary>
    /// <remarks>
    ///   Note, we cannot really use "Logs", because Unity already uses
    ///   that folder.
    /// </remarks>
    private const string LogsDirectory = "SimulationLogs";

    /// <summary>
    ///   Saves the supplied simulation data. The data will be stored in a file named
    ///   after the current date and time. 
    /// </summary>
    /// <param name="data">The simulation data that will be saved.</param>
    /// <seealso cref="LogsDirectory"/>
    public static void Save(LogData data)
    {
      var path = $"{LogsDirectory}/log_{GetDateTimeString()}.json";
      WriteToFile(path, JsonUtility.ToJson(data, true));
    }

    private static void WriteToFile(string path, string contents)
    {
      Directory.CreateDirectory(LogsDirectory);
      File.CreateText(path).Close(); // Create file if it doesn't exist
      File.WriteAllText(path, contents);
    }

    private static string GetDateTimeString()
    {
      var now = DateTime.Now;

      var dateStr = now.ToShortDateString().Replace("/", "-");
      var timeStr = now.ToLongTimeString().Replace(":", "");

      return $"{dateStr}_{timeStr}";
    }
  }
}