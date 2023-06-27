using System;
using System.Drawing;
using System.Windows.Forms;

namespace Tetris
{
    public partial class Tetris : Form
    {
        public const int UNIT = 35,
            COL = 10, ROW = 20;

        internal Bitmap field, preview;
        internal Tetro tetro, tetroNext = Tetro.New();

        private bool hint = false, clearRow = false;
        private static Pen pen = new Pen(Color.Gray), 
            penFrame = new Pen(Color.White);

        public Tetris()
        {
            InitializeComponent();
            // Form Appearance
            this.ClientSize = new Size((COL + 7) * UNIT, (ROW + 2) * UNIT);
            this.BackColor = Color.Black;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            // Field and Preview
            Field.Location = new Point(UNIT, UNIT);
            Field.Size = new Size(COL * UNIT + 1, ROW * UNIT + 1);
            Field.BackColor = Color.Black;
            field = new Bitmap(Field.Width, Field.Height);
            Graphics g = Graphics.FromImage(field);
            g.Clear(Color.Black);
            for (int i = 0; i <= COL; i++)
                g.DrawLine(pen, i * UNIT, 0, i * UNIT, field.Height);
            for (int i = 0; i <= ROW; i++)
                g.DrawLine(pen, 0, i * UNIT, field.Width, i * UNIT);
            g.Dispose();
            Preview.Location = new Point((COL + 2) * UNIT, UNIT);
            Preview.Size = new Size(4 * UNIT + 1, 4 * UNIT + 1);
            Preview.BackColor = Color.Black;
            preview = new Bitmap(Preview.Width, Preview.Height);
            g = Graphics.FromImage(preview);
            for (int i = 0; i <= 4; i++)
            {
                g.DrawLine(pen, i * UNIT, 0, i * UNIT, preview.Height);
                g.DrawLine(pen, 0, i * UNIT, preview.Width, i * UNIT);
            }
            g.DrawRectangle(penFrame, 0, 0, 4 * UNIT, 4 * UNIT);
            // Tetris
            tetro = tetroNext.Inherit();
            tetroNext = Tetro.New();
            // Control
            Timer.Enabled = true;
            Timer.Interval = 1000;
            this.KeyPreview = true;
        }

        private void Tetris_Load(object sender, EventArgs e) { }


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
                    g.FillRectangle(new SolidBrush(Color.White),
                        0, (tetro.y + dy) * UNIT, field.Width, UNIT);
            }
            if (fullRow[0] | fullRow[1] | fullRow[2] | fullRow[3])
            {
                clearRow = true;
                Field.Refresh();
                System.Threading.Thread.Sleep(Timer.Interval * 3 / 10);
                for (int dy = 0; dy < 4; dy++)
                {
                    if (!fullRow[dy])
                        continue;
                    for (int x = 0; x < COL; x++)
                    {
                        for (int y = tetro.y + dy; y > 0; y--)
                            Tetro.map[x, y] = Tetro.map[x, y - 1];
                        Tetro.map[x, 0] = false;
                    }
                    g.DrawImage(field.Clone(new Rectangle(
                        0, 0, field.Width, (tetro.y + dy) * UNIT),
                        field.PixelFormat), 0, UNIT);
                    g.FillRectangle(new SolidBrush(Color.Black), 
                        1, 1, COL * UNIT - 1, UNIT - 1);
                    g.DrawLine(pen, 0, 0, field.Width, 0);
                    for (int x = 0; x <= COL; x++)
                        g.DrawLine(pen, x * UNIT, 0, x * UNIT, UNIT);
                }
                clearRow = false;
            }
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

        private void Field_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(field, 0, 0);
            if (!clearRow)
            {
                if (hint)
                    e.Graphics.DrawImage(tetro.Hint(), tetro.x * UNIT, tetro.ymax * UNIT);
                e.Graphics.DrawImage(tetro.Show(), tetro.x * UNIT, tetro.y * UNIT);
            }
            e.Graphics.DrawRectangle(penFrame, 0, 0, COL * UNIT, ROW * UNIT);
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
