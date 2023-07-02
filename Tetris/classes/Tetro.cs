using System;
using System.Drawing;
using System.Linq;
using static Tetris.Const;

namespace Tetris
{
    public class Tetro : Tetromino, IPiece
    {
        private int id; // < Tetromino.TYPE
        private int spin; // < Tetromino.SPIN, i.e. orientation
        private int ymax; // block coordinate   
        // neither real nor pesudo random
        private const int REPEAT = 1; // bg7
        private static readonly int[] counts = new int[TYPE];

        public Tetro(int id, int spin, int x = 0, int y = 0)
        {
            this.id = id;
            this.spin = spin;
            this.x = x;
            this.y = ymax = y; // ymax reserved to update
        }

        public Tetro() : this(0, 0) { } // default constructor

        public void Change()
        {
            if (counts.Sum() >= REPEAT * TYPE)
                for (int i = 0; i < TYPE; i++)
                    counts[i] = 0;
            do
            {
                id = random.Next(TYPE);
            } while (counts[id] >= REPEAT);
            counts[id]++;
            spin = random.Next(SPIN);
        }

        public void Spawn(in Tetro that)
        {
            id = that.id;
            spin = that.spin;
            x = COL / 2 - 2;
            y = -CELL;
            UpdateYmax();
            for (int p = 0xF; (Code & p) == 0; p <<= CELL)
                y++; // Move down until the tetro appear
        }

        public static new void Initialize()
        {
            Entity.Initialize();
            for (int i = 0; i < TYPE; i++)
                counts[i] = 0;
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
            while (y > 0)
            {
                for (int x = 0; x < COL; x++)
                    matrix[x, y] = matrix[x, y - 1];
                y--;
            }
            for (int x = 0; x < COL; x++)
                matrix[x, 0] = false;
            /* 
            for (int x = 0; x < COL; x++)
            {
                for (int j = y; j > 0; j--)
                    matrix[x, j] = matrix[x, j - 1];
                matrix[x, 0] = false;
            }
            */
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
            Land();
            return false;
        }

        public void Land()
        {
            y = ymax;
            // lock tetro
            for (int dy = 0; dy < CELL; dy++)
            {
                for (int dx = 0; dx < CELL; dx++)
                {
                    if (!Empty(dx, dy))
                    {
                        try
                        {
                            matrix[x + dx, y + dy] = true;
                        }
                        catch
                        {
                            topout = true;
                        }
                    }
                }
            }
        }

        public void Rotate()
        {
            Spin++; if (!Offsetable()) Spin--;
        }

        public void CounterRotate()
        {
            Spin--; if (!Offsetable()) Spin++;
        }

        public void Swap(ref Tetro that)
        {
            (Shape, that.Shape) = (that.Shape, Shape);
            if (!Offsetable())
                (Shape, that.Shape) = (that.Shape, Shape);
        }

        private bool Feasible()
        {
            for (int dy = 0; dy < CELL; dy++)
            {
                for (int dx = 0; dx < CELL; dx++)
                {
                    if (!Empty(dx, dy))
                    {
                        int x = this.x + dx,
                            y = this.y + dy;
                        if (x < 0 || x >= COL || y >= ROW || (y >= 0 && matrix[x, y]))
                            return false;
                    }
                }
            }
            return true;
        }

        private void UpdateYmax()
        {
            int ymax = ROW;
            for (int dy = 0; dy < CELL; dy++)
            {
                for (int dx = 0; dx < CELL; dx++)
                {
                    if (!Empty(dx, dy))
                    {
                        int x = this.x + dx, 
                            y = this.y + dy;
                        while (y < ROW && (y < 0 || !matrix[x, y]))
                            y++;
                        ymax = Math.Min(ymax, y - dy);
                    }
                }
            }
            this.ymax = ymax - 1;
            /*
            int probe = 0x8000;
            int x, y, ymax = ROW; // block coordinate
            for (int i = 0; i < 16; i++)
            {
                if ((Code & probe) > 0)
                {
                    x = this.x + i % CELL; y = this.y + i / CELL;
                    while (y < ROW && (y < 0 || !matrix[x, y]))
                        y++;
                    ymax = Math.Min(ymax, y - i / CELL);
                }
                probe >>= 1;
            }
            this.ymax = ymax - 1;
            */
        }

        private bool Offsetable()
        {
            foreach (var dy in new int[] { 0, 1, 2 })
            {
                y += dy;
                foreach (var dx in new int[] { 0, 1, -1, 2, -2 })
                {
                    x += dx;
                    if (Feasible())
                    {
                        UpdateYmax();
                        return true;
                    }
                    x -= dx;
                }
                y -= dy;
            }
            return false;
        }
        #endregion

        public int YmaxU => Unit(ymax);
        private int Spin
        {
            get => spin;
            set
            {
                spin = value % SPIN;
                if (spin < 0) // value < 0
                    spin += SPIN;
            }
        }
        private (int, int) Shape
        {
            get => (id, spin);
            set => (id, spin) = value;
        }
        public static bool Matrix(int x, int y) => matrix[x, y];
        public int Code => Tetromino.BasicSet[id].codes[spin];
        public ref readonly Bitmap Piece => 
            ref Tetromino.BasicSet[id].pieces[spin];
        public ref readonly Bitmap Ghost => 
            ref Tetromino.BasicSet[id].ghosts[spin];
        public bool Empty(int dx, int dy) => 
            (Code & (1 << (15 - dx - dy * CELL))) == 0;
        public static bool TopOut => topout;
    }
}
