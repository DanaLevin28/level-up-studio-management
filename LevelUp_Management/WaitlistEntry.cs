using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace LevelUp_Management
{
    // Association Class: links Trainee <-> TrainingSession for the full-capacity case
    // (per CLAUDE.md Association Class pattern, both sides are object references).
    public class WaitlistEntry
    {
        private int waitlistEntryId;
        private Trainee trainee;
        private TrainingSession trainingSession;
        private int position;
        private DateTime joinTimestamp;
        private WaitlistStatus status;
        private DateTime? expirationTimestamp;

        public WaitlistEntry(int waitlistEntryId, Trainee trainee, TrainingSession trainingSession,
            int position, DateTime joinTimestamp, WaitlistStatus status, DateTime? expirationTimestamp,
            bool is_new)
        {
            this.waitlistEntryId = waitlistEntryId;
            this.trainee = trainee;
            this.trainingSession = trainingSession;
            this.position = position;
            this.joinTimestamp = joinTimestamp;
            this.status = status;
            this.expirationTimestamp = expirationTimestamp;
            if (is_new)
            {
                this.createWaitlistEntry();
                Program.WaitlistEntries.Add(this);
            }
        }

        public int getWaitlistEntryId() { return this.waitlistEntryId; }
        public Trainee getTrainee() { return this.trainee; }
        public TrainingSession getTrainingSession() { return this.trainingSession; }
        public int getPosition() { return this.position; }
        public DateTime getJoinTimestamp() { return this.joinTimestamp; }
        public WaitlistStatus getStatus() { return this.status; }
        public DateTime? getExpirationTimestamp() { return this.expirationTimestamp; }

        public void setTrainee(Trainee trainee) { this.trainee = trainee; }
        public void setTrainingSession(TrainingSession trainingSession) { this.trainingSession = trainingSession; }
        public void setPosition(int position) { this.position = position; }
        public void setJoinTimestamp(DateTime joinTimestamp) { this.joinTimestamp = joinTimestamp; }
        public void setStatus(WaitlistStatus status) { this.status = status; }
        public void setExpirationTimestamp(DateTime? expirationTimestamp) { this.expirationTimestamp = expirationTimestamp; }

        public void createWaitlistEntry()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_waitlist_entry_create @waitlist_entry_id, @trainee_id, @training_session_id, @position, @joinTimestamp, @status, @expirationTimestamp";
            cmd.Parameters.AddWithValue("@waitlist_entry_id", this.waitlistEntryId);
            cmd.Parameters.AddWithValue("@trainee_id", this.trainee.getTraineeId());
            cmd.Parameters.AddWithValue("@training_session_id", this.trainingSession.getTrainingSessionId());
            cmd.Parameters.AddWithValue("@position", this.position);
            cmd.Parameters.AddWithValue("@joinTimestamp", this.joinTimestamp);
            cmd.Parameters.AddWithValue("@status", this.status.ToString());
            cmd.Parameters.AddWithValue("@expirationTimestamp", (object)this.expirationTimestamp ?? DBNull.Value);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        public void updateWaitlistEntry()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_waitlist_entry_update @waitlist_entry_id, @trainee_id, @training_session_id, @position, @joinTimestamp, @status, @expirationTimestamp";
            cmd.Parameters.AddWithValue("@waitlist_entry_id", this.waitlistEntryId);
            cmd.Parameters.AddWithValue("@trainee_id", this.trainee.getTraineeId());
            cmd.Parameters.AddWithValue("@training_session_id", this.trainingSession.getTrainingSessionId());
            cmd.Parameters.AddWithValue("@position", this.position);
            cmd.Parameters.AddWithValue("@joinTimestamp", this.joinTimestamp);
            cmd.Parameters.AddWithValue("@status", this.status.ToString());
            cmd.Parameters.AddWithValue("@expirationTimestamp", (object)this.expirationTimestamp ?? DBNull.Value);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        public void deleteWaitlistEntry()
        {
            Program.WaitlistEntries.Remove(this);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_waitlist_entry_delete @waitlist_entry_id";
            cmd.Parameters.AddWithValue("@waitlist_entry_id", this.waitlistEntryId);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        public static void initWaitlistEntries()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_waitlist_entry_get_all";
            SQL_CON SC = new SQL_CON();
            SqlDataReader rdr = SC.execute_query(cmd);

            Program.WaitlistEntries = new List<WaitlistEntry>();

            // rdr ריק פירושו ש-execute_query כבר דיווח על תקלת התחברות;
            // משאירים רשימה ריקה במקום ליפול על NullReferenceException.
            if (rdr == null) return;

            while (rdr.Read())
            {
                int id = rdr.GetInt32(0);
                Trainee trainee = Trainee.seekTrainee(rdr.GetInt32(1));
                TrainingSession trainingSession = TrainingSession.seekTrainingSession(rdr.GetInt32(2));
                int position = rdr.GetInt32(3);
                DateTime joinTimestamp = rdr.GetDateTime(4);
                WaitlistStatus status = (WaitlistStatus)Enum.Parse(typeof(WaitlistStatus), rdr.GetString(5));
                DateTime? expirationTimestamp = rdr.IsDBNull(6) ? (DateTime?)null : rdr.GetDateTime(6);

                WaitlistEntry we = new WaitlistEntry(id, trainee, trainingSession, position, joinTimestamp, status, expirationTimestamp, false);
                Program.WaitlistEntries.Add(we);
            }
        }

        public static WaitlistEntry seekWaitlistEntry(int waitlistEntryId)
        {
            foreach (WaitlistEntry we in Program.WaitlistEntries)
            {
                if (we.getWaitlistEntryId() == waitlistEntryId)
                    return we;
            }
            return null;
        }

        public static int getNextWaitlistEntryId()
        {
            int maxId = 0;
            foreach (WaitlistEntry we in Program.WaitlistEntries)
            {
                if (we.getWaitlistEntryId() > maxId)
                    maxId = we.getWaitlistEntryId();
            }
            return maxId + 1;
        }
    }
}
