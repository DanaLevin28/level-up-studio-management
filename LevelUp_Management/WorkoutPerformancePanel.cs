using System;
using System.Data;
using System.Windows.Forms;

namespace LevelUp_Management
{
    public partial class WorkoutPerformancePanel : UserControl
    {
        private WorkoutPerformance selectedWorkoutPerformance;
        private Button btnSetGoal;

        public WorkoutPerformancePanel()
        {
            InitializeComponent();

            // Added in code (not the Designer), same pattern as
            // RegistrationPanel's btnAddToCalendar: placed on the action row
            // before applyPanel so UITheme sweeps it up and lays it out with
            // the other action buttons.
            btnSetGoal = new Button();
            btnSetGoal.Name = "btnSetGoal";
            btnSetGoal.Text = "🎯 הוסף יעד ל-Google Tasks";
            btnSetGoal.Size = new System.Drawing.Size(220, btnDelete.Height);
            btnSetGoal.Location = new System.Drawing.Point(btnDelete.Right + 20, btnDelete.Top);
            btnSetGoal.Click += btnSetGoal_Click;
            Controls.Add(btnSetGoal);

            UITheme.applyPanel(this, "תיעוד משקלים, סטים וחזרות עבור מתאמן באימון");

            // Filled teal button, same override RegistrationPanel applies to
            // btnAddToCalendar after applyPanel's default outline styling.
            System.Drawing.Color accent = System.Drawing.Color.FromArgb(45, 159, 147);
            btnSetGoal.FlatStyle = FlatStyle.Flat;
            btnSetGoal.BackColor = accent;
            btnSetGoal.ForeColor = System.Drawing.Color.White;
            btnSetGoal.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            btnSetGoal.FlatAppearance.BorderColor = accent;
            btnSetGoal.FlatAppearance.BorderSize = 1;
            btnSetGoal.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(36, 138, 128);
            btnSetGoal.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(36, 138, 128);
            btnSetGoal.MouseEnter += (s, e) => btnSetGoal.ForeColor = System.Drawing.Color.White;
            btnSetGoal.MouseLeave += (s, e) => btnSetGoal.ForeColor = System.Drawing.Color.White;
            btnSetGoal.AutoSize = true;
            btnSetGoal.AutoSizeMode = AutoSizeMode.GrowOnly;

            loadCombos();
            dtpDate.Value = DateTime.Today;
            loadWorkoutPerformances();
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

            DataTable trainerTable = new DataTable();
            trainerTable.Columns.Add("id", typeof(int));
            trainerTable.Columns.Add("display", typeof(string));
            foreach (Trainer tr in Program.Trainers)
                trainerTable.Rows.Add(tr.getTrainerId(), tr.getFullName() + " (" + tr.getTrainerId() + ")");
            cboTrainer.DataSource = trainerTable;
            cboTrainer.DisplayMember = "display";
            cboTrainer.ValueMember = "id";
        }

        private void loadWorkoutPerformances()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("workout_performance_id", typeof(int));
            dt.Columns.Add("trainee", typeof(string));
            dt.Columns.Add("session", typeof(string));
            dt.Columns.Add("trainer", typeof(string));
            dt.Columns.Add("date", typeof(DateTime));
            dt.Columns.Add("weight", typeof(decimal));
            dt.Columns.Add("sets", typeof(int));
            dt.Columns.Add("reps", typeof(int));
            dt.Columns.Add("calorieTarget", typeof(int));
            dt.Columns.Add("notes", typeof(string));

            foreach (WorkoutPerformance wp in Program.WorkoutPerformances)
            {
                dt.Rows.Add(wp.getWorkoutPerformanceId(), wp.getTrainee().getFullName(), buildSessionDisplay(wp.getTrainingSession()),
                    wp.getTrainer().getFullName(), wp.getDate(), wp.getWeight(), wp.getSets(), wp.getReps(),
                    wp.getCalorieTarget(), (object)wp.getNotes() ?? DBNull.Value);
            }

            dataGridView1.DataSource = dt;

            dataGridView1.Columns["workout_performance_id"].HeaderText = "מזהה";
            dataGridView1.Columns["trainee"].HeaderText = "מתאמן";
            dataGridView1.Columns["session"].HeaderText = "אימון";
            dataGridView1.Columns["trainer"].HeaderText = "מאמן";
            dataGridView1.Columns["date"].HeaderText = "תאריך";
            dataGridView1.Columns["weight"].HeaderText = "משקל";
            dataGridView1.Columns["sets"].HeaderText = "סטים";
            dataGridView1.Columns["reps"].HeaderText = "חזרות";
            dataGridView1.Columns["calorieTarget"].HeaderText = "יעד קלוריות";
            dataGridView1.Columns["notes"].HeaderText = "הערות";
            dataGridView1.Columns["date"].DefaultCellStyle.Format = "dd/MM/yyyy";
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;  //לחיצה על כותרת — מתעלמים

            int id = int.Parse(dataGridView1.Rows[e.RowIndex].Cells["workout_performance_id"].Value.ToString());
            selectedWorkoutPerformance = WorkoutPerformance.seekWorkoutPerformance(id);
            if (selectedWorkoutPerformance == null) return;

            cboTrainee.SelectedValue = selectedWorkoutPerformance.getTrainee().getTraineeId();
            cboTrainingSession.SelectedValue = selectedWorkoutPerformance.getTrainingSession().getTrainingSessionId();
            cboTrainer.SelectedValue = selectedWorkoutPerformance.getTrainer().getTrainerId();
            dtpDate.Value = selectedWorkoutPerformance.getDate();
            txtWeight.Text = selectedWorkoutPerformance.getWeight().ToString();
            txtSets.Text = selectedWorkoutPerformance.getSets().ToString();
            txtReps.Text = selectedWorkoutPerformance.getReps().ToString();
            txtCalorieTarget.Text = selectedWorkoutPerformance.getCalorieTarget().ToString();
            txtProteinTarget.Text = selectedWorkoutPerformance.getProteinTarget().ToString();
            txtDailyStepGoal.Text = selectedWorkoutPerformance.getDailyStepGoal().ToString();
            txtNotes.Text = selectedWorkoutPerformance.getNotes();
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
            if (cboTrainer.SelectedValue == null)
            {
                MessageBox.Show("יש לבחור מאמן", "שגיאה", MessageBoxButtons.OK);
                return false;
            }
            if (!decimal.TryParse(txtWeight.Text, out _))
            {
                MessageBox.Show("יש להזין משקל תקין", "שגיאה", MessageBoxButtons.OK);
                return false;
            }
            if (!int.TryParse(txtSets.Text, out _))
            {
                MessageBox.Show("יש להזין מספר סטים תקין", "שגיאה", MessageBoxButtons.OK);
                return false;
            }
            if (!int.TryParse(txtReps.Text, out _))
            {
                MessageBox.Show("יש להזין מספר חזרות תקין", "שגיאה", MessageBoxButtons.OK);
                return false;
            }
            if (!int.TryParse(txtCalorieTarget.Text, out _))
            {
                MessageBox.Show("יש להזין יעד קלוריות תקין", "שגיאה", MessageBoxButtons.OK);
                return false;
            }
            if (!int.TryParse(txtProteinTarget.Text, out _))
            {
                MessageBox.Show("יש להזין יעד חלבון תקין", "שגיאה", MessageBoxButtons.OK);
                return false;
            }
            if (!int.TryParse(txtDailyStepGoal.Text, out _))
            {
                MessageBox.Show("יש להזין יעד צעדים תקין", "שגיאה", MessageBoxButtons.OK);
                return false;
            }
            return true;
        }

        private void clearFields()
        {
            selectedWorkoutPerformance = null;
            if (cboTrainee.Items.Count > 0) cboTrainee.SelectedIndex = 0;
            if (cboTrainingSession.Items.Count > 0) cboTrainingSession.SelectedIndex = 0;
            if (cboTrainer.Items.Count > 0) cboTrainer.SelectedIndex = 0;
            dtpDate.Value = DateTime.Today;
            txtWeight.Clear();
            txtSets.Clear();
            txtReps.Clear();
            txtCalorieTarget.Clear();
            txtProteinTarget.Clear();
            txtDailyStepGoal.Clear();
            txtNotes.Clear();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!validateFields()) return;

            int id = WorkoutPerformance.getNextWorkoutPerformanceId();
            Trainee trainee = Trainee.seekTrainee((int)cboTrainee.SelectedValue);
            TrainingSession session = TrainingSession.seekTrainingSession((int)cboTrainingSession.SelectedValue);
            Trainer trainer = Trainer.seekTrainer((int)cboTrainer.SelectedValue);

            WorkoutPerformance wp = new WorkoutPerformance(id, trainee, session, trainer, dtpDate.Value.Date,
                decimal.Parse(txtWeight.Text), int.Parse(txtSets.Text), int.Parse(txtReps.Text),
                int.Parse(txtCalorieTarget.Text), int.Parse(txtProteinTarget.Text), int.Parse(txtDailyStepGoal.Text),
                txtNotes.Text, true);

            MessageBox.Show("ביצועי האימון תועדו בהצלחה", "הודעה", MessageBoxButtons.OK);
            clearFields();
            loadWorkoutPerformances();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedWorkoutPerformance == null)
            {
                MessageBox.Show("יש לבחור רשומה מהרשימה", "שגיאה", MessageBoxButtons.OK);
                return;
            }
            if (!validateFields()) return;

            selectedWorkoutPerformance.setTrainee(Trainee.seekTrainee((int)cboTrainee.SelectedValue));
            selectedWorkoutPerformance.setTrainingSession(TrainingSession.seekTrainingSession((int)cboTrainingSession.SelectedValue));
            selectedWorkoutPerformance.setTrainer(Trainer.seekTrainer((int)cboTrainer.SelectedValue));
            selectedWorkoutPerformance.setDate(dtpDate.Value.Date);
            selectedWorkoutPerformance.setWeight(decimal.Parse(txtWeight.Text));
            selectedWorkoutPerformance.setSets(int.Parse(txtSets.Text));
            selectedWorkoutPerformance.setReps(int.Parse(txtReps.Text));
            selectedWorkoutPerformance.setCalorieTarget(int.Parse(txtCalorieTarget.Text));
            selectedWorkoutPerformance.setProteinTarget(int.Parse(txtProteinTarget.Text));
            selectedWorkoutPerformance.setDailyStepGoal(int.Parse(txtDailyStepGoal.Text));
            selectedWorkoutPerformance.setNotes(txtNotes.Text);
            selectedWorkoutPerformance.updateWorkoutPerformance();

            MessageBox.Show("ביצועי האימון עודכנו בהצלחה", "הודעה", MessageBoxButtons.OK);
            clearFields();
            loadWorkoutPerformances();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedWorkoutPerformance == null)
            {
                MessageBox.Show("יש לבחור רשומה מהרשימה", "שגיאה", MessageBoxButtons.OK);
                return;
            }

            selectedWorkoutPerformance.deleteWorkoutPerformance();

            MessageBox.Show("הרשומה נמחקה בהצלחה", "הודעה", MessageBoxButtons.OK);
            clearFields();
            loadWorkoutPerformances();
        }

        private async void btnSetGoal_Click(object sender, EventArgs e)
        {
            if (selectedWorkoutPerformance == null)
            {
                MessageBox.Show("יש לבחור רשומת ביצוע מהרשימה", "שגיאה", MessageBoxButtons.OK);
                return;
            }

            string goalType = showGoalTypeDialog();
            if (goalType == null) return;  // הדיאלוג נסגר ללא בחירה — לא יוצרים כלום

            WorkoutPerformance wp = selectedWorkoutPerformance;
            string traineeName = wp.getTrainee().getFullName();
            DateTime dueDate = wp.getDate().AddDays(30);
            string title, notes;

            switch (goalType)
            {
                case "weight":
                    decimal targetWeight = wp.getWeight() > 0 ? Math.Round(wp.getWeight() * 1.1m, 1) : 50m;
                    title = "🏋️ יעד משקל - " + traineeName;
                    notes = string.Format("מתאמן/ת: {0}\nמשקל יעד: {1} ק\"ג\n\nLevel UP Studio",
                        traineeName, targetWeight);
                    break;
                case "calories":
                    int targetCalories = wp.getCalorieTarget() > 0 ? wp.getCalorieTarget() + 100 : 500;
                    title = "🔥 יעד קלוריות - " + traineeName;
                    notes = string.Format("מתאמן/ת: {0}\nיעד קלוריות: {1}\n\nLevel UP Studio",
                        traineeName, targetCalories);
                    break;
                case "reps":
                    int targetReps = wp.getReps() > 0 ? wp.getReps() + 5 : 20;
                    title = "💪 יעד חזרות - " + traineeName;
                    notes = string.Format("מתאמן/ת: {0}\nחזרות יעד: {1}\n\nLevel UP Studio",
                        traineeName, targetReps);
                    break;
                case "frequency":
                    title = "📈 יעד שבועי - " + traineeName;
                    notes = string.Format("מתאמן/ת: {0}\nיעד שבועי: 4 אימונים\n\nLevel UP Studio", traineeName);
                    break;
                default:
                    return;
            }

            btnSetGoal.Enabled = false;
            try
            {
                if (!GoogleTasksService.IsInitialized)
                    await GoogleTasksService.InitializeAsync();
                await GoogleTasksService.AddGoalAsync(title, notes, dueDate);
                MessageBox.Show("היעד נוסף בהצלחה לרשימת \"Level UP - יעדים\" ב-Google Tasks", "הודעה", MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show("שגיאה בהוספת היעד ל-Google Tasks: " + ex.Message, "שגיאה", MessageBoxButtons.OK);
            }
            finally
            {
                btnSetGoal.Enabled = true;
            }
        }

        // Explicit, one-off exception to CLAUDE.md's "no additional Forms or
        // dialogs" rule - see the note under "Decisions Already Made (this
        // project)". Scoped narrowly to this single goal-type picker for the
        // bonus Google Tasks feature; returns null (and creates nothing) if
        // the dialog is closed without a choice.
        private string showGoalTypeDialog()
        {
            string chosen = null;

            (string text, string type)[] options =
            {
                ("🏋️ יעד משקל", "weight"),
                ("🔥 יעד קלוריות", "calories"),
                ("💪 יעד חזרות", "reps"),
                ("📈 יעד שבועי", "frequency"),
            };

            using (Form dlg = new Form())
            {
                dlg.Text = "בחר סוג יעד";
                dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.MaximizeBox = false;
                dlg.MinimizeBox = false;
                dlg.ShowInTaskbar = false;
                dlg.RightToLeft = RightToLeft.Yes;
                dlg.RightToLeftLayout = true;
                dlg.BackColor = UITheme.Card;
                dlg.ClientSize = new System.Drawing.Size(300, 250);

                Label lbl = new Label();
                lbl.Text = "איזה יעד להגדיר?";
                lbl.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
                lbl.ForeColor = UITheme.TextPrimary;
                lbl.AutoSize = true;
                lbl.Location = new System.Drawing.Point(20, 20);
                dlg.Controls.Add(lbl);

                int y = 55;
                foreach (var opt in options)
                {
                    Button btn = new Button();
                    btn.Text = opt.text;
                    btn.Size = new System.Drawing.Size(260, 40);
                    btn.Location = new System.Drawing.Point(20, y);
                    UITheme.styleOutlineButton(btn, 1f);
                    string capturedType = opt.type;
                    btn.Click += (s, e) => { chosen = capturedType; dlg.Close(); };
                    dlg.Controls.Add(btn);
                    y += 48;
                }

                dlg.ShowDialog(this);
            }

            return chosen;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            mainForm.showPanel(null);
        }
    }
}
