namespace WindowsFormsApp1
{
    partial class frmGame
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnlArena   = new System.Windows.Forms.Panel();
            this.timerGame  = new System.Windows.Forms.Timer(this.components);
            this.label1     = new System.Windows.Forms.Label();
            this.label2     = new System.Windows.Forms.Label();
            this.label4     = new System.Windows.Forms.Label();
            this.SuspendLayout();
            //
            // pnlArena
            //
            this.pnlArena.BackColor = System.Drawing.Color.Black;
            this.pnlArena.Location  = new System.Drawing.Point(31, 29);
            this.pnlArena.Name      = "pnlArena";
            this.pnlArena.Size      = new System.Drawing.Size(572, 750);
            this.pnlArena.TabIndex  = 3;
            this.pnlArena.Paint    += new System.Windows.Forms.PaintEventHandler(this.pnlArena_Paint);
            this.pnlArena.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlArena_MouseMove);
            //
            // timerGame
            //
            this.timerGame.Enabled  = true;
            this.timerGame.Interval = 16;
            this.timerGame.Tick    += new System.EventHandler(this.timerGame_Tick);
            //
            // label1
            //
            this.label1.AutoSize  = true;
            this.label1.Font      = new System.Drawing.Font("Imprint MT Shadow", 10.8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location  = new System.Drawing.Point(27, 795);
            this.label1.Name      = "label1";
            this.label1.Size      = new System.Drawing.Size(136, 21);
            this.label1.TabIndex  = 4;
            this.label1.Text      = "PAUSE = ESC";
            //
            // label2
            //
            this.label2.AutoSize  = true;
            this.label2.Font      = new System.Drawing.Font("Imprint MT Shadow", 10.8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.IndianRed;
            this.label2.Location  = new System.Drawing.Point(228, 795);
            this.label2.Name      = "label2";
            this.label2.Size      = new System.Drawing.Size(186, 21);
            this.label2.TabIndex  = 5;
            this.label2.Text      = "MAIN MENU = M";
            //
            // label4
            //
            this.label4.AutoSize  = true;
            this.label4.Font      = new System.Drawing.Font("Imprint MT Shadow", 10.8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location  = new System.Drawing.Point(512, 795);
            this.label4.Name      = "label4";
            this.label4.Size      = new System.Drawing.Size(91, 21);
            this.label4.TabIndex  = 7;
            this.label4.Text      = "EXIT = X";
            //
            // frmGame
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode  = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor      = System.Drawing.Color.Black;
            this.ClientSize     = new System.Drawing.Size(634, 853);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pnlArena);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview     = true;
            this.MaximizeBox    = false;
            this.Name           = "frmGame";
            this.StartPosition  = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text           = "Neon Hockey";
            this.Load    += new System.EventHandler(this.frmGame_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmGame_KeyDown);
            this.KeyUp   += new System.Windows.Forms.KeyEventHandler(this.frmGame_KeyUp);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel  pnlArena;
        private System.Windows.Forms.Timer  timerGame;
        private System.Windows.Forms.Label  label1;
        private System.Windows.Forms.Label  label2;
        private System.Windows.Forms.Label  label4;
    }
}
