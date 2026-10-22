using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace LevelUp_Management
{
    public class FaultReport
    {
        private int faultReportId;
        private Equipment equipment;
        private Trainer trainer;
        private DateTime reportDate;
        private string description;
        private string status;

        public FaultReport(int faultReportId, Equipment equipment, Trainer trainer, DateTime reportDate,
            string description, string status, bool is_new)
        {
            this.faultReportId = faultReportId;
            this.equipment = equipment;
            this.trainer = trainer;
            this.reportDate = reportDate;
            this.description = description;
            this.status = status;
            if (is_new)
            {
                this.createFaultReport();
                Program.FaultReports.Add(this);
            }
        }

        public int getFaultReportId() { return this.faultReportId; }
        public Equipment getEquipment() { return this.equipment; }
        public Trainer getTrainer() { return this.trainer; }
        public DateTime getReportDate() { return this.reportDate; }
        public string getDescription() { return this.description; }
        public string getStatus() { return this.status; }

        public void setEquipment(Equipment equipment) { this.equipment = equipment; }
        public void setTrainer(Trainer trainer) { this.trainer = trainer; }
        public void setReportDate(DateTime reportDate) { this.reportDate = reportDate; }
        public void setDescription(string description) { this.description = description; }
        public void setStatus(string status) { this.status = status; }

        public void createFaultReport()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_fault_report_create @fault_report_id, @equipment_id, @trainer_id, @reportDate, @description, @status";
            cmd.Parameters.AddWithValue("@fault_report_id", this.faultReportId);
            cmd.Parameters.AddWithValue("@equipment_id", this.equipment.getEquipmentId());
            cmd.Parameters.AddWithValue("@trainer_id", this.trainer.getTrainerId());
            cmd.Parameters.AddWithValue("@reportDate", this.reportDate);
            cmd.Parameters.AddWithValue("@description", this.description);
            cmd.Parameters.AddWithValue("@status", this.status);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        public void updateFaultReport()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_fault_report_update @fault_report_id, @equipment_id, @trainer_id, @reportDate, @description, @status";
            cmd.Parameters.AddWithValue("@fault_report_id", this.faultReportId);
            cmd.Parameters.AddWithValue("@equipment_id", this.equipment.getEquipmentId());
            cmd.Parameters.AddWithValue("@trainer_id", this.trainer.getTrainerId());
            cmd.Parameters.AddWithValue("@reportDate", this.reportDate);
            cmd.Parameters.AddWithValue("@description", this.description);
            cmd.Parameters.AddWithValue("@status", this.status);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        public void deleteFaultReport()
        {
            Program.FaultReports.Remove(this);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_fault_report_delete @fault_report_id";
            cmd.Parameters.AddWithValue("@fault_report_id", this.faultReportId);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        public static void initFaultReports()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_fault_report_get_all";
            SQL_CON SC = new SQL_CON();
            SqlDataReader rdr = SC.execute_query(cmd);

            Program.FaultReports = new List<FaultReport>();

            // rdr ריק פירושו ש-execute_query כבר דיווח על תקלת התחברות;
            // משאירים רשימה ריקה במקום ליפול על NullReferenceException.
            if (rdr == null) return;

            while (rdr.Read())
            {
                int id = rdr.GetInt32(0);
                Equipment equipment = Equipment.seekEquipment(rdr.GetInt32(1));
                Trainer trainer = Trainer.seekTrainer(rdr.GetInt32(2));
                DateTime reportDate = rdr.GetDateTime(3);
                string description = rdr.GetString(4);
                string status = rdr.GetString(5);

                FaultReport fr = new FaultReport(id, equipment, trainer, reportDate, description, status, false);
                Program.FaultReports.Add(fr);
            }
        }

        public static FaultReport seekFaultReport(int faultReportId)
        {
            foreach (FaultReport fr in Program.FaultReports)
            {
                if (fr.getFaultReportId() == faultReportId)
                    return fr;
            }
            return null;
        }

        public static int getNextFaultReportId()
        {
            int maxId = 0;
            foreach (FaultReport fr in Program.FaultReports)
            {
                if (fr.getFaultReportId() > maxId)
                    maxId = fr.getFaultReportId();
            }
            return maxId + 1;
        }
    }
}
