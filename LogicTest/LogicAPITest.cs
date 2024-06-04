using Data;
using Logic;
using System.Numerics;

namespace LogicTest
{
    [TestClass]
    public class LogicAPITest
    {
        [TestMethod]
        public void TestCreateTableAndSpawnBalls()
        {
            MockDataAPI mockDataAPI = new MockDataAPI();
            LogicAPI logicAPI = LogicAPI.CreateLogicService(mockDataAPI);

            logicAPI.Start(20, 2, 100, 200);

            Table table = (Table)logicAPI.GetTableInfo();

            Assert.AreEqual(100, table.Width, "Table width is incorrect.");
            Assert.AreEqual(200, table.Height, "Table height is incorrect.");

            Assert.AreEqual(20, table.Balls.Count(), "Incorrect number of balls spawned.");
        }
    }

    class MockDataAPI : Data.DataAPI
    {
        public override IDataBall CreateBall(Vector2 pos, Vector2 velocity, Action<IDataBall> positionUpdatedCallback = null)
        {
            return new MockDataBall(pos, velocity, positionUpdatedCallback);
        }
    }

    class MockDataBall : IDataBall
    {
        public Vector2 Position { get; private set; }
        public Vector2 Velocity { get; set; }

        private readonly Action<IDataBall> _positionUpdatedCallback;

        public MockDataBall(Vector2 pos, Vector2 velocity, Action<IDataBall> positionUpdatedCallback)
        {
            Position = pos;
            Velocity = velocity;
            _positionUpdatedCallback = positionUpdatedCallback;
        }
    }
}
