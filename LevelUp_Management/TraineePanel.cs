using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LevelUp_Management
{
    public partial class TraineePanel : UserControl
    {
        private Trainee selectedTrainee;
        private Button btnHealth;

        public TraineePanel()
        {
            InitializeComponent();

            // Added in code (not the Designer), same pattern as RegistrationPanel's
            // btnAddToCalendar / WorkoutPerformancePanel's btnSetGoal: placed on the
            // action row before applyPanel so UITheme sweeps it up and lays it out
            // with the other action buttons.
            btnHealth = new Button();
            btnHealth.Name = "btnHealth";
            btnHealth.Text = "📋 הצהרת בריאות";
            btnHealth.Size = new System.Drawing.Size(160, btnDelete.Height);
            btnHealth.Location = new System.Drawing.Point(btnDelete.Right + 20, btnDelete.Top);
            btnHealth.Click += btnHealth_Click;
            Controls.Add(btnHealth);

            UITheme.applyPanel(this, "ניהול כרטיסי המתאמנים של הסטודיו — הוספה, עדכון ומחיקה");

            // Filled teal button, same override RegistrationPanel/WorkoutPerformancePanel
            // apply after applyPanel's default outline styling.
            Color accent = Color.FromArgb(45, 159, 147);
            btnHealth.FlatStyle = FlatStyle.Flat;
            btnHealth.BackColor = accent;
            btnHealth.ForeColor = Color.White;
            btnHealth.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnHealth.FlatAppearance.BorderColor = accent;
            btnHealth.FlatAppearance.BorderSize = 1;
            btnHealth.FlatAppearance.MouseOverBackColor = Color.FromArgb(36, 138, 128);
            btnHealth.FlatAppearance.MouseDownBackColor = Color.FromArgb(36, 138, 128);
            btnHealth.MouseEnter += (s, e) => btnHealth.ForeColor = Color.White;
            btnHealth.MouseLeave += (s, e) => btnHealth.ForeColor = Color.White;
            btnHealth.AutoSize = true;
            btnHealth.AutoSizeMode = AutoSizeMode.GrowOnly;

            dtpDateOfBirth.Value = DateTime.Today;
            dtpRegistrationDate.Value = DateTime.Today;
            chkIsActive.Checked = true;

            buildStatusLegend();
            dataGridView1.CellFormatting += dataGridView1_CellFormatting;

            loadTrainees();
        }

        // ------------------------------------------------------------------
        // Row status highlighting — purely presentational, added on top of
        // UITheme's grid styling. Does not touch the grid's columns or
        // DataSource, so loadTrainees()/CellClick above are untouched.
        // ------------------------------------------------------------------

        private static readonly Color StatusNewBg = Color.FromArgb(147, 197, 253);
        private static readonly Color StatusInactiveBg = Color.FromArgb(254, 226, 226);
        private static readonly Color StatusMedicalBg = Color.FromArgb(255, 237, 213);
        private static readonly Color StatusStaleBg = Color.FromArgb(254, 249, 195);

        // Placed inside UITheme's "bandGrid" host, above the grid's own frame.
        // buildGridHost() adds that frame with Dock=Fill first; a control added
        // afterward with Dock=Top lands above it (same ordering convention
        // applyShell already relies on for its own Top-docked bands).
        private void buildStatusLegend()
        {
            // Outer band: fixed-height strip docked above the grid frame.
            // Padding.Bottom is left as blank space below the border line —
            // Dock=Top only reserves space via Height on a plain Panel host
            // (Margin is ignored outside a Flow/TableLayoutPanel), so the gap
            // to the grid comes from sizing the band tall enough to hold that
            // padding, not from the padding alone.
            Panel legendBand = new Panel();
            legendBand.Dock = DockStyle.Top;
            legendBand.Padding = new Padding(0, 8, 0, 12);
            legendBand.Height = 45 + legendBand.Padding.Bottom;
            legendBand.BackColor = Color.White;
            legendBand.Paint += (s, e) =>
            {
                int borderY = legendBand.Height - legendBand.Padding.Bottom - 1;
                using (Pen pen = new Pen(Color.FromArgb(226, 232, 240)))
                    e.Graphics.DrawLine(pen, 0, borderY, legendBand.Width, borderY);
            };

            // Inner flow: sized to its own content so it can be centered
            // horizontally inside the full-width band above.
            FlowLayoutPanel legendItems = new FlowLayoutPanel();
            legendItems.FlowDirection = FlowDirection.RightToLeft;
            legendItems.AutoSize = true;
            legendItems.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            legendItems.WrapContents = true;
            legendItems.Anchor = AnchorStyles.Top;
            legendItems.Top = legendBand.Padding.Top;

            addLegendItem(legendItems, StatusNewBg, "מתאמן חדש");
            addLegendItem(legendItems, StatusInactiveBg, "לא פעיל");
            addLegendItem(legendItems, StatusStaleBg, "לא התאמן מעל 60 יום");
            addLegendItem(legendItems, StatusMedicalBg, "בעיה רפואית");
            addLegendItem(legendItems, Color.White, "תקין");

            legendBand.Controls.Add(legendItems);

            void centerLegendItems()
            {
                legendItems.Left = Math.Max(0, (legendBand.ClientSize.Width - legendItems.Width) / 2);
            }
            legendBand.Resize += (s, e) => centerLegendItems();
            legendItems.SizeChanged += (s, e) => centerLegendItems();

            Panel gridHost = Controls.Find("bandGrid", true).FirstOrDefault() as Panel;
            if (gridHost != null) gridHost.Controls.Add(legendBand);

            centerLegendItems();
        }

        private void addLegendItem(FlowLayoutPanel legend, Color color, string text)
        {
            FlowLayoutPanel item = new FlowLayoutPanel();
            item.FlowDirection = FlowDirection.RightToLeft;
            item.AutoSize = true;
            item.WrapContents = false;
            item.Margin = new Padding(0, 8, 20, 8);

            Panel swatch = new Panel();
            swatch.Size = new System.Drawing.Size(16, 16);
            swatch.BackColor = color;
            swatch.Margin = new Padding(6, 4, 6, 0);
            if (color == Color.White)
            {
                swatch.BorderStyle = BorderStyle.FixedSingle;
            }

            Label lbl = new Label();
            lbl.AutoSize = true;
            lbl.Text = text;
            lbl.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbl.ForeColor = Color.FromArgb(100, 116, 139);
            lbl.Margin = new Padding(0, 4, 0, 0);

            item.Controls.Add(lbl);
            item.Controls.Add(swatch);
            legend.Controls.Add(item);
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            object idVal = dataGridView1.Rows[e.RowIndex].Cells["trainee_id"].Value;
            if (idVal == null) return;

            Trainee t = Trainee.seekTrainee(Convert.ToInt32(idVal));
            if (t == null) return;

            bool isNew = (DateTime.Today - t.getRegistrationDate().Date).TotalDays <= 30;

            Color? bg;
            if (isNew) bg = StatusNewBg;
            else if (t.getStatus() != AccountStatus.ACTIVE) bg = StatusInactiveBg;
            else if (hasMedicalIssue(t)) bg = StatusMedicalBg;
            else if (hasNotTrainedRecently(t)) bg = StatusStaleBg;
            else bg = null;

            if (bg.HasValue) e.CellStyle.BackColor = bg.Value;

            if (isNew && dataGridView1.Columns[e.ColumnIndex].Name == "fullName" && e.Value != null)
            {
                e.Value = "🆕 " + e.Value;
                e.FormattingApplied = true;
            }

            // The health-declaration cell gets its own color, overriding the row color.
            if (dataGridView1.Columns[e.ColumnIndex].Name == "healthDeclaration")
            {
                HealthDeclaration hd = findHealthDeclaration(t);
                if (hd == null) e.CellStyle.BackColor = HealthRejectedBg;
                else if (hd.getStatus() == DeclarationStatus.APPROVED) e.CellStyle.BackColor = HealthApprovedBg;
                else if (hd.getStatus() == DeclarationStatus.PENDING) e.CellStyle.BackColor = HealthPendingBg;
                else e.CellStyle.BackColor = HealthRejectedBg;
            }
        }

        private static readonly Color HealthApprovedBg = Color.FromArgb(187, 247, 208);
        private static readonly Color HealthPendingBg = Color.FromArgb(253, 230, 138);
        private static readonly Color HealthRejectedBg = Color.FromArgb(254, 202, 202);

        // Trainee 1 : HealthDeclaration 0..1 (UNIQUE(trainee_id)) — at most one match.
        private HealthDeclaration findHealthDeclaration(Trainee t)
        {
            foreach (HealthDeclaration hd in Program.HealthDeclarations)
            {
                if (hd.getTrainee() == t)
                    return hd;
            }
            return null;
        }

        private static string healthStatusText(HealthDeclaration hd)
        {
            if (hd == null) return "אין";
            switch (hd.getStatus())
            {
                case DeclarationStatus.APPROVED: return "מאושרת";
                case DeclarationStatus.PENDING: return "ממתינה לאישור";
                case DeclarationStatus.REJECTED: return "נדחתה";
                default: return hd.getStatus().ToString();
            }
        }

        // Column not available on the grid, so this reaches into Program's
        // in-memory HealthDeclarations list directly (same lookup pattern
        // dataGridView1_CellClick already uses for the trainee itself).
        private bool hasMedicalIssue(Trainee t)
        {
            foreach (HealthDeclaration hd in Program.HealthDeclarations)
            {
                if (hd.getTrainee() != null && hd.getTrainee().getTraineeId() == t.getTraineeId()
                    && hd.getContent() != null && hd.getContent().Contains("מצבים רפואיים מדווחים"))
                    return true;
            }
            return false;
        }

        // Column not available on the grid; reaches into Program's in-memory
        // WorkoutPerformances list the same way. No performance on record at
        // all also counts as "hasn't trained in the last 60 days".
        private bool hasNotTrainedRecently(Trainee t)
        {
            DateTime? lastWorkout = null;
            foreach (WorkoutPerformance wp in Program.WorkoutPerformances)
            {
                if (wp.getTrainee() != null && wp.getTrainee().getTraineeId() == t.getTraineeId())
                    if (lastWorkout == null || wp.getDate() > lastWorkout.Value)
                        lastWorkout = wp.getDate();
            }

            if (lastWorkout == null) return true;
            return (DateTime.Today - lastWorkout.Value.Date).TotalDays > 60;
        }

        private void loadTrainees()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("trainee_id", typeof(int));
            dt.Columns.Add("fullName", typeof(string));
            dt.Columns.Add("phone", typeof(string));
            dt.Columns.Add("email", typeof(string));
            dt.Columns.Add("dateOfBirth", typeof(DateTime));
            dt.Columns.Add("experienceLevel", typeof(string));
            dt.Columns.Add("isActive", typeof(bool));
            dt.Columns.Add("registrationDate", typeof(DateTime));
            dt.Columns.Add("healthDeclaration", typeof(string));

            foreach (Trainee t in Program.Trainees)
            {
                dt.Rows.Add(t.getTraineeId(), t.getFullName(), t.getPhone(), t.getEmail(),
                    t.getDateOfBirth(), t.getExperienceLevel(), t.getStatus() == AccountStatus.ACTIVE, t.getRegistrationDate(),
                    healthStatusText(findHealthDeclaration(t)));
            }

            dataGridView1.DataSource = dt;

            dataGridView1.Columns["trainee_id"].HeaderText = "מזהה";
            dataGridView1.Columns["fullName"].HeaderText = "שם מלא";
            dataGridView1.Columns["phone"].HeaderText = "טלפון";
            dataGridView1.Columns["email"].HeaderText = "אימייל";
            dataGridView1.Columns["dateOfBirth"].HeaderText = "תאריך לידה";
            dataGridView1.Columns["experienceLevel"].HeaderText = "רמת ניסיון";
            dataGridView1.Columns["isActive"].HeaderText = "פעיל/ה";
            dataGridView1.Columns["registrationDate"].HeaderText = "תאריך הרשמה";
            dataGridView1.Columns["healthDeclaration"].HeaderText = "הצהרת בריאות";
            dataGridView1.Columns["dateOfBirth"].DefaultCellStyle.Format = "dd/MM/yyyy";
            dataGridView1.Columns["registrationDate"].DefaultCellStyle.Format = "dd/MM/yyyy";
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;  //לחיצה על כותרת — מתעלמים

            int id = int.Parse(dataGridView1.Rows[e.RowIndex].Cells["trainee_id"].Value.ToString());
            selectedTrainee = Trainee.seekTrainee(id);
            if (selectedTrainee == null) return;

            txtFullName.Text = selectedTrainee.getFullName();
            txtPhone.Text = selectedTrainee.getPhone();
            txtEmail.Text = selectedTrainee.getEmail();
            dtpDateOfBirth.Value = selectedTrainee.getDateOfBirth();
            txtExperienceLevel.Text = selectedTrainee.getExperienceLevel();
            chkIsActive.Checked = selectedTrainee.getStatus() == AccountStatus.ACTIVE;
            dtpRegistrationDate.Value = selectedTrainee.getRegistrationDate();
        }

        private bool validateFields()
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("יש להזין שם מלא", "שגיאה", MessageBoxButtons.OK);
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("יש להזין טלפון", "שגיאה", MessageBoxButtons.OK);
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("יש להזין אימייל", "שגיאה", MessageBoxButtons.OK);
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtExperienceLevel.Text))
            {
                MessageBox.Show("יש להזין רמת ניסיון", "שגיאה", MessageBoxButtons.OK);
                return false;
            }
            return true;
        }

        private void clearFields()
        {
            selectedTrainee = null;
            txtFullName.Clear();
            txtPhone.Clear();
            txtEmail.Clear();
            txtExperienceLevel.Clear();
            dtpDateOfBirth.Value = DateTime.Today;
            dtpRegistrationDate.Value = DateTime.Today;
            chkIsActive.Checked = true;
        }

        // Trainees has UNIQUE(email); the phone is checked too. Must run before
        // any write: a rejected INSERT would otherwise still leave the new object
        // in Program.Trainees and the panel would report success.
        private bool isDuplicateContact(Trainee current)
        {
            string email = txtEmail.Text.Trim();
            string phone = txtPhone.Text.Trim();
            foreach (Trainee t in Program.Trainees)
            {
                if (t == current) continue;
                if (string.Equals((t.getEmail() ?? "").Trim(), email, StringComparison.OrdinalIgnoreCase)
                    || (t.getPhone() ?? "").Trim() == phone)
                {
                    MessageBox.Show("קיים כבר מתאמן עם מספר טלפון או אימייל זה", "שגיאה", MessageBoxButtons.OK);
                    return true;
                }
            }
            return false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!validateFields()) return;
            if (isDuplicateContact(null)) return;

            int id = Trainee.getNextTraineeId();
            AccountStatus status = chkIsActive.Checked ? AccountStatus.ACTIVE : AccountStatus.INACTIVE;
            Trainee t = new Trainee(id, txtFullName.Text, txtPhone.Text, txtEmail.Text,
                dtpDateOfBirth.Value.Date, txtExperienceLevel.Text, status,
                dtpRegistrationDate.Value.Date, null, true);

            MessageBox.Show("המתאמן נוצר בהצלחה", "הודעה", MessageBoxButtons.OK);
            clearFields();
            loadTrainees();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedTrainee == null)
            {
                MessageBox.Show("יש לבחור מתאמן מהרשימה", "שגיאה", MessageBoxButtons.OK);
                return;
            }
            if (!validateFields()) return;
            if (isDuplicateContact(selectedTrainee)) return;

            selectedTrainee.setFullName(txtFullName.Text);
            selectedTrainee.setPhone(txtPhone.Text);
            selectedTrainee.setEmail(txtEmail.Text);
            selectedTrainee.setDateOfBirth(dtpDateOfBirth.Value.Date);
            selectedTrainee.setExperienceLevel(txtExperienceLevel.Text);
            selectedTrainee.setStatus(chkIsActive.Checked ? AccountStatus.ACTIVE : AccountStatus.INACTIVE);
            selectedTrainee.setRegistrationDate(dtpRegistrationDate.Value.Date);
            selectedTrainee.updateTrainee();

            MessageBox.Show("המתאמן עודכן בהצלחה", "הודעה", MessageBoxButtons.OK);
            clearFields();
            loadTrainees();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedTrainee == null)
            {
                MessageBox.Show("יש לבחור מתאמן מהרשימה", "שגיאה", MessageBoxButtons.OK);
                return;
            }

            selectedTrainee.setStatus(AccountStatus.INACTIVE);
            selectedTrainee.updateTrainee();

            MessageBox.Show("המתאמן הועבר לסטטוס לא-פעיל", "הודעה", MessageBoxButtons.OK);
            clearFields();
            loadTrainees();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            mainForm.showPanel(null);
        }

        // Explicit, one-off exception to CLAUDE.md's "no additional Forms or
        // dialogs" rule - confirmed with the group the same way as
        // WorkoutPerformancePanel's btnSetGoal_Click goal-type picker.
        private void btnHealth_Click(object sender, EventArgs e)
        {
            if (selectedTrainee == null)
            {
                MessageBox.Show("יש לבחור מתאמן מהרשימה", "שגיאה", MessageBoxButtons.OK);
                return;
            }

            showHealthDeclarationDialog(selectedTrainee);
        }

        private void showHealthDeclarationDialog(Trainee trainee)
        {
            Color accent = Color.FromArgb(45, 159, 147);
            string[] questions =
            {
                "האם יש לך מחלות לב או לחץ דם?",
                "האם יש לך בעיות נשימה או אסטמה?",
                "האם יש לך סוכרת?",
                "האם יש לך מגבלות פיזיות או פציעות?",
                "האם את/ה נוטל/ת תרופות באופן קבוע?",
                "האם יש לך אלרגיות?"
            };

            using (Form dlg = new Form())
            {
                dlg.Text = "הצהרת בריאות";
                dlg.RightToLeft = RightToLeft.Yes;
                dlg.RightToLeftLayout = true;
                dlg.BackColor = Color.White;
                dlg.Size = new System.Drawing.Size(750, 750);
                dlg.MinimumSize = new System.Drawing.Size(500, 600);
                dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
                dlg.StartPosition = FormStartPosition.CenterScreen;
                dlg.MaximizeBox = false;
                dlg.MinimizeBox = false;

                Label lblTitle = new Label();
                lblTitle.AutoSize = true;
                lblTitle.Text = "הצהרת בריאות — " + trainee.getFullName();
                lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
                lblTitle.ForeColor = accent;
                lblTitle.Location = new Point(20, 15);
                dlg.Controls.Add(lblTitle);

                Label lblSubtitle = new Label();
                lblSubtitle.AutoSize = true;
                lblSubtitle.Text = "אנא מלא/י את השאלות הבאות";
                lblSubtitle.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
                lblSubtitle.ForeColor = Color.Gray;
                lblSubtitle.Location = new Point(20, lblTitle.Bottom + 4);
                dlg.Controls.Add(lblSubtitle);

                int y = lblSubtitle.Bottom + 20;

                // HealthDeclarations has UNIQUE(trainee_id) — Trainee 1 : HealthDeclaration 0..1.
                // An existing declaration is shown, pre-filled and updated on save, not duplicated.
                HealthDeclaration existing = findHealthDeclaration(trainee);
                bool changed = false;

                if (existing != null)
                {
                    Label lblExisting = new Label();
                    lblExisting.AutoSize = false;
                    lblExisting.Size = new System.Drawing.Size(440, 34);
                    lblExisting.TextAlign = ContentAlignment.MiddleCenter;
                    lblExisting.BorderStyle = BorderStyle.FixedSingle;
                    lblExisting.BackColor = existing.getStatus() == DeclarationStatus.APPROVED ? HealthApprovedBg
                        : existing.getStatus() == DeclarationStatus.PENDING ? HealthPendingBg : HealthRejectedBg;
                    lblExisting.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                    lblExisting.Text = "הצהרה קיימת מתאריך " + existing.getDeclarationDate().ToString("dd/MM/yyyy")
                        + " - סטטוס: " + healthStatusText(existing);
                    lblExisting.Location = new Point(20, y);
                    dlg.Controls.Add(lblExisting);
                    y = lblExisting.Bottom + 6;

                    TextBox txtExisting = new TextBox();
                    txtExisting.Multiline = true;
                    txtExisting.ReadOnly = true;
                    txtExisting.ScrollBars = ScrollBars.Vertical;
                    txtExisting.BackColor = Color.FromArgb(248, 250, 252);
                    txtExisting.Height = 80;
                    txtExisting.Width = 440;
                    txtExisting.RightToLeft = RightToLeft.Yes;
                    txtExisting.Text = existing.getContent();
                    txtExisting.Location = new Point(20, y);
                    dlg.Controls.Add(txtExisting);
                    y = txtExisting.Bottom + 20;
                }

                List<CheckBox> questionBoxes = new List<CheckBox>();
                foreach (string q in questions)
                {
                    CheckBox chk = new CheckBox();
                    chk.AutoSize = true;
                    chk.Text = q;
                    chk.Font = new Font("Segoe UI", 10F);
                    chk.Location = new Point(20, y);
                    dlg.Controls.Add(chk);
                    questionBoxes.Add(chk);
                    y += 35;
                }

                Label lblNotes = new Label();
                lblNotes.AutoSize = true;
                lblNotes.Text = "פרטים נוספים / הערות:";
                lblNotes.Font = new Font("Segoe UI", 10F);
                lblNotes.Location = new Point(20, y + 5);
                dlg.Controls.Add(lblNotes);
                y = lblNotes.Bottom + 5;

                TextBox txtNotes = new TextBox();
                txtNotes.Multiline = true;
                txtNotes.Height = 80;
                txtNotes.Width = 440;
                txtNotes.RightToLeft = RightToLeft.Yes;
                txtNotes.Location = new Point(20, y);
                dlg.Controls.Add(txtNotes);
                y = txtNotes.Bottom + 15;

                CheckBox chkConfirm = new CheckBox();
                chkConfirm.AutoSize = true;
                chkConfirm.Text = "אני מאשר/ת שכל הפרטים שמסרתי נכונים ומדויקים";
                chkConfirm.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                chkConfirm.Location = new Point(20, y);
                dlg.Controls.Add(chkConfirm);
                y = chkConfirm.Bottom + 20;

                Button btnSaveDeclaration = new Button();
                btnSaveDeclaration.Text = existing != null ? "עדכן הצהרה" : "שמור הצהרה";
                btnSaveDeclaration.BackColor = accent;
                btnSaveDeclaration.ForeColor = Color.White;
                btnSaveDeclaration.FlatStyle = FlatStyle.Flat;
                btnSaveDeclaration.FlatAppearance.BorderSize = 0;
                btnSaveDeclaration.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
                btnSaveDeclaration.Location = new Point(20, y);
                btnSaveDeclaration.Size = new System.Drawing.Size(440, 42);
                btnSaveDeclaration.Enabled = false;
                dlg.Controls.Add(btnSaveDeclaration);

                chkConfirm.CheckedChanged += (s, e) => btnSaveDeclaration.Enabled = chkConfirm.Checked;

                y = btnSaveDeclaration.Bottom + 10;

                if (existing != null && existing.getContent() != null)
                {
                    string oldContent = existing.getContent();
                    bool anyQuestion = false;
                    foreach (CheckBox chk in questionBoxes)
                    {
                        chk.Checked = oldContent.Contains(chk.Text);
                        if (chk.Checked) anyQuestion = true;
                    }

                    int notesIndex = oldContent.IndexOf("הערות: ");
                    if (notesIndex >= 0)
                        txtNotes.Text = oldContent.Substring(notesIndex + "הערות: ".Length).Trim();
                    else if (!anyQuestion && !oldContent.StartsWith("לא דווחו מצבים רפואיים"))
                        // Free text not written by this dialog (e.g. seeded "אין הגבלות ידועות").
                        txtNotes.Text = oldContent.Trim();
                }

                if (existing != null && existing.getStatus() == DeclarationStatus.PENDING)
                {
                    Button btnApprove = new Button();
                    btnApprove.Text = "אשר הצהרה";
                    btnApprove.BackColor = Color.FromArgb(22, 163, 74);
                    btnApprove.ForeColor = Color.White;
                    btnApprove.FlatStyle = FlatStyle.Flat;
                    btnApprove.FlatAppearance.BorderSize = 0;
                    btnApprove.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                    btnApprove.Location = new Point(20, y);
                    btnApprove.Size = new System.Drawing.Size(215, 38);
                    dlg.Controls.Add(btnApprove);

                    Button btnReject = new Button();
                    btnReject.Text = "דחה הצהרה";
                    btnReject.BackColor = Color.FromArgb(220, 38, 38);
                    btnReject.ForeColor = Color.White;
                    btnReject.FlatStyle = FlatStyle.Flat;
                    btnReject.FlatAppearance.BorderSize = 0;
                    btnReject.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                    btnReject.Location = new Point(245, y);
                    btnReject.Size = new System.Drawing.Size(215, 38);
                    dlg.Controls.Add(btnReject);

                    y = btnApprove.Bottom + 10;

                    btnApprove.Click += (s, e) =>
                    {
                        existing.approve();
                        changed = true;
                        MessageBox.Show("הצהרת הבריאות אושרה", "הודעה", MessageBoxButtons.OK);
                        dlg.Close();
                    };

                    btnReject.Click += (s, e) =>
                    {
                        existing.setStatus(DeclarationStatus.REJECTED);
                        existing.updateHealthDeclaration();
                        changed = true;
                        MessageBox.Show("הצהרת הבריאות נדחתה", "הודעה", MessageBoxButtons.OK);
                        dlg.Close();
                    };
                }

                // Grow the dialog to fit the optional existing-declaration box and buttons.
                dlg.ClientSize = new System.Drawing.Size(dlg.ClientSize.Width, Math.Max(dlg.ClientSize.Height, y + 20));

                btnSaveDeclaration.Click += (s, e) =>
                {
                    List<string> checkedAnswers = new List<string>();
                    foreach (CheckBox chk in questionBoxes)
                        if (chk.Checked) checkedAnswers.Add(chk.Text);

                    StringBuilder content = new StringBuilder();
                    content.AppendLine(checkedAnswers.Count > 0
                        ? "מצבים רפואיים מדווחים: " + string.Join(", ", checkedAnswers)
                        : "לא דווחו מצבים רפואיים");
                    if (!string.IsNullOrWhiteSpace(txtNotes.Text))
                        content.AppendLine("הערות: " + txtNotes.Text.Trim());

                    // PENDING, not the pasted spec's assumed "Active": DeclarationStatus
                    // is PENDING/APPROVED/REJECTED (DeclarationStatus.cs) — a freshly
                    // submitted declaration awaits trainer/manager review (NFR-4).
                    if (existing != null)
                    {
                        // Changed content goes back to PENDING for a fresh review.
                        existing.setContent(content.ToString());
                        existing.setDeclarationDate(DateTime.Now);
                        existing.setStatus(DeclarationStatus.PENDING);
                        existing.updateHealthDeclaration();
                        changed = true;

                        MessageBox.Show("הצהרת הבריאות עודכנה בהצלחה", "הודעה", MessageBoxButtons.OK);
                        dlg.Close();
                        return;
                    }

                    int id = HealthDeclaration.getNextHealthDeclarationId();
                    new HealthDeclaration(id, trainee, DateTime.Now, DeclarationStatus.PENDING, content.ToString(), true);
                    changed = true;

                    MessageBox.Show("הצהרת הבריאות נשמרה בהצלחה! ✅", "הודעה", MessageBoxButtons.OK);
                    dlg.Close();
                };

                dlg.ShowDialog(this);

                // Refresh so the "הצהרת בריאות" column reflects the save/approve/reject.
                if (changed) loadTrainees();
            }
        }
    }
}
