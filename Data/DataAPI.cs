using System.Numerics;

namespace Data
{
    public abstract class DataAPI
    {
        public static DataAPI CreateDataService()
        {
            return new DataService();
        }

        public abstract IDataBall CreateBall(int id,Vector2 pos, Vector2 velocity, Action<IDataBall, Vector2, Vector2> positionUpdatedCallback = null);
    }
}
