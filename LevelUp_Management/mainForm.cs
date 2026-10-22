using System.Windows.Forms;

namespace LevelUp_Management
{
    public partial class mainForm : Form
    {
        private static mainForm instance;

        public mainForm()
        {
            InitializeComponent();
            instance = this;
            // Visual theme only — see UITheme.cs. No navigation or data behaviour
            // is defined there; the menu buttons keep the handlers below.
            UITheme.applyShell(this, panelMenu, panelMain, lblMenuTitle);
            panelMenu.Visible = false;   // hidden until a successful login
            showPanel(new LoginPanel());

            // Fade the window in on startup.
            Opacity = 0;
            Shown += mainForm_Shown;
        }

        private void mainForm_Shown(object sender, System.EventArgs e)
        {
            System.Diagnostics.Stopwatch clock = System.Diagnostics.Stopwatch.StartNew();
            Timer fade = new Timer();
            fade.Interval = 15;
            fade.Tick += (s, args) =>
            {
                double t = System.Math.Min(1.0, clock.ElapsedMilliseconds / 300.0);
                Opacity = t;
                if (t >= 1.0)
                {
                    fade.Stop();
                    fade.Dispose();
                }
            };
            fade.Start();
        }

        // Called by LoginPanel after credentials are accepted: reveal the menu
        // and open the dashboard (showPanel(null) shows HomePanel).
        public static void onLoginSuccess()
        {
            instance.panelMenu.Visible = true;
            showPanel(null);
        }

        // The only way to navigate between screens: mainForm.showPanel(new MyPanel());
        // showPanel(null) returns to the home state, which is the HomePanel
        // dashboard - the flat menu in panelMenu is permanent chrome and stays
        // visible either way.
        public static void showPanel(UserControl panel)
        {
            instance.panelMain.Controls.Clear();
            if (panel == null)
                panel = new HomePanel();
            panel.Dock = DockStyle.Fill;
            instance.panelMain.Controls.Add(panel);
        }

        private void btnManageTrainees_Click(object sender, System.EventArgs e)
        {
            showPanel(new TraineePanel());
        }

        private void btnRegisterToWorkout_Click(object sender, System.EventArgs e)
        {
            showPanel(new RegistrationPanel());
        }

        private void btnManageWaitlist_Click(object sender, System.EventArgs e)
        {
            showPanel(new WaitlistEntryPanel());
        }

        private void btnDocumentWorkoutPerformance_Click(object sender, System.EventArgs e)
        {
            showPanel(new WorkoutPerformancePanel());
        }

        private void btnRenewPayMembership_Click(object sender, System.EventArgs e)
        {
            showPanel(new MembershipPanel());
        }

        private void btnGenerateRentersBillingReport_Click(object sender, System.EventArgs e)
        {
            showPanel(new RentersBillingReportPanel());
        }
    }
}
