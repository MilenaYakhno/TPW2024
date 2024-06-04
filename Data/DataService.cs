using System;
using System.Numerics;

namespace Data
{
    internal class DataService : DataAPI
    {
        public override object CreateBall(Vector2 pos, Vector2 velocity, Action<object, Vector2, Vector2> positionUpdatedCallback = null)
        {
            return new Ball(pos, velocity, positionUpdatedCallback);
        }
        public override void SetBallVelocity(object ball, Vector2 newVelocity)
        {
            if (ball is Ball)
            {
                Ball castedBall = (Ball)ball;
                castedBall.Velocity = newVelocity;
            }
            else
            {
                throw new ArgumentException("The provided object is not a valid ball.");
            }
        }
    }
}
