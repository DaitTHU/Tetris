using System;
using System.Drawing;
using System.Security.Policy;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Tetris
{
    public partial class Tetris : Form
    {

        private const int UNIT = 35,
            COL = 10, ROW = 20;
        // private int
        private Graphics graphF, graphP;
        private Bitmap bitmapF, bitmapP;
        private Pen pen = new Pen(Color.Gray);

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
            bitmapF = new Bitmap(Field.Width, Field.Height);
            graphF = Graphics.FromImage(bitmapF);
            for (int i = 0; i <= COL; i++)
                graphF.DrawLine(pen, i * UNIT, 0, i * UNIT, bitmapF.Height);
            for (int i = 0; i <= ROW; i++)
                graphF.DrawLine(pen, 0, i * UNIT, bitmapF.Width, i * UNIT);
            Preview.Location = new Point((COL + 2) * UNIT, UNIT);
            Preview.Size = new Size (4 * UNIT + 1, 4 * UNIT + 1);
            Preview.BackColor = Color.Black;
            bitmapP = new Bitmap(Preview.Width, Preview.Height);
            graphP = Graphics.FromImage(bitmapP);
            for (int i = 0; i <= 4; i++)
                graphP.DrawLine(pen, i * UNIT, 0, i * UNIT, bitmapP.Height);
            for (int i = 0; i <= 4; i++)
                graphP.DrawLine(pen, 0, i * UNIT, bitmapP.Width, i * UNIT);
            // Control
            this.KeyPreview = true;
        }

        private void Tetris_Load(object sender, EventArgs e) { }

        private void Field_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(bitmapF, 0, 0);
        }

        private void Preview_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(bitmapP, 0, 0);
        }

        #region Keyboard Control
        private void Tetris_KeyDown(object sender, KeyEventArgs e)
        {

        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                case Keys.A:
                case Keys.S:
                case Keys.W:
                case Keys.D:
                    return true;
                case Keys.Escape:
                    this.Close();
                    return true;
            }
            return false;
        }
        #endregion
    }
}
