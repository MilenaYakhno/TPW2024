using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Numerics;
using System.Threading.Tasks;

namespace Data
{
    internal class DataLogger
    {
        private readonly BlockingCollection<LogEntry> _logEntries;
        private readonly object _fileHandlingLock = new object();
        private readonly string _logFilePath;
        private const int MaxQueueSize = 150;

        private static DataLogger? _instance = null;

        public static DataLogger GetInstance()
        {
            return _instance ??= new DataLogger();
        }

        private DataLogger()
        {
            string projectPath = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.Parent?.FullName ?? string.Empty;
            string logDirectory = Path.Combine(projectPath, "BallLogs");

            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            _logFilePath = Path.Combine(logDirectory, "logs.json");
            _logEntries = new BlockingCollection<LogEntry>(MaxQueueSize);

            if (!File.Exists(_logFilePath))
            {
                using (FileStream stream = File.Create(_logFilePath)) { }
            }

            Task.Run(ProcessLogQueue);
        }

        public void AddLog(LogEntry logEntry)
        {
            if (!_logEntries.TryAdd(logEntry))
            {
                LogEntry overflowLogEntry = new LogEntry(
                    -1,
                    Vector2.Zero,
                    Vector2.Zero,
                    DateTime.Now,
                    "Overflow - no logged information"
                );

                _logEntries.Add(overflowLogEntry);
            }
        }

        private void ProcessLogQueue()
        {
            foreach (var logEntry in _logEntries.GetConsumingEnumerable())
            {
                lock (_fileHandlingLock)
                {
                    try
                    {
                        string serializedLogEntry = JsonConvert.SerializeObject(logEntry, Formatting.Indented);
                        File.AppendAllText(_logFilePath, serializedLogEntry + Environment.NewLine);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to write log entry: {ex.Message}");
                    }
                }
            }
        }
    }
}
