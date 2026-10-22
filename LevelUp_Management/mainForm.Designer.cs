namespace LevelUp_Management
{
    partial class mainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.panelMenu = new System.Windows.Forms.Panel();
            this.btnGenerateRentersBillingReport = new System.Windows.Forms.Button();
            this.btnRenewPayMembership = new System.Windows.Forms.Button();
            this.btnDocumentWorkoutPerformance = new System.Windows.Forms.Button();
            this.btnManageWaitlist = new System.Windows.Forms.Button();
            this.btnRegisterToWorkout = new System.Windows.Forms.Button();
            this.btnManageTrainees = new System.Windows.Forms.Button();
            this.lblMenuTitle = new System.Windows.Forms.Label();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelMenu.SuspendLayout();
            this.SuspendLayout();
            //
            // panelMenu
            //
            this.panelMenu.Controls.Add(this.btnGenerateRentersBillingReport);
            this.panelMenu.Controls.Add(this.btnRenewPayMembership);
            this.panelMenu.Controls.Add(this.btnDocumentWorkoutPerformance);
            this.panelMenu.Controls.Add(this.btnManageWaitlist);
            this.panelMenu.Controls.Add(this.btnRegisterToWorkout);
            this.panelMenu.Controls.Add(this.btnManageTrainees);
            this.panelMenu.Controls.Add(this.lblMenuTitle);
            // Docked LEFT on purpose: the form sets RightToLeftLayout = true, which
            // mirrors the layout, so DockStyle.Left renders on the physical RIGHT —
            // where a Hebrew UI expects its navigation. See the RTL note in UITheme.cs.
            this.panelMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelMenu.Location = new System.Drawing.Point(0, 0);
            this.panelMenu.Name = "panelMenu";
            this.panelMenu.Size = new System.Drawing.Size(272, 760);
            this.panelMenu.TabIndex = 0;
            this.panelMenu.Visible = false;
            //
            // btnGenerateRentersBillingReport
            //
            this.btnGenerateRentersBillingReport.Location = new System.Drawing.Point(20, 405);
            this.btnGenerateRentersBillingReport.Name = "btnGenerateRentersBillingReport";
            this.btnGenerateRentersBillingReport.Size = new System.Drawing.Size(220, 50);
            this.btnGenerateRentersBillingReport.TabIndex = 6;
            this.btnGenerateRentersBillingReport.Text = "דוח חיוב למאמנים שוכרים";
            this.btnGenerateRentersBillingReport.UseVisualStyleBackColor = true;
            this.btnGenerateRentersBillingReport.Click += new System.EventHandler(this.btnGenerateRentersBillingReport_Click);
            //
            // btnRenewPayMembership
            //
            this.btnRenewPayMembership.Location = new System.Drawing.Point(20, 340);
            this.btnRenewPayMembership.Name = "btnRenewPayMembership";
            this.btnRenewPayMembership.Size = new System.Drawing.Size(220, 50);
            this.btnRenewPayMembership.TabIndex = 5;
            this.btnRenewPayMembership.Text = "חידוש ותשלום מנוי";
            this.btnRenewPayMembership.UseVisualStyleBackColor = true;
            this.btnRenewPayMembership.Click += new System.EventHandler(this.btnRenewPayMembership_Click);
            //
            // btnDocumentWorkoutPerformance
            //
            this.btnDocumentWorkoutPerformance.Location = new System.Drawing.Point(20, 275);
            this.btnDocumentWorkoutPerformance.Name = "btnDocumentWorkoutPerformance";
            this.btnDocumentWorkoutPerformance.Size = new System.Drawing.Size(220, 50);
            this.btnDocumentWorkoutPerformance.TabIndex = 4;
            this.btnDocumentWorkoutPerformance.Text = "תיעוד ביצועי אימון";
            this.btnDocumentWorkoutPerformance.UseVisualStyleBackColor = true;
            this.btnDocumentWorkoutPerformance.Click += new System.EventHandler(this.btnDocumentWorkoutPerformance_Click);
            //
            // btnManageWaitlist
            //
            this.btnManageWaitlist.Location = new System.Drawing.Point(20, 210);
            this.btnManageWaitlist.Name = "btnManageWaitlist";
            this.btnManageWaitlist.Size = new System.Drawing.Size(220, 50);
            this.btnManageWaitlist.TabIndex = 3;
            this.btnManageWaitlist.Text = "ניהול רשימת המתנה";
            this.btnManageWaitlist.UseVisualStyleBackColor = true;
            this.btnManageWaitlist.Click += new System.EventHandler(this.btnManageWaitlist_Click);
            //
            // btnRegisterToWorkout
            //
            this.btnRegisterToWorkout.Location = new System.Drawing.Point(20, 145);
            this.btnRegisterToWorkout.Name = "btnRegisterToWorkout";
            this.btnRegisterToWorkout.Size = new System.Drawing.Size(220, 50);
            this.btnRegisterToWorkout.TabIndex = 2;
            this.btnRegisterToWorkout.Text = "הרשמה לאימון";
            this.btnRegisterToWorkout.UseVisualStyleBackColor = true;
            this.btnRegisterToWorkout.Click += new System.EventHandler(this.btnRegisterToWorkout_Click);
            //
            // btnManageTrainees
            //
            this.btnManageTrainees.Location = new System.Drawing.Point(20, 80);
            this.btnManageTrainees.Name = "btnManageTrainees";
            this.btnManageTrainees.Size = new System.Drawing.Size(220, 50);
            this.btnManageTrainees.TabIndex = 1;
            this.btnManageTrainees.Text = "ניהול מתאמנים";
            this.btnManageTrainees.UseVisualStyleBackColor = true;
            this.btnManageTrainees.Click += new System.EventHandler(this.btnManageTrainees_Click);
            //
            // lblMenuTitle
            //
            this.lblMenuTitle.AutoSize = true;
            this.lblMenuTitle.Location = new System.Drawing.Point(20, 20);
            this.lblMenuTitle.Name = "lblMenuTitle";
            this.lblMenuTitle.Size = new System.Drawing.Size(150, 33);
            this.lblMenuTitle.TabIndex = 0;
            this.lblMenuTitle.Text = "תפריט ראשי";
            //
            // panelMain
            //
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(272, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(968, 760);
            this.panelMain.TabIndex = 1;
            //
            // mainForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1240, 760);
            this.MinimumSize = new System.Drawing.Size(980, 700);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            // Opens using the full screen rather than the fixed 1240x760 above.
            // That fixed size was chosen on one dev machine's display; on a
            // smaller or differently-scaled screen (e.g. a VM) it can leave too
            // little room for a panel's grid + form + action bar all at once,
            // clipping content at the bottom of the window. ClientSize/
            // MinimumSize above still apply if the user manually restores the
            // window to Normal size afterward.
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelMenu);
            this.Name = "mainForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Text = "LEVEL UP - ניהול סטודיו";
            this.panelMenu.ResumeLayout(false);
            this.panelMenu.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelMenu;
        private System.Windows.Forms.Label lblMenuTitle;
        private System.Windows.Forms.Button btnManageTrainees;
        private System.Windows.Forms.Button btnRegisterToWorkout;
        private System.Windows.Forms.Button btnManageWaitlist;
        private System.Windows.Forms.Button btnDocumentWorkoutPerformance;
        private System.Windows.Forms.Button btnRenewPayMembership;
        private System.Windows.Forms.Button btnGenerateRentersBillingReport;
        private System.Windows.Forms.Panel panelMain;
    }
}
