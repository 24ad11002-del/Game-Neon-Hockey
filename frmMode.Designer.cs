namespace WindowsFormsApp1
{
    partial class frmMode
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSingle = new System.Windows.Forms.Button();
            this.btnMulti = new System.Windows.Forms.Button();
            this.lbTitle = new System.Windows.Forms.Label();
            this.btnBack = new System.Windows.Forms.Button();
            this.pnlNames = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.lblP2 = new System.Windows.Forms.Label();
            this.lbnamePlayer1 = new System.Windows.Forms.Label();
            this.txtP2 = new System.Windows.Forms.TextBox();
            this.txtP1 = new System.Windows.Forms.TextBox();
            this.pnlNames.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSingle
            // 
            this.btnSingle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(0)))), ((int)(((byte)(50)))));
            this.btnSingle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSingle.Font = new System.Drawing.Font("Showcard Gothic", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSingle.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.btnSingle.Location = new System.Drawing.Point(120, 120);
            this.btnSingle.Name = "btnSingle";
            this.btnSingle.Size = new System.Drawing.Size(280, 70);
            this.btnSingle.TabIndex = 0;
            this.btnSingle.Text = "1 PLAYER";
            this.btnSingle.UseVisualStyleBackColor = false;
            this.btnSingle.Click += new System.EventHandler(this.btnSingle_Click);
            // 
            // btnMulti
            // 
            this.btnMulti.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(0)))), ((int)(((byte)(50)))));
            this.btnMulti.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMulti.Font = new System.Drawing.Font("Showcard Gothic", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMulti.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.btnMulti.Location = new System.Drawing.Point(120, 236);
            this.btnMulti.Name = "btnMulti";
            this.btnMulti.Size = new System.Drawing.Size(280, 70);
            this.btnMulti.TabIndex = 1;
            this.btnMulti.Text = "2 PLAYER";
            this.btnMulti.UseVisualStyleBackColor = false;
            this.btnMulti.Click += new System.EventHandler(this.btnMulti_Click);
            // 
            // lbTitle
            // 
            this.lbTitle.AutoSize = true;
            this.lbTitle.BackColor = System.Drawing.Color.Black;
            this.lbTitle.Font = new System.Drawing.Font("Segoe UI Black", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTitle.ForeColor = System.Drawing.Color.Cyan;
            this.lbTitle.Location = new System.Drawing.Point(16, 26);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(474, 81);
            this.lbTitle.TabIndex = 2;
            this.lbTitle.Text = "NEON HOCKEY";
            this.lbTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.Color.Black;
            this.btnBack.Font = new System.Drawing.Font("Showcard Gothic", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBack.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.btnBack.Location = new System.Drawing.Point(354, 354);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(120, 40);
            this.btnBack.TabIndex = 3;
            this.btnBack.Text = "BACK";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // pnlNames
            // 
            this.pnlNames.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(0)))), ((int)(((byte)(50)))));
            this.pnlNames.Controls.Add(this.label1);
            this.pnlNames.Controls.Add(this.btnConfirm);
            this.pnlNames.Controls.Add(this.lblP2);
            this.pnlNames.Controls.Add(this.lbnamePlayer1);
            this.pnlNames.Controls.Add(this.txtP2);
            this.pnlNames.Controls.Add(this.txtP1);
            this.pnlNames.Location = new System.Drawing.Point(12, 110);
            this.pnlNames.Name = "pnlNames";
            this.pnlNames.Size = new System.Drawing.Size(478, 312);
            this.pnlNames.TabIndex = 4;
            this.pnlNames.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(0)))), ((int)(((byte)(50)))));
            this.label1.Font = new System.Drawing.Font("Sitka Heading", 18F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.Cyan;
            this.label1.Location = new System.Drawing.Point(48, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(333, 43);
            this.label1.TabIndex = 5;
            this.label1.Text = "PLAYER INFORMATION";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnConfirm
            // 
            this.btnConfirm.BackColor = System.Drawing.Color.Black;
            this.btnConfirm.Font = new System.Drawing.Font("Showcard Gothic", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfirm.ForeColor = System.Drawing.Color.Lavender;
            this.btnConfirm.Location = new System.Drawing.Point(108, 228);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(270, 56);
            this.btnConfirm.TabIndex = 4;
            this.btnConfirm.Text = "LET\'S PLAY! ▶";
            this.btnConfirm.UseVisualStyleBackColor = false;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // lblP2
            // 
            this.lblP2.AutoSize = true;
            this.lblP2.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblP2.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.lblP2.Location = new System.Drawing.Point(51, 131);
            this.lblP2.Name = "lblP2";
            this.lblP2.Size = new System.Drawing.Size(183, 28);
            this.lblP2.TabIndex = 3;
            this.lblP2.Text = "🔴 Player 2 Name:";
            // 
            // lbnamePlayer1
            // 
            this.lbnamePlayer1.AutoSize = true;
            this.lbnamePlayer1.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbnamePlayer1.ForeColor = System.Drawing.Color.Crimson;
            this.lbnamePlayer1.Location = new System.Drawing.Point(43, 52);
            this.lbnamePlayer1.Name = "lbnamePlayer1";
            this.lbnamePlayer1.Size = new System.Drawing.Size(180, 28);
            this.lbnamePlayer1.TabIndex = 2;
            this.lbnamePlayer1.Text = "🔴 Player 1 Name:";
            // 
            // txtP2
            // 
            this.txtP2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(0)))), ((int)(((byte)(80)))));
            this.txtP2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtP2.ForeColor = System.Drawing.Color.White;
            this.txtP2.Location = new System.Drawing.Point(48, 165);
            this.txtP2.Name = "txtP2";
            this.txtP2.Size = new System.Drawing.Size(414, 34);
            this.txtP2.TabIndex = 1;
            // 
            // txtP1
            // 
            this.txtP1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(0)))), ((int)(((byte)(80)))));
            this.txtP1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtP1.ForeColor = System.Drawing.Color.White;
            this.txtP1.Location = new System.Drawing.Point(48, 86);
            this.txtP1.Name = "txtP1";
            this.txtP1.Size = new System.Drawing.Size(414, 34);
            this.txtP1.TabIndex = 0;
            // 
            // frmMode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(502, 434);
            this.Controls.Add(this.pnlNames);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.lbTitle);
            this.Controls.Add(this.btnMulti);
            this.Controls.Add(this.btnSingle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmMode";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmMode";
            this.pnlNames.ResumeLayout(false);
            this.pnlNames.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSingle;
        private System.Windows.Forms.Button btnMulti;
        private System.Windows.Forms.Label lbTitle;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Panel pnlNames;
        private System.Windows.Forms.Label lbnamePlayer1;
        private System.Windows.Forms.TextBox txtP2;
        private System.Windows.Forms.TextBox txtP1;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Label lblP2;
        private System.Windows.Forms.Label label1;
    }
}