using System.Drawing;

namespace Tetris
{
    public struct BasicTetro
    {
        public readonly int[] spin;
        public readonly Color color;
        public BasicTetro(int[] spin, Color color) : this()
        {
            this.spin = spin;
            this.color = color;
        }
        public static BasicTetro I => new BasicTetro(new int[] { 0x0F00, 0x4444, 0x0F00, 0x4444 }, Color.Red);
        public static BasicTetro O => new BasicTetro(new int[] { 0x0660, 0x0660, 0x0660, 0x0660 }, Color.Orange);
        public static BasicTetro T => new BasicTetro(new int[] { 0x04E0, 0x0464, 0x00E4, 0x04C4 }, Color.Yellow);
        public static BasicTetro L => new BasicTetro(new int[] { 0x0446, 0x02E0, 0x0622, 0x0740 }, Color.Green);
        public static BasicTetro J => new BasicTetro(new int[] { 0x2260, 0x0E20, 0x6440, 0x0470 }, Color.Blue);
        public static BasicTetro Z => new BasicTetro(new int[] { 0x0C60, 0x2640, 0x0C60, 0x2640 }, Color.Violet);
        public static BasicTetro S => new BasicTetro(new int[] { 0x0360, 0x4620, 0x0360, 0x4620 }, Color.Purple);
        public static BasicTetro X => new BasicTetro(new int[] { 0x0040, 0x0040, 0x0040, 0x0040 }, Color.White);
        public static BasicTetro[] All = { I, O, T, L, J, Z, S, X };
    }

    internal class Tetro
    {

    }
}
