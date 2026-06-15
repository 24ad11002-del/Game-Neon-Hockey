using System;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class frmScoreboard : Form
    {
        private static string DbPath =>
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "scores.db");

        public frmScoreboard()
        {
            InitializeComponent();
        }

        private void frmScoreboard_Load(object sender, EventArgs e)
        {
            InitializeDataGridView();
            LoadAllScores();
        }

        private void InitializeDataGridView()
        {
            dgvScoreboard.Columns.Clear();
            dgvScoreboard.Columns.Add("colRank", "#");
            dgvScoreboard.Columns.Add("colWinner", "Winner");
            dgvScoreboard.Columns.Add("colScore", "Score");
            dgvScoreboard.Columns.Add("colMode", "Mode");
            dgvScoreboard.Columns.Add("colDifficulty", "Difficulty");
            dgvScoreboard.Columns.Add("colDate", "Date");

            dgvScoreboard.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvScoreboard.Columns["colRank"].FillWeight = 30;
            dgvScoreboard.Columns["colWinner"].FillWeight = 100;
            dgvScoreboard.Columns["colScore"].FillWeight = 70;
            dgvScoreboard.Columns["colMode"].FillWeight = 70;
            dgvScoreboard.Columns["colDifficulty"].FillWeight = 70;
            dgvScoreboard.Columns["colDate"].FillWeight = 120;
            dgvScoreboard.AllowUserToAddRows = false;
        }

        private void LoadAllScores()
        {
            dgvScoreboard.Rows.Clear();
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

                    var reader = new SQLiteCommand(@"
                        SELECT Winner, Score1, Score2, Mode, Difficulty, PlayedAt
                        FROM   Scores
                        ORDER  BY Id DESC
                        LIMIT  100", conn).ExecuteReader();

                    int rank = 1;
                    while (reader.Read())
                    {
                        dgvScoreboard.Rows.Add(
                            rank++,
                            reader["Winner"].ToString(),
                            $"{reader["Score1"]} — {reader["Score2"]}",
                            reader["Mode"].ToString(),
                            reader["Difficulty"].ToString(),
                            reader["PlayedAt"].ToString()
                        );
                    }

                    // Thông báo nếu chưa có dữ liệu
                    if (dgvScoreboard.Rows.Count == 0)
                        dgvScoreboard.Rows.Add("—", "No games yet", "—", "—", "—", "—");
                }
            }
            catch (Exception ex)
            {
                dgvScoreboard.Rows.Add("!", "DB Error", ex.Message, "—", "—", "—");
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            foreach (Form f in Application.OpenForms)
            {
                if (f is frmMenu menu) { menu.Show(); this.Close(); return; }
            }
            new frmMenu().Show();
            this.Close();
        }
    }
}