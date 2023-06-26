namespace Tetris
{
    partial class Tetris
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.Field = new System.Windows.Forms.PictureBox();
            this.Preview = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.Field)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Preview)).BeginInit();
            this.SuspendLayout();
            // 
            // Field
            // 
            this.Field.Location = new System.Drawing.Point(69, 62);
            this.Field.Name = "Field";
            this.Field.Size = new System.Drawing.Size(100, 50);
            this.Field.TabIndex = 0;
            this.Field.TabStop = false;
            this.Field.Paint += new System.Windows.Forms.PaintEventHandler(this.Field_Paint);
            // 
            // Preview
            // 
            this.Preview.Location = new System.Drawing.Point(203, 62);
            this.Preview.Name = "Preview";
            this.Preview.Size = new System.Drawing.Size(100, 50);
            this.Preview.TabIndex = 1;
            this.Preview.TabStop = false;
            this.Preview.Paint += new System.Windows.Forms.PaintEventHandler(this.Preview_Paint);
            // 
            // Tetris
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(372, 210);
            this.Controls.Add(this.Preview);
            this.Controls.Add(this.Field);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "Tetris";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Tetris";
            this.Load += new System.EventHandler(this.Tetris_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Tetris_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.Field)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Preview)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox Field;
        private System.Windows.Forms.PictureBox Preview;
    }
}

