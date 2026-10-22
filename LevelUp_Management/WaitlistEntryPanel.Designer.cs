namespace LevelUp_Management
{
    partial class WaitlistEntryPanel
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
            this.lblPosition = new System.Windows.Forms.Label();
            this.txtPosition = new System.Windows.Forms.TextBox();
            this.lblJoinTimestamp = new System.Windows.Forms.Label();
            this.dtpJoinTimestamp = new System.Windows.Forms.DateTimePicker();
            this.lblStatus = new System.Windows.Forms.Label();
            this.cboStatus = new System.Windows.Forms.ComboBox();
            this.chkHasExpiration = new System.Windows.Forms.CheckBox();
            this.lblExpirationTime = new System.Windows.Forms.Label();
            this.dtpExpirationTime = new System.Windows.Forms.DateTimePicker();
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
            this.lblTitle.Size = new System.Drawing.Size(220, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "ניהול רשימת המתנה";
            //
            // dataGridView1
            //
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(20, 55);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(860, 220);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            //
            // lblTrainee
            //
            this.lblTrainee.AutoSize = true;
            this.lblTrainee.Location = new System.Drawing.Point(20, 293);
            this.lblTrainee.Name = "lblTrainee";
            this.lblTrainee.Size = new System.Drawing.Size(60, 20);
            this.lblTrainee.TabIndex = 2;
            this.lblTrainee.Text = "מתאמן";
            //
            // cboTrainee
            //
            this.cboTrainee.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTrainee.Location = new System.Drawing.Point(110, 290);
            this.cboTrainee.Name = "cboTrainee";
            this.cboTrainee.Size = new System.Drawing.Size(200, 23);
            this.cboTrainee.TabIndex = 3;
            //
            // lblTrainingSession
            //
            this.lblTrainingSession.AutoSize = true;
            this.lblTrainingSession.Location = new System.Drawing.Point(330, 293);
            this.lblTrainingSession.Name = "lblTrainingSession";
            this.lblTrainingSession.Size = new System.Drawing.Size(50, 20);
            this.lblTrainingSession.TabIndex = 4;
            this.lblTrainingSession.Text = "אימון";
            //
            // cboTrainingSession
            //
            this.cboTrainingSession.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTrainingSession.Location = new System.Drawing.Point(430, 290);
            this.cboTrainingSession.Name = "cboTrainingSession";
            this.cboTrainingSession.Size = new System.Drawing.Size(300, 23);
            this.cboTrainingSession.TabIndex = 5;
            //
            // lblPosition
            //
            this.lblPosition.AutoSize = true;
            this.lblPosition.Location = new System.Drawing.Point(20, 328);
            this.lblPosition.Name = "lblPosition";
            this.lblPosition.Size = new System.Drawing.Size(80, 20);
            this.lblPosition.TabIndex = 6;
            this.lblPosition.Text = "מיקום בתור";
            //
            // txtPosition
            //
            this.txtPosition.Location = new System.Drawing.Point(140, 325);
            this.txtPosition.Name = "txtPosition";
            this.txtPosition.Size = new System.Drawing.Size(80, 23);
            this.txtPosition.TabIndex = 7;
            //
            // lblJoinTimestamp
            //
            this.lblJoinTimestamp.AutoSize = true;
            this.lblJoinTimestamp.Location = new System.Drawing.Point(330, 328);
            this.lblJoinTimestamp.Name = "lblJoinTimestamp";
            this.lblJoinTimestamp.Size = new System.Drawing.Size(100, 20);
            this.lblJoinTimestamp.TabIndex = 8;
            this.lblJoinTimestamp.Text = "הצטרפות לתור";
            //
            // dtpJoinTimestamp
            //
            this.dtpJoinTimestamp.CustomFormat = "dd/MM/yyyy HH:mm";
            this.dtpJoinTimestamp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpJoinTimestamp.Location = new System.Drawing.Point(440, 325);
            this.dtpJoinTimestamp.Name = "dtpJoinTimestamp";
            this.dtpJoinTimestamp.Size = new System.Drawing.Size(170, 23);
            this.dtpJoinTimestamp.TabIndex = 9;
            //
            // lblStatus
            //
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(20, 363);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(50, 20);
            this.lblStatus.TabIndex = 10;
            this.lblStatus.Text = "סטטוס";
            //
            // cboStatus
            //
            this.cboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStatus.Location = new System.Drawing.Point(140, 360);
            this.cboStatus.Name = "cboStatus";
            this.cboStatus.Size = new System.Drawing.Size(150, 23);
            this.cboStatus.TabIndex = 11;
            //
            // chkHasExpiration
            //
            this.chkHasExpiration.AutoSize = true;
            this.chkHasExpiration.Location = new System.Drawing.Point(330, 363);
            this.chkHasExpiration.Name = "chkHasExpiration";
            this.chkHasExpiration.Size = new System.Drawing.Size(100, 24);
            this.chkHasExpiration.TabIndex = 12;
            this.chkHasExpiration.Text = "הצעה פעילה";
            this.chkHasExpiration.UseVisualStyleBackColor = true;
            this.chkHasExpiration.CheckedChanged += new System.EventHandler(this.chkHasExpiration_CheckedChanged);
            //
            // lblExpirationTime
            //
            this.lblExpirationTime.AutoSize = true;
            this.lblExpirationTime.Location = new System.Drawing.Point(440, 363);
            this.lblExpirationTime.Name = "lblExpirationTime";
            this.lblExpirationTime.Size = new System.Drawing.Size(90, 20);
            this.lblExpirationTime.TabIndex = 13;
            this.lblExpirationTime.Text = "תפוגת הצעה";
            //
            // dtpExpirationTime
            //
            this.dtpExpirationTime.CustomFormat = "dd/MM/yyyy HH:mm";
            this.dtpExpirationTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpExpirationTime.Location = new System.Drawing.Point(540, 360);
            this.dtpExpirationTime.Name = "dtpExpirationTime";
            this.dtpExpirationTime.Size = new System.Drawing.Size(170, 23);
            this.dtpExpirationTime.TabIndex = 14;
            //
            // btnSave
            //
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSave.Location = new System.Drawing.Point(20, 440);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(110, 40);
            this.btnSave.TabIndex = 15;
            this.btnSave.Text = "שמירה";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            //
            // btnUpdate
            //
            this.btnUpdate.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnUpdate.Location = new System.Drawing.Point(140, 440);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(110, 40);
            this.btnUpdate.TabIndex = 16;
            this.btnUpdate.Text = "עדכון";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            //
            // btnDelete
            //
            this.btnDelete.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnDelete.Location = new System.Drawing.Point(260, 440);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(110, 40);
            this.btnDelete.TabIndex = 17;
            this.btnDelete.Text = "מחיקה";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            //
            // btnBack
            //
            this.btnBack.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnBack.Location = new System.Drawing.Point(770, 440);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(110, 40);
            this.btnBack.TabIndex = 18;
            this.btnBack.Text = "חזרה";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            //
            // WaitlistEntryPanel
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.dtpExpirationTime);
            this.Controls.Add(this.lblExpirationTime);
            this.Controls.Add(this.chkHasExpiration);
            this.Controls.Add(this.cboStatus);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.dtpJoinTimestamp);
            this.Controls.Add(this.lblJoinTimestamp);
            this.Controls.Add(this.txtPosition);
            this.Controls.Add(this.lblPosition);
            this.Controls.Add(this.cboTrainingSession);
            this.Controls.Add(this.lblTrainingSession);
            this.Controls.Add(this.cboTrainee);
            this.Controls.Add(this.lblTrainee);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.lblTitle);
            this.Name = "WaitlistEntryPanel";
            this.Size = new System.Drawing.Size(900, 500);
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
        private System.Windows.Forms.Label lblPosition;
        private System.Windows.Forms.TextBox txtPosition;
        private System.Windows.Forms.Label lblJoinTimestamp;
        private System.Windows.Forms.DateTimePicker dtpJoinTimestamp;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox cboStatus;
        private System.Windows.Forms.CheckBox chkHasExpiration;
        private System.Windows.Forms.Label lblExpirationTime;
        private System.Windows.Forms.DateTimePicker dtpExpirationTime;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnBack;
    }
}
