using System.Collections.Concurrent;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Data
{
    internal class DataLogger
    {
        private readonly ConcurrentQueue<JObject> _logConcurrentQueue;
        private readonly JArray _logEntries;

        private Task? _loggingTask;
        private readonly object _fileWriteLock = new object();
        private readonly object _queueLock = new object();

        private readonly string _logFilePath;
        private const int MaxQueueSize = 100;

        private static DataLogger? _instance = null;

        public static DataLogger GetInstance()
        {
            return _instance ??= new DataLogger();
        }

        private DataLogger()
        {
            string projectPath = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.Parent?.FullName ?? string.Empty;
            string logDirectory = Path.Combine(projectPath, "BallLogs");

            _logFilePath = Path.Combine(logDirectory, "logs.json");
            _logConcurrentQueue = new ConcurrentQueue<JObject>();

            if (File.Exists(_logFilePath))
            {
                try
                {
                    string fileContent = File.ReadAllText(_logFilePath);

                    _logEntries = JArray.Parse(fileContent);
                }
                catch (JsonReaderException)
                {
                    _logEntries = new JArray();
                }
            }
            else
            {
                Directory.CreateDirectory(logDirectory);
                File.Create(_logFilePath).Dispose();

                _logEntries = new JArray();
            }
        }

        public void AddLogBall(LogBallEntry ball)
        {
            JObject logEntry = new JObject
            {
                ["ID"] = ball.ID,
                ["Time"] = ball.Time.ToString("o"),
                ["Position"] = JObject.FromObject(ball.Position)
            };

            EnqueueLogEntry(logEntry);
        }

        public void LogTable(DataAPI table)
        {
            ClearLogFile();

            JObject tableLogEntry = JObject.FromObject(table);
            _logEntries.Add(tableLogEntry);

            WriteLogEntriesToFile();
        }

        private void EnqueueLogEntry(JObject logEntry)
        {
            lock (_queueLock)
            {
                if (_logConcurrentQueue.Count < MaxQueueSize)
                {
                    _logConcurrentQueue.Enqueue(logEntry);

                    if (_loggingTask == null || _loggingTask.IsCompleted)
                    {
                        _loggingTask = Task.Run(ProcessLogQueue);
                    }
                }
                else
                {
                    JObject overflowLogEntry = new JObject
                    {
                        ["Time"] = DateTime.Now.ToString("o"),
                        ["Information"] = "Queue overflow"
                    };

                    _logEntries.Add(overflowLogEntry);
                }
            }
        }

        private void WriteLogEntriesToFile()
        {
            string serializedLogEntries = JsonConvert.SerializeObject(_logEntries, Formatting.Indented);

            lock (_fileWriteLock)
            {
                File.WriteAllText(_logFilePath, serializedLogEntries);
            }
        }

        private void ProcessLogQueue()
        {
            while (_logConcurrentQueue.TryDequeue(out JObject logEntry))
            {
                _logEntries.Add(logEntry);
            }

            WriteLogEntriesToFile();
        }

        private void ClearLogFile()
        {
            lock (_fileWriteLock)
            {
                _logEntries.Clear();

                File.WriteAllText(_logFilePath, string.Empty);
            }
        }
    }
}