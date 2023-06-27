using System;
using System.Drawing;
using System.Windows.Forms;

namespace Tetris
{
    public partial class Tetris : Form
    {
        internal const int UNIT = 35,
            COL = 10, ROW = 20;

        internal static Tetro tetro, tetroNext = Tetro.New();

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
            Tetro.field = new Bitmap(Field.Width, Field.Height);
            Graphics g = Graphics.FromImage(Tetro.field);
            Pen pen = new Pen(Color.Gray), penFrame = new Pen(Color.White);
            for (int i = 0; i <= COL; i++)
                g.DrawLine(pen, i * UNIT, 0, i * UNIT, Tetro.field.Height);
            for (int i = 0; i <= ROW; i++)
                g.DrawLine(pen, 0, i * UNIT, Tetro.field.Width, i * UNIT);
            g.DrawRectangle(penFrame, 0, 0, COL * UNIT, ROW * UNIT);
            g.Dispose();
            Preview.Location = new Point((COL + 2) * UNIT, UNIT);
            Preview.Size = new Size (4 * UNIT + 1, 4 * UNIT + 1);
            Preview.BackColor = Color.Black;
            Tetro.preview = new Bitmap(Preview.Width, Preview.Height);
            g = Graphics.FromImage(Tetro.preview);
            for (int i = 0; i <= 4; i++)
            {
                g.DrawLine(pen, i * UNIT, 0, i * UNIT, Tetro.preview.Height);
                g.DrawLine(pen, 0, i * UNIT, Tetro.preview.Width, i * UNIT);
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

        private void Field_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(Tetro.field, 0, 0);
            e.Graphics.DrawImage(tetro.Hint(), tetro.x * UNIT, tetro.ymax * UNIT);
            e.Graphics.DrawImage(tetro.Show(), tetro.x * UNIT, tetro.y * UNIT);
        }

        private void Preview_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(Tetro.preview, 0, 0);
            e.Graphics.DrawImage(tetroNext.Show(), 0, 0);
        }

        #region Keyboard Control
        private void Timer_Tick(object sender, EventArgs e)
        {
            tetro.Fall();
            Field.Refresh();
            Preview.Refresh();
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
                case Keys.Down:
                case Keys.S:
                    tetro.Fall();
                    Preview.Refresh();
                    break;
                case Keys.Up:
                case Keys.W:
                    tetro.Rotate();
                    break;
                case Keys.Space:
                case Keys.Enter:
                    tetro.Land();
                    Preview.Refresh();
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
