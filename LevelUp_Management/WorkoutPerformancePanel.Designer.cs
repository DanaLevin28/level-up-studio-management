namespace LevelUp_Management
{
    partial class WorkoutPerformancePanel
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) { components.Dispose(); }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.lblTrainee = new System.Windows.Forms.Label();
            this.cboTrainee = new System.Windows.Forms.ComboBox();
            this.lblTrainingSession = new System.Windows.Forms.Label();
            this.cboTrainingSession = new System.Windows.Forms.ComboBox();
            this.lblTrainer = new System.Windows.Forms.Label();
            this.cboTrainer = new System.Windows.Forms.ComboBox();
            this.lblDate = new System.Windows.Forms.Label();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.lblWeight = new System.Windows.Forms.Label();
            this.txtWeight = new System.Windows.Forms.TextBox();
            this.lblSets = new System.Windows.Forms.Label();
            this.txtSets = new System.Windows.Forms.TextBox();
            this.lblReps = new System.Windows.Forms.Label();
            this.txtReps = new System.Windows.Forms.TextBox();
            this.lblCalorieTarget = new System.Windows.Forms.Label();
            this.txtCalorieTarget = new System.Windows.Forms.TextBox();
            this.lblProteinTarget = new System.Windows.Forms.Label();
            this.txtProteinTarget = new System.Windows.Forms.TextBox();
            this.lblDailyStepGoal = new System.Windows.Forms.Label();
            this.txtDailyStepGoal = new System.Windows.Forms.TextBox();
            this.lblNotes = new System.Windows.Forms.Label();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            //
            // lblTitle
            //
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(200, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "תיעוד ביצועי אימון";
            //
            // dataGridView1
            //
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(20, 55);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(860, 200);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            //
            // lblTrainee
            //
            this.lblTrainee.AutoSize = true;
            this.lblTrainee.Location = new System.Drawing.Point(20, 273);
            this.lblTrainee.Name = "lblTrainee";
            this.lblTrainee.Size = new System.Drawing.Size(60, 20);
            this.lblTrainee.TabIndex = 2;
            this.lblTrainee.Text = "מתאמן";
            //
            // cboTrainee
            //
            this.cboTrainee.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTrainee.Location = new System.Drawing.Point(110, 270);
            this.cboTrainee.Name = "cboTrainee";
            this.cboTrainee.Size = new System.Drawing.Size(200, 23);
            this.cboTrainee.TabIndex = 3;
            //
            // lblTrainingSession
            //
            this.lblTrainingSession.AutoSize = true;
            this.lblTrainingSession.Location = new System.Drawing.Point(330, 273);
            this.lblTrainingSession.Name = "lblTrainingSession";
            this.lblTrainingSession.Size = new System.Drawing.Size(50, 20);
            this.lblTrainingSession.TabIndex = 4;
            this.lblTrainingSession.Text = "אימון";
            //
            // cboTrainingSession
            //
            this.cboTrainingSession.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTrainingSession.Location = new System.Drawing.Point(430, 270);
            this.cboTrainingSession.Name = "cboTrainingSession";
            this.cboTrainingSession.Size = new System.Drawing.Size(300, 23);
            this.cboTrainingSession.TabIndex = 5;
            //
            // lblTrainer
            //
            this.lblTrainer.AutoSize = true;
            this.lblTrainer.Location = new System.Drawing.Point(20, 308);
            this.lblTrainer.Name = "lblTrainer";
            this.lblTrainer.Size = new System.Drawing.Size(50, 20);
            this.lblTrainer.TabIndex = 6;
            this.lblTrainer.Text = "מאמן";
            //
            // cboTrainer
            //
            this.cboTrainer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTrainer.Location = new System.Drawing.Point(110, 305);
            this.cboTrainer.Name = "cboTrainer";
            this.cboTrainer.Size = new System.Drawing.Size(200, 23);
            this.cboTrainer.TabIndex = 7;
            //
            // lblDate
            //
            this.lblDate.AutoSize = true;
            this.lblDate.Location = new System.Drawing.Point(330, 308);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(55, 20);
            this.lblDate.TabIndex = 8;
            this.lblDate.Text = "תאריך";
            //
            // dtpDate
            //
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDate.Location = new System.Drawing.Point(430, 305);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(170, 23);
            this.dtpDate.TabIndex = 9;
            //
            // lblWeight
            //
            this.lblWeight.AutoSize = true;
            this.lblWeight.Location = new System.Drawing.Point(20, 343);
            this.lblWeight.Name = "lblWeight";
            this.lblWeight.Size = new System.Drawing.Size(50, 20);
            this.lblWeight.TabIndex = 10;
            this.lblWeight.Text = "משקל";
            //
            // txtWeight
            //
            this.txtWeight.Location = new System.Drawing.Point(110, 340);
            this.txtWeight.Name = "txtWeight";
            this.txtWeight.Size = new System.Drawing.Size(100, 23);
            this.txtWeight.TabIndex = 11;
            //
            // lblSets
            //
            this.lblSets.AutoSize = true;
            this.lblSets.Location = new System.Drawing.Point(330, 343);
            this.lblSets.Name = "lblSets";
            this.lblSets.Size = new System.Drawing.Size(40, 20);
            this.lblSets.TabIndex = 12;
            this.lblSets.Text = "סטים";
            //
            // txtSets
            //
            this.txtSets.Location = new System.Drawing.Point(430, 340);
            this.txtSets.Name = "txtSets";
            this.txtSets.Size = new System.Drawing.Size(100, 23);
            this.txtSets.TabIndex = 13;
            //
            // lblReps
            //
            this.lblReps.AutoSize = true;
            this.lblReps.Location = new System.Drawing.Point(20, 378);
            this.lblReps.Name = "lblReps";
            this.lblReps.Size = new System.Drawing.Size(55, 20);
            this.lblReps.TabIndex = 14;
            this.lblReps.Text = "חזרות";
            //
            // txtReps
            //
            this.txtReps.Location = new System.Drawing.Point(110, 375);
            this.txtReps.Name = "txtReps";
            this.txtReps.Size = new System.Drawing.Size(100, 23);
            this.txtReps.TabIndex = 15;
            //
            // lblCalorieTarget
            //
            this.lblCalorieTarget.AutoSize = true;
            this.lblCalorieTarget.Location = new System.Drawing.Point(330, 378);
            this.lblCalorieTarget.Name = "lblCalorieTarget";
            this.lblCalorieTarget.Size = new System.Drawing.Size(80, 20);
            this.lblCalorieTarget.TabIndex = 16;
            this.lblCalorieTarget.Text = "יעד קלוריות";
            //
            // txtCalorieTarget
            //
            this.txtCalorieTarget.Location = new System.Drawing.Point(430, 375);
            this.txtCalorieTarget.Name = "txtCalorieTarget";
            this.txtCalorieTarget.Size = new System.Drawing.Size(100, 23);
            this.txtCalorieTarget.TabIndex = 17;
            //
            // lblProteinTarget
            //
            this.lblProteinTarget.AutoSize = true;
            this.lblProteinTarget.Location = new System.Drawing.Point(20, 413);
            this.lblProteinTarget.Name = "lblProteinTarget";
            this.lblProteinTarget.Size = new System.Drawing.Size(70, 20);
            this.lblProteinTarget.TabIndex = 18;
            this.lblProteinTarget.Text = "יעד חלבון";
            //
            // txtProteinTarget
            //
            this.txtProteinTarget.Location = new System.Drawing.Point(110, 410);
            this.txtProteinTarget.Name = "txtProteinTarget";
            this.txtProteinTarget.Size = new System.Drawing.Size(100, 23);
            this.txtProteinTarget.TabIndex = 19;
            //
            // lblDailyStepGoal
            //
            this.lblDailyStepGoal.AutoSize = true;
            this.lblDailyStepGoal.Location = new System.Drawing.Point(330, 413);
            this.lblDailyStepGoal.Name = "lblDailyStepGoal";
            this.lblDailyStepGoal.Size = new System.Drawing.Size(70, 20);
            this.lblDailyStepGoal.TabIndex = 20;
            this.lblDailyStepGoal.Text = "יעד צעדים";
            //
            // txtDailyStepGoal
            //
            this.txtDailyStepGoal.Location = new System.Drawing.Point(430, 410);
            this.txtDailyStepGoal.Name = "txtDailyStepGoal";
            this.txtDailyStepGoal.Size = new System.Drawing.Size(100, 23);
            this.txtDailyStepGoal.TabIndex = 21;
            //
            // lblNotes
            //
            this.lblNotes.AutoSize = true;
            this.lblNotes.Location = new System.Drawing.Point(20, 448);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(50, 20);
            this.lblNotes.TabIndex = 22;
            this.lblNotes.Text = "הערות";
            //
            // txtNotes
            //
            this.txtNotes.Location = new System.Drawing.Point(110, 445);
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(620, 23);
            this.txtNotes.TabIndex = 23;
            //
            // btnSave
            //
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSave.Location = new System.Drawing.Point(20, 490);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(110, 40);
            this.btnSave.TabIndex = 24;
            this.btnSave.Text = "שמירה";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            //
            // btnUpdate
            //
            this.btnUpdate.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnUpdate.Location = new System.Drawing.Point(140, 490);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(110, 40);
            this.btnUpdate.TabIndex = 25;
            this.btnUpdate.Text = "עדכון";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            //
            // btnDelete
            //
            this.btnDelete.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnDelete.Location = new System.Drawing.Point(260, 490);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(110, 40);
            this.btnDelete.TabIndex = 26;
            this.btnDelete.Text = "מחיקה";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            //
            // btnBack
            //
            this.btnBack.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnBack.Location = new System.Drawing.Point(770, 490);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(110, 40);
            this.btnBack.TabIndex = 27;
            this.btnBack.Text = "חזרה";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            //
            // WorkoutPerformancePanel
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtNotes);
            this.Controls.Add(this.lblNotes);
            this.Controls.Add(this.txtDailyStepGoal);
            this.Controls.Add(this.lblDailyStepGoal);
            this.Controls.Add(this.txtProteinTarget);
            this.Controls.Add(this.lblProteinTarget);
            this.Controls.Add(this.txtCalorieTarget);
            this.Controls.Add(this.lblCalorieTarget);
            this.Controls.Add(this.txtReps);
            this.Controls.Add(this.lblReps);
            this.Controls.Add(this.txtSets);
            this.Controls.Add(this.lblSets);
            this.Controls.Add(this.txtWeight);
            this.Controls.Add(this.lblWeight);
            this.Controls.Add(this.dtpDate);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.cboTrainer);
            this.Controls.Add(this.lblTrainer);
            this.Controls.Add(this.cboTrainingSession);
            this.Controls.Add(this.lblTrainingSession);
            this.Controls.Add(this.cboTrainee);
            this.Controls.Add(this.lblTrainee);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.lblTitle);
            this.Name = "WorkoutPerformancePanel";
            this.Size = new System.Drawing.Size(900, 550);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label lblTrainee;
        private System.Windows.Forms.ComboBox cboTrainee;
        private System.Windows.Forms.Label lblTrainingSession;
        private System.Windows.Forms.ComboBox cboTrainingSession;
        private System.Windows.Forms.Label lblTrainer;
        private System.Windows.Forms.ComboBox cboTrainer;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Label lblWeight;
        private System.Windows.Forms.TextBox txtWeight;
        private System.Windows.Forms.Label lblSets;
        private System.Windows.Forms.TextBox txtSets;
        private System.Windows.Forms.Label lblReps;
        private System.Windows.Forms.TextBox txtReps;
        private System.Windows.Forms.Label lblCalorieTarget;
        private System.Windows.Forms.TextBox txtCalorieTarget;
        private System.Windows.Forms.Label lblProteinTarget;
        private System.Windows.Forms.TextBox txtProteinTarget;
        private System.Windows.Forms.Label lblDailyStepGoal;
        private System.Windows.Forms.TextBox txtDailyStepGoal;
        private System.Windows.Forms.Label lblNotes;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnBack;
    }
}
