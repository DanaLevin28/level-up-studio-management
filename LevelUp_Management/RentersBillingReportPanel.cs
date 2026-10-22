using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace LevelUp_Management
{
    public partial class RentersBillingReportPanel : UserControl
    {
        public RentersBillingReportPanel()
        {
            InitializeComponent();
            UITheme.applyPanel(this, "הפקת דוח חיוב חודשי עבור מאמנים שוכרים");
            loadCombos();
            dtpStartDate.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            dtpEndDate.Value = DateTime.Today;
        }

        private void loadCombos()
        {
            DataTable trainerTable = new DataTable();
            trainerTable.Columns.Add("id", typeof(int));
            trainerTable.Columns.Add("display", typeof(string));
            foreach (Trainer t in Program.Trainers)
                if (t.getEmploymentType() == EmploymentType.RENTING)
                    trainerTable.Rows.Add(t.getTrainerId(), t.getFullName());
            cboTrainer.DataSource = trainerTable;
            cboTrainer.DisplayMember = "display";
            cboTrainer.ValueMember = "id";
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (cboTrainer.SelectedValue == null)
            {
                MessageBox.Show("יש לבחור מאמן שוכר", "שגיאה", MessageBoxButtons.OK);
                return;
            }
            if (dtpEndDate.Value.Date < dtpStartDate.Value.Date)
            {
                MessageBox.Show("תאריך הסיום חייב להיות אחרי תאריך ההתחלה", "שגיאה", MessageBoxButtons.OK);
                return;
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_renters_billing_report @trainer_id, @start_date, @end_date";
            cmd.Parameters.AddWithValue("@trainer_id", (int)cboTrainer.SelectedValue);
            cmd.Parameters.AddWithValue("@start_date", dtpStartDate.Value.Date);
            cmd.Parameters.AddWithValue("@end_date", dtpEndDate.Value.Date);
            SQL_CON SC = new SQL_CON();
            SqlDataReader rdr = SC.execute_query(cmd);
            if (rdr == null) return;

            DataTable dt = new DataTable();
            dt.Columns.Add("date", typeof(DateTime));
            dt.Columns["date"].AllowDBNull = true;
            dt.Columns.Add("startTime", typeof(string));
            dt.Columns.Add("endTime", typeof(string));
            dt.Columns.Add("room", typeof(string));
            dt.Columns.Add("status", typeof(string));
            dt.Columns.Add("amount", typeof(decimal));
            dt.Columns.Add("totalWorkouts", typeof(int));
            dt.Columns["totalWorkouts"].AllowDBNull = true;
            dt.Columns.Add("totalCancellations", typeof(int));
            dt.Columns["totalCancellations"].AllowDBNull = true;

            while (rdr.Read())
            {
                // Read by column NAME: the SP also returns [מועד_ביטול] (a 9th
                // column) between status and amount, which shifted every
                // positional index after 4. The SP aliases are Hebrew.
                object date = rdr["תאריך"] is DBNull ? (object)DBNull.Value : rdr["תאריך"];
                string startTime = rdr["שעת_התחלה"] is DBNull ? "" : ((TimeSpan)rdr["שעת_התחלה"]).ToString(@"hh\:mm");
                string endTime = rdr["שעת_סיום"] is DBNull ? "" : ((TimeSpan)rdr["שעת_סיום"]).ToString(@"hh\:mm");
                string room = rdr["חדר"] is DBNull ? "" : Convert.ToString(rdr["חדר"]);
                string status = rdr["סטטוס"] is DBNull ? "" : Convert.ToString(rdr["סטטוס"]);
                decimal amount = rdr["סכום_לחיוב"] is DBNull ? 0m : Convert.ToDecimal(rdr["סכום_לחיוב"]);
                object totalWorkouts = rdr["סך_הכל_אימונים"] is DBNull ? (object)DBNull.Value : Convert.ToInt32(rdr["סך_הכל_אימונים"]);
                object totalCancellations = rdr["סך_הכל_ביטולים"] is DBNull ? (object)DBNull.Value : Convert.ToInt32(rdr["סך_הכל_ביטולים"]);

                dt.Rows.Add(date, startTime, endTime, room, status, amount, totalWorkouts, totalCancellations);
            }

            dataGridView1.DataSource = dt;

            dataGridView1.Columns["date"].HeaderText = "תאריך";
            dataGridView1.Columns["startTime"].HeaderText = "שעת התחלה";
            dataGridView1.Columns["endTime"].HeaderText = "שעת סיום";
            dataGridView1.Columns["room"].HeaderText = "חדר";
            dataGridView1.Columns["status"].HeaderText = "סטטוס";
            dataGridView1.Columns["amount"].HeaderText = "סכום לחיוב";
            dataGridView1.Columns["totalWorkouts"].HeaderText = "סך הכל אימונים";
            dataGridView1.Columns["totalCancellations"].HeaderText = "סך הכל ביטולים";
            dataGridView1.Columns["date"].DefaultCellStyle.Format = "dd/MM/yyyy";
            dataGridView1.Columns["amount"].DefaultCellStyle.Format = "0.00";
        }

        private void btnManageBookings_Click(object sender, EventArgs e)
        {
            mainForm.showPanel(new RoomBookingPanel());
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            mainForm.showPanel(null);
        }
    }
}
