using System.Diagnostics;
using System.Numerics;

namespace Data
{
    internal class Ball : IDataBall
    {
        private Vector2 _velocity;
        private Vector2 _pos;
        private Action<IDataBall> _positionUpdatedCallback;
        private readonly object _lock = new object();

        public Vector2 Velocity
        {
            get
            {
                lock (_lock)
                {
                    return _velocity;
                }
            }
            set
            {
                lock (_lock)
                {
                    _velocity = value;
                }
            }
        }

        public Vector2 Position
        {
            get
            {
                lock (_lock)
                {
                    return _pos;
                }
            }
        }

        public Ball(Vector2 pos, Vector2 velocity, Action<IDataBall> positionUpdatedCallback = null)
        {
            _pos = pos;
            _velocity = velocity;
            _positionUpdatedCallback = positionUpdatedCallback;

            Task.Run(UpdateAsync);
        }

        private async Task UpdateAsync()
        {
            float timeStep = 1f / 60f;
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();


            while (true)
            {
                TimeSpan previousTime = stopwatch.Elapsed;
                await Task.Delay(TimeSpan.FromSeconds(timeStep));

                lock (_lock)
                {
                    _pos += _velocity * (float)(stopwatch.Elapsed - previousTime).TotalSeconds;
                }

                _positionUpdatedCallback?.Invoke(this);
            }
        }
    }
}
