using System;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace Tetris
{
    public static class Const
    {
        public const int UNIT = 35,
            COL = 10, ROW = 20, CELL = 4,
            lineWidth = 1;
        internal static readonly Color gridColor = Color.DimGray,
            lineColor = Color.White;
        internal static readonly Pen penGrid = new Pen(gridColor, lineWidth),
            pen = new Pen(lineColor, lineWidth);
        internal static readonly SolidBrush brushHint = new SolidBrush(gridColor),
            brushLine = new SolidBrush(lineColor),
            eraser = new SolidBrush(Color.Transparent);
        internal static readonly Random random = new Random();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int Unit(int i) => i * UNIT;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IntSq(int x) => x * x;
    }
}
