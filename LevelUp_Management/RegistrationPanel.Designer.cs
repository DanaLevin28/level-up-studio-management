namespace LevelUp_Management
{
    partial class RegistrationPanel
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
            this.btnRegister = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblCancellationReason = new System.Windows.Forms.Label();
            this.txtCancellationReason = new System.Windows.Forms.TextBox();
            this.btnStartClass = new System.Windows.Forms.Button();
            this.btnMissClass = new System.Windows.Forms.Button();
            this.btnFinishClass = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            //
            // lblTitle
            //
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(180, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "הרשמה לאימון";
            //
            // dataGridView1
            //
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(20, 55);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
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
            // btnRegister
            //
            this.btnRegister.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnRegister.Location = new System.Drawing.Point(20, 335);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(150, 40);
            this.btnRegister.TabIndex = 6;
            this.btnRegister.Text = "הרשמה לאימון";
            this.btnRegister.UseVisualStyleBackColor = true;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            //
            // btnBack
            //
            this.btnBack.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnBack.Location = new System.Drawing.Point(770, 335);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(110, 40);
            this.btnBack.TabIndex = 7;
            this.btnBack.Text = "חזרה";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            //
            // btnCancel
            //
            this.btnCancel.Enabled = false;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCancel.Location = new System.Drawing.Point(20, 390);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(105, 40);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "ביטול";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            //
            // lblCancellationReason
            //
            this.lblCancellationReason.AutoSize = true;
            this.lblCancellationReason.Location = new System.Drawing.Point(550, 397);
            this.lblCancellationReason.Name = "lblCancellationReason";
            this.lblCancellationReason.Size = new System.Drawing.Size(90, 20);
            this.lblCancellationReason.TabIndex = 12;
            this.lblCancellationReason.Text = "סיבת ביטול:";
            //
            // txtCancellationReason
            //
            this.txtCancellationReason.Location = new System.Drawing.Point(650, 394);
            this.txtCancellationReason.Name = "txtCancellationReason";
            this.txtCancellationReason.Size = new System.Drawing.Size(220, 23);
            this.txtCancellationReason.TabIndex = 13;
            //
            // btnStartClass
            //
            this.btnStartClass.Enabled = false;
            this.btnStartClass.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnStartClass.Location = new System.Drawing.Point(135, 390);
            this.btnStartClass.Name = "btnStartClass";
            this.btnStartClass.Size = new System.Drawing.Size(130, 40);
            this.btnStartClass.TabIndex = 9;
            this.btnStartClass.Text = "התחלת אימון";
            this.btnStartClass.UseVisualStyleBackColor = true;
            this.btnStartClass.Click += new System.EventHandler(this.btnStartClass_Click);
            //
            // btnMissClass
            //
            this.btnMissClass.Enabled = false;
            this.btnMissClass.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnMissClass.Location = new System.Drawing.Point(275, 390);
            this.btnMissClass.Name = "btnMissClass";
            this.btnMissClass.Size = new System.Drawing.Size(130, 40);
            this.btnMissClass.TabIndex = 10;
            this.btnMissClass.Text = "סימון היעדרות";
            this.btnMissClass.UseVisualStyleBackColor = true;
            this.btnMissClass.Click += new System.EventHandler(this.btnMissClass_Click);
            //
            // btnFinishClass
            //
            this.btnFinishClass.Enabled = false;
            this.btnFinishClass.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnFinishClass.Location = new System.Drawing.Point(415, 390);
            this.btnFinishClass.Name = "btnFinishClass";
            this.btnFinishClass.Size = new System.Drawing.Size(110, 40);
            this.btnFinishClass.TabIndex = 11;
            this.btnFinishClass.Text = "סיום אימון";
            this.btnFinishClass.UseVisualStyleBackColor = true;
            this.btnFinishClass.Click += new System.EventHandler(this.btnFinishClass_Click);
            //
            // RegistrationPanel
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnFinishClass);
            this.Controls.Add(this.btnMissClass);
            this.Controls.Add(this.btnStartClass);
            this.Controls.Add(this.txtCancellationReason);
            this.Controls.Add(this.lblCancellationReason);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnRegister);
            this.Controls.Add(this.cboTrainingSession);
            this.Controls.Add(this.lblTrainingSession);
            this.Controls.Add(this.cboTrainee);
            this.Controls.Add(this.lblTrainee);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.lblTitle);
            this.Name = "RegistrationPanel";
            this.Size = new System.Drawing.Size(900, 450);
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
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblCancellationReason;
        private System.Windows.Forms.TextBox txtCancellationReason;
        private System.Windows.Forms.Button btnStartClass;
        private System.Windows.Forms.Button btnMissClass;
        private System.Windows.Forms.Button btnFinishClass;
    }
}
