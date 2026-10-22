namespace LevelUp_Management
{
    partial class MembershipPanel
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
            this.lblPurchaseDate = new System.Windows.Forms.Label();
            this.dtpPurchaseDate = new System.Windows.Forms.DateTimePicker();
            this.lblPlanName = new System.Windows.Forms.Label();
            this.txtPlanName = new System.Windows.Forms.TextBox();
            this.lblPlanType = new System.Windows.Forms.Label();
            this.txtPlanType = new System.Windows.Forms.TextBox();
            this.cboPlan = new System.Windows.Forms.ComboBox();
            this.lblPrice = new System.Windows.Forms.Label();
            this.txtPrice = new System.Windows.Forms.TextBox();
            this.lblAllowedEntries = new System.Windows.Forms.Label();
            this.txtAllowedEntries = new System.Windows.Forms.TextBox();
            this.lblValidityDays = new System.Windows.Forms.Label();
            this.txtValidityDays = new System.Windows.Forms.TextBox();
            this.lblStartDate = new System.Windows.Forms.Label();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.lblEndDate = new System.Windows.Forms.Label();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.lblRemainingEntries = new System.Windows.Forms.Label();
            this.txtRemainingEntries = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.cboStatus = new System.Windows.Forms.ComboBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnRenew = new System.Windows.Forms.Button();
            this.btnPayments = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnFreeze = new System.Windows.Forms.Button();
            this.btnUnfreeze = new System.Windows.Forms.Button();
            this.btnSuspend = new System.Windows.Forms.Button();
            this.btnExpire = new System.Windows.Forms.Button();
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
            this.lblTitle.Text = "חידוש ותשלום מנוי";
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
            // lblPurchaseDate
            //
            this.lblPurchaseDate.AutoSize = true;
            this.lblPurchaseDate.Location = new System.Drawing.Point(330, 293);
            this.lblPurchaseDate.Name = "lblPurchaseDate";
            this.lblPurchaseDate.Size = new System.Drawing.Size(90, 20);
            this.lblPurchaseDate.TabIndex = 4;
            this.lblPurchaseDate.Text = "תאריך רכישה";
            //
            // dtpPurchaseDate
            //
            this.dtpPurchaseDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpPurchaseDate.Location = new System.Drawing.Point(430, 290);
            this.dtpPurchaseDate.Name = "dtpPurchaseDate";
            this.dtpPurchaseDate.Size = new System.Drawing.Size(170, 23);
            this.dtpPurchaseDate.TabIndex = 5;
            //
            // lblPlanName
            //
            this.lblPlanName.AutoSize = true;
            this.lblPlanName.Location = new System.Drawing.Point(20, 328);
            this.lblPlanName.Name = "lblPlanName";
            this.lblPlanName.Size = new System.Drawing.Size(80, 20);
            this.lblPlanName.TabIndex = 6;
            this.lblPlanName.Text = "מסלול מנוי";
            //
            // txtPlanName
            //
            this.txtPlanName.Location = new System.Drawing.Point(110, 325);
            this.txtPlanName.Name = "txtPlanName";
            this.txtPlanName.Size = new System.Drawing.Size(200, 23);
            this.txtPlanName.TabIndex = 7;
            this.txtPlanName.Visible = false;
            //
            // lblPlanType
            //
            this.lblPlanType.AutoSize = true;
            this.lblPlanType.Location = new System.Drawing.Point(330, 328);
            this.lblPlanType.Name = "lblPlanType";
            this.lblPlanType.Size = new System.Drawing.Size(80, 20);
            this.lblPlanType.TabIndex = 8;
            this.lblPlanType.Text = "סוג מסלול";
            this.lblPlanType.Visible = false;
            //
            // txtPlanType
            //
            this.txtPlanType.Location = new System.Drawing.Point(430, 325);
            this.txtPlanType.Name = "txtPlanType";
            this.txtPlanType.Size = new System.Drawing.Size(220, 23);
            this.txtPlanType.TabIndex = 9;
            this.txtPlanType.Visible = false;
            //
            // cboPlan
            //
            this.cboPlan.BackColor = System.Drawing.Color.White;
            this.cboPlan.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.cboPlan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPlan.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboPlan.Location = new System.Drawing.Point(110, 325);
            this.cboPlan.Name = "cboPlan";
            this.cboPlan.Size = new System.Drawing.Size(540, 25);
            this.cboPlan.TabIndex = 7;
            this.cboPlan.SelectedIndexChanged += new System.EventHandler(this.cboPlan_SelectedIndexChanged);
            //
            // lblPrice
            //
            this.lblPrice.AutoSize = true;
            this.lblPrice.Location = new System.Drawing.Point(20, 363);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(40, 20);
            this.lblPrice.TabIndex = 10;
            this.lblPrice.Text = "מחיר";
            //
            // txtPrice
            //
            this.txtPrice.BackColor = System.Drawing.Color.FromArgb(245, 247, 246);
            this.txtPrice.Location = new System.Drawing.Point(110, 360);
            this.txtPrice.Name = "txtPrice";
            this.txtPrice.ReadOnly = true;
            this.txtPrice.Size = new System.Drawing.Size(100, 23);
            this.txtPrice.TabIndex = 11;
            //
            // lblAllowedEntries
            //
            this.lblAllowedEntries.AutoSize = true;
            this.lblAllowedEntries.Location = new System.Drawing.Point(330, 363);
            this.lblAllowedEntries.Name = "lblAllowedEntries";
            this.lblAllowedEntries.Size = new System.Drawing.Size(90, 20);
            this.lblAllowedEntries.TabIndex = 12;
            this.lblAllowedEntries.Text = "כניסות מותרות";
            //
            // txtAllowedEntries
            //
            this.txtAllowedEntries.BackColor = System.Drawing.Color.FromArgb(245, 247, 246);
            this.txtAllowedEntries.Location = new System.Drawing.Point(430, 360);
            this.txtAllowedEntries.Name = "txtAllowedEntries";
            this.txtAllowedEntries.ReadOnly = true;
            this.txtAllowedEntries.Size = new System.Drawing.Size(100, 23);
            this.txtAllowedEntries.TabIndex = 13;
            //
            // lblValidityDays
            //
            this.lblValidityDays.AutoSize = true;
            this.lblValidityDays.Location = new System.Drawing.Point(20, 398);
            this.lblValidityDays.Name = "lblValidityDays";
            this.lblValidityDays.Size = new System.Drawing.Size(80, 20);
            this.lblValidityDays.TabIndex = 14;
            this.lblValidityDays.Text = "ימי תוקף";
            //
            // txtValidityDays
            //
            this.txtValidityDays.BackColor = System.Drawing.Color.FromArgb(245, 247, 246);
            this.txtValidityDays.Location = new System.Drawing.Point(110, 395);
            this.txtValidityDays.Name = "txtValidityDays";
            this.txtValidityDays.ReadOnly = true;
            this.txtValidityDays.Size = new System.Drawing.Size(100, 23);
            this.txtValidityDays.TabIndex = 15;
            //
            // lblStartDate
            //
            this.lblStartDate.AutoSize = true;
            this.lblStartDate.Location = new System.Drawing.Point(330, 398);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(90, 20);
            this.lblStartDate.TabIndex = 16;
            this.lblStartDate.Text = "תאריך התחלה";
            //
            // dtpStartDate
            //
            this.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpStartDate.Location = new System.Drawing.Point(430, 395);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(170, 23);
            this.dtpStartDate.TabIndex = 17;
            //
            // lblEndDate
            //
            this.lblEndDate.AutoSize = true;
            this.lblEndDate.Location = new System.Drawing.Point(20, 433);
            this.lblEndDate.Name = "lblEndDate";
            this.lblEndDate.Size = new System.Drawing.Size(80, 20);
            this.lblEndDate.TabIndex = 18;
            this.lblEndDate.Text = "תאריך סיום";
            //
            // dtpEndDate
            //
            this.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEndDate.Location = new System.Drawing.Point(110, 430);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(170, 23);
            this.dtpEndDate.TabIndex = 19;
            //
            // lblRemainingEntries
            //
            this.lblRemainingEntries.AutoSize = true;
            this.lblRemainingEntries.Location = new System.Drawing.Point(330, 433);
            this.lblRemainingEntries.Name = "lblRemainingEntries";
            this.lblRemainingEntries.Size = new System.Drawing.Size(100, 20);
            this.lblRemainingEntries.TabIndex = 20;
            this.lblRemainingEntries.Text = "כניסות שנותרו";
            this.lblRemainingEntries.Visible = false;
            //
            // txtRemainingEntries
            //
            this.txtRemainingEntries.Location = new System.Drawing.Point(430, 430);
            this.txtRemainingEntries.Name = "txtRemainingEntries";
            this.txtRemainingEntries.Size = new System.Drawing.Size(100, 23);
            this.txtRemainingEntries.TabIndex = 21;
            this.txtRemainingEntries.Visible = false;
            //
            // lblStatus
            //
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(20, 468);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(50, 20);
            this.lblStatus.TabIndex = 22;
            this.lblStatus.Text = "סטטוס";
            //
            // cboStatus
            //
            this.cboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStatus.Location = new System.Drawing.Point(110, 465);
            this.cboStatus.Name = "cboStatus";
            this.cboStatus.Size = new System.Drawing.Size(150, 23);
            this.cboStatus.TabIndex = 23;
            //
            // btnSave
            //
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnSave.Location = new System.Drawing.Point(20, 515);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(70, 40);
            this.btnSave.TabIndex = 24;
            this.btnSave.Text = "שמירה";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            //
            // btnDelete
            //
            this.btnDelete.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnDelete.Location = new System.Drawing.Point(110, 515);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(70, 40);
            this.btnDelete.TabIndex = 25;
            this.btnDelete.Text = "מחיקה";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            //
            // btnRenew
            //
            this.btnRenew.BackColor = System.Drawing.Color.FromArgb(45, 159, 147);
            this.btnRenew.FlatAppearance.BorderSize = 0;
            this.btnRenew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRenew.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnRenew.ForeColor = System.Drawing.Color.White;
            this.btnRenew.Location = new System.Drawing.Point(200, 515);
            this.btnRenew.Name = "btnRenew";
            this.btnRenew.Size = new System.Drawing.Size(120, 40);
            this.btnRenew.TabIndex = 32;
            this.btnRenew.Text = "🔄 חידוש מנוי";
            this.btnRenew.UseVisualStyleBackColor = false;
            this.btnRenew.Enabled = false;
            this.btnRenew.Click += new System.EventHandler(this.btnRenew_Click);
            //
            // btnPayments
            //
            this.btnPayments.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnPayments.Location = new System.Drawing.Point(350, 515);
            this.btnPayments.Name = "btnPayments";
            this.btnPayments.Size = new System.Drawing.Size(80, 40);
            this.btnPayments.TabIndex = 26;
            this.btnPayments.Text = "תשלומים";
            this.btnPayments.UseVisualStyleBackColor = true;
            this.btnPayments.Click += new System.EventHandler(this.btnPayments_Click);
            //
            // btnBack
            //
            this.btnBack.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnBack.Location = new System.Drawing.Point(450, 515);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(70, 40);
            this.btnBack.TabIndex = 27;
            this.btnBack.Text = "חזרה";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            //
            // btnFreeze
            //
            this.btnFreeze.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnFreeze.Location = new System.Drawing.Point(20, 580);
            this.btnFreeze.Name = "btnFreeze";
            this.btnFreeze.Size = new System.Drawing.Size(105, 55);
            this.btnFreeze.TabIndex = 28;
            this.btnFreeze.Text = "הקפאה";
            this.btnFreeze.UseVisualStyleBackColor = true;
            this.btnFreeze.Click += new System.EventHandler(this.btnFreeze_Click);
            //
            // btnUnfreeze
            //
            this.btnUnfreeze.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnUnfreeze.Location = new System.Drawing.Point(150, 580);
            this.btnUnfreeze.Name = "btnUnfreeze";
            this.btnUnfreeze.Size = new System.Drawing.Size(130, 55);
            this.btnUnfreeze.TabIndex = 29;
            this.btnUnfreeze.Text = "ביטול הקפאה";
            this.btnUnfreeze.UseVisualStyleBackColor = true;
            this.btnUnfreeze.Click += new System.EventHandler(this.btnUnfreeze_Click);
            //
            // btnSuspend
            //
            this.btnSuspend.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSuspend.Location = new System.Drawing.Point(300, 580);
            this.btnSuspend.Name = "btnSuspend";
            this.btnSuspend.Size = new System.Drawing.Size(105, 55);
            this.btnSuspend.TabIndex = 30;
            this.btnSuspend.Text = "השעיה";
            this.btnSuspend.UseVisualStyleBackColor = true;
            this.btnSuspend.Click += new System.EventHandler(this.btnSuspend_Click);
            //
            // btnExpire
            //
            this.btnExpire.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnExpire.Location = new System.Drawing.Point(430, 580);
            this.btnExpire.Name = "btnExpire";
            this.btnExpire.Size = new System.Drawing.Size(130, 55);
            this.btnExpire.TabIndex = 31;
            this.btnExpire.Text = "פקיעת תוקף";
            this.btnExpire.UseVisualStyleBackColor = true;
            this.btnExpire.Click += new System.EventHandler(this.btnExpire_Click);
            //
            // MembershipPanel
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnExpire);
            this.Controls.Add(this.btnSuspend);
            this.Controls.Add(this.btnUnfreeze);
            this.Controls.Add(this.btnFreeze);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnPayments);
            this.Controls.Add(this.btnRenew);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.cboStatus);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.txtRemainingEntries);
            this.Controls.Add(this.lblRemainingEntries);
            this.Controls.Add(this.dtpEndDate);
            this.Controls.Add(this.lblEndDate);
            this.Controls.Add(this.dtpStartDate);
            this.Controls.Add(this.lblStartDate);
            this.Controls.Add(this.txtValidityDays);
            this.Controls.Add(this.lblValidityDays);
            this.Controls.Add(this.txtAllowedEntries);
            this.Controls.Add(this.lblAllowedEntries);
            this.Controls.Add(this.txtPrice);
            this.Controls.Add(this.lblPrice);
            this.Controls.Add(this.cboPlan);
            this.Controls.Add(this.txtPlanType);
            this.Controls.Add(this.lblPlanType);
            this.Controls.Add(this.txtPlanName);
            this.Controls.Add(this.lblPlanName);
            this.Controls.Add(this.dtpPurchaseDate);
            this.Controls.Add(this.lblPurchaseDate);
            this.Controls.Add(this.cboTrainee);
            this.Controls.Add(this.lblTrainee);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.lblTitle);
            this.Name = "MembershipPanel";
            this.Size = new System.Drawing.Size(900, 660);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label lblTrainee;
        private System.Windows.Forms.ComboBox cboTrainee;
        private System.Windows.Forms.Label lblPurchaseDate;
        private System.Windows.Forms.DateTimePicker dtpPurchaseDate;
        private System.Windows.Forms.Label lblPlanName;
        private System.Windows.Forms.TextBox txtPlanName;
        private System.Windows.Forms.Label lblPlanType;
        private System.Windows.Forms.TextBox txtPlanType;
        private System.Windows.Forms.ComboBox cboPlan;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.TextBox txtPrice;
        private System.Windows.Forms.Label lblAllowedEntries;
        private System.Windows.Forms.TextBox txtAllowedEntries;
        private System.Windows.Forms.Label lblValidityDays;
        private System.Windows.Forms.TextBox txtValidityDays;
        private System.Windows.Forms.Label lblStartDate;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.Label lblEndDate;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.Label lblRemainingEntries;
        private System.Windows.Forms.TextBox txtRemainingEntries;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox cboStatus;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnRenew;
        private System.Windows.Forms.Button btnPayments;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnFreeze;
        private System.Windows.Forms.Button btnUnfreeze;
        private System.Windows.Forms.Button btnSuspend;
        private System.Windows.Forms.Button btnExpire;
    }
}
