using System.Numerics;

namespace Data
{
    public abstract class DataAPI
    {
        public static DataAPI CreateDataService()
        {
            return new DataService();
        }

        public abstract object CreateBall(Vector2 pos, Vector2 velocity, Action<object, Vector2, Vector2> positionUpdatedCallback = null);
        public abstract void SetBallVelocity(object ball, Vector2 newVelocity);
    }
}
