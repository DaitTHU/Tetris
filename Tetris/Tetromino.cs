using System.Drawing;

namespace Tetris
{
    public readonly struct TetroInfo
    {
        public readonly int[] code; // int[4]
        public readonly Color color;
        public TetroInfo(int[] code, Color color)
        {
            this.code = code;
            this.color = color;
        }
    }

    public class Tetromino
    {
        protected const int Count = 7, // All.Length;
            SpinMod = 4;
        protected static readonly TetroInfo[] All = { I, O, T, L, J, Z, S };
        // private: 7 basic Tetromino
        private static TetroInfo I => new TetroInfo(new int[] { 0x0F00, 0x4444, 0x0F00, 0x4444 }, Color.Red);
        private static TetroInfo O => new TetroInfo(new int[] { 0x0660, 0x0660, 0x0660, 0x0660 }, Color.Orange);
        private static TetroInfo T => new TetroInfo(new int[] { 0x04E0, 0x0464, 0x00E4, 0x04C4 }, Color.Yellow);
        private static TetroInfo L => new TetroInfo(new int[] { 0x0446, 0x02E0, 0x0622, 0x0740 }, Color.Blue);
        private static TetroInfo J => new TetroInfo(new int[] { 0x2260, 0x0E20, 0x6440, 0x0470 }, Color.Purple);
        private static TetroInfo Z => new TetroInfo(new int[] { 0x0C60, 0x2640, 0x0C60, 0x2640 }, Color.Green);
        private static TetroInfo S => new TetroInfo(new int[] { 0x0360, 0x4620, 0x0360, 0x4620 }, Color.Cyan);
        // special
        private static TetroInfo X => new TetroInfo(new int[] { 0x0400, 0x0400, 0x0400, 0x0400 }, Color.White);
    }
}
