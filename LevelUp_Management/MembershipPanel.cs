using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace LevelUp_Management
{
    public partial class MembershipPanel : UserControl
    {
        private Membership selectedMembership;
        private Button btnRenewReminder;

        private struct PlanInfo
        {
            public double Price;
            public int AllowedEntries;
            public int ValidityDays;

            public PlanInfo(double price, int allowedEntries, int validityDays)
            {
                Price = price;
                AllowedEntries = allowedEntries;
                ValidityDays = validityDays;
            }
        }

        // Known plans with auto-fill data. Plan names that already exist in the DB
        // (read from the grid) but aren't in this catalog can still be selected —
        // they just don't trigger auto-fill (see cboPlan_SelectedIndexChanged).
        private static readonly Dictionary<string, PlanInfo> planCatalog = new Dictionary<string, PlanInfo>
        {
            ["מנוי חודשי — בסיסי"] = new PlanInfo(199, 12, 30),
            ["מנוי חודשי — פרימיום"] = new PlanInfo(299, 30, 30),
            ["מנוי רבעוני — בסיסי"] = new PlanInfo(499, 36, 90),
            ["מנוי רבעוני — פרימיום"] = new PlanInfo(749, 90, 90),
            ["מנוי חצי שנתי"] = new PlanInfo(1299, 180, 180),
            ["מנוי שנתי"] = new PlanInfo(2199, 365, 365),
            ["מנוי סטודנט"] = new PlanInfo(149, 12, 30),
            ["כרטיסייה 10 כניסות"] = new PlanInfo(249, 10, 60),
            ["כרטיסייה 20 כניסות"] = new PlanInfo(449, 20, 90),
            ["אימון בודד"] = new PlanInfo(45, 1, 1),
        };

        // Not given by the catalog above — derived from the plan name so txtPlanType
        // (still required by validateFields) gets filled for every catalog plan.
        private static string derivePlanType(string planName)
        {
            if (planName.Contains("פרימיום")) return "פרימיום";
            if (planName.Contains("כרטיסייה")) return "כרטיסייה";
            if (planName.Contains("בודד")) return "בודד";
            return "בסיסי";
        }

        public MembershipPanel()
        {
            InitializeComponent();
            btnRenewReminder = new Button();
            btnRenewReminder.Name = "btnRenewReminder";
            btnRenewReminder.Text = "📧 תזכורת חידוש";
            btnRenewReminder.Click += btnRenewReminder_Click;
            Button[] mainActions = { btnSave, btnDelete, btnRenew, btnRenewReminder, btnPayments, btnBack };
            // Removed before applyPanel() so UITheme's BandFlow never flows them in
            // with the form fields (it wraps by leftover width, splitting the row).
            foreach (Button b in mainActions) Controls.Remove(b);
            UITheme.applyPanel(this, "חידוש מנויים וכרטיסיות, מעקב תוקף וסטטוס תשלום");
            buildMainActionRow(mainActions);
            buildStatusLegend();
            dataGridView1.CellFormatting += dataGridView1_CellFormatting;
            loadCombos();
            // cboStatus is read-only: a new Membership always starts ACTIVE (btnSave_Click
            // hardcodes it), and existing status only changes via the state-transition
            // buttons below. This combo exists solely to display a selected row's status.
            cboStatus.Enabled = false;
            dtpPurchaseDate.Value = DateTime.Today;
            dtpStartDate.Value = DateTime.Today;
            dtpEndDate.Value = DateTime.Today.AddMonths(1);
            loadMemberships();
        }

        private void buildMainActionRow(Button[] buttons)
        {
            Panel row = new Panel();
            row.Name = "bandMainActions";
            row.Dock = DockStyle.Bottom;
            row.BackColor = UITheme.Card;

            // Sized from a status-row button (already styled by applyPanel) so both
            // rows stay identical even if UITheme's button metrics change.
            Button reference = btnFreeze;
            foreach (Button b in buttons)
            {
                // stylePrimaryButton resets Font, so the reference font is applied after it.
                UITheme.stylePrimaryButton(b, 1f);
                b.AutoSize = false;
                b.Font = reference.Font;
                b.Padding = reference.Padding;
                b.Height = reference.Height;
                b.Width = Math.Max(b.PreferredSize.Width, reference.Width);
                row.Controls.Add(b);
            }

            Controls.Add(row);
            // Dock order: the row must sit between the form band and the status-button
            // action bar, so it takes the action bar's slot and pushes it outermost.
            Control statusBar = Controls["bandActions"];
            if (statusBar != null)
                Controls.SetChildIndex(row, Controls.GetChildIndex(statusBar));

            row.SizeChanged += (sender, e) => layoutMainActionRow(row, buttons);
            layoutMainActionRow(row, buttons);
        }

        private void layoutMainActionRow(Panel row, Button[] buttons)
        {
            int pad = UITheme.dp(this, 24);
            int gap = UITheme.dp(this, 10);

            int buttonHeight = 0;
            foreach (Button b in buttons)
                buttonHeight = Math.Max(buttonHeight, b.Height);

            int wanted = Math.Max(UITheme.dp(this, 55), buttonHeight + UITheme.dp(this, 16));
            if (row.Height != wanted) row.Height = wanted;

            int y = (row.Height - buttonHeight) / 2;
            int x = pad;
            foreach (Button b in buttons)
            {
                b.Location = new Point(x, y);
                x += b.Width + gap;
            }
        }

        private static readonly Color StatusExpiredBg = Color.FromArgb(255, 220, 220);
        private static readonly Color StatusExpiringBg = Color.FromArgb(255, 248, 200);
        private static readonly Color StatusFrozenBg = Color.FromArgb(200, 220, 255);
        private static readonly Color StatusSuspendedBg = Color.FromArgb(255, 230, 200);
        private const int ExpiringSoonDays = 7;

        // Placed inside UITheme's "bandGrid" host, above the grid frame (same
        // approach as TraineePanel's legend).
        private void buildStatusLegend()
        {
            Panel legendBand = new Panel();
            legendBand.Dock = DockStyle.Top;
            // Padding.Bottom is the gap to the grid: a Dock=Top Panel only reserves
            // space through its Height, so the band is sized to include it.
            legendBand.Padding = new Padding(0, 4, 0, 10);
            legendBand.Height = 40 + legendBand.Padding.Bottom;
            legendBand.BackColor = Color.White;
            legendBand.Paint += (s, e) =>
            {
                int borderY = legendBand.Height - legendBand.Padding.Bottom - 1;
                using (Pen pen = new Pen(Color.FromArgb(226, 232, 240)))
                    e.Graphics.DrawLine(pen, 0, borderY, legendBand.Width, borderY);
            };

            FlowLayoutPanel legendItems = new FlowLayoutPanel();
            legendItems.FlowDirection = FlowDirection.RightToLeft;
            legendItems.AutoSize = true;
            legendItems.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            legendItems.WrapContents = true;
            legendItems.Anchor = AnchorStyles.Top;
            legendItems.Top = legendBand.Padding.Top;

            addLegendItem(legendItems, StatusExpiredBg, "פג תוקף");
            addLegendItem(legendItems, StatusExpiringBg, "עומד לפוג (7 ימים)");
            addLegendItem(legendItems, StatusFrozenBg, "מוקפא");
            addLegendItem(legendItems, StatusSuspendedBg, "מושעה");
            addLegendItem(legendItems, Color.White, "פעיל");

            legendBand.Controls.Add(legendItems);

            void centerLegendItems()
            {
                legendItems.Left = Math.Max(0, (legendBand.ClientSize.Width - legendItems.Width) / 2);
            }
            legendBand.Resize += (s, e) => centerLegendItems();
            legendItems.SizeChanged += (s, e) => centerLegendItems();

            Control[] gridHost = Controls.Find("bandGrid", true);
            if (gridHost.Length > 0) gridHost[0].Controls.Add(legendBand);

            centerLegendItems();
        }

        private void addLegendItem(FlowLayoutPanel legend, Color color, string text)
        {
            FlowLayoutPanel item = new FlowLayoutPanel();
            item.FlowDirection = FlowDirection.RightToLeft;
            item.AutoSize = true;
            item.WrapContents = false;
            item.Margin = new Padding(0, 6, 20, 6);

            Panel swatch = new Panel();
            swatch.Size = new Size(14, 14);
            swatch.BackColor = color;
            swatch.Margin = new Padding(6, 4, 6, 0);
            if (color == Color.White) swatch.BorderStyle = BorderStyle.FixedSingle;

            Label lbl = new Label();
            lbl.AutoSize = true;
            lbl.Text = text;
            lbl.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbl.ForeColor = Color.FromArgb(100, 116, 139);
            lbl.Margin = new Padding(0, 2, 0, 0);

            item.Controls.Add(lbl);
            item.Controls.Add(swatch);
            legend.Controls.Add(item);
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
            object statusVal = row.Cells["status"].Value;
            if (statusVal == null) return;

            Color? bg = null;
            switch (statusVal.ToString())
            {
                case nameof(MembershipStatus.EXPIRED):
                    bg = StatusExpiredBg;
                    break;
                case nameof(MembershipStatus.FROZEN):
                    bg = StatusFrozenBg;
                    break;
                case nameof(MembershipStatus.SUSPENDED):
                    bg = StatusSuspendedBg;
                    break;
                case nameof(MembershipStatus.ACTIVE):
                    if (row.Cells["endDate"].Value is DateTime endDate
                        && endDate.Date <= DateTime.Today.AddDays(ExpiringSoonDays))
                        bg = StatusExpiringBg;
                    break;
            }

            if (bg.HasValue) e.CellStyle.BackColor = bg.Value;
        }

        private void loadCombos()
        {
            DataTable traineeTable = new DataTable();
            traineeTable.Columns.Add("id", typeof(int));
            traineeTable.Columns.Add("display", typeof(string));
            foreach (Trainee t in Program.Trainees)
                traineeTable.Rows.Add(t.getTraineeId(), t.getFullName() + " (" + t.getTraineeId() + ")");
            cboTrainee.DataSource = traineeTable;
            cboTrainee.DisplayMember = "display";
            cboTrainee.ValueMember = "id";

            cboStatus.DataSource = Enum.GetValues(typeof(MembershipStatus));
        }

        // Rebuilds cboPlan's list from the unique plan names already in use (read
        // from the grid). Called whenever loadMemberships() refreshes the grid, so
        // it always reflects the current DB content.
        private void populatePlanCombo()
        {
            HashSet<string> existingNames = new HashSet<string>();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;
                object value = row.Cells["plan"].Value;
                if (value != null && !string.IsNullOrWhiteSpace(value.ToString()))
                    existingNames.Add(value.ToString());
            }

            cboPlan.Items.Clear();
            foreach (string name in existingNames)
                cboPlan.Items.Add(name);

            cboPlan.SelectedIndex = -1;
        }

        private void cboPlan_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = cboPlan.SelectedItem as string;
            if (string.IsNullOrEmpty(selected)) return;

            txtPlanName.Text = selected;

            if (planCatalog.TryGetValue(selected, out PlanInfo plan))
            {
                txtPlanType.Text = derivePlanType(selected);
                txtPrice.Text = plan.Price.ToString(CultureInfo.InvariantCulture);
                txtAllowedEntries.Text = plan.AllowedEntries.ToString();
                txtValidityDays.Text = plan.ValidityDays.ToString();
                dtpStartDate.Value = DateTime.Today;
                dtpEndDate.Value = DateTime.Today.AddDays(plan.ValidityDays);
                txtRemainingEntries.Text = plan.AllowedEntries.ToString();
            }
            // else: an existing DB plan name not in the catalog — leave price /
            // allowedEntries / validityDays / dates for the user to fill manually.
        }

        private void loadMemberships()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("membership_id", typeof(int));
            dt.Columns.Add("trainee", typeof(string));
            dt.Columns.Add("plan", typeof(string));
            dt.Columns.Add("startDate", typeof(DateTime));
            dt.Columns.Add("endDate", typeof(DateTime));
            dt.Columns.Add("remainingEntries", typeof(int));
            dt.Columns.Add("status", typeof(string));

            foreach (Membership m in Program.Memberships)
            {
                dt.Rows.Add(m.getMembershipId(), m.getTrainee().getFullName(), m.getPlanName(),
                    m.getStartDate(), m.getEndDate(), m.getRemainingEntries(), m.getStatus().ToString());
            }

            dataGridView1.DataSource = dt;

            dataGridView1.Columns["membership_id"].HeaderText = "מזהה";
            dataGridView1.Columns["trainee"].HeaderText = "מתאמן";
            dataGridView1.Columns["plan"].HeaderText = "מסלול מנוי";
            dataGridView1.Columns["startDate"].HeaderText = "תאריך התחלה";
            dataGridView1.Columns["endDate"].HeaderText = "תאריך סיום";
            dataGridView1.Columns["remainingEntries"].HeaderText = "כניסות שנותרו";
            dataGridView1.Columns["status"].HeaderText = "סטטוס";
            dataGridView1.Columns["startDate"].DefaultCellStyle.Format = "dd/MM/yyyy";
            dataGridView1.Columns["endDate"].DefaultCellStyle.Format = "dd/MM/yyyy";

            populatePlanCombo();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;  //לחיצה על כותרת — מתעלמים

            int id = int.Parse(dataGridView1.Rows[e.RowIndex].Cells["membership_id"].Value.ToString());
            selectedMembership = Membership.seekMembership(id);
            if (selectedMembership == null) return;

            cboTrainee.SelectedValue = selectedMembership.getTrainee().getTraineeId();
            dtpPurchaseDate.Value = selectedMembership.getPurchaseDate();
            txtPlanName.Text = selectedMembership.getPlanName();
            txtPlanType.Text = selectedMembership.getPlanType();
            txtPrice.Text = selectedMembership.getPrice().ToString(CultureInfo.InvariantCulture);
            txtAllowedEntries.Text = selectedMembership.getAllowedEntries().ToString();
            txtValidityDays.Text = selectedMembership.getValidityDays().ToString();
            dtpStartDate.Value = selectedMembership.getStartDate();
            dtpEndDate.Value = selectedMembership.getEndDate();
            txtRemainingEntries.Text = selectedMembership.getRemainingEntries().ToString();
            cboStatus.SelectedItem = selectedMembership.getStatus();

            btnRenew.Enabled = selectedMembership.getStatus() == MembershipStatus.EXPIRED
                || selectedMembership.getStatus() == MembershipStatus.SUSPENDED;
        }

        private bool validateFields()
        {
            if (cboTrainee.SelectedValue == null)
            {
                MessageBox.Show("יש לבחור מתאמן", "שגיאה", MessageBoxButtons.OK);
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtPlanName.Text))
            {
                MessageBox.Show("יש להזין שם מסלול", "שגיאה", MessageBoxButtons.OK);
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtPlanType.Text))
            {
                MessageBox.Show("יש להזין סוג מסלול", "שגיאה", MessageBoxButtons.OK);
                return false;
            }
            if (!double.TryParse(txtPrice.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out double price) || price < 0)
            {
                MessageBox.Show("יש להזין מחיר תקין", "שגיאה", MessageBoxButtons.OK);
                return false;
            }
            if (!int.TryParse(txtAllowedEntries.Text, out int allowedEntries) || allowedEntries < 0)
            {
                MessageBox.Show("יש להזין מספר כניסות מותרות תקין", "שגיאה", MessageBoxButtons.OK);
                return false;
            }
            if (!int.TryParse(txtValidityDays.Text, out int validityDays) || validityDays < 0)
            {
                MessageBox.Show("יש להזין מספר ימי תוקף תקין", "שגיאה", MessageBoxButtons.OK);
                return false;
            }
            if (!int.TryParse(txtRemainingEntries.Text, out int remaining) || remaining < 0)
            {
                MessageBox.Show("יש להזין מספר כניסות תקין", "שגיאה", MessageBoxButtons.OK);
                return false;
            }
            return true;
        }

        private void clearFields()
        {
            selectedMembership = null;
            if (cboTrainee.Items.Count > 0) cboTrainee.SelectedIndex = 0;
            dtpPurchaseDate.Value = DateTime.Today;
            cboPlan.SelectedIndex = -1;
            txtPlanName.Clear();
            txtPlanType.Clear();
            txtPrice.Clear();
            txtAllowedEntries.Clear();
            txtValidityDays.Clear();
            dtpStartDate.Value = DateTime.Today;
            dtpEndDate.Value = DateTime.Today.AddMonths(1);
            txtRemainingEntries.Clear();
            cboStatus.SelectedIndex = -1;
            btnRenew.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!validateFields()) return;

            int id = Membership.getNextMembershipId();
            Trainee trainee = Trainee.seekTrainee((int)cboTrainee.SelectedValue);
            double price = double.Parse(txtPrice.Text, NumberStyles.Number, CultureInfo.InvariantCulture);
            int allowedEntries = int.Parse(txtAllowedEntries.Text);
            int validityDays = int.Parse(txtValidityDays.Text);

            // A new membership always starts ACTIVE — the state diagram's nested
            // initial state routes unconditionally to Active, never to a
            // user-chosen status. cboStatus plays no part in creation.
            Membership m = new Membership(id, trainee, dtpPurchaseDate.Value.Date, txtPlanName.Text, txtPlanType.Text,
                price, allowedEntries, validityDays, dtpStartDate.Value.Date, dtpEndDate.Value.Date,
                int.Parse(txtRemainingEntries.Text), MembershipStatus.ACTIVE, true);

            MessageBox.Show("המנוי נוצר בהצלחה", "הודעה", MessageBoxButtons.OK);
            clearFields();
            loadMemberships();
        }

        // ====================================================================
        // State-transition buttons (docs/design/state-diagram.md — Membership
        // State Diagram, transitions 4-7). These replace the old generic
        // "עדכון" button, which let status be set to any value directly via
        // cboStatus — bypassing the state machine's guards entirely. Each
        // button calls its matching domain-verb method on Membership, which
        // enforces its own guard and throws InvalidOperationException (with a
        // Hebrew message) on failure. Transitions 8/9 (purge, after 7 years)
        // are time-triggered, not user-triggered, so there is no button for
        // them here — see Membership.purge().
        // ====================================================================

        private void btnFreeze_Click(object sender, EventArgs e)
        {
            if (selectedMembership == null)
            {
                MessageBox.Show("יש לבחור מנוי מהרשימה", "שגיאה", MessageBoxButtons.OK);
                return;
            }

            try
            {
                selectedMembership.freeze();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "שגיאה", MessageBoxButtons.OK);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("שגיאה בביצוע הפעולה: " + ex.Message, "שגיאה", MessageBoxButtons.OK);
                return;
            }

            MessageBox.Show("המנוי הוקפא בהצלחה", "הודעה", MessageBoxButtons.OK);
            clearFields();
            loadMemberships();
        }

        private void btnUnfreeze_Click(object sender, EventArgs e)
        {
            if (selectedMembership == null)
            {
                MessageBox.Show("יש לבחור מנוי מהרשימה", "שגיאה", MessageBoxButtons.OK);
                return;
            }

            try
            {
                selectedMembership.unfreeze();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "שגיאה", MessageBoxButtons.OK);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("שגיאה בביצוע הפעולה: " + ex.Message, "שגיאה", MessageBoxButtons.OK);
                return;
            }

            MessageBox.Show("הקפאת המנוי בוטלה בהצלחה", "הודעה", MessageBoxButtons.OK);
            clearFields();
            loadMemberships();
        }

        private void btnSuspend_Click(object sender, EventArgs e)
        {
            if (selectedMembership == null)
            {
                MessageBox.Show("יש לבחור מנוי מהרשימה", "שגיאה", MessageBoxButtons.OK);
                return;
            }

            DialogResult confirm = MessageBox.Show("האם התשלום אכן נכשל?", "אישור השעיה",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                selectedMembership.suspend(true);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "שגיאה", MessageBoxButtons.OK);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("שגיאה בביצוע הפעולה: " + ex.Message, "שגיאה", MessageBoxButtons.OK);
                return;
            }

            MessageBox.Show("המנוי הושעה בהצלחה", "הודעה", MessageBoxButtons.OK);
            clearFields();
            loadMemberships();
        }

        private void btnExpire_Click(object sender, EventArgs e)
        {
            if (selectedMembership == null)
            {
                MessageBox.Show("יש לבחור מנוי מהרשימה", "שגיאה", MessageBoxButtons.OK);
                return;
            }

            try
            {
                selectedMembership.expire();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "שגיאה", MessageBoxButtons.OK);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("שגיאה בביצוע הפעולה: " + ex.Message, "שגיאה", MessageBoxButtons.OK);
                return;
            }

            MessageBox.Show("תוקף המנוי פג בהצלחה", "הודעה", MessageBoxButtons.OK);
            clearFields();
            loadMemberships();
        }

        /// <summary>
        /// Not part of the state diagram's transitions 4-7 — a UI convenience that
        /// re-purchases a lapsed membership (EXPIRED/SUSPENDED) back to ACTIVE using
        /// whichever plan is currently reflected in the form fields (either freshly
        /// picked from cboPlan or already loaded from the selected row).
        /// </summary>
        private void btnRenew_Click(object sender, EventArgs e)
        {
            if (selectedMembership == null)
            {
                MessageBox.Show("יש לבחור מנוי מהרשימה", "שגיאה", MessageBoxButtons.OK);
                return;
            }

            if (selectedMembership.getStatus() != MembershipStatus.EXPIRED
                && selectedMembership.getStatus() != MembershipStatus.SUSPENDED)
            {
                MessageBox.Show("ניתן לחדש רק מנוי שפג תוקפו או הושעה.", "שגיאה", MessageBoxButtons.OK);
                return;
            }

            if (cboPlan.SelectedItem == null && string.IsNullOrWhiteSpace(txtPlanName.Text))
            {
                MessageBox.Show("יש לבחור מסלול מנוי לחידוש", "שגיאה", MessageBoxButtons.OK);
                return;
            }

            double price = double.Parse(txtPrice.Text, NumberStyles.Number, CultureInfo.InvariantCulture);
            int allowedEntries = int.Parse(txtAllowedEntries.Text);
            int validityDays = int.Parse(txtValidityDays.Text);

            selectedMembership.setStartDate(DateTime.Today);
            selectedMembership.setEndDate(DateTime.Today.AddDays(validityDays));
            selectedMembership.setRemainingEntries(allowedEntries);
            selectedMembership.setPrice(price);
            selectedMembership.setStatus(MembershipStatus.ACTIVE);

            try
            {
                selectedMembership.updateMembership();
            }
            catch (Exception ex)
            {
                MessageBox.Show("שגיאה בחידוש המנוי: " + ex.Message, "שגיאה", MessageBoxButtons.OK);
                return;
            }

            MessageBox.Show("המנוי חודש בהצלחה! ✅", "הודעה", MessageBoxButtons.OK);
            clearFields();
            loadMemberships();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedMembership == null)
            {
                MessageBox.Show("יש לבחור מנוי מהרשימה", "שגיאה", MessageBoxButtons.OK);
                return;
            }

            selectedMembership.deleteMembership();

            MessageBox.Show("המנוי נמחק בהצלחה", "הודעה", MessageBoxButtons.OK);
            clearFields();
            loadMemberships();
        }

        // Email reminder to renew — only for an EXPIRED membership or an ACTIVE one
        // expiring within ExpiringSoonDays (same rule as the grid's "expiring" colour).
        private async void btnRenewReminder_Click(object sender, EventArgs e)
        {
            if (selectedMembership == null)
            {
                MessageBox.Show("יש לבחור מנוי מהרשימה", "שגיאה", MessageBoxButtons.OK);
                return;
            }

            bool expired = selectedMembership.getStatus() == MembershipStatus.EXPIRED;
            bool expiringSoon = selectedMembership.getStatus() == MembershipStatus.ACTIVE
                && selectedMembership.getEndDate().Date <= DateTime.Today.AddDays(ExpiringSoonDays);
            if (!expired && !expiringSoon)
            {
                MessageBox.Show("ניתן לשלוח תזכורת רק למנוי שפג תוקפו או עומד לפוג ב-" + ExpiringSoonDays + " הימים הקרובים",
                    "שגיאה", MessageBoxButtons.OK);
                return;
            }

            Trainee trainee = selectedMembership.getTrainee();
            string body = EmailService.BuildHtmlBody(trainee.getFullName(),
                expired ? "המנוי שלך בסטודיו פג תוקף. זה הזמן לחדש ולהמשיך להתאמן!"
                        : "המנוי שלך בסטודיו עומד לפוג בקרוב. חדש/י אותו כדי להמשיך להתאמן ללא הפסקה!",
                new string[,]
                {
                    { "מסלול", selectedMembership.getPlanName() },
                    { "תאריך תפוגה", selectedMembership.getEndDate().ToString("dd/MM/yyyy") },
                    { "כניסות שנותרו", selectedMembership.getRemainingEntries().ToString() }
                },
                "לחידוש המנוי ניתן לפנות לצוות הסטודיו. מחכים לראות אותך!");

            btnRenewReminder.Enabled = false;
            try
            {
                if (await EmailService.SendEmailAsync(trainee.getEmail(), "תזכורת חידוש מנוי - Level UP Studio", body))
                    MessageBox.Show("תזכורת החידוש נשלחה בהצלחה אל " + trainee.getEmail(), "הודעה", MessageBoxButtons.OK);
            }
            finally
            {
                btnRenewReminder.Enabled = true;
            }
        }

        private void btnPayments_Click(object sender, EventArgs e)
        {
            mainForm.showPanel(new PaymentPanel());
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            mainForm.showPanel(null);
        }
    }
}
