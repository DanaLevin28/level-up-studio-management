using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LevelUp_Management
{
    // UC02 "Register to Workout". Registration into this panel happens exclusively
    // through the guarded, transactional sp_register_to_workout flow
    // (Trainee.registerToWorkout) - there is no raw CRUD path here by design, so
    // no registration can ever bypass the flow's guards (membership validity,
    // capacity, duplicate check, etc.). This panel is read-only list + one action.
    public partial class RegistrationPanel : UserControl
    {
        private Registration selectedRegistration;
        private Button btnAddToCalendar;
        private Button btnSendReminder;

        private static readonly Color ConfirmedColor = Color.FromArgb(220, 255, 220);
        private static readonly Color CancelledColor = Color.FromArgb(240, 240, 240);
        private static readonly Color WaitlistedColor = Color.FromArgb(255, 252, 220);
        private static readonly Color NoShowColor = Color.FromArgb(255, 220, 220);
        private static readonly Color AlmostFullColor = Color.FromArgb(255, 220, 170);
        private static readonly Color FullColor = Color.FromArgb(255, 190, 190);

        // Row flash after a successful registration: the new row starts bright
        // teal-green and fades back to its normal status colour over FlashMs.
        // Drawn in a second CellFormatting handler that runs after the status
        // colouring, so clearing flashRegistrationId restores it exactly.
        private static readonly Color FlashColor = Color.FromArgb(40, 220, 170);
        private const int FlashMs = 1200;
        private int flashRegistrationId = -1;
        private double flashProgress;
        private Timer flashTimer;
        private System.Diagnostics.Stopwatch flashClock;

        public RegistrationPanel()
        {
            InitializeComponent();
            dataGridView1.CellFormatting += dataGridView1_CellFormatting;
            dataGridView1.CellFormatting += flash_CellFormatting;
            Disposed += delegate { stopFlash(); };

            // Added in code (not the Designer) and placed in the action row before
            // applyPanel so UITheme lays it out with the other action buttons.
            btnAddToCalendar = new Button();
            btnAddToCalendar.Name = "btnAddToCalendar";
            btnAddToCalendar.Text = "📅 הוסף ליומן Google";
            btnAddToCalendar.Size = new System.Drawing.Size(190, btnFinishClass.Height);
            btnAddToCalendar.Location = new System.Drawing.Point(btnFinishClass.Left + 240, btnFinishClass.Top);
            btnAddToCalendar.Click += btnAddToCalendar_Click;
            Controls.Add(btnAddToCalendar);

            btnSendReminder = new Button();
            btnSendReminder.Name = "btnSendReminder";
            btnSendReminder.Text = "📧 שלח תזכורת";
            btnSendReminder.Size = new System.Drawing.Size(160, btnFinishClass.Height);
            btnSendReminder.Location = new System.Drawing.Point(btnAddToCalendar.Left + 240, btnFinishClass.Top);
            btnSendReminder.Click += btnSendReminder_Click;
            Controls.Add(btnSendReminder);

            UITheme.applyPanel(this, "רישום מתאמן לאימון קיים ומעקב אחר ההרשמות הקיימות");

            // Filled teal buttons. applyPanel already gave them the theme's outline
            // hover handlers (which recolour the text teal), so re-assert white after them.
            styleAccentButton(btnAddToCalendar, "📅 הוסף ליומן Google");
            styleAccentButton(btnSendReminder, "📧 שלח תזכורת");

            buildLegend();
            loadCombos();
            loadRegistrations();
        }

        private static void styleAccentButton(Button button, string text)
        {
            Color accent = Color.FromArgb(45, 159, 147);
            button.Text = text;
            button.FlatStyle = FlatStyle.Flat;
            button.BackColor = accent;
            button.ForeColor = Color.White;
            button.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            button.FlatAppearance.BorderColor = accent;
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(36, 138, 128);
            button.FlatAppearance.MouseDownBackColor = Color.FromArgb(36, 138, 128);
            button.MouseEnter += (s, e) => button.ForeColor = Color.White;
            button.MouseLeave += (s, e) => button.ForeColor = Color.White;
            button.AutoSize = true;
            button.AutoSizeMode = AutoSizeMode.GrowOnly;
        }

        //מקרא צבעים — רצועה ממורכזת מעל הטבלה, בתוך bandGrid של UITheme (כמו ב-TraineePanel).
        //Padding.Bottom נכלל בגובה הרצועה ויוצר את הרווח בין המקרא לטבלה.
        private void buildLegend()
        {
            Panel legendBand = new Panel();
            legendBand.Dock = DockStyle.Top;
            legendBand.Padding = new Padding(0, 0, 0, 10);
            legendBand.Height = 40 + legendBand.Padding.Bottom;
            legendBand.BackColor = Color.White;

            FlowLayoutPanel legend = new FlowLayoutPanel();
            legend.FlowDirection = FlowDirection.RightToLeft;
            legend.AutoSize = true;
            legend.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            legend.WrapContents = false;
            legend.Height = 40;

            addLegendItem(legend, ConfirmedColor, "אושר");
            addLegendItem(legend, CancelledColor, "בוטל");
            addLegendItem(legend, WaitlistedColor, "ממתין בתור");
            addLegendItem(legend, NoShowColor, "לא הגיע");
            addLegendItem(legend, FullColor, "מלא");
            addLegendItem(legend, AlmostFullColor, "כמעט מלא");

            legendBand.Controls.Add(legend);

            void centerLegend()
            {
                legend.Left = Math.Max(0, (legendBand.ClientSize.Width - legend.Width) / 2);
                legend.Top = Math.Max(0, (40 - legend.Height) / 2);
            }
            legendBand.Resize += (s, e) => centerLegend();
            legend.SizeChanged += (s, e) => centerLegend();

            Panel gridHost = Controls.Find("bandGrid", true).FirstOrDefault() as Panel;
            if (gridHost != null) gridHost.Controls.Add(legendBand);
            else Controls.Add(legendBand);

            centerLegend();
        }

        private static void addLegendItem(FlowLayoutPanel legend, Color color, string text)
        {
            Panel square = new Panel();
            square.Size = new Size(14, 14);
            square.BackColor = color;
            square.BorderStyle = BorderStyle.FixedSingle;
            square.Margin = new Padding(3, 12, 3, 3);

            Label label = new Label();
            label.AutoSize = true;
            label.Text = text;
            label.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label.Margin = new Padding(0, 10, 15, 3);

            legend.Controls.Add(square);
            legend.Controls.Add(label);
        }

        private static string buildSessionDisplay(TrainingSession ts)
        {
            return ts.getDate().ToString("dd/MM/yyyy") + " " + ts.getStartTime().ToString(@"hh\:mm") +
                " - " + ts.getWorkoutType().getName() + " (" + ts.getRoom().getRoomName() + ")";
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

            DataTable sessionTable = new DataTable();
            sessionTable.Columns.Add("id", typeof(int));
            sessionTable.Columns.Add("display", typeof(string));
            foreach (TrainingSession ts in Program.TrainingSessions)
                sessionTable.Rows.Add(ts.getTrainingSessionId(), buildSessionDisplay(ts));
            cboTrainingSession.DataSource = sessionTable;
            cboTrainingSession.DisplayMember = "display";
            cboTrainingSession.ValueMember = "id";
        }

        private void loadRegistrations()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("registration_id", typeof(int));
            dt.Columns.Add("trainee", typeof(string));
            dt.Columns.Add("session", typeof(string));
            dt.Columns.Add("registrationDate", typeof(DateTime));
            dt.Columns.Add("cancellationDate", typeof(DateTime));
            dt.Columns.Add("status", typeof(string));
            dt.Columns.Add("spots", typeof(string));
            dt.Columns.Add("freeSpots", typeof(int));   //עמודה מוסתרת — לצביעת תא המקומות הפנויים

            foreach (Registration r in Program.Registrations)
            {
                TrainingSession ts = r.getTrainingSession();
                int capacity = getSessionCapacity(ts);
                int taken = countTakenSpots(ts, capacity);

                dt.Rows.Add(r.getRegistrationId(), r.getTrainee().getFullName(), buildSessionDisplay(ts),
                    r.getRegistrationDate(), (object)r.getCancellationTimestamp() ?? DBNull.Value, r.getStatus().ToString(),
                    taken + "/" + capacity, capacity - taken);
            }

            dataGridView1.DataSource = dt;
            dataGridView1.Columns["freeSpots"].Visible = false;

            dataGridView1.Columns["registration_id"].HeaderText = "מזהה";
            dataGridView1.Columns["trainee"].HeaderText = "מתאמן";
            dataGridView1.Columns["session"].HeaderText = "אימון";
            dataGridView1.Columns["registrationDate"].HeaderText = "תאריך הרשמה";
            dataGridView1.Columns["cancellationDate"].HeaderText = "תאריך ביטול";
            dataGridView1.Columns["status"].HeaderText = "סטטוס";
            dataGridView1.Columns["spots"].HeaderText = "תפוסה";
            dataGridView1.Columns["registrationDate"].DefaultCellStyle.Format = "dd/MM/yyyy";
            dataGridView1.Columns["cancellationDate"].DefaultCellStyle.Format = "dd/MM/yyyy";
        }

        //קיבולת האימון — הנמוך מבין קיבולת החדר וקיבולת סוג האימון (כמו ב-WaitlistEntryPanel)
        private static int getSessionCapacity(TrainingSession ts)
        {
            return Math.Min(ts.getRoom().getMaxCapacity(), ts.getWorkoutType().getMaxCapacity());
        }

        //מקומות תפוסים — לפי availableSlots של האימון, אותו מקור ש-sp_register_to_workout בודק ומעדכן
        private static int countTakenSpots(TrainingSession ts, int capacity)
        {
            return Math.Max(0, capacity - ts.getAvailableSlots());
        }

        //צביעת שורות לפי סטטוס, ותא המקומות הפנויים לפי תפוסת האימון
        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

            if (dataGridView1.Columns[e.ColumnIndex].Name == "spots")
            {
                if (!(row.Cells["freeSpots"].Value is int free)) return;
                if (free <= 0) e.CellStyle.BackColor = FullColor;
                else if (free <= 2) e.CellStyle.BackColor = AlmostFullColor;
                else e.CellStyle.BackColor = Color.White;
                return;
            }

            object status = row.Cells["status"].Value;
            if (status == null) return;

            switch (status.ToString())
            {
                case "CONFIRMED":
                    e.CellStyle.BackColor = ConfirmedColor;
                    break;
                case "CANCELLED":
                    e.CellStyle.BackColor = CancelledColor;
                    e.CellStyle.ForeColor = Color.Gray;
                    break;
                case "WAITLISTED":
                    e.CellStyle.BackColor = WaitlistedColor;
                    break;
                case "NO_SHOW":
                    e.CellStyle.BackColor = NoShowColor;
                    break;
            }
        }

        private void clearFields()
        {
            if (cboTrainee.Items.Count > 0) cboTrainee.SelectedIndex = 0;
            if (cboTrainingSession.Items.Count > 0) cboTrainingSession.SelectedIndex = 0;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;  //לחיצה על כותרת — מתעלמים

            int id = int.Parse(dataGridView1.Rows[e.RowIndex].Cells["registration_id"].Value.ToString());
            selectedRegistration = Registration.seekRegistration(id);
            if (selectedRegistration == null) return;

            btnCancel.Enabled = true;
            btnStartClass.Enabled = true;
            btnMissClass.Enabled = true;
            btnFinishClass.Enabled = true;
        }

        private void clearSelection()
        {
            selectedRegistration = null;
            btnCancel.Enabled = false;
            btnStartClass.Enabled = false;
            btnMissClass.Enabled = false;
            btnFinishClass.Enabled = false;
        }

        // UC02 "Register to Workout" — the guarded flow (sp_register_to_workout /
        // Trainee.registerToWorkout). This is the only way to create a Registration
        // from this panel.
        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (cboTrainee.SelectedValue == null)
            {
                MessageBox.Show("יש לבחור מתאמן", "שגיאה", MessageBoxButtons.OK);
                return;
            }
            if (cboTrainingSession.SelectedValue == null)
            {
                MessageBox.Show("יש לבחור אימון", "שגיאה", MessageBoxButtons.OK);
                return;
            }

            Trainee trainee = Trainee.seekTrainee((int)cboTrainee.SelectedValue);
            int trainingSessionId = (int)cboTrainingSession.SelectedValue;

            string error = trainee.registerToWorkout(trainingSessionId);

            // Session full (SP error 51006) — offer the waitlist instead (UC02 «Extend» Join Waitlist).
            if (error == "האימון מלא, אין מקומות פנויים.")
            {
                offerWaitlist(trainee, TrainingSession.seekTrainingSession(trainingSessionId));
                return;
            }

            if (error != null)
            {
                MessageBox.Show(error, "שגיאה", MessageBoxButtons.OK);
                return;
            }

            MessageBox.Show("ההרשמה בוצעה בהצלחה", "הודעה", MessageBoxButtons.OK);
            clearFields();
            loadRegistrations();

            // The new registration is this trainee's highest-id row for the session.
            Registration created = Program.Registrations
                .Where(r => r.getTrainee() == trainee && r.getTrainingSession().getTrainingSessionId() == trainingSessionId)
                .OrderByDescending(r => r.getRegistrationId())
                .FirstOrDefault();
            if (created != null)
                flashRow(created.getRegistrationId());
        }

        // Select the row with this registration id, scroll it into view and
        // start the fade. Selection here is visual only - it does not change
        // selectedRegistration (that still happens only through CellClick).
        private void flashRow(int registrationId)
        {
            DataGridViewRow target = null;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["registration_id"].Value is int id && id == registrationId)
                {
                    target = row;
                    break;
                }
            }
            if (target == null) return;

            dataGridView1.ClearSelection();
            DataGridViewColumn firstVisible = dataGridView1.Columns.GetFirstColumn(DataGridViewElementStates.Visible);
            if (firstVisible != null)
                dataGridView1.CurrentCell = target.Cells[firstVisible.Index];   // also scrolls it into view
            target.Selected = true;

            stopFlash();
            flashRegistrationId = registrationId;
            flashProgress = 0;
            flashClock = System.Diagnostics.Stopwatch.StartNew();
            flashTimer = new Timer();
            flashTimer.Interval = 30;
            flashTimer.Tick += flashTimer_Tick;
            flashTimer.Start();
            dataGridView1.Invalidate();
        }

        private void flashTimer_Tick(object sender, EventArgs e)
        {
            flashProgress = Math.Min(1.0, flashClock.ElapsedMilliseconds / (double)FlashMs);
            if (flashProgress >= 1.0)
                stopFlash();
            else
                dataGridView1.Invalidate();
        }

        private void stopFlash()
        {
            if (flashTimer == null) return;
            flashTimer.Stop();
            flashTimer.Tick -= flashTimer_Tick;
            flashTimer.Dispose();
            flashTimer = null;

            // Turn the flash off and repaint: CellFormatting alone colours the row again.
            flashRegistrationId = -1;
            if (!dataGridView1.IsDisposed) dataGridView1.Invalidate();
        }

        // Runs after dataGridView1_CellFormatting: blends whatever colour it chose
        // (and the selection colour, since the row is selected) with FlashColor.
        private void flash_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (flashRegistrationId < 0 || e.RowIndex < 0) return;
            if (!(dataGridView1.Rows[e.RowIndex].Cells["registration_id"].Value is int id) || id != flashRegistrationId) return;

            double weight = 1 - flashProgress * flashProgress;   // lingers bright, then fades
            e.CellStyle.BackColor = blend(e.CellStyle.BackColor, FlashColor, weight);
            e.CellStyle.SelectionBackColor = blend(e.CellStyle.SelectionBackColor, FlashColor, weight);
        }

        private static Color blend(Color normal, Color flash, double weight)
        {
            return Color.FromArgb(
                (int)Math.Round(normal.R + (flash.R - normal.R) * weight),
                (int)Math.Round(normal.G + (flash.G - normal.G) * weight),
                (int)Math.Round(normal.B + (flash.B - normal.B) * weight));
        }

        private void offerWaitlist(Trainee trainee, TrainingSession session)
        {
            DialogResult answer = MessageBox.Show("האימון מלא. להצטרף לרשימת ההמתנה?", "אימון מלא",
                MessageBoxButtons.YesNo);
            if (answer != DialogResult.Yes) return;

            // Next position = highest WAITLISTED position in this session + 1.
            int maxPosition = 0;
            foreach (WaitlistEntry we in Program.WaitlistEntries)
            {
                if (we.getTrainingSession() != session || we.getStatus() != WaitlistStatus.WAITLISTED)
                    continue;

                if (we.getTrainee() == trainee)
                {
                    MessageBox.Show("המתאמן כבר נמצא ברשימת ההמתנה לאימון זה", "שגיאה", MessageBoxButtons.OK);
                    return;
                }
                if (we.getPosition() > maxPosition)
                    maxPosition = we.getPosition();
            }

            int position = maxPosition + 1;
            int id = WaitlistEntry.getNextWaitlistEntryId();
            new WaitlistEntry(id, trainee, session, position, DateTime.Now, WaitlistStatus.WAITLISTED, null, true);

            MessageBox.Show("נוספת לרשימת ההמתנה במקום " + position, "הודעה", MessageBoxButtons.OK);
        }

        // ====================================================================
        // State-transition buttons (docs/design/state-diagram.md — Registration
        // State Diagram). confirm() has no button here: nothing in this codebase
        // ever creates a Registration as PENDING (sp_register_to_workout always
        // inserts CONFIRMED directly), so a confirm button would never have a
        // row it could act on. purge() also has no button — it's time-triggered
        // (after 7 years), not user-triggered; see Registration.purge().
        // ====================================================================

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (selectedRegistration == null)
            {
                MessageBox.Show("יש לבחור רישום מהרשימה", "שגיאה", MessageBoxButtons.OK);
                return;
            }

            string reason = txtCancellationReason.Text.Trim();
            if (reason.Length == 0) reason = null;

            try
            {
                selectedRegistration.cancel(reason);
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

            MessageBox.Show("הרישום בוטל בהצלחה", "הודעה", MessageBoxButtons.OK);
            txtCancellationReason.Clear();
            clearSelection();
            loadRegistrations();
        }

        private void btnStartClass_Click(object sender, EventArgs e)
        {
            if (selectedRegistration == null)
            {
                MessageBox.Show("יש לבחור רישום מהרשימה", "שגיאה", MessageBoxButtons.OK);
                return;
            }

            try
            {
                selectedRegistration.startClass();
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

            MessageBox.Show("האימון סומן כפעיל בהצלחה", "הודעה", MessageBoxButtons.OK);
            clearSelection();
            loadRegistrations();
        }

        private void btnMissClass_Click(object sender, EventArgs e)
        {
            if (selectedRegistration == null)
            {
                MessageBox.Show("יש לבחור רישום מהרשימה", "שגיאה", MessageBoxButtons.OK);
                return;
            }

            try
            {
                selectedRegistration.missClass();
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

            MessageBox.Show("ההיעדרות סומנה בהצלחה", "הודעה", MessageBoxButtons.OK);
            clearSelection();
            loadRegistrations();
        }

        private void btnFinishClass_Click(object sender, EventArgs e)
        {
            if (selectedRegistration == null)
            {
                MessageBox.Show("יש לבחור רישום מהרשימה", "שגיאה", MessageBoxButtons.OK);
                return;
            }

            try
            {
                selectedRegistration.finishClass();
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

            MessageBox.Show("האימון סומן כהושלם בהצלחה", "הודעה", MessageBoxButtons.OK);
            clearSelection();
            loadRegistrations();
        }

        private async void btnAddToCalendar_Click(object sender, EventArgs e)
        {
            if (selectedRegistration == null)
            {
                MessageBox.Show("יש לבחור רישום מהרשימה", "שגיאה", MessageBoxButtons.OK);
                return;
            }

            TrainingSession ts = selectedRegistration.getTrainingSession();
            string workoutName = ts.getWorkoutType().getName();
            DateTime start = ts.getDate().Date + ts.getStartTime();

            btnAddToCalendar.Enabled = false;
            try
            {
                if (!GoogleCalendarService.IsInitialized)
                    await GoogleCalendarService.InitializeAsync();
                await GoogleCalendarService.AddWorkoutEventAsync(workoutName, start);
                MessageBox.Show("האימון נוסף ליומן Google בהצלחה", "הודעה", MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show("שגיאה בהוספה ליומן Google: " + ex.Message, "שגיאה", MessageBoxButtons.OK);
            }
            finally
            {
                btnAddToCalendar.Enabled = true;
            }
        }

        //תזכורת אימון במייל — רק לרישום מאושר
        private async void btnSendReminder_Click(object sender, EventArgs e)
        {
            if (selectedRegistration == null)
            {
                MessageBox.Show("יש לבחור רישום מהרשימה", "שגיאה", MessageBoxButtons.OK);
                return;
            }
            if (selectedRegistration.getStatus() != RegistrationStatus.CONFIRMED)
            {
                MessageBox.Show("ניתן לשלוח תזכורת רק לרישום בסטטוס מאושר", "שגיאה", MessageBoxButtons.OK);
                return;
            }

            Trainee trainee = selectedRegistration.getTrainee();
            TrainingSession ts = selectedRegistration.getTrainingSession();
            string body = EmailService.BuildHtmlBody(trainee.getFullName(),
                "רצינו להזכיר לך שנרשמת לאימון הקרוב בסטודיו:",
                new string[,]
                {
                    { "אימון", ts.getWorkoutType().getName() },
                    { "תאריך", ts.getDate().ToString("dd/MM/yyyy") },
                    { "שעה", ts.getStartTime().ToString(@"hh\:mm") + " - " + ts.getEndTime().ToString(@"hh\:mm") },
                    { "מיקום", ts.getRoom().getRoomName() + ", סטודיו Level UP" }
                },
                "נתראה באימון! אם לא תוכל/י להגיע, נא לבטל מראש.");

            btnSendReminder.Enabled = false;
            try
            {
                if (await EmailService.SendEmailAsync(trainee.getEmail(), "תזכורת אימון - Level UP Studio", body))
                    MessageBox.Show("התזכורת נשלחה בהצלחה אל " + trainee.getEmail(), "הודעה", MessageBoxButtons.OK);
            }
            finally
            {
                btnSendReminder.Enabled = true;
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            mainForm.showPanel(null);
        }
    }
}
