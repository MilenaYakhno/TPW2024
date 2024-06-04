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
            Vector2 initialPosition = new Vector2(10, 10);
            Vector2 initialVelocity = new Vector2(1, -1);

            DataAPI dataAPI = DataAPI.CreateDataService();

            // Act
            object ballObject = dataAPI.CreateBall(initialPosition, initialVelocity);
            Ball ball = ballObject as Ball;

            // Assert
            Assert.IsNotNull(ball, "Failed to create ball object.");

            Assert.AreEqual(initialPosition, ball.Position, "Initial position does not match.");
            Assert.AreEqual(initialVelocity, ball.Velocity, "Initial velocity does not match.");
        }
    }
}
