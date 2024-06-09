using System.Numerics;

namespace Data
{
    internal class LogBallEntry
    {
        public int ID;
        public Vector2 Position;
        public Vector2 Velocity;
        public DateTime Time;

        public LogBallEntry(int id, Vector2 position, Vector2 velocity, DateTime t)
        {
            ID = id;
            Position = position;
            Velocity = velocity;
            Time = t;
        }
    }
}
