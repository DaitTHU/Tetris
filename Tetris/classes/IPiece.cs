namespace Tetris
{
    public interface IPiece
    {
        void Left();
        void Right();
        bool Drop();
        void Land();
        void Rotate();
        void CounterRotate();
    }
}
