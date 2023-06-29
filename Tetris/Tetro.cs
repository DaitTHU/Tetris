using System;
using System.Drawing;
using static Tetris.Const;

namespace Tetris
{
    public class Tetro : Tetromino
    {
        private int id; // [0, Tetromino.Count - 1]
        private int spin; // [0, 3]
        private int x, y, ymax; // block coordinate
        private int code;
        private readonly SolidBrush brush; // fill color
        // static properties, for Tetris to use
        internal static readonly bool[,] map = new bool[COL, ROW]; // common map
        private static bool overflow = false; // once overflow, game over
        /*
        private static readonly int[] in20 = new int[Tetromino.Count],
            lastId = new int[5];
        */

        public Tetro(int id, int spin, int x = 0, int y = 0)
        {
            this.id = id;
            this.spin = spin;
            this.x = x;
            this.y = ymax = y; // ymax reserved to update
            code = All[id].code[spin];
            brush = new SolidBrush(All[id].color);
        }

        public static void Initialize()
        {
            for (int x = 0; x < COL; x++)
                for (int y = 0; y < ROW; y++)
                    map[x, y] = false;

            overflow = false;
        }

        public static Tetro New() => new Tetro(
            id: random.Next(Tetromino.Count), 
            spin: random.Next(Tetromino.SpinMod));

        public void Change()
        {
            id = random.Next(Tetromino.Count); 
            spin = random.Next(Tetromino.SpinMod);
            code = Tetromino.All[id].code[spin];
            brush.Color = Tetromino.All[id].color;
        }

        public void Change(int x, int y)
        {
            Change();
            this.x = x; this.y = y;
            UpdateYmax();
        }

        public void Move(in Tetro that)
        {
            id = that.id;
            spin = that.spin;
            x = COL / 2 - 2;
            y = -CELL;
            code = that.code;
            brush.Color = that.brush.Color;
            UpdateYmax();
            int probe = 0xF;
            while ((code & probe) == 0)
            {
                y++; // Move down until the tetro appear
                probe <<= CELL;
            }
        }

        public Bitmap Show(bool fixTetro = false)
        {
            Bitmap bitmap = new Bitmap(Unit(CELL) + 1, Unit(CELL) + 1);
            Graphics g = Graphics.FromImage(bitmap);
            int probe = 0x8000;
            int dx, dy;
            for (int i = 0; i < 16; i++)
            {
                if ((code & probe) > 0)
                {
                    dx = i % CELL; dy = i / CELL;
                    if (fixTetro)
                    { // when the tetro is to fix
                        if (y + dy < 0)
                            overflow = true; // game over
                        else
                            map[x + dx, y + dy] = true;
                    }
                    g.FillRectangle(brush, Unit(dx), Unit(dy), UNIT, UNIT);
                    g.DrawRectangle(pen, Unit(dx), Unit(dy), UNIT, UNIT);
                }
                probe >>= 1;
            }
            g.Dispose();
            return bitmap;
        }

        public Bitmap Shadow()
        {
            Bitmap bitmap = new Bitmap(Unit(CELL) + 1, Unit(CELL) + 1);
            Graphics g = Graphics.FromImage(bitmap);
            int probe = 0x8000;
            for (int i = 0; i < 16; i++)
            {
                if ((code & probe) > 0)
                    g.FillRectangle(brushHint, Unit(i % CELL) + 1,
                        Unit(i / CELL), UNIT - 1, UNIT);
                probe >>= 1;
            }
            return bitmap;
        }

        private bool Feasible()
        {
            // i.e. collision detect
            int probe = 0x8000;
            int x, y; // block coordinate
            for (int i = 0; i < 16; i++)
            {
                if ((code & probe) > 0)
                {
                    x = this.x + i % CELL; y = this.y + i / CELL;
                    if (x < 0 || x >= COL || y >= ROW ||
                        (y >= 0 && map[x, y]))
                        return false;
                }
                probe >>= 1;
            }
            return true;
        }

        private void UpdateYmax()
        {
            int probe = 0x8000;
            int x, y, ymax = ROW; // block coordinate
            for (int i = 0; i < 16; i++)
            {
                if ((code & probe) > 0)
                {
                    x = this.x + i % CELL; y = this.y + i / CELL;
                    while (y < ROW && (y < 0 || !map[x, y]))
                        y++;
                    ymax = Math.Min(ymax, y - i / CELL);
                }
                probe >>= 1;
            }
            this.ymax = ymax - 1;
        }

        #region operation
        public void Left()
        {
            x--; if (!Feasible()) x++; else UpdateYmax();
        }

        public void Right()
        {
            x++; if (!Feasible()) x--; else UpdateYmax();
        }

        public bool Fall()
        {
            if (y < ymax)
            {
                y++;
                return true;
            }
            return false;
        }

        public void Land()
        {
            y = ymax;
        }

        public void Rotate()
        {
            spin = (spin + 1) % Tetromino.SpinMod;
            code = Tetromino.All[id].code[spin];
            foreach (var dx in new int[] { 0, 1, 1, -3, -1 })
            {
                x += dx;
                if (Feasible())
                {
                    UpdateYmax();
                    return;
                }
            }
            x += 2;
            spin = (spin + 3) % Tetromino.SpinMod;
            code = Tetromino.All[id].code[spin];
        }

        public void RotateBack()
        {
            spin = (spin + 3) % Tetromino.SpinMod;
            code = Tetromino.All[id].code[spin];
            foreach (var dx in new int[] { 0, 1, 1, -3, -1 })
            {
                x += dx;
                if (Feasible())
                {
                    UpdateYmax();
                    return;
                }
            }
            x += 2;
            spin = (spin + 1) % Tetromino.SpinMod;
            code = Tetromino.All[id].code[spin];
        }

        public void Swap(ref Tetro that)
        {
            (ExceptXY, that.ExceptXY) = (that.ExceptXY, ExceptXY);
            foreach (var dx in new int[] { 0, 1, 1, -3, -1 })
            {
                x += dx;
                if (Feasible())
                {
                    UpdateYmax();
                    return;
                }
            }
            x += 2;
            (ExceptXY, that.ExceptXY) = (that.ExceptXY, ExceptXY);
        }

        #endregion

        #region get & set
        public int X => x;
        public int Y => y;
        public int Ymax => ymax;
        public static bool Overflow => overflow;
        public int XU => x * UNIT;
        public int YU => y * UNIT;
        public int YmaxU => ymax * UNIT;
        private (int, int, int, Color) ExceptXY
        {
            get { return (id, spin, code, brush.Color); }
            set { (id, spin, code, brush.Color) = value; }
        }
        #endregion
    }

    public class Block
    {
        private int x, y;
        private readonly Color color;
        public Block(int x, int y, Color color)
        {
            this.x = x;
            this.y = y;
            this.color = color;
        }

        public Block(int x = 0, int y = 0) : this(x, y, lineColor) { }

        public Bitmap Show()
        {
            Tetro.map[x, y] = true;
            Bitmap bitmap = new Bitmap(UNIT + 1, UNIT + 1);
            Graphics.FromImage(bitmap).Clear(color);
            return bitmap;
        }

        public bool Occupied => Tetro.map[x, y];
        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public int XU => x * UNIT;
        public int YU => y * UNIT;
    }

}
