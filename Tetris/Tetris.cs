using System;
using System.Drawing;
using System.Windows.Forms;
using static Tetris.Const;
using CompositingMode = System.Drawing.Drawing2D.CompositingMode;
using Thread = System.Threading.Thread;

namespace Tetris
{
    public partial class Tetris : Form
    {
        private Tetro tetro = new Tetro(),
            tetroNext = Tetro.New();
        private int score = 0, lines = 0;
        private bool hint = false, clearRow = false;
        private Block garbage = new Block();
        private readonly Bitmap background, field, preview;

        public Tetris()
        {
            InitializeComponent();
            // Form Appearance
            this.ClientSize = new Size(Unit(COL + 7), Unit(ROW + 2));
            this.BackColor = Color.Black;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.KeyPreview = true;
            // Field
            Field.Location = new Point(Unit(), Unit());
            Field.Size = new Size(Unit(COL) + 1, Unit(ROW) + 1);
            Field.BackColor = Color.Black;
            background = new Bitmap(Field.Width, Field.Height);
            field = new Bitmap(Field.Width, Field.Height);
            // Preview
            Preview.Location = new Point(Unit(COL + 2), Unit());
            Preview.Size = new Size(Unit(CELL) + 1, Unit(CELL) + 1);
            Preview.BackColor = Color.Black;
            preview = new Bitmap(Preview.Width, Preview.Height);
            // Label
            Score.Location = new Point(Unit(COL + 2), Unit(6));
            Lines.Location = new Point(Unit(COL + 2), Unit(8));
            Score.Width = Lines.Width = Unit(CELL);
            NewGame();
        }

        private void Tetris_Load(object sender, EventArgs e)
        {
            // Field background
            Graphics g = Graphics.FromImage(background);
            for (int x = 0; x <= COL; x++)
                g.DrawLine(penGrid, Unit(x), 0, Unit(x), field.Height);
            for (int y = 0; y <= ROW; y++)
                g.DrawLine(penGrid, 0, Unit(y), field.Width, Unit(y));
            g.Dispose();
            // Preview background
            g = Graphics.FromImage(preview);
            for (int i = 0; i <= CELL; i++)
            {
                g.DrawLine(penGrid, Unit(i), 0, Unit(i), preview.Height);
                g.DrawLine(penGrid, 0, Unit(i), preview.Width, Unit(i));
            }
            g.DrawRectangle(pen, 0, 0, Unit(CELL), Unit(CELL));
            g.Dispose();
        }

        #region game code
        private void NewGame()
        {
            score = lines = 0;
            Score.Text = "SCORE\n" + score.ToString();
            Lines.Text = "LINES\n" + lines.ToString();
            Timer.Interval = 1000;
            Tetro.Initialize();
            Graphics.FromImage(field).Clear(Color.Transparent);
            // AddObstacle();
            NewTetro();
        }

        private void NewTetro()
        {
            tetro.Spawn(tetroNext);
            tetroNext.Change();
            Field.Refresh();
            Preview.Refresh();
            Timer.Stop();
            Timer.Start(); // restart timer
        }

        private void LineClear()
        {
            score++;
            Score.Text = "SCORE\n" + score.ToString();
            Thread.Sleep(Timer.Interval / 10);
            Graphics g = Graphics.FromImage(field);
            g.DrawImage(tetro.Piece(lockTetro: true), tetro.XU, tetro.YU);
            if (Tetro.TopOut)
            {
                Field.Refresh();
                Timer.Stop();
                new InfoBox("game over!");
                NewGame();
                return;
            }
            // clear full row(s)
            var fullRow = new bool[CELL];
            for (int y = tetro.Y; y < tetro.Y + CELL; y++)
            {
                if (!Tetro.LineFilled(y))
                    continue;
                fullRow[y - tetro.Y] = clearRow = true;
                g.FillRectangle(brushLine, 0, Unit(y), field.Width, Unit());
                lines++; score += 200;
            }
            Lines.Text = "LINES\n" + lines.ToString();
            if (clearRow)
            {
                Field.Refresh();
                Thread.Sleep(Timer.Interval * 3 / 10);
                for (int y = tetro.Y; y < tetro.Y + CELL; y++)
                {
                    if (!fullRow[y - tetro.Y])
                        continue;
                    Tetro.LineSettle(y);
                    // image
                    g.CompositingMode = CompositingMode.SourceCopy;
                    Bitmap fieldAbove = field.Clone(new Rectangle(0, 0, field.Width, Unit(y)), field.PixelFormat);
                    g.FillRectangle(eraser, 0, 0, field.Width, Unit(y + 1) + 1);
                    g.DrawImage(fieldAbove, 0, Unit());
                    for (int x = 0; x < COL; x++)
                        if (Tetro.Matrix(x, y) || (y + 1 < ROW && Tetro.Matrix(x, y + 1)))
                            g.DrawLine(pen, Unit(x), Unit(y + 1), Unit(x + 1), Unit(y + 1));
                }
                score -= 100; // score = 200 * clearline - 100;
                Score.Text = "SCORE\n" + score.ToString();
                Timer.Interval = 100 + Pow2(Math.Max(30 - lines / 10, 0));
                clearRow = false;
            }
            g.Dispose();
            NewTetro();
        }

        private void Garbage()
        {
            // obstacle
            Graphics g = Graphics.FromImage(field);
            for (int i = 0; i < COL * ROW / 4; i++)
            {
                bool fullrow = true;
                do
                {
                    garbage.Change();
                    if (garbage.Occupied)
                        continue;
                    for (int x = 0; x < COL; x++)
                    {
                        if (x == garbage.X)
                            continue;
                        if (!Tetro.Matrix(x, garbage.Y))
                        {
                            fullrow = false;
                            break;
                        }
                    }
                } while (fullrow);
                g.DrawImage(garbage.Show(), garbage.XU, garbage.YU);
            }
            g.Dispose();
        }
        #endregion

        private void Field_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(background, 0, 0);
            e.Graphics.DrawImage(field, 0, 0);
            if (!clearRow)
            {
                if (hint)
                    e.Graphics.DrawImage(tetro.Ghost(), tetro.XU, tetro.YmaxU);
                e.Graphics.DrawImage(tetro.Piece(), tetro.XU, tetro.YU);
            }
            e.Graphics.DrawRectangle(pen, 0, 0, Unit(COL), Unit(ROW));
        }

        private void Preview_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(preview, 0, 0);
            e.Graphics.DrawImage(tetroNext.Piece(), 0, 0);
        }

        #region Keyboard Control
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!tetro.Drop())
                LineClear();
            Field.Refresh();
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Left:
                case Keys.A:
                    tetro.Left();
                    break;
                case Keys.Right:
                case Keys.D:
                    tetro.Right();
                    break;
                case Keys.Up:
                case Keys.W:
                    tetro.Rotate();
                    break;
                case Keys.Z:
                    tetro.CounterRotate();
                    break;
                case Keys.Down:
                case Keys.S:
                    if (!tetro.Drop())
                        LineClear();
                    break;
                case Keys.Space:
                case Keys.Enter:
                    tetro.Land();
                    LineClear();
                    break;
                case Keys.E: // hint
                    hint = !hint;
                    break;
                case Keys.Q: // swap
                    tetro.Swap(ref tetroNext);
                    Preview.Refresh();
                    break;
                case Keys.R: // restart
                    CheckBox infoBox = new CheckBox("restart?");
                    Timer.Stop();
                    if (infoBox.ShowDialog() == DialogResult.Yes)
                        NewGame();
                    Timer.Start();
                    break;
                case Keys.P: // pause
                    Timer.Stop();
                    new InfoBox("pausing...");
                    Timer.Start();
                    break;
                case Keys.Escape:
                    this.Close();
                    return true;
            }
            Field.Refresh();
            return false;
        }
        #endregion
    }
}
