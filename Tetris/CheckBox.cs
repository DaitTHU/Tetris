using System;
using System.Windows.Forms;

namespace Tetris
{
    public partial class CheckBox : Form
    {
        public CheckBox(string info, bool decision = true)
        {
            InitializeComponent();
            //this.ClientSize = new Size(400, 300);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            //Info.Location = new Point(0, 50);
            //Info.Size = new Size(this.ClientSize.Width, 50);
            Info.Text = info.PadLeft(8 + info.Length / 2);
            //Info.TextAlign = ContentAlignment.TopCenter;
            Yes.Visible = decision;
            No.Visible = decision;
            OK.Visible = !decision;
        }

        private void Yes_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
            Close();
        }

        private void No_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            Close();
        }

        private void OK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }

    public partial class InfoBox : CheckBox
    {
        public InfoBox(string info) : base(info, false)
        { Text = "Info"; ShowDialog(); }
    }
}
