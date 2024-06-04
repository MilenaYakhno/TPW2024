using Data;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Logic
{
    internal class Table
    {
        private float _width;
        private float _height;
        private List<IDataBall> _balls;

        public float Width { get => _width; }
        public float Height { get => _height; }
        public IEnumerable<IDataBall> Balls => _balls;

        public Table(float width, float height)
        {
            _width = width;
            _height = height;
            _balls = new List<IDataBall>();
        }

        public void AddBall(IDataBall ball)
        {
            if (_balls.Contains(ball))
            {
                throw new ArgumentException("The ball already exists in the table.");
            }

            _balls.Add(ball);
        }

        public IDataBall GetBall(IDataBall ball)
        {
            int index = _balls.IndexOf(ball);

            if (index == -1)
            {
                throw new ArgumentException("The ball does not exist in the table.");
            }
            else
            {
                return _balls[index];
            }
        }

        public int GetBallIndex(IDataBall ball)
        {
            return _balls.IndexOf(ball);
        }
    }
}
