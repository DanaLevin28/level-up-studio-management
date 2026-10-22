using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace LevelUp_Management
{
    public class Trainer
    {
        private int trainerId;
        private string fullName;
        private string phone;
        private string email;
        private EmploymentType employmentType;
        private decimal hourlyRate;
        private string specialization;
        private AccountStatus status;

        public Trainer(int trainerId, string fullName, string phone, string email,
            EmploymentType employmentType, decimal hourlyRate, string specialization, AccountStatus status, bool is_new)
        {
            this.trainerId = trainerId;
            this.fullName = fullName;
            this.phone = phone;
            this.email = email;
            this.employmentType = employmentType;
            this.hourlyRate = hourlyRate;
            this.specialization = specialization;
            this.status = status;
            if (is_new)
            {
                this.createTrainer();
                Program.Trainers.Add(this);
            }
        }

        public int getTrainerId() { return this.trainerId; }
        public string getFullName() { return this.fullName; }
        public string getPhone() { return this.phone; }
        public string getEmail() { return this.email; }
        public EmploymentType getEmploymentType() { return this.employmentType; }
        public decimal getHourlyRate() { return this.hourlyRate; }
        public string getSpecialization() { return this.specialization; }
        public AccountStatus getStatus() { return this.status; }

        public void setFullName(string fullName) { this.fullName = fullName; }
        public void setPhone(string phone) { this.phone = phone; }
        public void setEmail(string email) { this.email = email; }
        public void setEmploymentType(EmploymentType employmentType) { this.employmentType = employmentType; }
        public void setHourlyRate(decimal hourlyRate) { this.hourlyRate = hourlyRate; }
        public void setSpecialization(string specialization) { this.specialization = specialization; }
        public void setStatus(AccountStatus status) { this.status = status; }

        public void createTrainer()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_trainer_create @trainer_id, @fullName, @phone, @email, @employmentType, @hourlyRate, @specialization, @status";
            cmd.Parameters.AddWithValue("@trainer_id", this.trainerId);
            cmd.Parameters.AddWithValue("@fullName", this.fullName);
            cmd.Parameters.AddWithValue("@phone", this.phone);
            cmd.Parameters.AddWithValue("@email", this.email);
            cmd.Parameters.AddWithValue("@employmentType", this.employmentType.ToString());
            cmd.Parameters.AddWithValue("@hourlyRate", this.hourlyRate);
            cmd.Parameters.AddWithValue("@specialization", (object)this.specialization ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@status", this.status.ToString());
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        public void updateTrainer()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_trainer_update @trainer_id, @fullName, @phone, @email, @employmentType, @hourlyRate, @specialization, @status";
            cmd.Parameters.AddWithValue("@trainer_id", this.trainerId);
            cmd.Parameters.AddWithValue("@fullName", this.fullName);
            cmd.Parameters.AddWithValue("@phone", this.phone);
            cmd.Parameters.AddWithValue("@email", this.email);
            cmd.Parameters.AddWithValue("@employmentType", this.employmentType.ToString());
            cmd.Parameters.AddWithValue("@hourlyRate", this.hourlyRate);
            cmd.Parameters.AddWithValue("@specialization", (object)this.specialization ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@status", this.status.ToString());
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        public void deleteTrainer()
        {
            Program.Trainers.Remove(this);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_trainer_delete @trainer_id";
            cmd.Parameters.AddWithValue("@trainer_id", this.trainerId);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        public static void initTrainers()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_trainer_get_all";
            SQL_CON SC = new SQL_CON();
            SqlDataReader rdr = SC.execute_query(cmd);

            Program.Trainers = new List<Trainer>();

            // rdr ריק פירושו ש-execute_query כבר דיווח על תקלת התחברות;
            // משאירים רשימה ריקה במקום ליפול על NullReferenceException.
            if (rdr == null) return;

            while (rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string fullName = rdr.GetString(1);
                string phone = rdr.GetString(2);
                string email = rdr.GetString(3);
                EmploymentType employmentType = (EmploymentType)Enum.Parse(typeof(EmploymentType), rdr.GetString(4));
                decimal hourlyRate = rdr.GetDecimal(5);
                string specialization = rdr.IsDBNull(6) ? null : rdr.GetString(6);
                AccountStatus status = (AccountStatus)Enum.Parse(typeof(AccountStatus), rdr.GetString(7));

                Trainer t = new Trainer(id, fullName, phone, email, employmentType, hourlyRate, specialization, status, false);
                Program.Trainers.Add(t);
            }
        }

        public static Trainer seekTrainer(int trainerId)
        {
            foreach (Trainer t in Program.Trainers)
            {
                if (t.getTrainerId() == trainerId)
                    return t;
            }
            return null;
        }

        public static int getNextTrainerId()
        {
            int maxId = 0;
            foreach (Trainer t in Program.Trainers)
            {
                if (t.getTrainerId() > maxId)
                    maxId = t.getTrainerId();
            }
            return maxId + 1;
        }
    }
}
