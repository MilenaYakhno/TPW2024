using System.Numerics;

namespace Data
{
    internal class LogEntry
    {
        public int ID { get; }
        public Vector2 Position { get; }
        public Vector2 Velocity { get; }
        public DateTime Time { get; }

        public LogEntry(int id, Vector2 position, Vector2 velocity, DateTime t)
        {
            ID = id;
            Position = position;
            Velocity = velocity;
            Time = t;
        }
    }
}
