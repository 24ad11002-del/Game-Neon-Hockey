using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class frmGame : Form
    {
        // ── Puck physics ──────────────────────────────────────────────────────────
        float puckSpeedX = 5f;
        float puckSpeedY = 5f;

        private float puckPosX;
        private float puckPosY;

        private const float PUCK_RADIUS   = 26f;   // half of original 52 px designer width
        private const float PUCK_DIAMETER = PUCK_RADIUS * 2f;

        // Restitution coefficients (applied to bounced velocity component after reflection)
        // CORNER = 0.80: both axes hit simultaneously — enough energy to escape, not too lively
        // WALL   = 0.30: single surface — lossy per spec; parallel component is unaffected
        private const float WALL_RESTITUTION   = 0.30f;
        private const float CORNER_RESTITUTION = 0.80f;

        // Radius of the rounded corner arcs (visual + physics must match).
        // Arc centers sit at (wall+CR, wall+CR) etc.; the puck center is clamped
        // to distance ≤ CR−PUCK_RADIUS from each arc center, which is geometrically
        // tangent to the flat-wall boundary so there is zero gap between surfaces.
        private const float CORNER_RADIUS = 60f;

        float puckMaxSpeed        = 18f;
        float puckSpeedMultiplier = 1.05f;

        // ── Puck trail ────────────────────────────────────────────────────────────
        private readonly Queue<PointF> _puckTrail = new Queue<PointF>();
        private const int TRAIL_LENGTH = 4;

        // ── AI ────────────────────────────────────────────────────────────────────
        private bool  isAIMode       = false;
        private float aiSpeed        = 6f;
        private float aiReactionZone = 200f;
        private float aiTargetX;
        private float aiTargetY;
        private float aiPosX;
        private float aiPosY;

        // ── Arena / players ───────────────────────────────────────────────────────
        int wall = 20;

        int scorePlayer1 = 0;
        int scorePlayer2 = 0;

        int player1X = 250;
        int player1Y = 500;
        int player2X = 250;
        int player2Y = 80;

        int playerSize = 56;   // 30 % smaller than original 80 — organic anti-pinning measure
        int goalWidth  = 160;
        int maxScore   = 4;   // first player to reach this score wins instantly

        // ── Win condition ─────────────────────────────────────────────────────────
        // Simple "first to 4" — no lead gap required.

        // ── Anti-pinning overload ─────────────────────────────────────────────────
        // Accumulates each tick the puck is simultaneously touching a wall AND a mallet.
        // When it hits PINNING_THRESHOLD, TriggerOverloadBurst fires: puck ejected,
        // offending mallet(s) pushed back, and mouse input is temporarily ignored.
        private float _pinningPressure    = 0f;
        private const float PINNING_THRESHOLD  = 2.0f;   // ticks * ACCUMULATE to reach burst
        private const float PINNING_ACCUMULATE = 0.12f;  // ~17 ticks ≈ 270 ms of pinning
        private const float PINNING_DECAY      = 0.30f;  // drops to 0 in ~7 ticks when free
        private const float BURST_SPEED        = 20f;    // px/tick ejection speed
        private const int   BURST_STUN_TICKS   = 12;     // ~190 ms input freeze

        // Per-player stun counters — while > 0, mouse / keyboard input is ignored
        private int _p1StunTimer = 0;
        private int _p2StunTimer = 0;

        // ── Screen shake ──────────────────────────────────────────────────────────
        private int    shakeTime = 0;
        private Random _shakeRng = new Random();

        // ── Goal animation ────────────────────────────────────────────────────────
        private bool  showGoalText  = false;
        private int   _goalScorer   = 0;
        private int   goalTextTimer = 0;
        private float goalScale     = 0.2f;
        private float goalAlpha     = 0f;
        private bool  goalAnimating = false;

        // ── Sounds ────────────────────────────────────────────────────────────────
        private System.Media.SoundPlayer wallSound;
        private System.Media.SoundPlayer goalSound;

        // ── Collision particle effects ────────────────────────────────────────────
        private readonly List<CollisionEffect> collisionEffects = new List<CollisionEffect>();

        private class CollisionEffect
        {
            public float X        { get; set; }
            public float Y        { get; set; }
            public float Size     { get; set; }
            public float Alpha    { get; set; }
            public int   Lifetime { get; set; }
            public Color Color    { get; set; }
            public float Velocity { get; set; }
        }

        // ── Game state ────────────────────────────────────────────────────────────
        private bool gameOver            = false;
        private int  winnerPlayer        = 0;
        private float winnerAlpha        = 0f;
        private int  winnerAnimationTimer = 0;
        private bool isPaused            = false;

        // ── GDI+ cached objects ───────────────────────────────────────────────────
        // Border glow — 20 pens per wall, widths 1–20, static color/alpha
        private Pen[] _pinkGlowPens;
        private Pen[] _magentaGlowPens;
        private Pen[] _blueGlowPens;
        private Pen[] _purpleGlowPens;
        private Pen   _whiteBorderPen;

        // Court markings (shared pen: White 80-alpha, width 4)
        private Pen _courtPen;

        // Score
        private Font         _scoreFont;
        private StringFormat _centerSF;
        private SolidBrush   _grayScoreBrush;

        // Players
        private Pen[]      _p1GlowPens;
        private Pen        _p1BorderPen;
        private SolidBrush _p1CenterBrush;
        private Pen[]      _p2GlowPens;
        private Pen        _p2BorderPen;
        private SolidBrush _p2CenterBrush;
        private SolidBrush _whiteFillBrush;

        // Puck body + glow
        private SolidBrush _puckFillBrush;
        private Pen        _puckBorderPen;
        private Pen[]      _puckGlowPens;   // indices 1–8

        // Trail — Gold with fixed alphas 10, 25, 50, 80
        private SolidBrush[] _trailBrushes;

        // Pause screen
        private SolidBrush _pauseOverlayBrush;
        private Font        _pauseTitleFont;
        private Font        _pauseHintFont;

        // Debug OSD
        private Font _debugFont;

        // Cached border paths — built once at load, reused every frame (no per-frame alloc)
        private GraphicsPath _leftBorderPath;
        private GraphicsPath _rightBorderPath;

        // ── Key state ─────────────────────────────────────────────────────────────
        bool w, a, s, d;
        bool up, down, left, right;

        private enum AIState { DEFEND, INTERCEPT, ATTACK, ESCAPE_CORNER }
        private AIState currentState = AIState.DEFEND;

        // ─────────────────────────────────────────────────────────────────────────
        public frmGame()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.KeyPreview     = true;
            typeof(Panel)
                .GetProperty("DoubleBuffered",
                    System.Reflection.BindingFlags.Instance |
                    System.Reflection.BindingFlags.NonPublic)
                ?.SetValue(pnlArena, true, null);
            ApplyDifficultySettings();
        }

        // ── GDI+ cache ───────────────────────────────────────────────────────────
        private void InitGdiCache()
        {
            _pinkGlowPens    = new Pen[21];
            _magentaGlowPens = new Pen[21];
            _blueGlowPens    = new Pen[21];
            _purpleGlowPens  = new Pen[21];
            for (int i = 1; i <= 20; i++)
            {
                _pinkGlowPens[i]    = new Pen(Color.FromArgb(20, Color.HotPink),      i);
                _magentaGlowPens[i] = new Pen(Color.FromArgb(20, Color.Magenta),      i);
                _blueGlowPens[i]    = new Pen(Color.FromArgb(20, Color.DodgerBlue),   i);
                _purpleGlowPens[i]  = new Pen(Color.FromArgb(20, Color.MediumPurple), i);
            }
            _whiteBorderPen = new Pen(Color.White, 4);
            _courtPen       = new Pen(Color.FromArgb(80, Color.White), 4);

            _scoreFont      = new Font("Arial", 28, FontStyle.Bold);
            _centerSF       = new StringFormat { Alignment = StringAlignment.Center };
            _grayScoreBrush = new SolidBrush(Color.Gray);
            _whiteFillBrush = new SolidBrush(Color.White);

            _p1GlowPens = new Pen[11];
            _p2GlowPens = new Pen[11];
            for (int i = 1; i <= 10; i++)
            {
                _p1GlowPens[i] = new Pen(Color.FromArgb(12, Color.Red),        i);
                _p2GlowPens[i] = new Pen(Color.FromArgb(12, Color.DodgerBlue), i);
            }
            _p1BorderPen   = new Pen(Color.Red,        3);
            _p2BorderPen   = new Pen(Color.DodgerBlue, 3);
            _p1CenterBrush = new SolidBrush(Color.Red);
            _p2CenterBrush = new SolidBrush(Color.DodgerBlue);

            _puckFillBrush = new SolidBrush(Color.Gold);
            _puckBorderPen = new Pen(Color.White, 2);
            _puckGlowPens  = new Pen[9];
            for (int i = 1; i <= 8; i++)
                _puckGlowPens[i] = new Pen(Color.FromArgb(15, Color.Gold), i * 2);

            int[] trailAlphas = { 10, 25, 50, 80 };
            _trailBrushes = new SolidBrush[trailAlphas.Length];
            for (int i = 0; i < trailAlphas.Length; i++)
                _trailBrushes[i] = new SolidBrush(Color.FromArgb(trailAlphas[i], Color.Gold));

            _pauseOverlayBrush = new SolidBrush(Color.FromArgb(180, 0, 0, 0));
            _pauseTitleFont    = new Font("Arial", 36, FontStyle.Bold);
            _pauseHintFont     = new Font("Arial", 14);
            _debugFont         = new Font("Consolas", 9f);

            // Border paths depend on panel size — panel is sized by the time Load fires
            _leftBorderPath  = BuildBorderPath(leftHalf: true);
            _rightBorderPath = BuildBorderPath(leftHalf: false);
        }

        private void DisposeGdiCache()
        {
            for (int i = 1; i <= 20; i++)
            {
                _pinkGlowPens?[i]?.Dispose();
                _magentaGlowPens?[i]?.Dispose();
                _blueGlowPens?[i]?.Dispose();
                _purpleGlowPens?[i]?.Dispose();
            }
            _whiteBorderPen?.Dispose();
            _courtPen?.Dispose();
            _scoreFont?.Dispose();
            _centerSF?.Dispose();
            _grayScoreBrush?.Dispose();
            _whiteFillBrush?.Dispose();
            for (int i = 1; i <= 10; i++) { _p1GlowPens?[i]?.Dispose(); _p2GlowPens?[i]?.Dispose(); }
            _p1BorderPen?.Dispose(); _p2BorderPen?.Dispose();
            _p1CenterBrush?.Dispose(); _p2CenterBrush?.Dispose();
            _puckFillBrush?.Dispose(); _puckBorderPen?.Dispose();
            for (int i = 1; i <= 8; i++) _puckGlowPens?[i]?.Dispose();
            if (_trailBrushes != null) foreach (var b in _trailBrushes) b?.Dispose();
            _pauseOverlayBrush?.Dispose();
            _pauseTitleFont?.Dispose();
            _pauseHintFont?.Dispose();
            _debugFont?.Dispose();
            _leftBorderPath?.Dispose();
            _rightBorderPath?.Dispose();
        }

        // ── Pause ─────────────────────────────────────────────────────────────────
        private void TogglePause()
        {
            isPaused = !isPaused;
            if (isPaused) { pnlArena.Invalidate(); timerGame.Stop(); }
            else          { timerGame.Start(); this.Focus(); }
        }

        // ── Goal / winner ─────────────────────────────────────────────────────────
        private void ShowGoal(int scorer)
        {
            goalAnimating = true;
            goalScale     = 0.2f;
            goalAlpha     = 1f;
            goalTextTimer = 60;
            showGoalText  = true;
            _goalScorer   = scorer;
            Task.Run(() => goalSound?.Play());
        }

        private void ShowWinner(int winner, int score1, int score2)
        {
            new frmWinner(winner, score1, score2).Show();
            this.Hide();
        }

        // ── Form load ─────────────────────────────────────────────────────────────
        private void frmGame_Load(object sender, EventArgs e)
        {
            InitGdiCache();
            this.FormClosed += (s, ev) => DisposeGdiCache();

            timerGame.Start();
            this.Focus();
            pnlArena.BackColor = Color.Black;

            aiTargetX = pnlArena.Width  / 2f - playerSize / 2f;
            aiTargetY = wall + 40f;
            aiPosX    = player2X;
            aiPosY    = player2Y;

            puckPosX = pnlArena.Width  / 2f;
            puckPosY = pnlArena.Height / 2f;

            pnlArena.MouseMove += pnlArena_MouseMove;

            try { wallSound = new System.Media.SoundPlayer("wall.wav"); goalSound = new System.Media.SoundPlayer("goal.wav"); }
            catch { }

            isAIMode     = (NeonHockey.GameSetting.Mode == "Single");
            showGoalText = false;
            goalTextTimer = 0;
            gameOver      = false;
        }

        // ── Draw helpers ──────────────────────────────────────────────────────────

        // Builds one continuous open GraphicsPath for either the left or right U-shaped
        // half of the rounded border.  Every line→arc join is geometrically exact so
        // GDI+ renders the entire path as one unbroken stroke — no seam, no overlap.
        //
        // Left U (clockwise):
        //   bottom-left goal edge → bottom-left wall seg → BL arc (90°→180°)
        //   → left wall → TL arc (180°→270°) → top-left wall seg → top-left goal edge
        //
        // Right U (clockwise):
        //   top-right goal edge → top-right wall seg → TR arc (270°→360°)
        //   → right wall → BR arc (0°→90°) → bottom-right wall seg → bottom-right goal edge
        //
        // Join verification (Left U example, all coordinates in panel pixels):
        //   AddLine end: (lx+cr, bot)  == AddArc BL start at 90°: (lx+cr, bot)       ✓
        //   AddArc BL end at 180°: (lx, bot-cr)  == AddLine start: (lx, bot-cr)       ✓
        //   AddLine end: (lx, ty+cr)  == AddArc TL start at 180°: (lx, ty+cr)         ✓
        //   AddArc TL end at 270°: (lx+cr, ty)  == AddLine start: (lx+cr, ty)         ✓
        private GraphicsPath BuildBorderPath(bool leftHalf)
        {
            int gap = 160;
            int cx  = pnlArena.Width  / 2;
            int w2  = pnlArena.Width;
            int h   = pnlArena.Height;
            int cr  = (int)CORNER_RADIUS;
            int lx  = wall,       rx  = w2 - wall;
            int ty  = wall,       bot = h  - wall;

            var path = new GraphicsPath();

            if (leftHalf)
            {
                var arcBL = new Rectangle(lx, bot - cr * 2, cr * 2, cr * 2);
                var arcTL = new Rectangle(lx, ty,            cr * 2, cr * 2);

                path.AddLine(cx - gap / 2, bot, lx + cr, bot);  // bottom-left seg (→ left)
                path.AddArc(arcBL, 90, 90);                       // BL corner: 90°→180°
                path.AddLine(lx, bot - cr, lx, ty + cr);         // left wall (↑)
                path.AddArc(arcTL, 180, 90);                      // TL corner: 180°→270°
                path.AddLine(lx + cr, ty, cx - gap / 2, ty);     // top-left seg (→ right)
            }
            else
            {
                var arcTR = new Rectangle(rx - cr * 2, ty,            cr * 2, cr * 2);
                var arcBR = new Rectangle(rx - cr * 2, bot - cr * 2,  cr * 2, cr * 2);

                path.AddLine(cx + gap / 2, ty, rx - cr, ty);     // top-right seg (→ right)
                path.AddArc(arcTR, 270, 90);                       // TR corner: 270°→360°
                path.AddLine(rx, ty + cr, rx, bot - cr);          // right wall (↓)
                path.AddArc(arcBR, 0, 90);                         // BR corner: 0°→90°
                path.AddLine(rx - cr, bot, cx + gap / 2, bot);    // bottom-right seg (→ left)
            }

            return path;
        }

        // Draws the neon border using two cached GraphicsPath objects.
        // Each path covers one half of the border (left U + right U) so the goal gaps
        // are preserved while every corner arc blends seamlessly into its adjacent walls.
        //
        // Draw-call count: 20 glow layers × 4 colour passes × 2 paths = 80 path draws,
        // plus 2 white-outline draws — 82 total vs 200+ in the previous per-segment approach.
        // The per-frame GDI allocation is zero: all pens and paths are cached.
        //
        // Colour scheme (all four cached wall colours preserved):
        //   Left  path: pink  + blue   → left wall is pink, top/bot-left segs blend pink-blue
        //   Right path: magenta + purple → right wall is magenta, top/bot-right segs blend magenta-purple
        //   Corner arcs inherit the blended tone of their two adjacent segments naturally.
        private void DrawGlowBorder(Graphics g)
        {
            for (int i = 20; i >= 1; i--)
            {
                g.DrawPath(_pinkGlowPens[i],    _leftBorderPath);
                g.DrawPath(_blueGlowPens[i],    _leftBorderPath);
                g.DrawPath(_magentaGlowPens[i], _rightBorderPath);
                g.DrawPath(_purpleGlowPens[i],  _rightBorderPath);
            }
            g.DrawPath(_whiteBorderPen, _leftBorderPath);
            g.DrawPath(_whiteBorderPen, _rightBorderPath);
        }

        private void DrawCenterLine(Graphics g)
            => g.DrawLine(_courtPen, 0, pnlArena.Height / 2, pnlArena.Width, pnlArena.Height / 2);

        private void DrawCenterCircle(Graphics g)
        {
            int size = 180;
            g.DrawEllipse(_courtPen,
                pnlArena.Width / 2 - size / 2, pnlArena.Height / 2 - size / 2, size, size);
        }

        private void DrawGoalArcs(Graphics g)
        {
            g.DrawArc(_courtPen, pnlArena.Width / 2 - 90, -90,                  180, 180,   0, 180);
            g.DrawArc(_courtPen, pnlArena.Width / 2 - 90, pnlArena.Height - 90, 180, 180, 180, 180);
        }

        private void DrawPlayer(Graphics g, int x, int y,
            Pen[] glowPens, Pen borderPen, SolidBrush centerBrush)
        {
            for (int i = 10; i >= 1; i--)
                g.DrawEllipse(glowPens[i], x, y, playerSize, playerSize);
            g.FillEllipse(_whiteFillBrush, x, y, playerSize, playerSize);
            g.DrawEllipse(borderPen, x, y, playerSize, playerSize);
            g.FillEllipse(centerBrush, x + playerSize / 2 - 12, y + playerSize / 2 - 12, 24, 24);
        }

        private void DrawPuck(Graphics g)
        {
            // Trail — oldest position = lowest alpha
            var trail = _puckTrail.ToArray();
            int offset = _trailBrushes.Length - trail.Length;
            for (int i = 0; i < trail.Length; i++)
            {
                int bi = offset + i;
                if (bi < 0) continue;
                float r = PUCK_RADIUS * 0.80f;
                g.FillEllipse(_trailBrushes[bi], trail[i].X - r, trail[i].Y - r, r * 2f, r * 2f);
            }

            // Glow halo
            for (int i = 8; i >= 1; i--)
            {
                float e = i * 1.5f;
                g.DrawEllipse(_puckGlowPens[i],
                    puckPosX - PUCK_RADIUS - e, puckPosY - PUCK_RADIUS - e,
                    PUCK_DIAMETER + e * 2f, PUCK_DIAMETER + e * 2f);
            }

            // Pinning-pressure warning ring — grows and brightens as pressure builds
            if (_pinningPressure > 0.2f)
            {
                float ratio = Math.Min(1f, _pinningPressure / PINNING_THRESHOLD);
                int   alpha = (int)(ratio * 210);
                float r     = PUCK_RADIUS + ratio * 22f;
                using (var pp = new Pen(Color.FromArgb(alpha, Color.OrangeRed), 2.5f))
                    g.DrawEllipse(pp, puckPosX - r, puckPosY - r, r * 2f, r * 2f);
            }

            // Solid body + white ring
            g.FillEllipse(_puckFillBrush,
                puckPosX - PUCK_RADIUS, puckPosY - PUCK_RADIUS, PUCK_DIAMETER, PUCK_DIAMETER);
            g.DrawEllipse(_puckBorderPen,
                puckPosX - PUCK_RADIUS, puckPosY - PUCK_RADIUS, PUCK_DIAMETER, PUCK_DIAMETER);
        }

        private void DrawCollisionEffects(Graphics g)
        {
            for (int i = collisionEffects.Count - 1; i >= 0; i--)
            {
                var fx = collisionEffects[i];
                using (Pen pen = new Pen(Color.FromArgb((int)(fx.Alpha * 255), fx.Color), 2))
                    g.DrawEllipse(pen,
                        fx.X - fx.Size / 2, fx.Y - fx.Size / 2, fx.Size, fx.Size);

                for (int j = 0; j < 8; j++)
                {
                    float angle = (float)(j * Math.PI * 2 / 8);
                    float px = fx.X + (float)Math.Cos(angle) * fx.Velocity;
                    float py = fx.Y + (float)Math.Sin(angle) * fx.Velocity;
                    using (SolidBrush br = new SolidBrush(Color.FromArgb((int)(fx.Alpha * 200), fx.Color)))
                        g.FillEllipse(br, px - 2, py - 2, 4, 4);
                }
            }
        }

        // ── Paint ─────────────────────────────────────────────────────────────────
        private void pnlArena_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.Black);

            // Screen shake — applied before any drawing so everything shakes together
            if (shakeTime > 0)
            {
                g.TranslateTransform(
                    _shakeRng.Next(-shakeTime, shakeTime + 1),
                    _shakeRng.Next(-shakeTime, shakeTime + 1));
                shakeTime--;
            }

            DrawGlowBorder(g);
            DrawCenterLine(g);
            DrawCenterCircle(g);

            g.DrawString(scorePlayer2.ToString(), _scoreFont, _grayScoreBrush,
                pnlArena.Width / 2, pnlArena.Height / 2 - 60, _centerSF);
            g.DrawString(scorePlayer1.ToString(), _scoreFont, _grayScoreBrush,
                pnlArena.Width / 2, pnlArena.Height / 2 + 20, _centerSF);

            DrawGoalArcs(g);
            DrawPuck(g);
            DrawPlayer(g, player1X, player1Y, _p1GlowPens, _p1BorderPen, _p1CenterBrush);
            DrawPlayer(g, player2X, player2Y, _p2GlowPens, _p2BorderPen, _p2CenterBrush);
            DrawCollisionEffects(g);

            // GOAL! banner (variable-alpha text — can't cache font size)
            if (showGoalText)
            {
                int   cx        = pnlArena.Width  / 2;
                int   cy        = pnlArena.Height / 2;
                Color goalColor = (_goalScorer == 1)
                    ? Color.FromArgb(255, 80, 80) : Color.FromArgb(0, 180, 255);

                int boxW = 420, boxH = 200;
                var boxRect = new Rectangle(cx - boxW / 2, cy - boxH / 2, boxW, boxH);

                using (var bg = new SolidBrush(Color.FromArgb((int)(goalAlpha * 160), 5, 0, 35)))
                    g.FillRectangle(bg, boxRect);
                for (int i = 12; i >= 1; i--)
                    using (var gp = new Pen(Color.FromArgb((int)(goalAlpha * (30 - i * 2)), goalColor), i))
                        g.DrawRectangle(gp, boxRect);
                using (var bp = new Pen(Color.FromArgb((int)(goalAlpha * 220), goalColor), 3))
                    g.DrawRectangle(bp, boxRect);

                float fontSize = Math.Max(4f, 62f * goalScale);
                using (var f = new Font("Impact", fontSize, FontStyle.Bold))
                {
                    SizeF sz = g.MeasureString("GOAL!", f);
                    for (int i = 8; i >= 1; i--)
                        using (var gb = new SolidBrush(Color.FromArgb((int)(goalAlpha * (50 - i * 5)), goalColor)))
                            g.DrawString("GOAL!", f, gb, cx - sz.Width / 2 + i, cy - sz.Height / 2 - 30 + i);
                    using (var mb = new SolidBrush(Color.FromArgb((int)(goalAlpha * 255), Color.Gold)))
                        g.DrawString("GOAL!", f, mb, cx - sz.Width / 2, cy - sz.Height / 2 - 30);
                }

                string sub = $"PLAYER {_goalScorer} SCORES!";
                using (var f2 = new Font("Segoe UI Black", 16f, FontStyle.Bold))
                {
                    SizeF sz2 = g.MeasureString(sub, f2);
                    using (var sb = new SolidBrush(Color.FromArgb((int)(goalAlpha * 220), goalColor)))
                        g.DrawString(sub, f2, sb, cx - sz2.Width / 2, cy + 35);
                }

                string scoreStr = $"{scorePlayer1}  —  {scorePlayer2}";
                using (var f3 = new Font("Segoe UI", 13f, FontStyle.Bold))
                {
                    SizeF sz3 = g.MeasureString(scoreStr, f3);
                    using (var sb3 = new SolidBrush(Color.FromArgb((int)(goalAlpha * 180), Color.White)))
                        g.DrawString(scoreStr, f3, sb3, cx - sz3.Width / 2, cy + 68);
                }
            }

            // Pause overlay
            if (isPaused)
            {
                g.FillRectangle(_pauseOverlayBrush, 0, 0, pnlArena.Width, pnlArena.Height);
                g.DrawString("PAUSED",         _pauseTitleFont, Brushes.White,     pnlArena.Width / 2, pnlArena.Height / 2 - 80, _centerSF);
                g.DrawString("ESC - Continue", _pauseHintFont,  Brushes.LightGray, pnlArena.Width / 2, pnlArena.Height / 2,      _centerSF);
                g.DrawString("M - Main Menu",  _pauseHintFont,  Brushes.LightGray, pnlArena.Width / 2, pnlArena.Height / 2 + 30, _centerSF);
                g.DrawString("X - Exit",       _pauseHintFont,  Brushes.LightGray, pnlArena.Width / 2, pnlArena.Height / 2 + 60, _centerSF);
            }

            // ── Debug OSD — always drawn last, shake-transform cancelled so text is stable
            g.ResetTransform();
            g.DrawString($"AI Stun:  {_p2StunTimer,3}", _debugFont, Brushes.LimeGreen, 5,  5);
            g.DrawString($"P1 Stun:  {_p1StunTimer,3}", _debugFont, Brushes.LimeGreen, 5, 18);
            g.DrawString($"Pressure: {_pinningPressure:F2}", _debugFont, Brushes.LimeGreen, 5, 31);
        }

        // ── Game loop ─────────────────────────────────────────────────────────────
        private void timerGame_Tick(object sender, EventArgs e)
        {
            if (isPaused)  return;
            if (gameOver)  { pnlArena.Invalidate(); return; }

            int playerSpeed = 12;
            int middleLine  = pnlArena.Height / 2;

            // Stun timers — tick down first so movement blocks below use the updated value
            if (_p1StunTimer > 0) _p1StunTimer--;
            if (_p2StunTimer > 0) _p2StunTimer--;

            // Player 1 — WASD (mouse stun handled in pnlArena_MouseMove)
            if (w) player1Y -= playerSpeed;
            if (s) player1Y += playerSpeed;
            if (a) player1X -= playerSpeed;
            if (d) player1X += playerSpeed;

            // Player 2 — arrows or AI
            if (isAIMode) UpdateAI();
            else if (_p2StunTimer <= 0)
            {
                if (up)    player2Y -= playerSpeed;
                if (down)  player2Y += playerSpeed;
                if (left)  player2X -= playerSpeed;
                if (right) player2X += playerSpeed;
            }

            // Clamp P1 to bottom half
            player1X = Math.Max(wall, Math.Min(pnlArena.Width  - wall - playerSize, player1X));
            player1Y = Math.Max(middleLine, Math.Min(pnlArena.Height - wall - playerSize, player1Y));

            // Clamp P2 to top half
            player2X = Math.Max(wall, Math.Min(pnlArena.Width - wall - playerSize, player2X));
            player2Y = Math.Max(wall, Math.Min(middleLine - playerSize,             player2Y));

            // ── Trail ─────────────────────────────────────────────────────────────
            _puckTrail.Enqueue(new PointF(puckPosX, puckPosY));
            if (_puckTrail.Count > TRAIL_LENGTH) _puckTrail.Dequeue();

            // ── Puck movement ──────────────────────────────────────────────────────
            puckPosX += puckSpeedX;
            puckPosY += puckSpeedY;

            float leftBound   = wall  + PUCK_RADIUS;
            float rightBound  = pnlArena.Width  - wall - PUCK_RADIUS;
            float topBound    = wall  + PUCK_RADIUS;
            float bottomBound = pnlArena.Height - wall - PUCK_RADIUS;

            float W  = pnlArena.Width;
            float H  = pnlArena.Height;
            float CR = CORNER_RADIUS;

            // ── Corner-zone detection ─────────────────────────────────────────────
            // A corner zone is the square region where both axes are within CR of
            // the inner wall corner.  Only one zone can be active at a time.
            bool inTL = puckPosX < wall + CR && puckPosY < wall + CR;
            bool inTR = puckPosX > W - wall - CR && puckPosY < wall + CR;
            bool inBL = puckPosX < wall + CR && puckPosY > H - wall - CR;
            bool inBR = puckPosX > W - wall - CR && puckPosY > H - wall - CR;
            bool inAnyCorner = inTL || inTR || inBL || inBR;

            bool hitWallX = false;
            bool hitWallY = false;

            if (inAnyCorner)
            {
                // ── Arc-circle collision ──────────────────────────────────────────
                // The physical surface is a quarter-circle of radius CR centred at
                // the arc centre.  The puck centre must stay ≤ (CR − PUCK_RADIUS)
                // from that centre — exactly tangent to the flat-wall bounds so
                // there is no gap between the straight and curved surfaces.
                //
                //   maxDist = CR − PUCK_RADIUS = 60 − 26 = 34 px
                //
                // Outward normal n = (puck − arcCentre) / dist points away from
                // the arena interior.  When vDotN > 0 the puck is moving outward
                // (into the curved wall).  Reflection formula:
                //
                //   v' = v − (1 + e) · (v · n) · n    where e = WALL_RESTITUTION
                //
                // The tangential component is unchanged; only the normal component
                // loses energy, identical to a flat-wall bounce.
                float arcCx = (inTL || inBL) ? wall + CR : W - wall - CR;
                float arcCy = (inTL || inTR) ? wall + CR : H - wall - CR;

                float adx  = puckPosX - arcCx;
                float ady  = puckPosY - arcCy;
                float dist = (float)Math.Sqrt(adx * adx + ady * ady);
                float maxDist = CR - PUCK_RADIUS;

                if (dist > maxDist && dist > 0.001f)
                {
                    float nx = adx / dist;
                    float ny = ady / dist;

                    // Push puck back inside the arc boundary
                    float pen = dist - maxDist;
                    puckPosX -= nx * pen;
                    puckPosY -= ny * pen;

                    float vDotN = puckSpeedX * nx + puckSpeedY * ny;
                    if (vDotN > 0f) // moving outward → apply reflection
                    {
                        puckSpeedX -= (1f + WALL_RESTITUTION) * vDotN * nx;
                        puckSpeedY -= (1f + WALL_RESTITUTION) * vDotN * ny;

                        wallSound?.Play();
                        StartShake(6);
                        Color arcColor = inTL ? Color.HotPink
                                       : inTR ? Color.Magenta
                                       : inBL ? Color.MediumPurple
                                       :        Color.DodgerBlue;
                        AddCollisionEffect(puckPosX, puckPosY, arcColor);
                        hitWallX = true;
                        hitWallY = true;
                    }
                }
            }
            else
            {
                // ── Flat wall checks (straight segments only) ─────────────────────

                // Left wall
                if (puckPosX < leftBound)
                {
                    puckPosX   = leftBound + (leftBound - puckPosX);
                    puckSpeedX = Math.Abs(puckSpeedX);
                    hitWallX   = true;
                    wallSound?.Play(); StartShake(6);
                    AddCollisionEffect(puckPosX, puckPosY, Color.HotPink);
                }

                // Right wall
                if (puckPosX > rightBound)
                {
                    puckPosX   = rightBound - (puckPosX - rightBound);
                    puckSpeedX = -Math.Abs(puckSpeedX);
                    hitWallX   = true;
                    wallSound?.Play(); StartShake(6);
                    AddCollisionEffect(puckPosX, puckPosY, Color.Magenta);
                }

                // Top wall / P1 scores (goal gap is at centre — never in a corner zone)
                if (puckPosY < topBound)
                {
                    int goalLeft  = pnlArena.Width / 2 - goalWidth / 2;
                    int goalRight = pnlArena.Width / 2 + goalWidth / 2;
                    bool inGoal   = (puckPosX - PUCK_RADIUS > goalLeft) && (puckPosX + PUCK_RADIUS < goalRight);
                    if (inGoal)
                    {
                        scorePlayer1++;
                        ShowGoal(1); StartShake(12);
                        AddCollisionEffect(puckPosX, puckPosY, Color.Gold);
                        if (CheckWinCondition(scorePlayer1, scorePlayer2)) { timerGame.Stop(); gameOver = true; ShowWinner(1, scorePlayer1, scorePlayer2); return; }
                        ResetPuck(); return;
                    }
                    else
                    {
                        puckPosY   = topBound + (topBound - puckPosY);
                        puckSpeedY = Math.Abs(puckSpeedY);
                        hitWallY   = true;
                        wallSound?.Play(); StartShake(6);
                        AddCollisionEffect(puckPosX, puckPosY, Color.DodgerBlue);
                    }
                }

                // Bottom wall / P2 scores
                if (puckPosY > bottomBound)
                {
                    int goalLeft  = pnlArena.Width / 2 - goalWidth / 2;
                    int goalRight = pnlArena.Width / 2 + goalWidth / 2;
                    bool inGoal   = (puckPosX - PUCK_RADIUS > goalLeft) && (puckPosX + PUCK_RADIUS < goalRight);
                    if (inGoal)
                    {
                        scorePlayer2++;
                        ShowGoal(2); StartShake(12); goalSound?.Play();
                        AddCollisionEffect(puckPosX, puckPosY, Color.Gold);
                        if (CheckWinCondition(scorePlayer2, scorePlayer1)) { timerGame.Stop(); gameOver = true; ShowWinner(2, scorePlayer1, scorePlayer2); return; }
                        ResetPuck(); return;
                    }
                    else
                    {
                        puckPosY   = bottomBound - (puckPosY - bottomBound);
                        puckSpeedY = -Math.Abs(puckSpeedY);
                        hitWallY   = true;
                        wallSound?.Play(); StartShake(6);
                        AddCollisionEffect(puckPosX, puckPosY, Color.MediumPurple);
                    }
                }

                // Dynamic restitution (flat walls only; arc reflection applies its own above)
                if      (hitWallX && hitWallY) { puckSpeedX *= CORNER_RESTITUTION; puckSpeedY *= CORNER_RESTITUTION; }
                else if (hitWallX)               puckSpeedX *= WALL_RESTITUTION;
                else if (hitWallY)               puckSpeedY *= WALL_RESTITUTION;
            }

            // Mallet collisions
            ResolveMalletCollision(player1X, player1Y, Color.Red);
            ResolveMalletCollision(player2X, player2Y, Color.DodgerBlue);

            // ── Anti-pinning pressure ─────────────────────────────────────────────
            // A puck near any wall while a mallet is also touching it accumulates pressure.
            // When pressure reaches threshold, TriggerOverloadBurst ejects the puck.
            {
                const float PROX = 5f;
                float lb = wall + PUCK_RADIUS,  rb = pnlArena.Width  - wall - PUCK_RADIUS;
                float tb = wall + PUCK_RADIUS,  bb = pnlArena.Height - wall - PUCK_RADIUS;

                bool puckAtWall = puckPosX <= lb + PROX || puckPosX >= rb - PROX
                               || puckPosY <= tb + PROX || puckPosY >= bb - PROX;
                bool p1Near = IsMalletPinningPuck(player1X, player1Y);
                bool p2Near = IsMalletPinningPuck(player2X, player2Y);

                if (puckAtWall && (p1Near || p2Near))
                {
                    _pinningPressure += PINNING_ACCUMULATE;
                    if (_pinningPressure >= PINNING_THRESHOLD)
                        TriggerOverloadBurst(p1Near, p2Near);
                }
                else
                {
                    _pinningPressure = Math.Max(0f, _pinningPressure - PINNING_DECAY);
                }
            }

            // Speed floor — prevent stall from sustained corner energy loss
            float spd = (float)Math.Sqrt(puckSpeedX * puckSpeedX + puckSpeedY * puckSpeedY);
            if (spd > 0.01f && spd < 3.5f)
            {
                puckSpeedX = puckSpeedX / spd * 3.5f;
                puckSpeedY = puckSpeedY / spd * 3.5f;
            }

            // Collision effects tick
            for (int i = collisionEffects.Count - 1; i >= 0; i--)
            {
                var fx = collisionEffects[i];
                fx.Lifetime--;
                fx.Size    += 2;
                fx.Alpha    = Math.Max(0, fx.Alpha - 0.05f);
                fx.Velocity *= 0.95f;
                if (fx.Lifetime <= 0) collisionEffects.RemoveAt(i);
            }

            // Goal animation tick
            if (goalAnimating)
            {
                goalScale += 0.08f;
                if (goalScale >= 1.5f) goalScale = 1.5f;
                goalTextTimer--;
                if (goalTextTimer <= 0)
                {
                    goalAlpha -= 0.05f;
                    if (goalAlpha <= 0) { goalAlpha = 0; goalAnimating = false; showGoalText = false; }
                }
            }

            pnlArena.Invalidate();
        }

        // ── Physics helpers ───────────────────────────────────────────────────────

        // Penetration-resolved mallet collision.
        // The vDotN >= 0 guard is the critical fix for corner-pinning freeze:
        // without it the mallet re-applies force every tick the puck is embedded,
        // locking it in place against the wall indefinitely.
        private void ResolveMalletCollision(int malletX, int malletY, Color effectColor)
        {
            float malletRadius = playerSize / 2f;
            float minDist      = PUCK_RADIUS + malletRadius;

            float mcx  = malletX + malletRadius;
            float mcy  = malletY + malletRadius;
            float dx   = puckPosX - mcx;
            float dy   = puckPosY - mcy;
            float dist = (float)Math.Sqrt(dx * dx + dy * dy);

            if (dist >= minDist || dist < 0.001f) return;

            float nx = dx / dist;
            float ny = dy / dist;

            // 1. Push puck out so circles are exactly touching
            float penetration = minDist - dist;
            puckPosX += nx * penetration;
            puckPosY += ny * penetration;

            // 2. Already separating — don't apply impulse (prevents oscillation lock)
            float vDotN = puckSpeedX * nx + puckSpeedY * ny;
            if (vDotN >= 0f) return;

            // 3. Launch along normal with multiplier, minimum 8 px/tick
            float incomingSpeed = (float)Math.Sqrt(puckSpeedX * puckSpeedX + puckSpeedY * puckSpeedY);
            float launchSpeed   = Math.Max(incomingSpeed * puckSpeedMultiplier, 8f);
            puckSpeedX = nx * launchSpeed;
            puckSpeedY = ny * launchSpeed;

            // 4. Clamp to max speed
            float finalSpeed = (float)Math.Sqrt(puckSpeedX * puckSpeedX + puckSpeedY * puckSpeedY);
            if (finalSpeed > puckMaxSpeed)
            {
                puckSpeedX = puckSpeedX / finalSpeed * puckMaxSpeed;
                puckSpeedY = puckSpeedY / finalSpeed * puckMaxSpeed;
            }

            // 5. Secondary wall resolve — mallet may have pushed puck into a boundary
            ResolveWallsAfterMallet();

            AddCollisionEffect(puckPosX, puckPosY, effectColor);
            StartShake(4);
            wallSound?.Play();
        }

        // Clamp after mallet push — corner zones use arc resolution; straight walls use flat snap.
        private void ResolveWallsAfterMallet()
        {
            float leftBound   = wall  + PUCK_RADIUS;
            float rightBound  = pnlArena.Width  - wall - PUCK_RADIUS;
            float topBound    = wall  + PUCK_RADIUS;
            float bottomBound = pnlArena.Height - wall - PUCK_RADIUS;
            float W = pnlArena.Width, H = pnlArena.Height, CR = CORNER_RADIUS;

            bool inTL = puckPosX < wall + CR && puckPosY < wall + CR;
            bool inTR = puckPosX > W - wall - CR && puckPosY < wall + CR;
            bool inBL = puckPosX < wall + CR && puckPosY > H - wall - CR;
            bool inBR = puckPosX > W - wall - CR && puckPosY > H - wall - CR;

            if (inTL || inTR || inBL || inBR)
            {
                float arcCx = (inTL || inBL) ? wall + CR : W - wall - CR;
                float arcCy = (inTL || inTR) ? wall + CR : H - wall - CR;
                float dx   = puckPosX - arcCx;
                float dy   = puckPosY - arcCy;
                float dist = (float)Math.Sqrt(dx * dx + dy * dy);
                float maxDist = CR - PUCK_RADIUS;
                if (dist > maxDist && dist > 0.001f)
                {
                    float nx = dx / dist, ny = dy / dist;
                    float pen = dist - maxDist;
                    puckPosX -= nx * pen;
                    puckPosY -= ny * pen;
                    float vDotN = puckSpeedX * nx + puckSpeedY * ny;
                    if (vDotN > 0f)
                    {
                        puckSpeedX -= (1f + CORNER_RESTITUTION) * vDotN * nx;
                        puckSpeedY -= (1f + CORNER_RESTITUTION) * vDotN * ny;
                    }
                }
            }
            else
            {
                if (puckPosX < leftBound)   { puckPosX = leftBound;   puckSpeedX =  Math.Abs(puckSpeedX) * CORNER_RESTITUTION; }
                if (puckPosX > rightBound)  { puckPosX = rightBound;  puckSpeedX = -Math.Abs(puckSpeedX) * CORNER_RESTITUTION; }
                if (puckPosY < topBound)    { puckPosY = topBound;    puckSpeedY =  Math.Abs(puckSpeedY) * CORNER_RESTITUTION; }
                if (puckPosY > bottomBound) { puckPosY = bottomBound; puckSpeedY = -Math.Abs(puckSpeedY) * CORNER_RESTITUTION; }
            }
        }

        private void AddCollisionEffect(float x, float y, Color color)
        {
            collisionEffects.Add(new CollisionEffect
                { X = x, Y = y, Size = 20, Alpha = 1f, Lifetime = 20, Color = color, Velocity = 15 });
        }

        private void StartShake(int duration = 10)
        {
            if (duration > shakeTime) shakeTime = duration; // don't downgrade an active stronger shake
        }

        // ── Reset / Restart ───────────────────────────────────────────────────────
        private void ResetPuck()
        {
            puckPosX   = pnlArena.Width  / 2f;
            puckPosY   = pnlArena.Height / 2f;
            puckSpeedX = 5f;
            puckSpeedY = 5f;
            _puckTrail.Clear();
            _pinningPressure = 0f;
        }

        private void RestartGame()
        {
            aiTargetX = pnlArena.Width / 2f - playerSize / 2f;
            aiTargetY = wall + 40f;
            aiPosX = player2X;
            aiPosY = player2Y;
            scorePlayer1 = 0;
            scorePlayer2 = 0;
            gameOver     = false;
            winnerPlayer = 0;
            winnerAlpha  = 0f;
            winnerAnimationTimer = 0;
            ApplyDifficultySettings();
            ResetPuck();
            collisionEffects.Clear();
            showGoalText     = false;
            goalAnimating    = false;
            _pinningPressure = 0f;
            _p1StunTimer     = 0;
            _p2StunTimer     = 0;
            timerGame.Start();
        }

        // ── AI with lookahead ─────────────────────────────────────────────────────
        private void UpdateAI()
        {
            // ── Stun guard (CRITICAL) ─────────────────────────────────────────────
            // When TriggerOverloadBurst fires, it physically pushes player2X/Y.
            // Without this return, UpdateAI() would overwrite that position in the
            // same tick, making the burst invisible to the AI entirely.
            // The sync keeps aiPosX/Y current so the AI doesn't snap back on wake-up.
            if (_p2StunTimer > 0)
            {
                aiPosX = player2X;
                aiPosY = player2Y;
                return;
            }

            int    middleLine = pnlArena.Height / 2;
            string diff       = NeonHockey.GameSetting.Difficulty ?? "Medium";

            // Predict puck position N ticks ahead; Easy reads slower, Hard anticipates more
            float lookAhead = (diff == "Hard") ? 10f : (diff == "Easy") ? 5f : 8f;
            float puckX = puckPosX + puckSpeedX * lookAhead;
            float puckY = puckPosY + puckSpeedY * lookAhead;

            // Clamp prediction so AI doesn't chase a phantom outside the arena
            puckX = Math.Max(wall + PUCK_RADIUS, Math.Min(pnlArena.Width  - wall - PUCK_RADIUS, puckX));
            puckY = Math.Max(wall + PUCK_RADIUS, Math.Min(middleLine,                            puckY));

            // ── Corner trap detection (real puck position, not predicted) ─────────
            // "Deep corner" = puck is simultaneously near the top wall AND a side wall.
            // In this configuration, pressing forward triggers the burst; retreat instead.
            float cornerDepth   = PUCK_RADIUS + 35f;  // 61 px from arena edge = 15 px past boundary
            bool  nearTopWall   = puckPosY < wall + cornerDepth;
            bool  nearLeftWall  = puckPosX < wall + cornerDepth;
            bool  nearRightWall = puckPosX > pnlArena.Width - wall - cornerDepth;
            bool  puckDeepInCorner = nearTopWall && (nearLeftWall || nearRightWall);

            // Pinning pressure retreat: back off early when sustained contact is detected,
            // before the burst fires. Uses 40 % of threshold as the early-warning trigger.
            bool pressureBuilding = _pinningPressure > PINNING_THRESHOLD * 0.40f;

            bool puckInCorner = puckX < wall + 80 || puckX > pnlArena.Width - wall - 80 || puckY < wall + 80;
            bool puckInAISide = puckPosY < middleLine;

            if (puckDeepInCorner)
            {
                // Smart retreat: hover diagonally outside the corner so the burst doesn't
                // trigger and the AI can react once the puck bounces clear.
                // Safe distance is 55 px from the wall + mallet radius so the mallet center
                // stays outside the puck+mallet collision range (PUCK_RADIUS + malletRadius).
                float safeX = nearLeftWall
                    ? wall + playerSize + 55f                            // left corner → back right
                    : pnlArena.Width - wall - playerSize - 55f;          // right corner → back left
                float safeY = wall + playerSize + 55f;

                aiTargetX = safeX - playerSize / 2f;
                aiTargetY = safeY - playerSize / 2f;
            }
            else if (pressureBuilding)
            {
                // Pressure building without full corner detection — lerp back toward
                // center faster than the normal home lerp to break contact.
                float homeX = pnlArena.Width / 2f - playerSize / 2f;
                float homeY = wall + 40f;
                aiTargetX += (homeX - aiTargetX) * 0.10f;
                aiTargetY += (homeY - aiTargetY) * 0.10f;
            }
            else if (puckInCorner)
            {
                aiTargetX = pnlArena.Width / 2f - playerSize / 2f;
                aiTargetY = puckY + 60;
            }
            else if (puckInAISide)
            {
                float goalX = pnlArena.Width / 2f;
                float dirX  = goalX - puckX;
                float dirY  = middleLine - puckY;
                float len   = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
                if (len > 0) { dirX /= len; dirY /= len; }
                aiTargetX = puckX - dirX * 70 - playerSize / 2f;
                aiTargetY = puckY - dirY * 70 - playerSize / 2f;
            }
            else
            {
                float homeX = pnlArena.Width / 2f - playerSize / 2f;
                float homeY = wall + 40f;
                aiTargetX += (homeX - aiTargetX) * 0.03f;
                aiTargetY += (homeY - aiTargetY) * 0.03f;
            }

            float dx  = aiTargetX - aiPosX;
            float dy  = aiTargetY - aiPosY;
            float dist = (float)Math.Sqrt(dx * dx + dy * dy);
            if (dist > 1)
            {
                float speed = Math.Min(aiSpeed, dist);
                aiPosX += dx / dist * speed;
                aiPosY += dy / dist * speed;
            }

            aiPosX = Math.Max(wall, Math.Min(pnlArena.Width  - wall - playerSize, aiPosX));
            aiPosY = Math.Max(wall, Math.Min(middleLine - playerSize,              aiPosY));
            player2X = (int)aiPosX;
            player2Y = (int)aiPosY;
        }

        // ── Difficulty ────────────────────────────────────────────────────────────
        private void ApplyDifficultySettings()
        {
            string difficulty = NeonHockey.GameSetting.Difficulty ?? "Medium";
            switch (difficulty)
            {
                case "Easy":   aiSpeed = 3.5f; puckMaxSpeed = 15f; puckSpeedMultiplier = 1.02f; break;
                case "Medium": aiSpeed = 6f;   puckMaxSpeed = 18f; puckSpeedMultiplier = 1.05f; break;
                case "Hard":   aiSpeed = 9f;   puckMaxSpeed = 22f; puckSpeedMultiplier = 1.08f; break;
            }
        }

        // ── Input ─────────────────────────────────────────────────────────────────
        private void pnlArena_MouseMove(object sender, MouseEventArgs e)
        {
            if (_p1StunTimer > 0) return; // burst pushback — ignore mouse until stun expires
            int mid = pnlArena.Height / 2;
            player1X = Math.Max(wall, Math.Min(pnlArena.Width  - wall - playerSize, e.X - playerSize / 2));
            player1Y = Math.Max(mid,  Math.Min(pnlArena.Height - wall - playerSize, e.Y - playerSize / 2));
        }

        private void frmGame_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) TogglePause();
            if (isPaused && e.KeyCode == Keys.M) { new frmMenu().Show(); this.Close(); }
            if (isPaused && e.KeyCode == Keys.X) Application.Exit();

            if (e.KeyCode == Keys.W) w = true;
            if (e.KeyCode == Keys.S) s = true;
            if (e.KeyCode == Keys.A) a = true;
            if (e.KeyCode == Keys.D) d = true;
            if (e.KeyCode == Keys.Up)    up    = true;
            if (e.KeyCode == Keys.Down)  down  = true;
            if (e.KeyCode == Keys.Left)  left  = true;
            if (e.KeyCode == Keys.Right) right = true;

            // ── Debug hotkeys (active only during live play) ──────────────────────
            if (!gameOver && !isPaused)
            {
                if (e.KeyCode == Keys.D1) Debug_AICornerPin();
                if (e.KeyCode == Keys.D2) Debug_PlayerCornerPin();
                if (e.KeyCode == Keys.D3) Debug_WinSetup();
            }

            if (gameOver)
            {
                if (e.KeyCode == Keys.Space)  RestartGame();
                if (e.KeyCode == Keys.Escape) this.Close();
            }
        }

        private void frmGame_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) w = false;
            if (e.KeyCode == Keys.S) s = false;
            if (e.KeyCode == Keys.A) a = false;
            if (e.KeyCode == Keys.D) d = false;
            if (e.KeyCode == Keys.Up)    up    = false;
            if (e.KeyCode == Keys.Down)  down  = false;
            if (e.KeyCode == Keys.Left)  left  = false;
            if (e.KeyCode == Keys.Right) right = false;
        }

        // ── Debug scenario helpers ────────────────────────────────────────────────

        // [1] Teleports puck and AI mallet into the top-left corner to immediately
        // trigger multi-body compression and test the Overload Burst + AI Stun.
        private void Debug_AICornerPin()
        {
            float mr = playerSize / 2f;
            int   mid = pnlArena.Height / 2;

            // Puck deep in the top-left corner (5 px inside boundary)
            puckPosX  = wall + PUCK_RADIUS + 5f;
            puckPosY  = wall + PUCK_RADIUS + 5f;
            puckSpeedX = 0f;
            puckSpeedY = 0f;

            // AI mallet center 20 px from puck center, pressing in from inside arena
            float p2cx = puckPosX + 20f;
            float p2cy = puckPosY + 20f;
            player2X  = (int)Math.Max(wall, Math.Min(pnlArena.Width - wall - playerSize, p2cx - mr));
            player2Y  = (int)Math.Max(wall, Math.Min(mid - playerSize,                   p2cy - mr));
            aiPosX    = player2X;
            aiPosY    = player2Y;

            _pinningPressure = 0f;
            _p1StunTimer     = 0;
            _p2StunTimer     = 0;
            _puckTrail.Clear();
        }

        // [2] Same as above but teleports Player 1's mallet into the bottom-left corner.
        private void Debug_PlayerCornerPin()
        {
            float mr  = playerSize / 2f;
            int   mid = pnlArena.Height / 2;

            // Puck deep in the bottom-left corner
            puckPosX  = wall + PUCK_RADIUS + 5f;
            puckPosY  = pnlArena.Height - wall - PUCK_RADIUS - 5f;
            puckSpeedX = 0f;
            puckSpeedY = 0f;

            // P1 mallet center 20 px from puck center, pressing in from inside arena
            float p1cx = puckPosX + 20f;
            float p1cy = puckPosY - 20f;  // offset upward since puck is at bottom
            player1X  = (int)Math.Max(wall, Math.Min(pnlArena.Width - wall - playerSize, p1cx - mr));
            player1Y  = (int)Math.Max(mid,  Math.Min(pnlArena.Height - wall - playerSize, p1cy - mr));

            _pinningPressure = 0f;
            _p1StunTimer     = 0;
            _p2StunTimer     = 0;
            _puckTrail.Clear();
        }

        // [3] Sets score to 3–0 and places the puck near the AI goal heading upward.
        // One successful shot verifies the "First to 4" win screen triggers.
        private void Debug_WinSetup()
        {
            scorePlayer1 = 3;
            scorePlayer2 = 0;
            puckPosX   = pnlArena.Width  / 2f;
            puckPosY   = wall + PUCK_RADIUS + 100f;  // 100 px below top wall
            puckSpeedX = 0f;
            puckSpeedY = -6f;                         // heading toward AI's goal
            _puckTrail.Clear();
        }

        // ── Win condition ─────────────────────────────────────────────────────────
        // First player to reach maxScore wins instantly — no gap rule.
        private bool CheckWinCondition(int score, int opponentScore)
            => score >= maxScore;

        // ── Anti-pinning helpers ──────────────────────────────────────────────────
        // Returns true if this mallet is within collision range + a small tolerance,
        // meaning it could be actively pressing the puck against a boundary.
        private bool IsMalletPinningPuck(int malletX, int malletY)
        {
            float malletRadius = playerSize / 2f;
            float threshold    = PUCK_RADIUS + malletRadius + 8f; // 8 px tolerance
            float mcx = malletX + malletRadius;
            float mcy = malletY + malletRadius;
            float dx  = puckPosX - mcx;
            float dy  = puckPosY - mcy;
            return (dx * dx + dy * dy) < (threshold * threshold);
        }

        // Called when _pinningPressure >= PINNING_THRESHOLD.
        // Two-step physics response — puck escape and mallet pushback use separate
        // vectors so each is geometrically correct for its purpose.
        private void TriggerOverloadBurst(bool p1Pinning, bool p2Pinning)
        {
            _pinningPressure = 0f;

            float mr  = playerSize / 2f;
            int   mid = pnlArena.Height / 2;

            // ── Step 1: Puck escape — outward wall-normal direction ────────────────
            // The puck must escape AWAY from whichever boundary it is pressed against.
            // The outward normal of each touched wall gives the correct exit direction.
            // (Using the inverse mallet→puck vector would push the puck deeper into the
            // wall, so wall normals are used here instead of the mallet vector.)
            float lb = wall + PUCK_RADIUS,  rb = pnlArena.Width  - wall - PUCK_RADIUS;
            float tb = wall + PUCK_RADIUS,  bb = pnlArena.Height - wall - PUCK_RADIUS;
            const float PROX = 8f;

            float ejX = 0f, ejY = 0f;
            if (puckPosX <= lb + PROX) ejX += 1f;   // near left wall  → push right
            if (puckPosX >= rb - PROX) ejX -= 1f;   // near right wall → push left
            if (puckPosY <= tb + PROX) ejY += 1f;   // near top wall   → push down
            if (puckPosY >= bb - PROX) ejY -= 1f;   // near bottom wall→ push up

            float ejLen = (float)Math.Sqrt(ejX * ejX + ejY * ejY);
            if (ejLen < 0.001f) { ejX = 0f; ejY = 1f; }   // no wall detected — fall back
            else                { ejX /= ejLen; ejY /= ejLen; }

            puckSpeedX = ejX * BURST_SPEED;   // above puckMaxSpeed intentionally
            puckSpeedY = ejY * BURST_SPEED;

            // ── Step 2: Mallet pushback — puck→mallet vector ─────────────────────
            // Vector from puck center to mallet center gives the exact direction to
            // translate the mallet so it moves directly away from the puck:
            //
            //   Dx  = Mallet.CenterX − Puck.CenterX
            //   Dy  = Mallet.CenterY − Puck.CenterY
            //   Len = sqrt(Dx² + Dy²)
            //   NormX = Dx / Len,  NormY = Dy / Len
            //
            // Translating (NormX × PUSH_DIST, NormY × PUSH_DIST) moves the mallet
            // directly away from the puck center, regardless of wall geometry.
            const float PUSH_DIST = 60f;

            if (p1Pinning)
            {
                float mcx = player1X + mr;
                float mcy = player1Y + mr;
                float dx  = mcx - puckPosX;   // Dx = Mallet.CenterX − Puck.CenterX
                float dy  = mcy - puckPosY;   // Dy = Mallet.CenterY − Puck.CenterY
                float len = (float)Math.Sqrt(dx * dx + dy * dy);
                if (len > 0.001f) { dx /= len; dy /= len; }   // normalize to unit vector

                // Translate mallet along (NormX, NormY) — away from puck
                player1X = (int)Math.Max(wall, Math.Min(pnlArena.Width - wall - playerSize,
                    player1X + dx * PUSH_DIST));
                player1Y = (int)Math.Max(mid,  Math.Min(pnlArena.Height - wall - playerSize,
                    player1Y + dy * PUSH_DIST));
                _p1StunTimer = BURST_STUN_TICKS;   // freeze mouse input during recovery
            }

            if (p2Pinning)
            {
                float mcx = player2X + mr;
                float mcy = player2Y + mr;
                float dx  = mcx - puckPosX;
                float dy  = mcy - puckPosY;
                float len = (float)Math.Sqrt(dx * dx + dy * dy);
                if (len > 0.001f) { dx /= len; dy /= len; }

                player2X = (int)Math.Max(wall, Math.Min(pnlArena.Width - wall - playerSize,
                    player2X + dx * PUSH_DIST));
                player2Y = (int)Math.Max(wall, Math.Min(mid - playerSize,
                    player2Y + dy * PUSH_DIST));
                // Sync AI float coords to the pushed position — without this, UpdateAI()
                // would snap back to the pre-burst location the moment the stun expires.
                aiPosX = player2X;
                aiPosY = player2Y;
                _p2StunTimer = BURST_STUN_TICKS;
            }

            StartShake(12);
            AddCollisionEffect(puckPosX, puckPosY, Color.OrangeRed);
            AddCollisionEffect(puckPosX, puckPosY, Color.White);
            Task.Run(() => wallSound?.Play());
        }
    }
}
