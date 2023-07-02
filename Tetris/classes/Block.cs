using System.Drawing;
using static Tetris.Const;

namespace Tetris
{
    public class Block : Entity
    {
        private readonly Bitmap piece;

        public Block(int x, int y, Color color)
        {
            this.x = x;
            this.y = y;
            piece = new Bitmap(Unit() + 1, Unit() + 1);
            Graphics g = Graphics.FromImage(piece);
            g.Clear(color);
            g.DrawRectangle(pen, 0, 0, Unit(), Unit());
        }

        public Block() : this(0, 0, gridColor) { }

        public void Change(int height)
        {
            while (true)
            {
                int index = random.Next(COL * height);
                x = index % COL;
                y = index / COL + ROW - height;
                if (matrix[x, y])
                    continue;
                for (int x = 0; x < COL; x++)
                {
                    if (x == this.x)
                        continue;
                    if (!matrix[x, y])
                        return;
                }
            }
        }

        public ref readonly Bitmap Piece
        {
            get
            {
                matrix[x, y] = true;
                return ref piece;
            }
        }
    }
}
