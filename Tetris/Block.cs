using System.Drawing;
using static Tetris.Const;

namespace Tetris
{
    public abstract class Entity
    {
        protected int x, y;
        protected static readonly bool[,] matrix = new bool[COL, ROW]; // i.e. playfield
        protected static bool topout = false; // once top out, game over
        public int X => x;
        public int Y => y;
        public int XU => Unit(x);
        public int YU => Unit(y);

    }

    public class Block : Entity
    {
        private readonly Color color;
        public Block(int x, int y, Color color)
        {
            this.x = x;
            this.y = y;
            this.color = color;
        }

        public Block() : this(0, 0, lineColor) { }

        public void Change()
        {
            int index = random.Next(COL * ROW / 2);
            x = index % COL;
            y = index / COL + ROW / 2;
        }

        public Bitmap Show()
        {
            matrix[x, y] = true;
            Bitmap bitmap = new Bitmap(Unit() + 1, Unit() + 1);
            Graphics.FromImage(bitmap).Clear(color);
            return bitmap;
        }

        public bool Occupied => Tetro.Matrix(x, y);

    }

}
