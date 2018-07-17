namespace Assets.Scripts.Match3Engine
{
    public class Match3TileModel
    {
        public Match3TileModel(int x, int y, int index)
        {
            X = x;
            Y = y;
            Index = index;
        }

        public int X { get; private set; }
        public int Y { get; private set; }
        public int Index { get; private set; }

        internal void ChangeCoordinates(int? x = null, int? y = null)
        {
            X = x ?? X;
            Y = y ?? Y;
        }
    }
}
