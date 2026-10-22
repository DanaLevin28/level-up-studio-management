using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace LevelUp_Management
{
    public class Trainee
    {
        private int traineeId;
        private string fullName;
        private string phone;
        private string email;
        private DateTime dateOfBirth;
        private string experienceLevel;
        private AccountStatus status;
        private DateTime registrationDate;
        private string medicalLimitations;

        public Trainee(int traineeId, string fullName, string phone, string email,
            DateTime dateOfBirth, string experienceLevel, AccountStatus status, DateTime registrationDate,
            string medicalLimitations, bool is_new)
        {
            this.traineeId = traineeId;
            this.fullName = fullName;
            this.phone = phone;
            this.email = email;
            this.dateOfBirth = dateOfBirth;
            this.experienceLevel = experienceLevel;
            this.status = status;
            this.registrationDate = registrationDate;
            this.medicalLimitations = medicalLimitations;
            if (is_new)
            {
                this.createTrainee();
                Program.Trainees.Add(this);
            }
        }

        public int getTraineeId() { return this.traineeId; }
        public string getFullName() { return this.fullName; }
        public string getPhone() { return this.phone; }
        public string getEmail() { return this.email; }
        public DateTime getDateOfBirth() { return this.dateOfBirth; }
        public string getExperienceLevel() { return this.experienceLevel; }
        public AccountStatus getStatus() { return this.status; }
        public DateTime getRegistrationDate() { return this.registrationDate; }
        public string getMedicalLimitations() { return this.medicalLimitations; }

        public void setFullName(string fullName) { this.fullName = fullName; }
        public void setPhone(string phone) { this.phone = phone; }
        public void setEmail(string email) { this.email = email; }
        public void setDateOfBirth(DateTime dateOfBirth) { this.dateOfBirth = dateOfBirth; }
        public void setExperienceLevel(string experienceLevel) { this.experienceLevel = experienceLevel; }
        public void setStatus(AccountStatus status) { this.status = status; }
        public void setRegistrationDate(DateTime registrationDate) { this.registrationDate = registrationDate; }
        public void setMedicalLimitations(string medicalLimitations) { this.medicalLimitations = medicalLimitations; }

        public void createTrainee()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_trainee_create @trainee_id, @fullName, @phone, @email, @dateOfBirth, @experienceLevel, @status, @registrationDate, @medicalLimitations";
            cmd.Parameters.AddWithValue("@trainee_id", this.traineeId);
            cmd.Parameters.AddWithValue("@fullName", this.fullName);
            cmd.Parameters.AddWithValue("@phone", this.phone);
            cmd.Parameters.AddWithValue("@email", this.email);
            cmd.Parameters.AddWithValue("@dateOfBirth", this.dateOfBirth);
            cmd.Parameters.AddWithValue("@experienceLevel", this.experienceLevel);
            cmd.Parameters.AddWithValue("@status", this.status.ToString());
            cmd.Parameters.AddWithValue("@registrationDate", this.registrationDate);
            cmd.Parameters.AddWithValue("@medicalLimitations", (object)this.medicalLimitations ?? DBNull.Value);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        public void updateTrainee()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_trainee_update @trainee_id, @fullName, @phone, @email, @dateOfBirth, @experienceLevel, @status, @registrationDate, @medicalLimitations";
            cmd.Parameters.AddWithValue("@trainee_id", this.traineeId);
            cmd.Parameters.AddWithValue("@fullName", this.fullName);
            cmd.Parameters.AddWithValue("@phone", this.phone);
            cmd.Parameters.AddWithValue("@email", this.email);
            cmd.Parameters.AddWithValue("@dateOfBirth", this.dateOfBirth);
            cmd.Parameters.AddWithValue("@experienceLevel", this.experienceLevel);
            cmd.Parameters.AddWithValue("@status", this.status.ToString());
            cmd.Parameters.AddWithValue("@registrationDate", this.registrationDate);
            cmd.Parameters.AddWithValue("@medicalLimitations", (object)this.medicalLimitations ?? DBNull.Value);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        public void deleteTrainee()
        {
            Program.Trainees.Remove(this);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_trainee_delete @trainee_id";
            cmd.Parameters.AddWithValue("@trainee_id", this.traineeId);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        // ====================================================================
        // Domain-verb method (UC02 "Register to Workout") — not CRUD. Calls
        // the orchestrated, guarded, transactional sp_register_to_workout
        // (scripts/stored_procedures.sql) instead of Registration's generic
        // createRegistration(). On success, mirrors every DB side effect onto
        // the in-memory Program lists directly (no further DB round-trips,
        // since the SP already wrote them); on failure, maps the SP's guard
        // error codes (51001-51006) to the matching Hebrew message instead of
        // exposing the raw SqlException to the caller.
        // ====================================================================

        /// <summary>
        /// Registers this trainee to the given training session via the guarded
        /// UC02 flow. Returns null on success, or a Hebrew message describing
        /// which guard failed (to be shown to the user, e.g. in a MessageBox).
        /// </summary>
        public string registerToWorkout(int trainingSessionId)
        {
            int registrationId = Registration.getNextRegistrationId();
            DateTime registrationDate = DateTime.Now;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_register_to_workout @registration_id, @trainee_id, @training_session_id, @registrationDate, @out_membership_id OUTPUT";
            cmd.Parameters.AddWithValue("@registration_id", registrationId);
            cmd.Parameters.AddWithValue("@trainee_id", this.traineeId);
            cmd.Parameters.AddWithValue("@training_session_id", trainingSessionId);
            cmd.Parameters.AddWithValue("@registrationDate", registrationDate);
            SqlParameter outMembershipId = new SqlParameter("@out_membership_id", SqlDbType.Int);
            outMembershipId.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(outMembershipId);

            try
            {
                SQL_CON SC = new SQL_CON();
                SC.execute_non_query_flow(cmd);
            }
            catch (SqlException ex)
            {
                switch (ex.Number)
                {
                    case 51001: return "המתאמן/ת המבוקש/ת אינו/ה קיים/ת במערכת.";
                    case 51002: return "לא ניתן לרשום מתאמן/ת לא פעיל/ה לאימון.";
                    case 51003: return "האימון המבוקש אינו קיים במערכת.";
                    case 51004: return "המתאמן/ת כבר רשום/ה לאימון זה.";
                    case 51005: return "אין למתאמן/ת מנוי בתוקף, או שלא נותרו כניסות במנוי.";
                    case 51006: return "האימון מלא, אין מקומות פנויים.";
                    default: return "שגיאה בביצוע ההרשמה: " + ex.Message;
                }
            }

            // Success — every write already happened in the SP's transaction.
            // Mirror it onto the in-memory lists so the UI reflects it without a reload.
            TrainingSession session = TrainingSession.seekTrainingSession(trainingSessionId);
            session.setAvailableSlots(session.getAvailableSlots() - 1);

            int membershipId = (int)outMembershipId.Value;
            Membership membership = Membership.seekMembership(membershipId);
            membership.setRemainingEntries(membership.getRemainingEntries() - 1);

            Registration registration = new Registration(registrationId, this, session,
                registrationDate, null, null, RegistrationStatus.CONFIRMED, false);
            Program.Registrations.Add(registration);

            return null;
        }

        public static void initTrainees()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_trainee_get_all";
            SQL_CON SC = new SQL_CON();
            SqlDataReader rdr = SC.execute_query(cmd);

            Program.Trainees = new List<Trainee>();

            // rdr ריק פירושו ש-execute_query כבר דיווח על תקלת התחברות;
            // משאירים רשימה ריקה במקום ליפול על NullReferenceException.
            if (rdr == null) return;

            while (rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string fullName = rdr.GetString(1);
                string phone = rdr.GetString(2);
                string email = rdr.GetString(3);
                DateTime dateOfBirth = rdr.GetDateTime(4);
                string experienceLevel = rdr.GetString(5);
                AccountStatus status = (AccountStatus)Enum.Parse(typeof(AccountStatus), rdr.GetString(6));
                DateTime registrationDate = rdr.GetDateTime(7);
                string medicalLimitations = rdr.IsDBNull(8) ? null : rdr.GetString(8);

                Trainee t = new Trainee(id, fullName, phone, email, dateOfBirth, experienceLevel, status, registrationDate, medicalLimitations, false);
                Program.Trainees.Add(t);
            }
        }

        public static Trainee seekTrainee(int traineeId)
        {
            foreach (Trainee t in Program.Trainees)
            {
                if (t.getTraineeId() == traineeId)
                    return t;
            }
            return null;
        }

        public static int getNextTraineeId()
        {
            int maxId = 0;
            foreach (Trainee t in Program.Trainees)
            {
                if (t.getTraineeId() > maxId)
                    maxId = t.getTraineeId();
            }
            return maxId + 1;
        }
    }
}
