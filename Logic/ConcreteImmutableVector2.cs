namespace Logic
{
    internal class ConcreteImmutableVector2 : ImmutableVector2
    {
        private readonly float _x;
        private readonly float _y;

        public override float X { get => _x; }
        public override float Y { get => _y; }

        internal ConcreteImmutableVector2(float x, float y)
        {
            _x = x;
            _y = y;
        }
    }
}
