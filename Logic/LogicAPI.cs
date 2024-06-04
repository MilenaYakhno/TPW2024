using Data;

namespace Logic
{
    public abstract class LogicAPI
    {
        public abstract event EventHandler<BallPositionEventArgs> OnBallPositionUpdated;

        public static LogicAPI CreateLogicService(DataAPI dataAPI = null)
        {
            return new LogicService(dataAPI);
        }

        public abstract void Start(int ballCount, float ballRadius, float tableWidth, float tableHeight);
        public abstract object GetTableInfo();
    }
}