namespace WindowsFormsApp1
{
    partial class frmWinner
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
            this.dgvScoreboard = new System.Windows.Forms.DataGridView();
            this.btnMainMenu = new System.Windows.Forms.Button();
            this.pnlScore = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.lblScoreTitle = new System.Windows.Forms.Label();
            this.lblWinner = new System.Windows.Forms.Label();
            this.lblTrophy = new System.Windows.Forms.Label();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnchoilai = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.lbnnguoithang = new System.Windows.Forms.Label();
            this.btnmenu = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblFinalScore = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnPlayAgain = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvScoreboard)).BeginInit();
            this.pnlScore.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvScoreboard
            // 
            this.dgvScoreboard.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvScoreboard.Location = new System.Drawing.Point(14, 299);
            this.dgvScoreboard.Name = "dgvScoreboard";
            this.dgvScoreboard.RowHeadersWidth = 51;
            this.dgvScoreboard.RowTemplate.Height = 24;
            this.dgvScoreboard.Size = new System.Drawing.Size(672, 279);
            this.dgvScoreboard.TabIndex = 8;
            // 
            // btnMainMenu
            // 
            this.btnMainMenu.Font = new System.Drawing.Font("Segoe UI Black", 12F, System.Drawing.FontStyle.Bold);
            this.btnMainMenu.ForeColor = System.Drawing.Color.Brown;
            this.btnMainMenu.Location = new System.Drawing.Point(274, 616);
            this.btnMainMenu.Name = "btnMainMenu";
            this.btnMainMenu.Size = new System.Drawing.Size(209, 64);
            this.btnMainMenu.TabIndex = 9;
            this.btnMainMenu.Text = "🏠 MAIN MENU";
            this.btnMainMenu.UseVisualStyleBackColor = true;
            // 
            // pnlScore
            // 
            this.pnlScore.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.pnlScore.Controls.Add(this.label1);
            this.pnlScore.Controls.Add(this.lblScoreTitle);
            this.pnlScore.Location = new System.Drawing.Point(237, 116);
            this.pnlScore.Name = "pnlScore";
            this.pnlScore.Size = new System.Drawing.Size(311, 116);
            this.pnlScore.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Showcard Gothic", 16F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.Crimson;
            this.label1.Location = new System.Drawing.Point(121, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 33);
            this.label1.TabIndex = 13;
            this.label1.Text = "3 - 1";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblScoreTitle
            // 
            this.lblScoreTitle.AutoSize = true;
            this.lblScoreTitle.Font = new System.Drawing.Font("Showcard Gothic", 16F, System.Drawing.FontStyle.Bold);
            this.lblScoreTitle.ForeColor = System.Drawing.Color.Crimson;
            this.lblScoreTitle.Location = new System.Drawing.Point(58, 19);
            this.lblScoreTitle.Name = "lblScoreTitle";
            this.lblScoreTitle.Size = new System.Drawing.Size(188, 33);
            this.lblScoreTitle.TabIndex = 12;
            this.lblScoreTitle.Text = "FINAL SCORE";
            this.lblScoreTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblWinner
            // 
            this.lblWinner.AutoSize = true;
            this.lblWinner.Font = new System.Drawing.Font("Impact", 42F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWinner.ForeColor = System.Drawing.Color.Brown;
            this.lblWinner.Location = new System.Drawing.Point(165, 14);
            this.lblWinner.Name = "lblWinner";
            this.lblWinner.Size = new System.Drawing.Size(467, 85);
            this.lblWinner.TabIndex = 3;
            this.lblWinner.Text = "PLAYER 1 WINS!";
            // 
            // lblTrophy
            // 
            this.lblTrophy.AutoSize = true;
            this.lblTrophy.Font = new System.Drawing.Font("Segoe UI Emoji", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTrophy.ForeColor = System.Drawing.Color.Brown;
            this.lblTrophy.Location = new System.Drawing.Point(47, 14);
            this.lblTrophy.Name = "lblTrophy";
            this.lblTrophy.Size = new System.Drawing.Size(112, 80);
            this.lblTrophy.TabIndex = 2;
            this.lblTrophy.Text = "🏆";
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.Color.Gray;
            this.pnlMain.Controls.Add(this.panel1);
            this.pnlMain.Controls.Add(this.btnPlayAgain);
            this.pnlMain.Controls.Add(this.label2);
            this.pnlMain.Controls.Add(this.dgvScoreboard);
            this.pnlMain.Controls.Add(this.lblTrophy);
            this.pnlMain.Controls.Add(this.lblWinner);
            this.pnlMain.Controls.Add(this.btnMainMenu);
            this.pnlMain.Controls.Add(this.pnlScore);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(724, 357);
            this.pnlMain.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.panel1.Controls.Add(this.btnchoilai);
            this.panel1.Controls.Add(this.btnExit);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.lbnnguoithang);
            this.panel1.Controls.Add(this.btnmenu);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(724, 357);
            this.panel1.TabIndex = 16;
            // 
            // btnchoilai
            // 
            this.btnchoilai.Font = new System.Drawing.Font("Segoe UI Black", 12F, System.Drawing.FontStyle.Bold);
            this.btnchoilai.ForeColor = System.Drawing.Color.Brown;
            this.btnchoilai.Location = new System.Drawing.Point(22, 257);
            this.btnchoilai.Name = "btnchoilai";
            this.btnchoilai.Size = new System.Drawing.Size(176, 64);
            this.btnchoilai.TabIndex = 17;
            this.btnchoilai.Text = "▶ PLAY AGAIN";
            this.btnchoilai.UseVisualStyleBackColor = true;
            this.btnchoilai.Click += new System.EventHandler(this.btnchoilai_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("Segoe UI Black", 12F, System.Drawing.FontStyle.Bold);
            this.btnExit.ForeColor = System.Drawing.Color.Brown;
            this.btnExit.Location = new System.Drawing.Point(494, 257);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(209, 64);
            this.btnExit.TabIndex = 16;
            this.btnExit.Text = "✖ EXIT GAME";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Emoji", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Brown;
            this.label4.Location = new System.Drawing.Point(47, 14);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(112, 80);
            this.label4.TabIndex = 2;
            this.label4.Text = "🏆";
            // 
            // lbnnguoithang
            // 
            this.lbnnguoithang.AutoSize = true;
            this.lbnnguoithang.Font = new System.Drawing.Font("Impact", 42F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbnnguoithang.ForeColor = System.Drawing.Color.Brown;
            this.lbnnguoithang.Location = new System.Drawing.Point(165, 14);
            this.lbnnguoithang.Name = "lbnnguoithang";
            this.lbnnguoithang.Size = new System.Drawing.Size(467, 85);
            this.lbnnguoithang.TabIndex = 3;
            this.lbnnguoithang.Text = "PLAYER 1 WINS!";
            // 
            // btnmenu
            // 
            this.btnmenu.Font = new System.Drawing.Font("Segoe UI Black", 12F, System.Drawing.FontStyle.Bold);
            this.btnmenu.ForeColor = System.Drawing.Color.Brown;
            this.btnmenu.Location = new System.Drawing.Point(237, 257);
            this.btnmenu.Name = "btnmenu";
            this.btnmenu.Size = new System.Drawing.Size(209, 64);
            this.btnmenu.TabIndex = 9;
            this.btnmenu.Text = "🏠 MAIN MENU";
            this.btnmenu.UseVisualStyleBackColor = true;
            this.btnmenu.Click += new System.EventHandler(this.btnmenu_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(0)))), ((int)(((byte)(50)))));
            this.panel2.Controls.Add(this.lblFinalScore);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Location = new System.Drawing.Point(216, 102);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(329, 127);
            this.panel2.TabIndex = 1;
            // 
            // lblFinalScore
            // 
            this.lblFinalScore.AutoSize = true;
            this.lblFinalScore.Font = new System.Drawing.Font("Showcard Gothic", 18F, System.Drawing.FontStyle.Bold);
            this.lblFinalScore.ForeColor = System.Drawing.Color.Crimson;
            this.lblFinalScore.Location = new System.Drawing.Point(121, 67);
            this.lblFinalScore.Name = "lblFinalScore";
            this.lblFinalScore.Size = new System.Drawing.Size(73, 37);
            this.lblFinalScore.TabIndex = 13;
            this.lblFinalScore.Text = "3 - 1";
            this.lblFinalScore.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Showcard Gothic", 18F, System.Drawing.FontStyle.Bold);
            this.label7.ForeColor = System.Drawing.Color.Crimson;
            this.label7.Location = new System.Drawing.Point(58, 19);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(209, 37);
            this.label7.TabIndex = 12;
            this.label7.Text = "FINAL SCORE";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnPlayAgain
            // 
            this.btnPlayAgain.Font = new System.Drawing.Font("Segoe UI Black", 12F, System.Drawing.FontStyle.Bold);
            this.btnPlayAgain.ForeColor = System.Drawing.Color.Brown;
            this.btnPlayAgain.Location = new System.Drawing.Point(14, 616);
            this.btnPlayAgain.Name = "btnPlayAgain";
            this.btnPlayAgain.Size = new System.Drawing.Size(170, 64);
            this.btnPlayAgain.TabIndex = 15;
            this.btnPlayAgain.Text = "▶ PLAY AGAIN";
            this.btnPlayAgain.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Showcard Gothic", 16F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.Brown;
            this.label2.Location = new System.Drawing.Point(16, 245);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(236, 33);
            this.label2.TabIndex = 14;
            this.label2.Text = "🏅 TOP SCORES   ";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmWinner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(724, 357);
            this.Controls.Add(this.pnlMain);
            this.ForeColor = System.Drawing.Color.DarkBlue;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmWinner";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Game Over";
            this.Load += new System.EventHandler(this.frmWinner_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvScoreboard)).EndInit();
            this.pnlScore.ResumeLayout(false);
            this.pnlScore.PerformLayout();
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView dgvScoreboard;
        private System.Windows.Forms.Button btnMainMenu;
        private System.Windows.Forms.Panel pnlScore;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblScoreTitle;
        private System.Windows.Forms.Label lblWinner;
        private System.Windows.Forms.Label lblTrophy;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbnnguoithang;
        private System.Windows.Forms.Button btnmenu;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblFinalScore;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnPlayAgain;
        private System.Windows.Forms.Button btnchoilai;
    }
}