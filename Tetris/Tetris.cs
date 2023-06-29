using System;
using System.Drawing;
using System.Windows.Forms;
using static Tetris.Const;

namespace Tetris
{
    public partial class Tetris : Form
    {
        private Tetro tetro = new Tetro(0, 0), 
            tetroNext = Tetro.New();
        private int score = 0, lines = 0;
        private bool hint = false, clearRow = false;
        private Block obstacle = new Block(0, 0);
        private readonly Bitmap background, field, preview;

        public Tetris()
        {
            InitializeComponent();
            // Form Appearance
            this.ClientSize = new Size(Unit(COL + 7), Unit(ROW + 2));
            this.BackColor = Color.Black;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            // Field
            Field.Location = new Point(UNIT, UNIT);
            Field.Size = new Size(Unit(COL) + 1, Unit(ROW) + 1);
            Field.BackColor = Color.Black;
            background = new Bitmap(Field.Width, Field.Height);
            field = new Bitmap(Field.Width, Field.Height);
            // Preview
            Preview.Location = new Point(Unit(COL + 2), UNIT);
            Preview.Size = new Size(Unit(CELL) + 1, Unit(CELL) + 1);
            Preview.BackColor = Color.Black;
            preview = new Bitmap(Preview.Width, Preview.Height);
            // Label
            Score.Location = new Point(Unit(COL + 2), Unit(6));
            Lines.Location = new Point(Unit(COL + 2), Unit(8));
            Score.Width = Lines.Width = Unit(CELL);
            // Control
            Timer.Enabled = true;
            this.KeyPreview = true;
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
            AddObstacle();
            NewTetro();
        }

        private void NewTetro()
        {
            tetro.Move(tetroNext);
            tetroNext.Change();
            Field.Refresh();
            Preview.Refresh();
            Timer.Stop();
            Timer.Start(); // restart timer
        }

        private void ClearRow()
        {
            score++; 
            Score.Text = "SCORE\n" + score.ToString();
            System.Threading.Thread.Sleep(Timer.Interval / 10);
            Graphics g = Graphics.FromImage(field);
            g.DrawImage(tetro.Show(fixTetro: true), tetro.XU, tetro.YU);
            if (Tetro.Overflow)
            {
                Field.Refresh();
                Timer.Stop();
                new InfoBox("game over!");
                NewGame();
                return;
            }
            // clear full row(s)
            var fullRow = new bool[CELL];
            for (int dy = Math.Max(0, -tetro.Y); 
                dy < Math.Min(CELL, ROW - tetro.Y); dy++)
            {
                fullRow[dy] = true;
                for (int x = 0; x < COL; x++)
                    fullRow[dy] &= Tetro.map[x, tetro.Y + dy];
                if (fullRow[dy])
                {
                    clearRow = true;
                    g.FillRectangle(brushLine, 
                        0, Unit(tetro.Y + dy), field.Width, UNIT);
                    lines++; score += 200;
                    Lines.Text = "LINES\n" + lines.ToString();
                }
            }
            if (clearRow)
            {
                Field.Refresh();
                System.Threading.Thread.Sleep(Timer.Interval * 3 / 10);
                for (int dy = 0; dy < CELL; dy++)
                {
                    if (!fullRow[dy])
                        continue;
                    int yline = tetro.Y + dy;
                    g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                    Bitmap fieldAbove = field.Clone(new Rectangle(0, 0, field.Width, Unit(yline)), field.PixelFormat);
                    g.FillRectangle(eraser, 0, 0, field.Width, Unit(yline + 1) + 1);
                    g.DrawImage(fieldAbove, 0, UNIT);
                    for (int x = 0; x < COL; x++)
                    {
                        for (int y = yline; y > 0; y--)
                            Tetro.map[x, y] = Tetro.map[x, y - 1];
                        Tetro.map[x, 0] = false;
                        if (Tetro.map[x, yline] || (yline + 1 < ROW && Tetro.map[x, yline + 1]))
                            g.DrawLine(pen, Unit(x), Unit(yline + 1), Unit(x + 1), Unit(yline + 1));
                    }
                }
                score -= 100; // score = 200 * clearline - 100;
                Score.Text = "SCORE\n" + score.ToString();
                Timer.Interval = 100 + IntSq(Math.Max(30 - lines / 10, 0));
                clearRow = false;
            }
            g.Dispose();
            NewTetro();
        }

        private void AddObstacle()
        {
            // obstacle
            Graphics g = Graphics.FromImage(field);
            for (int i = 0; i < COL * ROW / 4; i++)
            {
                bool fullrow = true;
                do
                {
                    int index = random.Next(COL * ROW / 2);
                    obstacle.X = index % COL;
                    obstacle.Y = index / COL + ROW / 2;
                    if (obstacle.Occupied)
                        continue;
                    for (int x = 0; x < COL; x++)
                    {
                        if (x == obstacle.X)
                            continue;
                        if (!Tetro.map[x, obstacle.Y])
                        {
                            fullrow = false;
                            break;
                        }
                    }
                } while (fullrow);
                g.DrawImage(obstacle.Show(), obstacle.XU, obstacle.YU);
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
                    e.Graphics.DrawImage(tetro.Shadow(), tetro.XU, tetro.YmaxU);
                e.Graphics.DrawImage(tetro.Show(), tetro.XU, tetro.YU);
            }
            e.Graphics.DrawRectangle(pen, 0, 0, Unit(COL), Unit(ROW));
        }

        private void Preview_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(preview, 0, 0);
            e.Graphics.DrawImage(tetroNext.Show(), 0, 0);
        }

        #region Keyboard Control
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!tetro.Fall())
                ClearRow();
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
                    tetro.RotateBack();
                    break;
                case Keys.Down:
                case Keys.S:
                    if (!tetro.Fall())
                        ClearRow();
                    break;
                case Keys.Space:
                case Keys.Enter:
                    tetro.Land();
                    ClearRow();
                    break;
                case Keys.E: // hint
                    hint = !hint;
                    break;
                case Keys.Q: // swap
                    tetro.Swap(ref tetroNext);
                    Preview.Refresh();
                    break;
                case Keys.R: // restart
                    /*
                    CheckBox infoBox = new CheckBox("restart?");
                    Timer.Stop();
                    if (infoBox.ShowDialog() == DialogResult.Yes)
                    */
                        NewGame();
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
