using System;
using System.Numerics;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Data
{
    internal class Ball
    {
        private Vector2 _pos;
        private Vector2 _velocity;
        private Action<object, Vector2, Vector2> _positionUpdatedCallback;

        public Vector2 Position => _pos;
        public Vector2 Velocity { get => _velocity; set => _velocity = value; }

        public Ball(Vector2 pos, Vector2 velocity, Action<object, Vector2, Vector2> positionUpdatedCallback = null)
        {
            _pos = pos;
            _velocity = velocity;
            _positionUpdatedCallback = positionUpdatedCallback;

            Task.Run(UpdateAsync);
        }

        private async Task UpdateAsync()
        {
            Stopwatch stopwatch = new Stopwatch();
            float timeStep = 1f / 60f;

            while (true)
            {
                stopwatch.Start();
                await Task.Delay(TimeSpan.FromSeconds(timeStep));

                lock(this)
                {
                    stopwatch.Stop();

                    float deltaTime = (float)stopwatch.Elapsed.TotalSeconds;
                    _pos += _velocity * deltaTime;

                    _positionUpdatedCallback?.Invoke(this, _pos, _velocity);
                }

                stopwatch.Reset();
            }
        }
    }
}
