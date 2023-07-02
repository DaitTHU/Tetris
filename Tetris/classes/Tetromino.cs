using System.Drawing;

namespace Tetris
{
    // C# don't allow multiple inheritance for Tetro
    public abstract class Tetromino : Entity 
    {
        public const int TYPE = 7; // BasicSet.Length;
        // private: 7 basic Tetromino
        private static readonly TetroInfo I = new TetroInfo(new int[]
            { 0x0F00, 0x4444, 0x0F00, 0x4444 }, Color.Red);
        private static readonly TetroInfo O = new TetroInfo(new int[] 
            { 0x0660, 0x0660, 0x0660, 0x0660 }, Color.Orange);
        private static readonly TetroInfo T = new TetroInfo(new int[] 
            { 0x04E0, 0x0464, 0x00E4, 0x04C4 }, Color.Yellow);
        private static readonly TetroInfo L = new TetroInfo(new int[] 
            { 0x0446, 0x02E0, 0x0622, 0x0740 }, Color.Blue);
        private static readonly TetroInfo J = new TetroInfo(new int[] 
            { 0x2260, 0x0E20, 0x6440, 0x0470 }, Color.Purple);
        private static readonly TetroInfo Z = new TetroInfo(new int[] 
            { 0x0C60, 0x2640, 0x0C60, 0x2640 }, Color.Green);
        private static readonly TetroInfo S = new TetroInfo(new int[] 
            { 0x0360, 0x4620, 0x0360, 0x4620 }, Color.Cyan);
        // private static readonly TetroInfo X = new TetroInfo(new int[]
        // { 0x0400, 0x0400, 0x0400, 0x0400 }, Color.White);
        protected static readonly TetroInfo[] BasicSet = { I, O, T, L, J, Z, S };
    }
}
