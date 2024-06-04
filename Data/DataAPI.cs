using System.Numerics;

namespace Data
{
    public abstract class DataAPI
    {
        public static DataAPI CreateDataService()
        {
            return new DataService();
        }

        public abstract IDataBall CreateBall(Vector2 pos, Vector2 velocity, Action<IDataBall> positionUpdatedCallback = null);
    }
}
