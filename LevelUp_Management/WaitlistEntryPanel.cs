using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LevelUp_Management
{
    public partial class WaitlistEntryPanel : UserControl
    {
        private WaitlistEntry selectedWaitlistEntry;
        private Button btnPromote;

        private static readonly Color WaitlistedColor = Color.FromArgb(255, 252, 220);
        private static readonly Color ConfirmedColor = Color.FromArgb(220, 255, 220);
        private static readonly Color CancelledColor = Color.FromArgb(240, 240, 240);
        private static readonly Color NoShowColor = Color.FromArgb(255, 220, 220);
        private static readonly Color SpotOpenColor = Color.FromArgb(180, 255, 180);

        // Row flash after a successful promotion or join: the affected row starts
        // bright teal-green and fades back to its normal status colour over FlashMs.
        // Drawn in a second CellFormatting handler that runs after the status
        // colouring, so clearing flashEntryId restores it exactly.
        private static readonly Color FlashColor = Color.FromArgb(40, 220, 170);
        private const int FlashMs = 1200;
        private int flashEntryId = -1;
        private double flashProgress;
        private Timer flashTimer;
        private System.Diagnostics.Stopwatch flashClock;

        public WaitlistEntryPanel()
        {
            InitializeComponent();
            buildPromoteButton();
            buildLegend();
            dataGridView1.CellFormatting += dataGridView1_CellFormatting;
            dataGridView1.CellFormatting += flash_CellFormatting;
            Disposed += delegate { stopFlash(); };
            dataGridView1.HandleCreated += delegate { applySessionColumnWidth(); };
            UITheme.applyPanel(this, "תור ההמתנה לאימונים מלאים וקידום המתאמן הבא בתור");

            //applyPanel מעצב את כל כפתורי הפעולה — כפתור הקידום מקבל אחריו רקע טורקיז וטקסט לבן
            btnPromote.FlatStyle = FlatStyle.Flat;
            btnPromote.FlatAppearance.BorderSize = 0;
            btnPromote.UseVisualStyleBackColor = false;
            btnPromote.FlatAppearance.MouseOverBackColor = UITheme.AccentHover;
            btnPromote.FlatAppearance.MouseDownBackColor = UITheme.AccentHover;
            applyPromoteColors();

            //applyPanel הוסיף לכפתור מטפלי ריחוף שצובעים את הטקסט בטורקיז (כמו הרקע) —
            //המטפלים כאן רצים אחריהם ומחזירים טקסט לבן על רקע טורקיז בכל מצב
            btnPromote.MouseEnter += delegate { applyPromoteColors(); };
            btnPromote.MouseLeave += delegate { applyPromoteColors(); };
            btnPromote.MouseUp += delegate { applyPromoteColors(); };
            btnPromote.GotFocus += delegate { applyPromoteColors(); };
            btnPromote.LostFocus += delegate { applyPromoteColors(); };

            this.Load += WaitlistEntryPanel_Load;
            loadCombos();
            dtpJoinTimestamp.Value = DateTime.Now;
            chkHasExpiration.Checked = false;
            dtpExpirationTime.Enabled = false;
            loadWaitlistEntries();
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

            cboStatus.DataSource = Enum.GetValues(typeof(WaitlistStatus));
        }

        private static string buildSessionDisplay(TrainingSession ts)
        {
            return ts.getDate().ToString("dd/MM/yyyy") + " " + ts.getStartTime().ToString(@"hh\:mm") +
                " - " + ts.getWorkoutType().getName() + " (" + ts.getRoom().getRoomName() + ")";
        }

        //כפתור קידום לאימון — ממוקם בשורה של שמירה/עדכון/מחיקה כדי ש-applyPanel ישייך אותו לשורת הפעולות
        private void buildPromoteButton()
        {
            btnPromote = new Button();
            btnPromote.Name = "btnPromote";
            btnPromote.Text = "🔼 קידום לאימון";
            btnPromote.Font = btnDelete.Font;
            btnPromote.Size = new Size(btnDelete.Width + 40, btnDelete.Height);
            btnPromote.Location = new Point(btnDelete.Right + 10, btnDelete.Top);
            btnPromote.Click += btnPromote_Click;
            this.Controls.Add(btnPromote);
        }

        //מקרא צבעים — ממוקם מעל הטבלה כדי ש-applyPanel ישייך אותו לרצועה העליונה
        private void buildLegend()
        {
            FlowLayoutPanel legend = new FlowLayoutPanel();
            legend.Name = "flpLegend";
            legend.AutoSize = true;
            legend.WrapContents = false;
            legend.RightToLeft = RightToLeft.Yes;
            legend.BackColor = Color.Transparent;
            legend.Location = new Point(dataGridView1.Left, dataGridView1.Top - 30);

            addLegendItem(legend, SpotOpenColor, "ממתין — יש מקום פנוי");
            addLegendItem(legend, WaitlistedColor, "ממתין בתור");
            addLegendItem(legend, ConfirmedColor, "אושר");
            addLegendItem(legend, CancelledColor, "בוטל");
            addLegendItem(legend, NoShowColor, "לא הגיע");

            this.Controls.Add(legend);
        }

        private static void addLegendItem(FlowLayoutPanel legend, Color color, string text)
        {
            Panel square = new Panel();
            square.Size = new Size(14, 14);
            square.BackColor = color;
            square.BorderStyle = BorderStyle.FixedSingle;
            square.Margin = new Padding(3, 5, 3, 3);

            Label label = new Label();
            label.AutoSize = true;
            label.Text = text;
            label.Margin = new Padding(0, 3, 15, 3);

            legend.Controls.Add(square);
            legend.Controls.Add(label);
        }

        private void loadWaitlistEntries()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("waitlist_entry_id", typeof(int));
            dt.Columns.Add("trainee", typeof(string));
            dt.Columns.Add("session", typeof(string));
            dt.Columns.Add("position", typeof(int));
            dt.Columns.Add("joinTimestamp", typeof(DateTime));
            dt.Columns.Add("status", typeof(string));
            dt.Columns.Add("expirationTime", typeof(DateTime));
            dt.Columns.Add("spots", typeof(string));
            dt.Columns.Add("sessionStart", typeof(DateTime));   //עמודה מוסתרת — למיון לפי מועד האימון
            dt.Columns.Add("spotOpen", typeof(bool));           //עמודה מוסתרת — ממתין בתור לאימון שיש בו מקום פנוי

            foreach (WaitlistEntry we in Program.WaitlistEntries)
            {
                TrainingSession ts = we.getTrainingSession();
                int capacity = getSessionCapacity(ts);
                int taken = countTakenSpots(ts, capacity);
                bool spotOpen = we.getStatus() == WaitlistStatus.WAITLISTED && ts.getAvailableSlots() > 0;
                string spots = taken + "/" + capacity + (spotOpen ? "  🟢 יש מקום פנוי!" : "");

                dt.Rows.Add(we.getWaitlistEntryId(), we.getTrainee().getFullName(), buildSessionDisplay(ts),
                    we.getPosition(), we.getJoinTimestamp(), we.getStatus().ToString(), (object)we.getExpirationTimestamp() ?? DBNull.Value,
                    spots, ts.getDate().Date + ts.getStartTime(), spotOpen);
            }

            //האימון הקרוב ביותר ראשון, ובתוך כל אימון — לפי המיקום בתור
            dt.DefaultView.Sort = "sessionStart ASC, position ASC";
            dataGridView1.DataSource = dt.DefaultView;

            dataGridView1.Columns["sessionStart"].Visible = false;
            dataGridView1.Columns["spotOpen"].Visible = false;
            if (dataGridView1.IsHandleCreated) applySessionColumnWidth();

            dataGridView1.Columns["waitlist_entry_id"].HeaderText = "מזהה";
            dataGridView1.Columns["trainee"].HeaderText = "מתאמן";
            dataGridView1.Columns["session"].HeaderText = "אימון";
            dataGridView1.Columns["position"].HeaderText = "מיקום בתור";
            dataGridView1.Columns["joinTimestamp"].HeaderText = "הצטרפות לתור";
            dataGridView1.Columns["status"].HeaderText = "סטטוס";
            dataGridView1.Columns["expirationTime"].HeaderText = "תפוגת הצעה";
            dataGridView1.Columns["spots"].HeaderText = "תפוסה";
            dataGridView1.Columns["joinTimestamp"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            dataGridView1.Columns["expirationTime"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
        }

        //רוחב עמודת האימון — רק אחרי שהטבלה נוצרה על המסך; לפני כן WinForms זורק NullReferenceException בחישוב עמודות ה-Fill
        private void applySessionColumnWidth()
        {
            DataGridViewColumn session = dataGridView1.Columns["session"];
            if (session == null) return;
            session.MinimumWidth = 350;
            session.FillWeight = 250;
        }

        //צביעת שורות לפי סטטוס — ב-CellFormatting כדי שהצבע יישמר גם כשהשורות נוצרות מחדש
        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
            object status = row.Cells["status"].Value;
            if (status == null) return;

            //ממתין בתור לאימון שהתפנה בו מקום — ירוק בוהק, סימן שאפשר לקדם
            if (row.Cells["spotOpen"].Value is bool spotOpen && spotOpen)
            {
                e.CellStyle.BackColor = SpotOpenColor;
                return;
            }

            switch (status.ToString())
            {
                case "WAITLISTED":
                    e.CellStyle.BackColor = WaitlistedColor;
                    break;
                case "CONFIRMED":
                    e.CellStyle.BackColor = ConfirmedColor;
                    break;
                case "CANCELLED":
                    e.CellStyle.BackColor = CancelledColor;
                    e.CellStyle.ForeColor = Color.Gray;
                    break;
                case "NO_SHOW":
                    e.CellStyle.BackColor = NoShowColor;
                    break;
            }
        }

        //התראה חצי-אוטומטית: מתאמנים ראשונים בתור שממתינים לקידום
        private void WaitlistEntryPanel_Load(object sender, EventArgs e)
        {
            int count = Program.WaitlistEntries.Count(we => we.getStatus() == WaitlistStatus.WAITLISTED && we.getPosition() == 1);
            if (count > 0)
                MessageBox.Show("שים לב! יש " + count + " מתאמנים ראשונים בתור שממתינים לקידום", "הודעה", MessageBoxButtons.OK);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;  //לחיצה על כותרת — מתעלמים

            int id = int.Parse(dataGridView1.Rows[e.RowIndex].Cells["waitlist_entry_id"].Value.ToString());
            selectedWaitlistEntry = WaitlistEntry.seekWaitlistEntry(id);
            if (selectedWaitlistEntry == null) return;

            cboTrainee.SelectedValue = selectedWaitlistEntry.getTrainee().getTraineeId();
            cboTrainingSession.SelectedValue = selectedWaitlistEntry.getTrainingSession().getTrainingSessionId();
            txtPosition.Text = selectedWaitlistEntry.getPosition().ToString();
            dtpJoinTimestamp.Value = selectedWaitlistEntry.getJoinTimestamp();
            cboStatus.SelectedItem = selectedWaitlistEntry.getStatus();

            if (selectedWaitlistEntry.getExpirationTimestamp().HasValue)
            {
                chkHasExpiration.Checked = true;
                dtpExpirationTime.Value = selectedWaitlistEntry.getExpirationTimestamp().Value;
            }
            else
            {
                chkHasExpiration.Checked = false;
            }
        }

        private void chkHasExpiration_CheckedChanged(object sender, EventArgs e)
        {
            dtpExpirationTime.Enabled = chkHasExpiration.Checked;
        }

        private bool validateFields()
        {
            if (cboTrainee.SelectedValue == null)
            {
                MessageBox.Show("יש לבחור מתאמן", "שגיאה", MessageBoxButtons.OK);
                return false;
            }
            if (cboTrainingSession.SelectedValue == null)
            {
                MessageBox.Show("יש לבחור אימון", "שגיאה", MessageBoxButtons.OK);
                return false;
            }
            if (!int.TryParse(txtPosition.Text, out int position) || position <= 0)
            {
                MessageBox.Show("יש להזין מיקום בתור תקין", "שגיאה", MessageBoxButtons.OK);
                return false;
            }
            return true;
        }

        private void clearFields()
        {
            selectedWaitlistEntry = null;
            if (cboTrainee.Items.Count > 0) cboTrainee.SelectedIndex = 0;
            if (cboTrainingSession.Items.Count > 0) cboTrainingSession.SelectedIndex = 0;
            txtPosition.Clear();
            dtpJoinTimestamp.Value = DateTime.Now;
            chkHasExpiration.Checked = false;
            if (cboStatus.Items.Count > 0) cboStatus.SelectedIndex = 0;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!validateFields()) return;

            int id = WaitlistEntry.getNextWaitlistEntryId();
            Trainee trainee = Trainee.seekTrainee((int)cboTrainee.SelectedValue);
            TrainingSession session = TrainingSession.seekTrainingSession((int)cboTrainingSession.SelectedValue);
            int position = int.Parse(txtPosition.Text);
            DateTime? expirationTime = chkHasExpiration.Checked ? (DateTime?)dtpExpirationTime.Value : null;
            WaitlistStatus status = (WaitlistStatus)cboStatus.SelectedItem;

            WaitlistEntry we = new WaitlistEntry(id, trainee, session, position, dtpJoinTimestamp.Value, status, expirationTime, true);

            MessageBox.Show("הרשומה ברשימת ההמתנה נוצרה בהצלחה", "הודעה", MessageBoxButtons.OK);
            clearFields();
            loadWaitlistEntries();
            flashRow(we.getWaitlistEntryId());
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedWaitlistEntry == null)
            {
                MessageBox.Show("יש לבחור רשומה מהרשימה", "שגיאה", MessageBoxButtons.OK);
                return;
            }
            if (!validateFields()) return;

            selectedWaitlistEntry.setTrainee(Trainee.seekTrainee((int)cboTrainee.SelectedValue));
            selectedWaitlistEntry.setTrainingSession(TrainingSession.seekTrainingSession((int)cboTrainingSession.SelectedValue));
            selectedWaitlistEntry.setPosition(int.Parse(txtPosition.Text));
            selectedWaitlistEntry.setJoinTimestamp(dtpJoinTimestamp.Value);
            selectedWaitlistEntry.setStatus((WaitlistStatus)cboStatus.SelectedItem);
            selectedWaitlistEntry.setExpirationTimestamp(chkHasExpiration.Checked ? (DateTime?)dtpExpirationTime.Value : null);
            selectedWaitlistEntry.updateWaitlistEntry();

            MessageBox.Show("הרשומה עודכנה בהצלחה", "הודעה", MessageBoxButtons.OK);
            clearFields();
            loadWaitlistEntries();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedWaitlistEntry == null)
            {
                MessageBox.Show("יש לבחור רשומה מהרשימה", "שגיאה", MessageBoxButtons.OK);
                return;
            }

            selectedWaitlistEntry.deleteWaitlistEntry();

            MessageBox.Show("הרשומה נמחקה בהצלחה", "הודעה", MessageBoxButtons.OK);
            clearFields();
            loadWaitlistEntries();
        }

        private async void btnPromote_Click(object sender, EventArgs e)
        {
            if (selectedWaitlistEntry == null)
            {
                MessageBox.Show("יש לבחור רשומה מהרשימה", "שגיאה", MessageBoxButtons.OK);
                return;
            }
            if (selectedWaitlistEntry.getStatus() != WaitlistStatus.WAITLISTED)
            {
                MessageBox.Show("ניתן לקדם רק מתאמן בסטטוס ממתין בתור", "שגיאה", MessageBoxButtons.OK);
                return;
            }

            WaitlistEntry promoted = selectedWaitlistEntry;
            Trainee trainee = promoted.getTrainee();
            TrainingSession session = promoted.getTrainingSession();

            //בדיקת מנוי בתוקף — אותו כלל כמו ב-sp_register_to_workout: המנוי האחרון (endDate מאוחר ביותר)
            //חייב להיות ACTIVE, בתוקף ועם כניסות שנותרו
            if (!hasValidMembership(trainee))
            {
                MessageBox.Show("למתאמן אין מנוי בתוקף! לא ניתן לקדם", "שגיאה", MessageBoxButtons.OK);
                return;
            }

            //בדיקת מקום פנוי לפי availableSlots — אותו מקור ש-sp_register_to_workout בודק
            int capacity = getSessionCapacity(session);
            int taken = countTakenSpots(session, capacity);
            if (session.getAvailableSlots() <= 0)
            {
                MessageBox.Show("האימון מלא (" + taken + "/" + capacity + ")! לא ניתן לקדם", "שגיאה", MessageBoxButtons.OK);
                return;
            }

            string trainingName = session.getWorkoutType().getName();
            DialogResult answer = MessageBox.Show("לקדם את " + trainee.getFullName() + " לאימון " + trainingName + "? (תנוכה כניסה מהמנוי)",
                "קידום לאימון", MessageBoxButtons.YesNo);
            if (answer != DialogResult.Yes) return;

            //יצירת הרשמה + ניכוי כניסה מהמנוי + הורדת מקום פנוי — באותו מסלול של RegistrationPanel
            //(sp_register_to_workout, טרנזקציה אחת). ה-SP כבר מנכה את הכניסה, לכן אין ניכוי נוסף כאן.
            string error = trainee.registerToWorkout(session.getTrainingSessionId());
            if (error != null)
            {
                MessageBox.Show(error, "שגיאה", MessageBoxButtons.OK);
                return;
            }

            //עדכון רשומת התור
            int promotedPosition = promoted.getPosition();
            promoted.setStatus(WaitlistStatus.CONFIRMED);
            promoted.updateWaitlistEntry();

            //קידום שאר הממתינים לאותו אימון במקום אחד
            foreach (WaitlistEntry we in Program.WaitlistEntries)
            {
                if (we != promoted && we.getTrainingSession() == session &&
                    we.getStatus() == WaitlistStatus.WAITLISTED && we.getPosition() > promotedPosition)
                {
                    we.setPosition(we.getPosition() - 1);
                    we.updateWaitlistEntry();
                }
            }

            MessageBox.Show("המתאמן קודם בהצלחה! ✅ נוצרה הרשמה חדשה ונוכתה כניסה מהמנוי", "הודעה", MessageBoxButtons.OK);
            clearFields();
            loadWaitlistEntries();
            flashRow(promoted.getWaitlistEntryId());

            //הודעה למתאמן במייל — כשל בשליחה לא מבטל את הקידום שכבר נשמר
            string body = EmailService.BuildHtmlBody(trainee.getFullName(),
                "חדשות טובות! התפנה מקום באימון שחיכית לו ונרשמת אליו אוטומטית:",
                new string[,]
                {
                    { "אימון", trainingName },
                    { "תאריך", session.getDate().ToString("dd/MM/yyyy") },
                    { "שעה", session.getStartTime().ToString(@"hh\:mm") + " - " + session.getEndTime().ToString(@"hh\:mm") },
                    { "מיקום", session.getRoom().getRoomName() + ", סטודיו Level UP" }
                },
                "נתראה באימון! אם לא תוכל/י להגיע, נא לבטל מראש כדי לפנות את המקום לממתין הבא.");

            btnPromote.Enabled = false;
            try
            {
                if (await EmailService.SendEmailAsync(trainee.getEmail(), "התפנה מקום באימון! - Level UP Studio", body))
                    MessageBox.Show("נשלחה הודעה במייל אל " + trainee.getEmail(), "הודעה", MessageBoxButtons.OK);
            }
            finally
            {
                btnPromote.Enabled = true;
            }

            applyPromoteColors();
        }

        // Select the row with this entry id, scroll it into view and start the
        // fade. Selection here is visual only - it does not change
        // selectedWaitlistEntry (that still happens only through CellClick).
        private void flashRow(int waitlistEntryId)
        {
            DataGridViewRow target = null;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["waitlist_entry_id"].Value is int id && id == waitlistEntryId)
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
            flashEntryId = waitlistEntryId;
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
            flashEntryId = -1;
            if (!dataGridView1.IsDisposed) dataGridView1.Invalidate();
        }

        // Runs after dataGridView1_CellFormatting: blends whatever colour it chose
        // (and the selection colour, since the row is selected) with FlashColor.
        private void flash_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (flashEntryId < 0 || e.RowIndex < 0) return;
            if (!(dataGridView1.Rows[e.RowIndex].Cells["waitlist_entry_id"].Value is int id) || id != flashEntryId) return;

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

        //צבעי כפתור הקידום — רקע טורקיז וטקסט לבן
        private void applyPromoteColors()
        {
            btnPromote.BackColor = UITheme.Accent;
            btnPromote.ForeColor = Color.White;
        }

        //קיבולת האימון — הנמוך מבין קיבולת החדר וקיבולת סוג האימון (אימון לא יכול לחרוג מקיבולת החדר)
        private static int getSessionCapacity(TrainingSession ts)
        {
            return Math.Min(ts.getRoom().getMaxCapacity(), ts.getWorkoutType().getMaxCapacity());
        }

        //מקומות תפוסים — לפי availableSlots של האימון, אותו מקור ש-sp_register_to_workout בודק ומעדכן
        private static int countTakenSpots(TrainingSession ts, int capacity)
        {
            return Math.Max(0, capacity - ts.getAvailableSlots());
        }

        private static bool hasValidMembership(Trainee trainee)
        {
            Membership latest = Program.Memberships
                .Where(m => m.getTrainee() == trainee)
                .OrderByDescending(m => m.getEndDate())
                .FirstOrDefault();

            return latest != null && latest.getStatus() == MembershipStatus.ACTIVE &&
                latest.getEndDate() >= DateTime.Today && latest.getRemainingEntries() > 0;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            mainForm.showPanel(null);
        }
    }
}
