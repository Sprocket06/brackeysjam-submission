namespace Projection.UI
{
    public struct Thickness
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;

        public Thickness(int left, int top, int right, int bottom)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }

        public Thickness(int leftRight, int topBottom)
        {
            Left = Right = leftRight;
            Top = Bottom = topBottom;
        }

        public Thickness(int uniform)
            => Left = Top = Right = Bottom = uniform;
    }
}