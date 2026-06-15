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
    public partial class frmDifficulty : Form
    {
        public frmDifficulty()
        {
            InitializeComponent();
        }

        private void btnEasy_Click(object sender, EventArgs e)
        {
            GameSetting.Difficulty = "Easy";

            frmGame game = new frmGame();
            game.Show();

            this.Hide();
        }

        private void btnMedium_Click(object sender, EventArgs e)
        {
            GameSetting.Difficulty = "Medium";

            frmGame game = new frmGame();
            game.Show();

            this.Hide();
        }

        private void btnHard_Click(object sender, EventArgs e)
        {
            GameSetting.Difficulty = "Hard";
            
            frmGame game = new frmGame();
            game.Show();
            
            this.Hide();
        }
    }
}
