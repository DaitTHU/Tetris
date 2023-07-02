using System.Drawing;
using static Tetris.Const;

namespace Tetris
{
    public readonly struct TetroInfo
    {
        public readonly int[] codes; // int[4], 4 orientations
        public readonly Bitmap[] pieces, ghosts;
        public TetroInfo(int[] codes, Color color)
        {
            this.codes = codes;
            int module = codes.Length;
            pieces = new Bitmap[module];
            ghosts = new Bitmap[module];
            SolidBrush brush = new SolidBrush(color), 
                brushGhost = new SolidBrush(Color.FromArgb(255 / 3, color));
            for (int i = 0; i < module; i++)
            {
                pieces[i] = new Bitmap(Unit(CELL) + 1, Unit(CELL) + 1);
                ghosts[i] = new Bitmap(Unit(CELL) + 1, Unit(CELL) + 1);
                Graphics g = Graphics.FromImage(pieces[i]),
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
                        gGhost.FillRectangle(brushGhost, 
                            XU + 1, YU + 1, Unit() - 1, Unit() - 1);
                    }
                    probe >>= 1;
                }
                g.Dispose(); gGhost.Dispose();
            }
            brush.Dispose(); brushGhost.Dispose();
        }
    }
}
