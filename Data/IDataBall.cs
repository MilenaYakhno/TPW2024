using System.Numerics;

namespace Data
{
    public interface IDataBall
    {
        public int ID { get; }
        public float Time { get; set; }
        Vector2 Position { get; }
        Vector2 Velocity { get; set; }
    }
}
