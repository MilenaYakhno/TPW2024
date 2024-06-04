namespace Logic
{
    internal class ConcreteBallPositionEventArgs : BallPositionEventArgs
    {
        private readonly int _index;
        private readonly ImmutableVector2 _position;

        public override int Index { get => _index; }
        public override ImmutableVector2 Position { get => _position; }

        public ConcreteBallPositionEventArgs(int index, ImmutableVector2 position)
        {
            _index = index;
            _position = position;
        }
    }
}
