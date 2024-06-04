using System;
using System.Collections.Generic;
using System.Numerics;

namespace Logic
{
    internal class Table
    {
        private float _width;
        private float _height;
        private Dictionary<object, (Vector2 Position, Vector2 Velocity)> _balls;

        public float Width => _width;
        public float Height => _height;
        public Dictionary<object, (Vector2 Position, Vector2 Velocity)> Balls => _balls;

        public Table(float width, float height)
        {
            _width = width;
            _height = height;
            _balls = new Dictionary<object, (Vector2 Position, Vector2 Velocity)>();
        }

        public void AddBall(object ball, Vector2 position, Vector2 velocity)
        {
            _balls.Add(ball, (position, velocity));
        }

        public void UpdateBall(object ball, Vector2 newPosition, Vector2 newVelocity)
        {
            if (_balls.ContainsKey(ball))
            {
                _balls[ball] = (newPosition, newVelocity);
            }
            else
            {
                throw new ArgumentException("Specified ball object not found in the table.");
            }
        }

        public (Vector2 Position, Vector2 Velocity) GetBall(object ball)
        {
            if (_balls.ContainsKey(ball))
            {
                return _balls[ball];
            }
            else
            {
                throw new ArgumentException("Specified ball object not found in the table.");
            }
        }
    }
}
