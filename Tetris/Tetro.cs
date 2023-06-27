using System;
using System.Drawing;

namespace Tetris
{
    internal struct Tetro
    {
        private readonly int id; // [0, Tetromino.Count - 1]
        private int spin; // [0, 3]
        internal int x, y, ymax; // block coordinate
        internal Point Coordinate
        {
            get => new Point(x, y);
            set { x = value.X; y = value.Y; }
        }
        private int code;
        private readonly Color color;
        // static properties: field map
        internal static readonly bool[,] map = new bool[COL, ROW];
        private static readonly Random random = new Random();
        public const int UNIT = Tetris.UNIT, 
            COL = Tetris.COL, ROW = Tetris.ROW;

        public Tetro(int id, int spin, int x = 0, int y = 0)
        {
            this.id = id;
            this.spin = spin;
            this.x = x;
            this.y = y;
            this.ymax = y;
            code = Tetromino.All[id].code[spin];
            color = Tetromino.All[id].color;
        }

        public static Tetro New()
        {
            return new Tetro(random.Next(7), random.Next(4));
        }

        public Tetro Inherit()
        {
            Tetro tetro = new Tetro(id, spin, COL / 2 - 2, -3);
            int probe = 0xF;
            while ((tetro.code & probe) == 0)
            {
                tetro.y++; // Move down until the tetro appear
                probe <<= 4;
            }
            tetro.UpdateYmax();
            return tetro;
        }

        public Tetro Copy()
        {
            return new Tetro(id, spin, x, y);
        }

        public Bitmap Show(bool fixTetro = false)
        {
            Bitmap bitmap = new Bitmap(4 * UNIT + 1, 4 * UNIT + 1);
            Graphics g = Graphics.FromImage(bitmap);
            Pen pen = new Pen(Color.White);
            Brush brush = new SolidBrush(color);
            int probe = 0x8000;
            int dx, dy;
            for (int i = 0; i < 16; i++)
            {
                if ((code & probe) > 0)
                {
                    dx = i % 4; dy = i / 4;
                    if (fixTetro)
                    { // when the tetro is to fix
                        if (y + dy < 0)
                        {
                            // game over
                            return bitmap;
                        }
                        map[x + dx, y + dy] = true;
                    }
                    g.FillRectangle(brush, dx * UNIT, dy * UNIT, UNIT, UNIT);
                    g.DrawRectangle(pen, dx * UNIT, dy * UNIT, UNIT, UNIT);
                }
                probe >>= 1;
            }
            return bitmap;
        }

        public Bitmap Hint()
        {
            Bitmap bitmap = new Bitmap(4 * UNIT + 1, 4 * UNIT + 1);
            Graphics g = Graphics.FromImage(bitmap);
            Brush brush = new SolidBrush(Color.Gray);
            int probe = 0x8000;
            for (int i = 0; i < 16; i++)
            {
                if ((code & probe) > 0)
                    g.FillRectangle(brush, (i % 4) * UNIT + 1, 
                        (i / 4) * UNIT, UNIT - 1, UNIT);
                probe >>= 1;
            }
            return bitmap;
        }

        public bool Feasible()
        {
            // i.e. collision detect
            int probe = 0x8000;
            int x, y; // block coordinate
            for (int i = 0; i < 16; i++)
            {
                if ((code & probe) > 0)
                {
                    x = this.x + i % 4; y = this.y + i / 4;
                    if (x < 0 || x >= COL || y >= ROW ||
                        (y >= 0 && map[x, y]))
                        return false;
                }
                probe >>= 1;
            }
            return true;
        }

        #region operation
        public void UpdateYmax()
        {
            int probe = 0x8000;
            int x, y, ymax = ROW; // block coordinate
            for (int i = 0; i < 16; i++)
            {
                if ((code & probe) > 0)
                {
                    x = this.x + i % 4; y = this.y + i / 4;
                    while (y < ROW && (y < 0 || !map[x, y]))
                        y++;
                    ymax = Math.Min(ymax, y - i / 4);
                }
                probe >>= 1;
            }
            this.ymax = ymax - 1;
        }

        public void Left()
        {
            x--; if (!Feasible()) x++; else UpdateYmax();
        }

        public void Right()
        {
            x++; if (!Feasible()) x--; else UpdateYmax();
        }

        public void Rotate()
        {
            spin = (spin + 1) % 4;
            code = Tetromino.All[id].code[spin];
            foreach (var dx in new int[] { 0, 1, 1, -3, -1})
            {
                x += dx;
                if (Feasible())
                {
                    UpdateYmax();
                    return;
                }
            }
            spin = (spin + 3) % 4;
            code = Tetromino.All[id].code[spin];
        }

        #endregion
    }
}
