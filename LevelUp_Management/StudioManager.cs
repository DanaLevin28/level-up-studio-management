using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace LevelUp_Management
{
    public class StudioManager
    {
        private int studioManagerId;
        private string username;
        private string password;
        private string fullName;
        private string email;
        private string phone;
        private AccountStatus status;

        public StudioManager(int studioManagerId, string username, string password, string fullName,
            string email, string phone, AccountStatus status, bool is_new)
        {
            this.studioManagerId = studioManagerId;
            this.username = username;
            this.password = password;
            this.fullName = fullName;
            this.email = email;
            this.phone = phone;
            this.status = status;
            if (is_new)
            {
                this.createStudioManager();
                Program.StudioManagers.Add(this);
            }
        }

        public int getStudioManagerId() { return this.studioManagerId; }
        public string getUsername() { return this.username; }
        public string getPassword() { return this.password; }
        public string getFullName() { return this.fullName; }
        public string getEmail() { return this.email; }
        public string getPhone() { return this.phone; }
        public AccountStatus getStatus() { return this.status; }

        public void setUsername(string username) { this.username = username; }
        public void setPassword(string password) { this.password = password; }
        public void setFullName(string fullName) { this.fullName = fullName; }
        public void setEmail(string email) { this.email = email; }
        public void setPhone(string phone) { this.phone = phone; }
        public void setStatus(AccountStatus status) { this.status = status; }

        public void createStudioManager()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_studio_manager_create @studio_manager_id, @username, @password, @fullName, @email, @phone, @status";
            cmd.Parameters.AddWithValue("@studio_manager_id", this.studioManagerId);
            cmd.Parameters.AddWithValue("@username", this.username);
            cmd.Parameters.AddWithValue("@password", this.password);
            cmd.Parameters.AddWithValue("@fullName", this.fullName);
            cmd.Parameters.AddWithValue("@email", this.email);
            cmd.Parameters.AddWithValue("@phone", this.phone);
            cmd.Parameters.AddWithValue("@status", this.status.ToString());
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        public void updateStudioManager()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_studio_manager_update @studio_manager_id, @username, @password, @fullName, @email, @phone, @status";
            cmd.Parameters.AddWithValue("@studio_manager_id", this.studioManagerId);
            cmd.Parameters.AddWithValue("@username", this.username);
            cmd.Parameters.AddWithValue("@password", this.password);
            cmd.Parameters.AddWithValue("@fullName", this.fullName);
            cmd.Parameters.AddWithValue("@email", this.email);
            cmd.Parameters.AddWithValue("@phone", this.phone);
            cmd.Parameters.AddWithValue("@status", this.status.ToString());
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        public void deleteStudioManager()
        {
            Program.StudioManagers.Remove(this);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_studio_manager_delete @studio_manager_id";
            cmd.Parameters.AddWithValue("@studio_manager_id", this.studioManagerId);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        // ====================================================================
        // Domain methods from the Class Diagram — stubs only, TODO until each
        // one's real implementation is designed.
        // ====================================================================

        public void generateRentersBillingReport(string month)
        {
            // TODO: generate billing report for renting trainers
        }

        public void createTrainee(Trainee t)
        {
            // TODO: delegate to Trainee creation
        }

        public void updateTrainee(Trainee t)
        {
            t.updateTrainee();
        }

        public void deactivateTrainee(int traineeId)
        {
            Trainee t = Trainee.seekTrainee(traineeId);
            if (t != null)
            {
                // TODO: deactivate trainee
            }
        }

        public int getActiveMemberCount()
        {
            int count = 0;
            foreach (Trainee t in Program.Trainees)
            {
                // TODO: count active trainees
                count++;
            }
            return count;
        }

        public static void initStudioManagers()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_studio_manager_get_all";
            SQL_CON SC = new SQL_CON();
            SqlDataReader rdr = SC.execute_query(cmd);

            Program.StudioManagers = new List<StudioManager>();

            // rdr ריק פירושו ש-execute_query כבר דיווח על תקלת התחברות;
            // משאירים רשימה ריקה במקום ליפול על NullReferenceException.
            if (rdr == null) return;

            while (rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string username = rdr.GetString(1);
                string password = rdr.GetString(2);
                string fullName = rdr.GetString(3);
                string email = rdr.GetString(4);
                string phone = rdr.GetString(5);
                AccountStatus status = (AccountStatus)Enum.Parse(typeof(AccountStatus), rdr.GetString(6));

                StudioManager sm = new StudioManager(id, username, password, fullName, email, phone, status, false);
                Program.StudioManagers.Add(sm);
            }
        }

        public static StudioManager seekStudioManager(int studioManagerId)
        {
            foreach (StudioManager sm in Program.StudioManagers)
            {
                if (sm.getStudioManagerId() == studioManagerId)
                    return sm;
            }
            return null;
        }

        public static int getNextStudioManagerId()
        {
            int maxId = 0;
            foreach (StudioManager sm in Program.StudioManagers)
            {
                if (sm.getStudioManagerId() > maxId)
                    maxId = sm.getStudioManagerId();
            }
            return maxId + 1;
        }
    }
}
