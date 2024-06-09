using System.Numerics;

namespace Data
{
    internal class DataService : DataAPI
    {
        public override IDataBall CreateBall(int id, Vector2 pos, Vector2 velocity, Action<IDataBall, Vector2, Vector2> positionUpdatedCallback = null)
        {
            return new DataBall(id, pos, velocity, positionUpdatedCallback);
        }
    }
}
