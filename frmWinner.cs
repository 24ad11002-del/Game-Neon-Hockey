using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApp1
{
    public partial class frmWinner : Form
    {
        private int _winnerPlayer;
        private int _score1;
        private int _score2;
        private string _mode;

        private static string DbPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "scores.db");
        public frmWinner(int winnerPlayer, int score1, int score2)
        {
            InitializeComponent();
            _winnerPlayer = winnerPlayer;
            _score1 = score1;
            _score2 = score2;
            _mode = NeonHockey.GameSetting.Mode ?? "Multi";
        }
        public frmWinner()
        {
            InitializeComponent();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ApplyWinnerStyle();
            SaveResultToDatabase();
            LoadScoreboard();
        }
        private void frmWinner_Load(object sender, EventArgs e)
        {
        }
        //Giao diện theo người thắng
        private void ApplyWinnerStyle()
        {
            bool p1Wins = (_winnerPlayer == 1);
            Color winColor = p1Wins ? Color.FromArgb(255, 80, 80) : Color.FromArgb(0, 180, 255);

            string winnerName = (_winnerPlayer == 1)
                ? (NeonHockey.GameSetting.Player1Name ?? "Player 1")
                : (NeonHockey.GameSetting.Player2Name ?? "Player 2");
            lbnnguoithang.Text = $"{winnerName} WINS!";
            lbnnguoithang.ForeColor = winColor;
            lblTrophy.ForeColor = Color.Gold;
            lblFinalScore.Text = $"{_score1}  —  {_score2}";
            lblFinalScore.ForeColor = winColor;
            btnchoilai.FlatAppearance.BorderColor = winColor;
        }
        //Lưu kết quả vào SQLite
        private void SaveResultToDatabase()
        {
            try
            {
                using (var conn = new SQLiteConnection($"Data Source={DbPath};Version=3;"))
                {
                    conn.Open();

                    new SQLiteCommand(@"
                        CREATE TABLE IF NOT EXISTS Scores (
                            Id          INTEGER PRIMARY KEY AUTOINCREMENT,
                            Player1Name TEXT NOT NULL,
                            Player2Name TEXT NOT NULL,
                            Winner      TEXT NOT NULL,
                            Score1      INTEGER NOT NULL,
                            Score2      INTEGER NOT NULL,
                            Mode        TEXT NOT NULL,
                            Difficulty  TEXT NOT NULL,
                            PlayedAt    TEXT NOT NULL
                        )", conn).ExecuteNonQuery();

                    string winnerName = (_winnerPlayer == 1)
                        ? (NeonHockey.GameSetting.Player1Name ?? "Player 1")
                        : (NeonHockey.GameSetting.Player2Name ?? "Player 2");

                    var cmd = new SQLiteCommand(@"
                        INSERT INTO Scores
                        (Player1Name, Player2Name, Winner, Score1, Score2, Mode, Difficulty, PlayedAt)
                        VALUES
                        (@p1, @p2, @winner, @s1, @s2, @mode, @diff, @date)", conn);

                    cmd.Parameters.AddWithValue("@p1", NeonHockey.GameSetting.Player1Name ?? "Player 1");
                    cmd.Parameters.AddWithValue("@p2", NeonHockey.GameSetting.Player2Name ?? "Player 2");
                    cmd.Parameters.AddWithValue("@winner", winnerName);
                    cmd.Parameters.AddWithValue("@s1", _score1);
                    cmd.Parameters.AddWithValue("@s2", _score2);
                    cmd.Parameters.AddWithValue("@mode", _mode);
                    cmd.Parameters.AddWithValue("@diff", NeonHockey.GameSetting.Difficulty ?? "Normal");
                    cmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu DB: " + ex.Message, "Database Error");
            }
        }
        //Load top 10 vào DataGridView
        private void LoadScoreboard()
        {
            if (dgvScoreboard.Columns.Count == 0)
            {
                dgvScoreboard.Columns.Add("colRank", "#");
                dgvScoreboard.Columns.Add("colWinner", "Winner");
                dgvScoreboard.Columns.Add("colScore", "Score");
                dgvScoreboard.Columns.Add("colMode", "Mode");
                dgvScoreboard.Columns.Add("colDate", "Date");
                dgvScoreboard.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvScoreboard.Columns["colRank"].FillWeight = 25;
                dgvScoreboard.Columns["colWinner"].FillWeight = 120;
                dgvScoreboard.Columns["colScore"].FillWeight = 65;
                dgvScoreboard.Columns["colMode"].FillWeight = 70;
                dgvScoreboard.Columns["colDate"].FillWeight = 110;
            }

            dgvScoreboard.Rows.Clear();

            try
            {
                using (var conn = new SQLiteConnection($"Data Source={DbPath};Version=3;"))
                {
                    conn.Open();
                    var reader = new SQLiteCommand(@"
                        SELECT Winner, Score1, Score2, Mode, PlayedAt
                        FROM   Scores
                        ORDER  BY Id DESC
                        LIMIT  10", conn).ExecuteReader();

                    int rank = 1;
                    while (reader.Read())
                    {
                        dgvScoreboard.Rows.Add(
                            rank++,
                            reader["Winner"].ToString(),
                            $"{reader["Score1"]} — {reader["Score2"]}",
                            reader["Mode"].ToString(),
                            reader["PlayedAt"].ToString()
                        );
                    }
                }

                // Highlight dòng vừa chơi (dòng đầu = mới nhất)
                if (dgvScoreboard.Rows.Count > 0)
                {
                    dgvScoreboard.Rows[0].DefaultCellStyle.BackColor = Color.FromArgb(30, 0, 80);
                    dgvScoreboard.Rows[0].DefaultCellStyle.ForeColor = Color.Gold;
                    dgvScoreboard.Rows[0].DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                }
            }
            catch (Exception ex)
            {
                dgvScoreboard.Rows.Add(1, "Lỗi DB", "—", "—", ex.Message);
            }
        }

        private void btnchoilai_Click(object sender, EventArgs e)
        {
            new frmGame().Show();
            this.Close();
        }

        private void btnmenu_Click(object sender, EventArgs e)
        {
            foreach (Form f in Application.OpenForms)
            {
                if (f is frmMenu menu) { menu.Show(); this.Close(); return; }
            }
            new frmMenu().Show();
            this.Close();
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
