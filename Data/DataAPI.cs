﻿using System.Numerics;

namespace Data
{
    public abstract class DataAPI
    {
        public static DataAPI CreateDataService()
        {
            return new DataService();
        }

        public abstract object CreateBall(Vector2 pos, Vector2 velocity);
        public abstract Vector2 GetBallPosition(object ball);
        public abstract Vector2 GetBallVelocity(object ball);
    }
}