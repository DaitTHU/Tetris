﻿using System.Drawing;

namespace Tetris
{
    internal struct Tetromino
    {
        public readonly int[] code;
        public readonly Color color;
        public Tetromino(int[] code, Color color) : this()
        {
            this.code = code;
            this.color = color;
        }
        public static Tetromino I => new Tetromino(new int[] { 0x0F00, 0x4444, 0x0F00, 0x4444 }, Color.Red);
        public static Tetromino O => new Tetromino(new int[] { 0x0660, 0x0660, 0x0660, 0x0660 }, Color.Orange);
        public static Tetromino T => new Tetromino(new int[] { 0x04E0, 0x0464, 0x00E4, 0x04C4 }, Color.Yellow);
        public static Tetromino L => new Tetromino(new int[] { 0x0446, 0x02E0, 0x0622, 0x0740 }, Color.Green);
        public static Tetromino J => new Tetromino(new int[] { 0x2260, 0x0E20, 0x6440, 0x0470 }, Color.Blue);
        public static Tetromino Z => new Tetromino(new int[] { 0x0C60, 0x2640, 0x0C60, 0x2640 }, Color.Violet);
        public static Tetromino S => new Tetromino(new int[] { 0x0360, 0x4620, 0x0360, 0x4620 }, Color.Purple);
        public static Tetromino X => new Tetromino(new int[] { 0x0040, 0x0040, 0x0040, 0x0040 }, Color.White);
        public static Tetromino[] All = { I, O, T, L, J, Z, S, X };
        // public static int Count = All.Length;
    }
}