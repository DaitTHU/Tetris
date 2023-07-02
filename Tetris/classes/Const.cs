using System;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace Tetris
{
    public static class Const
    {
        private const int UNIT = 35;
        public const int COL = 10, ROW = 20, CELL = 4;
        public const int SPIN = 4;
        public const int lineWidth = 10;
        internal static readonly Color 
            gridColor = Color.DimGray,
            lineColor = Color.White;
        internal static readonly Pen // penGrid = new Pen(gridColor, lineWidth),
            pen = new Pen(lineColor, lineWidth);
        internal static readonly SolidBrush 
            brush = new SolidBrush(lineColor),
            eraser = new SolidBrush(Color.Transparent);
        internal static readonly Random random = new Random();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int Unit(int i = 1) => i * UNIT;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Pow2(int x) => x * x;
    }
}
