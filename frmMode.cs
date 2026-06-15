using NeonHockey;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class frmMode : Form
    {
        public frmMode()
        {
            InitializeComponent();
        }

        private void btnSingle_Click(object sender, EventArgs e)
        {
            GameSetting.Mode = "Single";

            //Single
            lblP2.Visible = false;
            txtP2.Visible = false;
            txtP2.Text = "CPU";

            //Ấn nút mode, hiện pannel
            btnSingle.Visible = false;
            btnMulti.Visible = false;
            pnlNames.Visible = true;

            txtP1.Focus();
            txtP1.SelectAll();
        }
        private void btnMulti_Click(object sender, EventArgs e)
        {
            GameSetting.Mode = "Multi";

            // Multi: hiện cả 2 ô nhập tên
            lblP2.Visible = true;
            txtP2.Visible = true;
            txtP2.Text = "Player 2";

            btnSingle.Visible = false;
            btnMulti.Visible = false;
            pnlNames.Visible = true;

            txtP1.Focus();
            txtP1.SelectAll();
        }
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            // Lưu tên vào GameSetting, dùng default nếu bỏ trống
            string name1 = txtP1.Text.Trim();
            string name2 = txtP2.Text.Trim();

            GameSetting.Player1Name = string.IsNullOrEmpty(name1) ? "Player 1" : name1;
            GameSetting.Player2Name = string.IsNullOrEmpty(name2) ? (GameSetting.Mode == "Single" ? "CPU" : "Player 2") : name2;

            // Tiếp tục sang chọn độ khó
            var diff = new frmDifficulty();
            diff.Show();
            this.Hide();
        }
        private void btnBack_Click(object sender, EventArgs e)
        {
            // Nếu đang ở panel nhập tên → quay lại chọn mode
            if (pnlNames.Visible)
            {
                pnlNames.Visible = false;
                btnSingle.Visible = true;
                btnMulti.Visible = true;
                return;
            }
            // Còn lại → về frmMenu
            foreach (Form f in Application.OpenForms)
            {
                if (f is frmMenu menu) { menu.Show(); break; }
            }
            this.Close();
        }
    }
}
