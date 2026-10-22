using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace LevelUp_Management
{
    public class HealthDeclaration
    {
        private int healthDeclarationId;
        private Trainee trainee;
        private DateTime declarationDate;
        private DeclarationStatus status;
        private string content;

        public HealthDeclaration(int healthDeclarationId, Trainee trainee, DateTime declarationDate,
            DeclarationStatus status, string content, bool is_new)
        {
            this.healthDeclarationId = healthDeclarationId;
            this.trainee = trainee;
            this.declarationDate = declarationDate;
            this.status = status;
            this.content = content;
            if (is_new)
            {
                this.createHealthDeclaration();
                Program.HealthDeclarations.Add(this);
            }
        }

        public int getHealthDeclarationId() { return this.healthDeclarationId; }
        public Trainee getTrainee() { return this.trainee; }
        public DateTime getDeclarationDate() { return this.declarationDate; }
        public DeclarationStatus getStatus() { return this.status; }
        public string getContent() { return this.content; }

        public void setTrainee(Trainee trainee) { this.trainee = trainee; }
        public void setDeclarationDate(DateTime declarationDate) { this.declarationDate = declarationDate; }
        public void setStatus(DeclarationStatus status) { this.status = status; }
        public void setContent(string content) { this.content = content; }

        public void createHealthDeclaration()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_health_declaration_create @health_declaration_id, @trainee_id, @declarationDate, @status, @content";
            cmd.Parameters.AddWithValue("@health_declaration_id", this.healthDeclarationId);
            cmd.Parameters.AddWithValue("@trainee_id", this.trainee.getTraineeId());
            cmd.Parameters.AddWithValue("@declarationDate", this.declarationDate);
            cmd.Parameters.AddWithValue("@status", this.status.ToString());
            cmd.Parameters.AddWithValue("@content", this.content);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        public void updateHealthDeclaration()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_health_declaration_update @health_declaration_id, @trainee_id, @declarationDate, @status, @content";
            cmd.Parameters.AddWithValue("@health_declaration_id", this.healthDeclarationId);
            cmd.Parameters.AddWithValue("@trainee_id", this.trainee.getTraineeId());
            cmd.Parameters.AddWithValue("@declarationDate", this.declarationDate);
            cmd.Parameters.AddWithValue("@status", this.status.ToString());
            cmd.Parameters.AddWithValue("@content", this.content);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        // approve() : void from the Class Diagram — marks the declaration as
        // reviewed and approved by a trainer/manager (NFR-4) and persists it.
        public void approve()
        {
            this.status = DeclarationStatus.APPROVED;
            this.updateHealthDeclaration();
        }

        public void deleteHealthDeclaration()
        {
            Program.HealthDeclarations.Remove(this);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_health_declaration_delete @health_declaration_id";
            cmd.Parameters.AddWithValue("@health_declaration_id", this.healthDeclarationId);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        public static void initHealthDeclarations()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_health_declaration_get_all";
            SQL_CON SC = new SQL_CON();
            SqlDataReader rdr = SC.execute_query(cmd);

            Program.HealthDeclarations = new List<HealthDeclaration>();

            // rdr ריק פירושו ש-execute_query כבר דיווח על תקלת התחברות;
            // משאירים רשימה ריקה במקום ליפול על NullReferenceException.
            if (rdr == null) return;

            while (rdr.Read())
            {
                int id = rdr.GetInt32(0);
                Trainee trainee = Trainee.seekTrainee(rdr.GetInt32(1));
                DateTime declarationDate = rdr.GetDateTime(2);
                DeclarationStatus status = (DeclarationStatus)Enum.Parse(typeof(DeclarationStatus), rdr.GetString(3));
                string content = rdr.GetString(4);

                HealthDeclaration hd = new HealthDeclaration(id, trainee, declarationDate, status, content, false);
                Program.HealthDeclarations.Add(hd);
            }
        }

        public static HealthDeclaration seekHealthDeclaration(int healthDeclarationId)
        {
            foreach (HealthDeclaration hd in Program.HealthDeclarations)
            {
                if (hd.getHealthDeclarationId() == healthDeclarationId)
                    return hd;
            }
            return null;
        }

        public static int getNextHealthDeclarationId()
        {
            int maxId = 0;
            foreach (HealthDeclaration hd in Program.HealthDeclarations)
            {
                if (hd.getHealthDeclarationId() > maxId)
                    maxId = hd.getHealthDeclarationId();
            }
            return maxId + 1;
        }
    }
}
