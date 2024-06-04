using System.Numerics;

namespace Data
{
    internal class DataService : DataAPI
    {
        public override IDataBall CreateBall(Vector2 pos, Vector2 velocity, Action<IDataBall> positionUpdatedCallback = null)
        {
            return new Ball(pos, velocity, positionUpdatedCallback);
        }
    }
}
