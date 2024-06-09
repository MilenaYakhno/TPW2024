using System;
using System.Diagnostics;
using System.Numerics;

namespace Data
{
    internal class DataBall : IDataBall
    {
        private Vector2 _velocity;
        private Vector2 _position;
        private Action<IDataBall, Vector2, Vector2>? _positionUpdatedCallback;
        private readonly object _lock = new object();
        private float _elapsedTime;

        private readonly DataLogger _logger = DataLogger.GetInstance();

        public int ID { get; }
        public float Time
        {
            get => _elapsedTime;
            set => _elapsedTime = value;
        }

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
                    return _position;
                }
            }
        }

        public DataBall(int id, Vector2 pos, Vector2 velocity, Action<IDataBall, Vector2, Vector2>? positionUpdatedCallback = null)
        {
            ID = id;
            _position = pos;
            _velocity = velocity;
            _positionUpdatedCallback = positionUpdatedCallback;

            Task.Run(UpdateAsync);
        }

        private async Task UpdateAsync()
        {
            const float timeStep = 1f / 60f; // 60 FPS
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            while (true)
            {
                long previousTime = stopwatch.ElapsedMilliseconds;
                await Task.Delay(TimeSpan.FromSeconds(timeStep));

                lock (_lock)
                {
                    long currentTime = stopwatch.ElapsedMilliseconds;
                    float elapsedTime = (currentTime - previousTime) / 1000f;

                    _position += _velocity * elapsedTime;
                    Time = elapsedTime;

                    //Log
                    LogBallEntry logBall = new LogBallEntry(ID, new Vector2(_position.X, _position.Y), new Vector2(_velocity.X, _velocity.Y), DateTime.Now);
                    _logger.AddLogBall(logBall);
                }

                _positionUpdatedCallback?.Invoke(this, _position, _velocity);
            }
        }
    }
}
