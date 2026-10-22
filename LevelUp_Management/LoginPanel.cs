using System;
using System.Drawing;
using System.Windows.Forms;

namespace LevelUp_Management
{
    // =========================================================================
    // LoginPanel — the first screen shown in panelMain. Checks the entered
    // username/password against Program.StudioManagers (already loaded by
    // initLists()) and, on success, hands over to mainForm.onLoginSuccess().
    //
    // Like HomePanel, it is built in code (no Designer.cs): few controls, no
    // layout worth a designer file. Login is an NFR precondition, not a UC.
    // =========================================================================
    public class LoginPanel : UserControl
    {
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Label lblError;
        private Button btnLogin;
        private Panel formHost;

        // Shake-on-failure animation. formHost is docked, so its own Location
        // can't move; its children are shaken instead and restored afterwards.
        private const int ShakeMs = 400;
        private const int ShakeSwings = 6;
        private Timer shakeTimer;
        private System.Diagnostics.Stopwatch shakeClock;
        private Point[] shakeOrigins;

        public LoginPanel()
        {
            build();
        }

        private void startShake()
        {
            if (shakeTimer != null) return;   // already shaking - let it finish

            shakeOrigins = new Point[formHost.Controls.Count];
            for (int i = 0; i < shakeOrigins.Length; i++)
                shakeOrigins[i] = formHost.Controls[i].Location;

            shakeClock = System.Diagnostics.Stopwatch.StartNew();
            shakeTimer = new Timer();
            shakeTimer.Interval = 20;
            shakeTimer.Tick += shakeTimer_Tick;
            shakeTimer.Start();
        }

        private void shakeTimer_Tick(object sender, EventArgs e)
        {
            double t = Math.Min(1.0, shakeClock.ElapsedMilliseconds / (double)ShakeMs);
            if (t >= 1.0)
            {
                stopShake();
                return;
            }

            // ShakeSwings half-waves of a sine: left, right, left, ... at ~10px.
            int offset = (int)Math.Round(UITheme.dp(this, 10) * Math.Sin(Math.PI * ShakeSwings * t));
            for (int i = 0; i < shakeOrigins.Length; i++)
                formHost.Controls[i].Location = new Point(shakeOrigins[i].X + offset, shakeOrigins[i].Y);
        }

        private void stopShake()
        {
            if (shakeTimer == null) return;
            shakeTimer.Stop();
            shakeTimer.Tick -= shakeTimer_Tick;
            shakeTimer.Dispose();
            shakeTimer = null;

            // Restore every control exactly to where it was.
            for (int i = 0; i < shakeOrigins.Length; i++)
                formHost.Controls[i].Location = shakeOrigins[i];
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                stopShake();
            base.Dispose(disposing);
        }

        private void build()
        {
            SuspendLayout();

            RightToLeft = RightToLeft.Yes;
            BackColor = UITheme.Canvas;
            Padding = new Padding(UITheme.dp(this, 28));

            // Added first = innermost: the form card sits under the hero banner.
            Controls.Add(buildForm());
            Controls.Add(buildHero());

            ResumeLayout(true);
        }

        // Teal banner — same look as HomePanel.buildHero()
        private Panel buildHero()
        {
            Panel hero = new Panel();
            hero.Name = "hero";
            hero.Dock = DockStyle.Top;
            hero.BackColor = UITheme.Card;

            int pad = UITheme.dp(this, 32);
            int y = pad;

            Label eyebrow = new Label();
            eyebrow.AutoSize = true;
            eyebrow.Text = "LEVEL UP";
            eyebrow.Font = new Font("Trebuchet MS", 9.5F, FontStyle.Bold);
            eyebrow.ForeColor = UITheme.Accent;
            eyebrow.BackColor = Color.Transparent;
            eyebrow.RightToLeft = RightToLeft.No;   // Latin wordmark: do not mirror
            eyebrow.Location = new Point(pad, y);
            hero.Controls.Add(eyebrow);
            y += eyebrow.PreferredSize.Height + UITheme.dp(this, 8);

            Label title = new Label();
            title.AutoSize = true;
            title.Text = "Studio Login";
            title.Font = new Font("Trebuchet MS", 22F, FontStyle.Bold);
            title.ForeColor = UITheme.TextPrimary;
            title.BackColor = Color.Transparent;
            title.RightToLeft = RightToLeft.No;
            title.Location = new Point(pad, y);
            hero.Controls.Add(title);
            y += title.PreferredSize.Height;

            hero.Height = y + pad;

            // Card vs Canvas is too close to read as a band on its own, so the
            // hairlines define its edges (same as HomePanel's hero).
            Panel topRule = new Panel();
            topRule.Dock = DockStyle.Top;
            topRule.Height = Math.Max(1, UITheme.dp(this, 1));
            topRule.BackColor = UITheme.Line;
            hero.Controls.Add(topRule);

            Panel bottomRule = new Panel();
            bottomRule.Dock = DockStyle.Bottom;
            bottomRule.Height = Math.Max(1, UITheme.dp(this, 1));
            bottomRule.BackColor = UITheme.Line;
            hero.Controls.Add(bottomRule);

            return hero;
        }

        private void styleInput(TextBox t)
        {
            t.BackColor = UITheme.Card;
            t.ForeColor = UITheme.TextPrimary;
            t.BorderStyle = BorderStyle.FixedSingle;
        }

        // Canvas-coloured host for the two fields, the error line and the button
        private Control buildForm()
        {
            Panel host = new Panel();
            host.Name = "formHost";
            host.Dock = DockStyle.Top;
            host.BackColor = UITheme.Canvas;

            int fieldW = UITheme.dp(this, 360);
            int gap = UITheme.dp(this, 6);
            int y = UITheme.dp(this, 32);

            Label lblUser = makeCaption("שם משתמש");
            lblUser.Location = new Point(0, y);
            y += lblUser.PreferredSize.Height + gap;

            txtUsername = new TextBox();
            txtUsername.Name = "txtUsername";
            txtUsername.Width = fieldW;
            txtUsername.Font = UITheme.Input;
            styleInput(txtUsername);
            txtUsername.Location = new Point(0, y);
            y += txtUsername.PreferredSize.Height + UITheme.dp(this, 16);

            Label lblPass = makeCaption("סיסמה");
            lblPass.Location = new Point(0, y);
            y += lblPass.PreferredSize.Height + gap;

            txtPassword = new TextBox();
            txtPassword.Name = "txtPassword";
            txtPassword.Width = fieldW;
            txtPassword.Font = UITheme.Input;
            styleInput(txtPassword);
            txtPassword.UseSystemPasswordChar = true;
            txtPassword.Location = new Point(0, y);
            y += txtPassword.PreferredSize.Height + UITheme.dp(this, 12);

            lblError = new Label();
            lblError.Name = "lblError";
            lblError.AutoSize = false;
            lblError.Size = new Size(fieldW, UITheme.dp(this, 24));
            lblError.Font = new Font("Trebuchet MS", 9.75F, FontStyle.Regular);
            lblError.ForeColor = UITheme.AccentOrange;   // the theme's danger colour
            lblError.BackColor = Color.Transparent;
            lblError.Visible = false;
            lblError.Location = new Point(0, y);
            y += lblError.Height + UITheme.dp(this, 8);

            btnLogin = new Button();
            btnLogin.Name = "btnLogin";
            btnLogin.Text = "התחברות";
            btnLogin.Size = new Size(fieldW, UITheme.dp(this, 44));
            btnLogin.Location = new Point(0, y);
            UITheme.stylePrimaryButton(btnLogin, Math.Max(1f, DeviceDpi / 96f));
            btnLogin.Click += btnLogin_Click;
            y += btnLogin.Height;

            host.Controls.Add(lblUser);
            host.Controls.Add(txtUsername);
            host.Controls.Add(lblPass);
            host.Controls.Add(txtPassword);
            host.Controls.Add(lblError);
            host.Controls.Add(btnLogin);
            host.Height = y + UITheme.dp(this, 8);
            formHost = host;

            // Enter submits from either field.
            txtUsername.KeyDown += enterSubmits;
            txtPassword.KeyDown += enterSubmits;

            return host;
        }

        private Label makeCaption(string text)
        {
            Label l = new Label();
            l.AutoSize = true;
            l.Text = text;
            l.Font = UITheme.FieldLabel;
            l.ForeColor = UITheme.TextPrimary;
            l.BackColor = Color.Transparent;
            return l;
        }

        private void enterSubmits(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnLogin_Click(sender, EventArgs.Empty);
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (username == "")
            {
                MessageBox.Show("יש להזין שם משתמש", "שגיאה", MessageBoxButtons.OK);
                return;
            }
            if (password == "")
            {
                MessageBox.Show("יש להזין סיסמה", "שגיאה", MessageBoxButtons.OK);
                return;
            }

            // Username is case-insensitive, password exact (plain equality — no hashing in the diagram).
            StudioManager match = null;
            foreach (StudioManager sm in Program.StudioManagers)
            {
                if (string.Equals(sm.getUsername(), username, StringComparison.OrdinalIgnoreCase)
                    && sm.getPassword() == password)
                {
                    match = sm;
                    break;
                }
            }

            if (match == null)
            {
                showError("שם משתמש או סיסמה שגויים");
                startShake();
                return;
            }
            if (match.getStatus() != AccountStatus.ACTIVE)
            {
                showError("החשבון אינו פעיל");
                return;
            }

            lblError.Visible = false;
            mainForm.onLoginSuccess();
        }

        private void showError(string message)
        {
            lblError.Text = message;
            lblError.Visible = true;
            txtPassword.Clear();
            txtPassword.Focus();
        }
    }
}
