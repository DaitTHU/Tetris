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
            this.components = new System.ComponentModel.Container();
            this.Field = new System.Windows.Forms.PictureBox();
            this.Preview = new System.Windows.Forms.PictureBox();
            this.Timer = new System.Windows.Forms.Timer(this.components);
            this.Score = new System.Windows.Forms.Label();
            this.Lines = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Field)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Preview)).BeginInit();
            this.SuspendLayout();
            // 
            // Field
            // 
            this.Field.Location = new System.Drawing.Point(73, 46);
            this.Field.Name = "Field";
            this.Field.Size = new System.Drawing.Size(100, 50);
            this.Field.TabIndex = 0;
            this.Field.TabStop = false;
            this.Field.Paint += new System.Windows.Forms.PaintEventHandler(this.Field_Paint);
            // 
            // Preview
            // 
            this.Preview.Location = new System.Drawing.Point(203, 46);
            this.Preview.Name = "Preview";
            this.Preview.Size = new System.Drawing.Size(100, 50);
            this.Preview.TabIndex = 1;
            this.Preview.TabStop = false;
            this.Preview.Paint += new System.Windows.Forms.PaintEventHandler(this.Preview_Paint);
            // 
            // Timer
            // 
            this.Timer.Enabled = true;
            this.Timer.Interval = 1000;
            this.Timer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // Score
            // 
            this.Score.AutoSize = true;
            this.Score.BackColor = System.Drawing.Color.Transparent;
            this.Score.Font = new System.Drawing.Font("Consolas", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Score.ForeColor = System.Drawing.Color.White;
            this.Score.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Score.Location = new System.Drawing.Point(66, 125);
            this.Score.Name = "Score";
            this.Score.Size = new System.Drawing.Size(107, 37);
            this.Score.TabIndex = 2;
            this.Score.Text = "score";
            // 
            // Lines
            // 
            this.Lines.AutoSize = true;
            this.Lines.BackColor = System.Drawing.Color.Transparent;
            this.Lines.Font = new System.Drawing.Font("Consolas", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Lines.ForeColor = System.Drawing.Color.White;
            this.Lines.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Lines.Location = new System.Drawing.Point(196, 125);
            this.Lines.Name = "Lines";
            this.Lines.Size = new System.Drawing.Size(107, 37);
            this.Lines.TabIndex = 3;
            this.Lines.Text = "lines";
            // 
            // Tetris
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(374, 260);
            this.Controls.Add(this.Lines);
            this.Controls.Add(this.Score);
            this.Controls.Add(this.Preview);
            this.Controls.Add(this.Field);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "Tetris";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Tetris";
            this.Load += new System.EventHandler(this.Tetris_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Field)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Preview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox Field;
        private System.Windows.Forms.PictureBox Preview;
        private System.Windows.Forms.Timer Timer;
        private System.Windows.Forms.Label Score;
        private System.Windows.Forms.Label Lines;
    }
}

