using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Numerics;

namespace Data
{
    internal class DataLogger : IDisposable
    {
        private readonly BlockingCollection<LogEntry> _buffer;
        private readonly int _bufferSize = 100;
        private readonly string _logFilePath;
        private readonly StreamWriter _streamWriter;

        private static DataLogger? _instance = null;

        public static DataLogger GetInstance()
        {
            return _instance ??= new DataLogger();
        }

        private DataLogger()
        {
            _logFilePath = "logs.json";
            _buffer = new BlockingCollection<LogEntry>(_bufferSize);
            _streamWriter = new StreamWriter(_logFilePath, true);

            Task.Run(ProcessLogQueue);
        }

        public void AddLog(LogEntry logEntry)
        {
            bool isLogEntryAdded = _buffer.TryAdd(logEntry);

            if (!isLogEntryAdded)
            {
                Console.WriteLine($"Failed to write log entry: buffer overflow");
            }
        }

        private void ProcessLogQueue()
        {
            foreach (var logEntry in _buffer.GetConsumingEnumerable())
            {
                try
                {
                    string serializedLogEntry = JsonConvert.SerializeObject(logEntry, Formatting.Indented);

                    _streamWriter.WriteLine(serializedLogEntry);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to write log entry: {ex.Message}");
                }
            }
        }

        public void Dispose()
        {
            _buffer.Dispose();
            _streamWriter.Dispose();
        }
    }
}
