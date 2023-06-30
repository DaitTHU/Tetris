using System;
using System.Drawing;
using System.Linq;
using static Tetris.Const;

namespace Tetris
{
    public class Tetro : Tetromino
    {
        private int id; // < Tetromino.TypeNumber
        private int spin; // < Tetromino.SpinModule, i.e. orientation
        private int ymax; // block coordinate
        // private
        private int code;
        private readonly SolidBrush brush; // fill color        
        // neither real nor pesudo random
        private const int MAXREPEAT = 2;
        private static readonly int[] counts = new int[TypeNumber];

        public Tetro(int id, int spin, int x = 0, int y = 0)
        {
            this.id = id;
            this.spin = spin;
            this.x = x;
            this.y = ymax = y; // ymax reserved to update
            code = BasicSet[id].code[spin];
            brush = new SolidBrush(BasicSet[id].color);
        }

        public Tetro() : this(0, 0) { }

        public static Tetro New() => new Tetro(
            id: random.Next(TypeNumber), spin: random.Next(SpinModule));

        public void Change()
        {
            if (counts.Sum() >= MAXREPEAT * TypeNumber)
                for (int i = 0; i < TypeNumber; i++)
                    counts[i] = 0;
            do
            {
                id = random.Next(TypeNumber);
            } while (counts[id] >= MAXREPEAT);
            counts[id]++;
            Spin = random.Next(SpinModule);
            brush.Color = BasicSet[id].color;
        }

        public void Change(int x, int y)
        {
            Change();
            this.x = x; this.y = y;
            UpdateYmax();
        }

        public void Spawn(in Tetro that)
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

        public Bitmap Piece(bool lockTetro = false)
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
                    if (lockTetro)
                    { // when the tetro is to fix
                        if (y + dy < 0)
                            topout = true; // game over
                        else
                            matrix[x + dx, y + dy] = true;
                    }
                    g.FillRectangle(brush, Unit(dx), Unit(dy), Unit(), Unit());
                    g.DrawRectangle(pen, Unit(dx), Unit(dy), Unit(), Unit());
                }
                probe >>= 1;
            }
            g.Dispose();
            return bitmap;
        }

        public Bitmap Ghost()
        {
            Bitmap bitmap = new Bitmap(Unit(CELL) + 1, Unit(CELL) + 1);
            Graphics g = Graphics.FromImage(bitmap);
            int probe = 0x8000;
            for (int i = 0; i < 16; i++)
            {
                if ((code & probe) > 0)
                    g.FillRectangle(brushGhost, Unit(i % CELL) + 1,
                        Unit(i / CELL), Unit() - 1, Unit());
                probe >>= 1;
            }
            g.Dispose();
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
                        (y >= 0 && matrix[x, y]))
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
                    while (y < ROW && (y < 0 || !matrix[x, y]))
                        y++;
                    ymax = Math.Min(ymax, y - i / CELL);
                }
                probe >>= 1;
            }
            this.ymax = ymax - 1;
        }

        public static void Initialize()
        {
            for (int x = 0; x < COL; x++)
                for (int y = 0; y < ROW; y++)
                    matrix[x, y] = false;
            for (int i = 0; i < TypeNumber; i++)
                counts[i] = 0;
            topout = false;
        }

        public static bool LineFilled(int y)
        {
            if (y < 0 || y >= ROW)
                return false;
            for (int x = 0; x < COL; x++)
                if (!matrix[x, y])
                    return false;
            return true;
        }

        public static void LineSettle(int y)
        {
            for (int x = 0; x < COL; x++)
            {
                for (int j = y; j > 0; j--)
                    matrix[x, j] = matrix[x, j - 1];
                matrix[x, 0] = false;
            }
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

        public bool Drop()
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
            Spin++;
            if (Offsetable())
                return;
            Spin--;
        }

        public void CounterRotate()
        {
            Spin--;
            if (Offsetable())
                return;
            Spin++;
        }

        public void Swap(ref Tetro that)
        {
            (ExceptXY, that.ExceptXY) = (that.ExceptXY, ExceptXY);
            if (Offsetable())
                return;
            (ExceptXY, that.ExceptXY) = (that.ExceptXY, ExceptXY);
        }

        private bool Offsetable()
        {
            foreach (var dx in new int[] { 0, 1, 1, -3, -1 })
            {
                x += dx;
                if (Feasible())
                {
                    UpdateYmax();
                    return true;
                }
            }
            x += 2;
            return false;
        }
        #endregion

        public int Ymax => ymax;
        public int YmaxU => Unit(ymax);
        private int Spin
        {
            get => spin;
            set
            {
                spin = value % SpinModule;
                if (spin < 0) // value < 0
                    spin += SpinModule;
                code = BasicSet[id].code[spin]; // update code
            }
        }
        private (int, int, int, Color) ExceptXY
        {
            get => (id, spin, code, brush.Color);
            set => (id, spin, code, brush.Color) = value;
        }
        public static bool Matrix(int x, int y) => matrix[x, y];
        public static bool TopOut => topout;
    }

}
