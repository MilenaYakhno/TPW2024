using Data;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace Logic
{
    internal class LogicService : LogicAPI
    {
        private DataAPI? _dataAPI;
        private Table _table;
        private float _ballRadius;
        private float _ballMass = 1.0f; // New field for ball mass
        private Timer _updateTimer;
        private float _ballSpeed = 50f;

        public override event EventHandler<List<Vector2>> OnBallsPositionsUpdated;

        private void RaiseBallPositionsUpdated(List<Vector2> positions)
        {
            OnBallsPositionsUpdated?.Invoke(this, positions);
        }

        public LogicService(DataAPI? dataAPI = null)
        {
            _dataAPI = dataAPI ?? DataAPI.CreateDataService();
        }

        public override void Start(int ballCount, float ballRadius, float tableWidth, float tableHeight)
        {
            _ballRadius = ballRadius;
            CreateTable(tableWidth, tableHeight);
            SpawnBalls(ballCount, ballRadius);
            StartUpdateTask();
        }

        private async void StartUpdateTask()
        {
            while (true)
            {
                UpdateBallPositions(null);
                await Task.Delay(TimeSpan.FromSeconds(1f / 60f));
            }
        }

        private void UpdateBallPositions(object state)
        {
            List<Vector2> positions = new List<Vector2>();

            // Create a snapshot of the balls dictionary
            Dictionary<object, (Vector2 Position, Vector2 Velocity)> ballsSnapshot = new Dictionary<object, (Vector2 Position, Vector2 Velocity)>(_table.Balls);

            // Iterate through the snapshot of the balls and get their positions
            foreach (var kvp in ballsSnapshot)
            {
                positions.Add(kvp.Value.Position);
            }

            // Raise the BallPositionsUpdated event
            RaiseBallPositionsUpdated(positions);
        }

        public override object GetTableInfo()
        {
            return _table;
        }

        private void CreateTable(float width, float height)
        {
            _table = new Table(width, height);
        }

        private void SpawnBalls(int amount, float radius)
        {
            Random random = new Random();

            int numRows = (int)Math.Ceiling(Math.Sqrt(amount));
            int numCols = (int)Math.Ceiling((float)amount / numRows);

            float maxDisplacement = radius * 2;

            Action<object, Vector2, Vector2> positionUpdatedCallback = UpdateBall;

            for (int row = 0; row < numRows; row++)
            {
                for (int col = 0; col < numCols; col++)
                {
                    float x = (float)(random.NextDouble() * maxDisplacement) + col * (_table.Width - maxDisplacement) / (numCols - 1);
                    float y = (float)(random.NextDouble() * maxDisplacement) + row * (_table.Height - maxDisplacement) / (numRows - 1);

                    Vector2 pos = new Vector2(x, y);
                    Vector2 vel = GetRandomVelocity(random) * _ballSpeed;

                    _table.AddBall(_dataAPI.CreateBall(pos, vel, positionUpdatedCallback), pos, vel);
                }
            }
        }

        private void UpdateBall(object ball, Vector2 pos, Vector2 vel)
        {
            lock (this)
            {
                _table.UpdateBall(ball, pos, vel);
                CheckWallCollision(ball, pos, vel);
                CheckBallCollision(ball, pos);
            }
        }

        private void CheckWallCollision(object ball, Vector2 Position, Vector2 Velocity)
        {
            if (_table == null)
            {
                Console.WriteLine("Table is not created yet.");
                return;
            }

            // Check collisions with the walls
            if (Position.X - _ballRadius <= 0 && Velocity.X < 0 || Position.X + _ballRadius >= _table.Width && Velocity.X > 0)
            {
                _dataAPI.SetBallVelocity(ball, new Vector2(-Velocity.X, Velocity.Y));
            }

            if (Position.Y - _ballRadius <= 0 && Velocity.Y < 0 || Position.Y + _ballRadius >= _table.Height && Velocity.Y > 0)
            {
                _dataAPI.SetBallVelocity(ball, new Vector2(Velocity.X, -Velocity.Y));
            }
        }

        private void CheckBallCollision(object ball, Vector2 position)
        {
            if (_table == null)
            {
                Console.WriteLine("Table is not created yet.");
                return;
            }

            foreach (var otherBall in _table.Balls.Keys)
            {
                if (otherBall != ball)
                {
                    var (otherPosition, _) = _table.GetBall(otherBall);
                    var distance = Vector2.Distance(position, otherPosition);
                    var totalRadius = 2 * _ballRadius; // Since _ballRadius is the radius of each ball

                    if (distance <= totalRadius)
                    {
                        ResolveBallCollision(ball, otherBall);
                    }
                }
            }
        }

        private void ResolveBallCollision(object ball1, object ball2)
        {
            var (pos1, vel1) = _table.GetBall(ball1);
            var (pos2, vel2) = _table.GetBall(ball2);

            var direction = Vector2.Normalize(pos2 - pos1);

            var relativeVelocity = vel2 - vel1;

            var relativeSpeed = Vector2.Dot(relativeVelocity, direction);

            // Check if the balls are moving towards each other
            if (relativeSpeed < 0)
            {
                var velocityChange = relativeSpeed * direction * (_ballMass / (_ballMass + _ballMass));

                var newVel1 = vel1 - velocityChange;
                var newVel2 = vel2 + velocityChange;

                var finalVel1 = Vector2.Normalize(newVel1) * _ballSpeed;
                var finalVel2 = Vector2.Normalize(newVel2) * _ballSpeed;

                _dataAPI.SetBallVelocity(ball1, finalVel2);
                _dataAPI.SetBallVelocity(ball2, finalVel1);
            }
        }

        private Vector2 GetRandomVelocity(Random random)
        {
            float angle = (float)(random.NextDouble() * 2 * Math.PI);
            return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }
    }
}
