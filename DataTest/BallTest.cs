using Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace DataTest
{
    [TestClass]
    public class BallTest
    {
        [TestMethod]
        public void TestBallCreation()
        {
            // Arrange
            int id = 1;
            Vector2 initialPosition = new Vector2(10, 10);
            Vector2 initialVelocity = new Vector2(1, -1);

            DataAPI dataAPI = DataAPI.CreateDataService();

            // Act
            object ballObject = dataAPI.CreateBall(id, initialPosition, initialVelocity);
            DataBall ball = ballObject as DataBall;

            // Assert
            Assert.IsNotNull(ball, "Failed to create ball object.");

            Assert.AreEqual(initialPosition, ball.Position, "Initial position does not match.");
            Assert.AreEqual(initialVelocity, ball.Velocity, "Initial velocity does not match.");
        }
    }
}
