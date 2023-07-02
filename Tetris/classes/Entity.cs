using static Tetris.Const;

namespace Tetris
{
    public abstract class Entity
    {
        protected int x, y;
        protected static readonly bool[,] matrix = new bool[COL, ROW]; // i.e. playfield
        protected static bool topout = false; // once top out, game over
        protected static void Initialize()
        {
            for (int x = 0; x < COL; x++)
                for (int y = 0; y < ROW; y++)
                    matrix[x, y] = false;
            topout = false;
        }
        public int X => x;
        public int Y => y;
        public int XU => Unit(x);
        public int YU => Unit(y);

    }

}
