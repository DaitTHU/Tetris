using System;
using System.Drawing;
using System.Windows.Forms;

namespace Tetris
{
    public partial class Tetris : Form
    {
        public const int UNIT = 35,
            COL = 10, ROW = 20;

        private readonly Bitmap background;
        private Bitmap field, preview;
        private Tetro tetro, tetroNext = Tetro.New(),
            obstacle = new Tetro(7, 0);
        private int score = 0, lines = 0;

        private bool hint = false, clearRow = false;
        private static readonly Pen pen = new Pen(Color.Gray),
            penFrame = new Pen(Color.White);
        private static readonly Brush eraser = new SolidBrush(Color.Transparent);
        private readonly Random random = new Random();

        public Tetris()
        {
            InitializeComponent();
            // Form Appearance
            this.ClientSize = new Size((COL + 7) * UNIT, (ROW + 2) * UNIT);
            this.BackColor = Color.Black;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            // Field
            Field.Location = new Point(UNIT, UNIT);
            Field.Size = new Size(COL * UNIT + 1, ROW * UNIT + 1);
            Field.BackColor = Color.Black;
            background = new Bitmap(Field.Width, Field.Height);
            field = new Bitmap(Field.Width, Field.Height);
            // Preview
            Preview.Location = new Point((COL + 2) * UNIT, UNIT);
            Preview.Size = new Size(4 * UNIT + 1, 4 * UNIT + 1);
            Preview.BackColor = Color.Black;
            preview = new Bitmap(Preview.Width, Preview.Height);
            // Label
            Score.Location = new Point((COL + 2) * UNIT, 6 * UNIT);
            Lines.Location = new Point((COL + 2) * UNIT, 8 * UNIT);
            Score.Size = Lines.Size = new Size(4 * UNIT, UNIT);
            // Control
            Timer.Enabled = true;
            this.KeyPreview = true;
            NewGame();
        }

        private void Tetris_Load(object sender, EventArgs e) { }

        #region game code
        private void NewGame()
        {
            score = lines = 0;
            Score.Text = "SCORE\n" + score.ToString();
            Lines.Text = "LINES\n" + lines.ToString();
            Timer.Interval = 1000;
            // Field
            Graphics g = Graphics.FromImage(background);
            for (int x = 0; x <= COL; x++)
                g.DrawLine(pen, x * UNIT, 0, x * UNIT, field.Height);
            for (int y = 0; y <= ROW; y++)
                g.DrawLine(pen, 0, y * UNIT, field.Width, y * UNIT);
            g.Dispose();
            // Preview
            g = Graphics.FromImage(preview);
            for (int i = 0; i <= 4; i++)
            {
                g.DrawLine(pen, i * UNIT, 0, i * UNIT, preview.Height);
                g.DrawLine(pen, 0, i * UNIT, preview.Width, i * UNIT);
            }
            g.DrawRectangle(penFrame, 0, 0, 4 * UNIT, 4 * UNIT);
            g.Dispose();
            //Obstacle();
            // Tetro
            tetro = tetroNext.Inherit();
            tetroNext = Tetro.New();
        }

        private void Fall()
        {
            if (tetro.y < tetro.ymax)
                tetro.y++;
            else
                Land();
        }

        private void Land()
        {
            tetro.y = tetro.ymax;
            score++;
            Graphics g = Graphics.FromImage(field);
            g.DrawImage(tetro.Show(true), tetro.x * UNIT, tetro.y * UNIT);
            // clear full row(s)
            var fullRow = new bool[4];
            for (int dy = Math.Max(0, -tetro.y); dy < Math.Min(4, ROW - tetro.y); dy++)
            {
                fullRow[dy] = true;
                for (int x = 0; x < COL; x++)
                    fullRow[dy] &= Tetro.map[x, tetro.y + dy];
                if (fullRow[dy])
                {
                    g.FillRectangle(new SolidBrush(Color.White),
                        0, (tetro.y + dy) * UNIT, field.Width, UNIT);
                    lines++; score += 200;
                    Lines.Text = "LINES\n" + lines.ToString();
                }
            }
            if (fullRow[0] | fullRow[1] | fullRow[2] | fullRow[3])
            {
                clearRow = true;
                score -= 100; // 200 * line - 100;
                Field.Refresh();
                System.Threading.Thread.Sleep(Timer.Interval * 3 / 10);
                for (int dy = 0; dy < 4; dy++)
                {
                    if (!fullRow[dy])
                        continue;
                    int yline = tetro.y + dy;
                    for (int x = 0; x < COL; x++)
                    {
                        for (int y = yline; y > 0; y--)
                            Tetro.map[x, y] = Tetro.map[x, y - 1];
                        Tetro.map[x, 0] = false;
                        if ((yline + 1 >= ROW || !Tetro.map[x, yline + 1]) & !Tetro.map[x, yline])
                            g.DrawLine(pen, x * UNIT, (yline + 1) * UNIT, 
                                (x + 1) * UNIT, (yline + 1) * UNIT);
                    }
                    //Bitmap imageAbove = field.Clone(new Rectangle(0, 0, field.Width, yline * UNIT), field.PixelFormat);
                    g.FillRectangle(eraser, 0, 0, field.Width, (yline + 1) * UNIT);
                    //g.DrawImage(imageAbove, 0, UNIT);
                }
                clearRow = false;
            }
            Score.Text = "SCORE\n" + score.ToString();
            tetro = tetroNext.Inherit();
            tetroNext = Tetro.New();
            Field.Refresh();
            Preview.Refresh();
        }

        private void Swap()
        {
            Tetro tetroSwap = tetroNext.Copy();
            tetroSwap.Coordinate = tetro.Coordinate;
            foreach (var dx in new int[] { 0, 1, 1, -3, -1 })
            {
                tetroSwap.x += dx;
                if (tetroSwap.Feasible())
                {
                    tetro.Coordinate = tetroNext.Coordinate;
                    (tetro, tetroNext) = (tetroSwap, tetro);
                    tetro.UpdateYmax();
                    Preview.Refresh();
                    return;
                }
            }            
        }
        
        private void Obstacle()
        {
            // obstacle
            Graphics g = Graphics.FromImage(field);
            for (int i = 0; i < COL * ROW / 4; i++)
            {
                bool fullrow = true;
                do
                {
                    int index = random.Next(COL * ROW / 2);
                    obstacle.x = index % COL;
                    obstacle.y = index / COL + ROW / 2;
                    if (Tetro.map[obstacle.x, obstacle.y])
                        continue;
                    for (int x = 0; x < COL; x++)
                        if (!(x == obstacle.x || Tetro.map[x, obstacle.y]))
                        {
                            fullrow = false;
                            break;
                        }
                } while (fullrow);
                Tetro.map[obstacle.x, obstacle.y] = true;
                g.DrawImage(obstacle.Show(true), obstacle.x * UNIT, obstacle.y * UNIT);
            }
            g.Dispose();
        }
        #endregion

        private void Field_Paint(object sender, PaintEventArgs e)
        {
            //e.Graphics.DrawImage(background, 0, 0);
            e.Graphics.DrawImage(field, 0, 0);
            if (!clearRow)
            {
                if (hint)
                    e.Graphics.DrawImage(tetro.Hint(), tetro.x * UNIT, tetro.ymax * UNIT);
                e.Graphics.DrawImage(tetro.Show(), tetro.x * UNIT, tetro.y * UNIT);
            }
            //e.Graphics.DrawRectangle(penFrame, 0, 0, COL * UNIT, ROW * UNIT);
        }

        private void Preview_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(preview, 0, 0);
            e.Graphics.DrawImage(tetroNext.Show(), 0, 0);
        }

        #region Keyboard Control
        private void Timer_Tick(object sender, EventArgs e)
        {
            Fall();
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
                case Keys.Down:
                case Keys.S:
                    Fall();
                    break;
                case Keys.Space:
                case Keys.Enter:
                    Land();
                    break;
                case Keys.E: // hint
                    hint = !hint;
                    break;
                case Keys.Q: // swap
                    Swap();
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
