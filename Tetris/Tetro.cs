using System;
using System.Drawing;

namespace Tetris
{
    internal struct Tetromino
    {
        public readonly int[] code;
        public readonly Color color;
        public Tetromino(int[] code, Color color) : this()
        {
            this.code = code;
            this.color = color;
        }
        public static Tetromino I => new Tetromino(new int[] { 0x0F00, 0x4444, 0x0F00, 0x4444 }, Color.Red);
        public static Tetromino O => new Tetromino(new int[] { 0x0660, 0x0660, 0x0660, 0x0660 }, Color.Orange);
        public static Tetromino T => new Tetromino(new int[] { 0x04E0, 0x0464, 0x00E4, 0x04C4 }, Color.Yellow);
        public static Tetromino L => new Tetromino(new int[] { 0x0446, 0x02E0, 0x0622, 0x0740 }, Color.Green);
        public static Tetromino J => new Tetromino(new int[] { 0x2260, 0x0E20, 0x6440, 0x0470 }, Color.Blue);
        public static Tetromino Z => new Tetromino(new int[] { 0x0C60, 0x2640, 0x0C60, 0x2640 }, Color.Violet);
        public static Tetromino S => new Tetromino(new int[] { 0x0360, 0x4620, 0x0360, 0x4620 }, Color.Purple);
        public static Tetromino X => new Tetromino(new int[] { 0x0040, 0x0040, 0x0040, 0x0040 }, Color.White);
        public static Tetromino[] All = { I, O, T, L, J, Z, S, X };
        // public static int Count = All.Length;
    }

    internal struct Tetro
    {
        public readonly int id; // [0, Tetromino.Count - 1]
        public int spin; // [0, 3]
        public int x, y; // block coordinate
        // static properties: field map, preview
        private static bool[,] map = new bool[Tetris.COL, Tetris.ROW];
        internal static Bitmap field, preview;
        private static readonly Random random = new Random();
        public Tetro(int id, int spin, int x = 0, int y = 0)
        {
            this.id = id;
            this.spin = spin;
            this.x = x;
            this.y = y;
        }

        public static Tetro New()
        {
            return new Tetro(random.Next(7), random.Next(4));
        }

        public Tetro Inherit()
        {
            Tetro tetro = new Tetro(id, spin, Tetris.COL / 2 - 2, -3);
            int code = Tetromino.All[id].code[spin];
            int probe = 0xF;
            while ((code & probe) == 0)
            {
                tetro.y++; // Move down until the tetro appear
                probe <<= 4;
            }
            return tetro;
        }

        public Bitmap Show()
        {
            Bitmap bitmap = new Bitmap(4 * Tetris.UNIT + 1, 4 * Tetris.UNIT + 1);
            Graphics g = Graphics.FromImage(bitmap);
            Pen pen = new Pen(Color.White);
            Brush brush = new SolidBrush(Tetromino.All[id].color);
            int code = Tetromino.All[id].code[spin];
            int probe = 0x8000;
            int x, y; // bit coordinate
            for (int i = 0; i < 16; i++)
            {
                if ((code & probe) > 0)
                {
                    x = (i % 4) * Tetris.UNIT; y = (i / 4) * Tetris.UNIT;
                    g.FillRectangle(brush, x, y, Tetris.UNIT, Tetris.UNIT);
                    g.DrawRectangle(pen, x, y, Tetris.UNIT, Tetris.UNIT);
                }
                probe >>= 1;
            }
            return bitmap;
        }

        public bool Feasible()
        {
            // i.d. collision detect
            int code = Tetromino.All[id].code[spin];
            int probe = 0x8000;
            int x, y; // block coordinate
            for (int i = 0; i < 16; i++)
            {
                if ((code & probe) > 0)
                {
                    x = this.x + i % 4; y = this.y + i / 4;
                    if (x < 0 || x >= Tetris.COL || y >= Tetris.ROW ||
                        (y >= 0 && map[x, y]))
                        return false;
                }
                probe >>= 1;
            }
            return true;
        }

        #region operation
        public void Left()
        {
            x--; if (!Feasible()) x++;
        }

        public void Right()
        {
            x++; if (!Feasible()) x--;
        }

        public void Rotate()
        {
            spin = (spin + 1) % 4;
            if (!Feasible())
                spin = (spin - 1) % 4;                
        }

        public void Fall()
        {
            y++;
            if (!Feasible())
            {
                y--;
                Land();
            }
        }

        public void Land()
        {
            do { y++; } while (Feasible());
            y--;
            Tetris.tetro = Tetris.tetroNext.Inherit();
            Tetris.tetroNext = Tetro.New();
        }

        #endregion
    }
}
