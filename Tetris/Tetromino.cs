using System.Drawing;
using static Tetris.Const;

namespace Tetris
{
    public readonly struct TetroInfo
    {
        public readonly int[] codes; // int[4], 4 orientations
        public readonly Color color;
        public readonly Bitmap[] pieces, ghosts;
        public TetroInfo(int[] codes, Color color)
        {
            this.codes = codes;
            this.color = color;
            int module = codes.Length;
            pieces = new Bitmap[module];
            ghosts = new Bitmap[module];
            Graphics g, gGhost;
            SolidBrush brush = new SolidBrush(color), 
                brushGhost = new SolidBrush(Color.FromArgb(255 / 3, color));
            for (int i = 0; i < module; i++)
            {
                pieces[i] = new Bitmap(Unit(CELL) + 1, Unit(CELL) + 1);
                ghosts[i] = new Bitmap(Unit(CELL) + 1, Unit(CELL) + 1);
                g = Graphics.FromImage(pieces[i]);
                gGhost = Graphics.FromImage(ghosts[i]);
                int probe = 1 << (Pow2(CELL) - 1);
                for (int j = 0; j < Pow2(CELL); j++)
                {
                    if ((codes[i] & probe) > 0)
                    {
                        int XU = Unit(j % CELL), 
                            YU = Unit(j / CELL);
                        g.FillRectangle(brush, XU, YU, Unit(), Unit());
                        g.DrawRectangle(pen, XU, YU, Unit(), Unit());
                        gGhost.FillRectangle(brushGhost, XU + 1, YU + 1, Unit() - 1, Unit() - 1);
                    }
                    probe >>= 1;
                }
            }
        }
    }

    public abstract class Tetromino : Entity // C# don't allow multiple inheritance for Tetro
    {
        public const int TYPE = 7, // BasicSet.Length;
            SPIN = 4;
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
