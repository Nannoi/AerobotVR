using UnityEngine;
using System.IO;

public class LogToFile : MonoBehaviour
{
    private string logFilePath; // Path to the log file

    void Start()
    {
        // Define the log file path (use persistentDataPath for mobile platforms)
        string logDirectory = Path.Combine(Application.persistentDataPath, "Logs");
        Directory.CreateDirectory(logDirectory); // Create directory if it doesn't exist
        logFilePath = Path.Combine(logDirectory, "log.txt");

        // Subscribe to the log message event
        Application.logMessageReceived += LogMessageReceived;
    }

    void LogMessageReceived(string condition, string stackTrace, LogType type)
    {
        string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        // Create or append to the log file
        using (StreamWriter writer = File.AppendText(logFilePath))
        {
            writer.WriteLine($"[{timestamp}] [{type}] {condition}");
            if (type == LogType.Error || type == LogType.Exception)
            {
                writer.WriteLine(stackTrace);
            }
        }
    }

    void OnDestroy()
    {// Unsubscribe from the log message event
        Application.logMessageReceived -= LogMessageReceived;
    }
}


