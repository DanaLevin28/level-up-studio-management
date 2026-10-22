using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace LevelUp_Management
{
    // Association Class: links Trainee <-> TrainingSession (per CLAUDE.md Association
    // Class pattern, both sides are stored as object references, not raw IDs).
    public class Registration
    {
        private int registrationId;
        private Trainee trainee;
        private TrainingSession trainingSession;
        private DateTime registrationDate;
        private DateTime? cancellationTimestamp;
        private string cancellationReason;
        private RegistrationStatus status;

        public Registration(int registrationId, Trainee trainee, TrainingSession trainingSession,
            DateTime registrationDate, DateTime? cancellationTimestamp, string cancellationReason,
            RegistrationStatus status, bool is_new)
        {
            this.registrationId = registrationId;
            this.trainee = trainee;
            this.trainingSession = trainingSession;
            this.registrationDate = registrationDate;
            this.cancellationTimestamp = cancellationTimestamp;
            this.cancellationReason = cancellationReason;
            this.status = status;
            if (is_new)
            {
                this.createRegistration();
                Program.Registrations.Add(this);
            }
        }

        public int getRegistrationId() { return this.registrationId; }
        public Trainee getTrainee() { return this.trainee; }
        public TrainingSession getTrainingSession() { return this.trainingSession; }
        public DateTime getRegistrationDate() { return this.registrationDate; }
        public DateTime? getCancellationTimestamp() { return this.cancellationTimestamp; }
        public string getCancellationReason() { return this.cancellationReason; }
        public RegistrationStatus getStatus() { return this.status; }

        public void setTrainee(Trainee trainee) { this.trainee = trainee; }
        public void setTrainingSession(TrainingSession trainingSession) { this.trainingSession = trainingSession; }
        public void setRegistrationDate(DateTime registrationDate) { this.registrationDate = registrationDate; }
        public void setCancellationTimestamp(DateTime? cancellationTimestamp) { this.cancellationTimestamp = cancellationTimestamp; }
        public void setCancellationReason(string cancellationReason) { this.cancellationReason = cancellationReason; }
        public void setStatus(RegistrationStatus status) { this.status = status; }

        public void createRegistration()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_registration_create @registration_id, @trainee_id, @training_session_id, @registrationDate, @cancellationTimestamp, @cancellationReason, @status";
            cmd.Parameters.AddWithValue("@registration_id", this.registrationId);
            cmd.Parameters.AddWithValue("@trainee_id", this.trainee.getTraineeId());
            cmd.Parameters.AddWithValue("@training_session_id", this.trainingSession.getTrainingSessionId());
            cmd.Parameters.AddWithValue("@registrationDate", this.registrationDate);
            cmd.Parameters.AddWithValue("@cancellationTimestamp", (object)this.cancellationTimestamp ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@cancellationReason", (object)this.cancellationReason ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@status", this.status.ToString());
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        public void updateRegistration()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_registration_update @registration_id, @trainee_id, @training_session_id, @registrationDate, @cancellationTimestamp, @cancellationReason, @status";
            cmd.Parameters.AddWithValue("@registration_id", this.registrationId);
            cmd.Parameters.AddWithValue("@trainee_id", this.trainee.getTraineeId());
            cmd.Parameters.AddWithValue("@training_session_id", this.trainingSession.getTrainingSessionId());
            cmd.Parameters.AddWithValue("@registrationDate", this.registrationDate);
            cmd.Parameters.AddWithValue("@cancellationTimestamp", (object)this.cancellationTimestamp ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@cancellationReason", (object)this.cancellationReason ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@status", this.status.ToString());
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        public void deleteRegistration()
        {
            Program.Registrations.Remove(this);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_registration_delete @registration_id";
            cmd.Parameters.AddWithValue("@registration_id", this.registrationId);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        // ====================================================================
        // State-transition methods (docs/design/state-diagram.md — "2.
        // Registration State Diagram"). Domain-verb methods, not CRUD: each
        // checks its transition's guard against this object's current
        // in-memory status — and, where the diagram's guard references
        // outside data (membership validity, time until class start),
        // against that data too — then calls the matching orchestrated,
        // transactional stored procedure (scripts/stored_procedures.sql). On
        // success, the in-memory fields are mirrored directly so the running
        // app reflects the change without a reload.
        // ====================================================================

        /// <summary>
        /// Pending -> Confirmed (transition 5, guard [paymentProcessed]).
        /// "Payment processed" is interpreted the same way UC02's existing
        /// membership-validity check does: the trainee holds an ACTIVE,
        /// unexpired membership with entries remaining.
        /// </summary>
        public void confirm()
        {
            if (this.status != RegistrationStatus.PENDING)
                throw new InvalidOperationException("ניתן לאשר רק רישום הממתין לאישור.");

            bool hasValidMembership = false;
            foreach (Membership m in Program.Memberships)
            {
                if (m.getTrainee() == this.trainee && m.getStatus() == MembershipStatus.ACTIVE &&
                    m.getEndDate() >= DateTime.Today && m.getRemainingEntries() > 0)
                {
                    hasValidMembership = true;
                    break;
                }
            }
            if (!hasValidMembership)
                throw new InvalidOperationException("אין למתאמן/ת מנוי בתוקף עם כניסות זמינות — לא ניתן לאשר את הרישום.");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_registration_confirm @registration_id";
            cmd.Parameters.AddWithValue("@registration_id", this.registrationId);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query_flow(cmd);

            this.status = RegistrationStatus.CONFIRMED;

            // TODO: if this becomes the real registration-creation path (Pending
            // -> Confirmed) rather than sp_register_to_workout's direct-to-CONFIRMED
            // flow, the group needs to decide where remainingEntries gets
            // decremented, to avoid double-decrementing across both flows.
        }

        /// <summary>
        /// Pending/Waitlisted -> Cancelled (transitions 3-4, no guard), or
        /// Confirmed -> Cancelled (transitions 9-10, guard [over 12 hours] /
        /// [under 12 hours]). Single public entry point matching the
        /// diagram's one "cancel" trigger: internally dispatches to a plain
        /// cancel or a late cancel (with chargeLateFee) depending on hours
        /// remaining before class start — the hoursUntilStart calculation
        /// exists in exactly this one place. Per
        /// docs/design/state-diagram.md's implementation-scoping note, #9/#10
        /// are treated as scoped to Confirmed only, not the group transition
        /// off ActiveBooking the source diagram literally draws — Active
        /// means the class already started, which doesn't fit "cancel."
        /// </summary>
        public void cancel(string reason)
        {
            RegistrationStatus previousStatus = this.status;

            if (this.status == RegistrationStatus.CONFIRMED)
            {
                double hoursUntilStart = (this.trainingSession.getDate().Date + this.trainingSession.getStartTime() - DateTime.Now).TotalHours;
                if (hoursUntilStart <= 12)
                {
                    lateCancel(reason);
                    return;
                }
            }
            else if (this.status != RegistrationStatus.PENDING && this.status != RegistrationStatus.WAITLISTED)
            {
                throw new InvalidOperationException("לא ניתן לבטל רישום במצב זה.");
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_registration_cancel @registration_id, @cancellationReason";
            cmd.Parameters.AddWithValue("@registration_id", this.registrationId);
            cmd.Parameters.AddWithValue("@cancellationReason", (object)reason ?? DBNull.Value);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query_flow(cmd);

            this.status = RegistrationStatus.CANCELLED;
            this.cancellationTimestamp = DateTime.Now;
            this.cancellationReason = reason;

            if (previousStatus == RegistrationStatus.CONFIRMED)
                releaseSlot();
        }

        /// <summary>
        /// A cancelled CONFIRMED registration gives its spot back to the session.
        /// sp_registration_cancel / sp_registration_late_cancel don't touch
        /// availableSlots, so it is restored here (capped at the session capacity =
        /// min(room, workout type)) and saved through updateTrainingSession().
        /// </summary>
        private void releaseSlot()
        {
            int capacity = Math.Min(this.trainingSession.getRoom().getMaxCapacity(),
                                    this.trainingSession.getWorkoutType().getMaxCapacity());
            if (this.trainingSession.getAvailableSlots() >= capacity) return;

            this.trainingSession.setAvailableSlots(this.trainingSession.getAvailableSlots() + 1);
            this.trainingSession.updateTrainingSession();
        }

        /// <summary>
        /// Confirmed -> Cancelled with 12 hours or less before class start
        /// (transition 10, guard [under 12 hours], effect / chargeLateFee).
        /// Private: only reachable through cancel()'s own branching above —
        /// the guard is already checked there before dispatch, so this
        /// method trusts the caller and just performs the write. The
        /// diagram has one "cancel" trigger, not two separate
        /// caller-visible actions.
        /// </summary>
        private void lateCancel(string reason)
        {
            RegistrationStatus previousStatus = this.status;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_registration_late_cancel @registration_id, @cancellationReason";
            cmd.Parameters.AddWithValue("@registration_id", this.registrationId);
            cmd.Parameters.AddWithValue("@cancellationReason", (object)reason ?? DBNull.Value);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query_flow(cmd);

            this.status = RegistrationStatus.CANCELLED;
            this.cancellationTimestamp = DateTime.Now;
            this.cancellationReason = reason;

            if (previousStatus == RegistrationStatus.CONFIRMED)
                releaseSlot();

            // chargeLateFee() is a stub (throws NotImplementedException — see its own TODO).
            // Isolated here so the cancellation above, which already succeeded, is never
            // rolled back or reported as failed just because the fee side effect isn't built
            // yet. Once chargeLateFee() is implemented for real, any other exception it throws
            // (e.g. a SqlException from the fee write) will no longer be caught here and will
            // propagate normally — only the "not implemented" case is swallowed.
            try
            {
                this.chargeLateFee();
            }
            catch (NotImplementedException)
            {
            }
        }

        /// <summary>
        /// Side effect of transition 10 (Confirmed -> Cancelled, under 12 hours before
        /// class start). Stub only — matches the purge() pattern. Called from lateCancel()
        /// (isolated in its own try/catch there so this not being built yet never fails the
        /// cancellation itself).
        /// </summary>
        public void chargeLateFee()
        {
            // TODO: state-diagram.md's transition 10 effect is `/ chargeLateFee`, אך לא
            // מתועד באף מקום מה גובה הקנס או היכן יש לתעד אותו (שורת Payment חדשה כנגד
            // המנוי של המתאמן? טבלת קנסות ייעודית?). נדרשת החלטת קבוצה לפני מימוש בפועל.
            throw new NotImplementedException("chargeLateFee() טרם מומש - נדרש החלטת קבוצה, ראו הערת TODO.");
        }

        /// <summary>Confirmed -> Active (transition 7, guard [isClassTime]).</summary>
        public void startClass()
        {
            if (this.status != RegistrationStatus.CONFIRMED)
                throw new InvalidOperationException("ניתן להתחיל אימון רק לרישום מאושר.");

            DateTime sessionStart = this.trainingSession.getDate().Date + this.trainingSession.getStartTime();
            if (DateTime.Now < sessionStart)
                throw new InvalidOperationException("האימון עדיין לא התחיל.");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_registration_start_class @registration_id";
            cmd.Parameters.AddWithValue("@registration_id", this.registrationId);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query_flow(cmd);

            this.status = RegistrationStatus.ACTIVE;
        }

        /// <summary>
        /// Confirmed -> No_Show (transition 8, no bracketed guard beyond
        /// status). Per docs/design/state-diagram.md's corrected
        /// transcription (verified against the original Visual Paradigm PDF),
        /// this fires from Confirmed only — not a group transition off
        /// ActiveBooking.
        /// </summary>
        public void missClass()
        {
            if (this.status != RegistrationStatus.CONFIRMED)
                throw new InvalidOperationException("ניתן לסמן היעדרות רק לרישום מאושר שטרם החל.");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_registration_miss_class @registration_id";
            cmd.Parameters.AddWithValue("@registration_id", this.registrationId);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query_flow(cmd);

            this.status = RegistrationStatus.NO_SHOW;
        }

        /// <summary>Active -> Completed (transition 11, no guard).</summary>
        public void finishClass()
        {
            if (this.status != RegistrationStatus.ACTIVE)
                throw new InvalidOperationException("ניתן לסיים רק אימון פעיל.");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_registration_finish_class @registration_id";
            cmd.Parameters.AddWithValue("@registration_id", this.registrationId);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query_flow(cmd);

            this.status = RegistrationStatus.COMPLETED;
        }

        /// <summary>
        /// Cancelled/No_Show/Completed -> Final (transitions 12-14, trigger
        /// after(7 years), effect / purge). Stub only, kept for Class
        /// Diagram / State Diagram consistency — deliberately not wired to
        /// any UI button, since this transition is time-triggered rather
        /// than user-triggered.
        /// </summary>
        public void purge()
        {
            if (this.status != RegistrationStatus.CANCELLED && this.status != RegistrationStatus.NO_SHOW &&
                this.status != RegistrationStatus.COMPLETED)
                throw new InvalidOperationException("ניתן למחוק לצמיתות רק רישום שבוטל, שהסתיים בהיעדרות, או שהושלם.");

            // TODO: לא מומש בפועל. המעבר למצב Final מתבצע כעבור 7 שנים משעת
            // הכניסה למצב CANCELLED/NO_SHOW/COMPLETED (מעברים 12-14 בתרשים
            // המצבים), ולא כתוצאה מפעולת משתמש במסך - אין לחבר כפתור UI
            // לפעולה זו. מימוש אמיתי דורש תהליך מתוזמן (SQL Agent Job, או
            // Scheduler ברמת האפליקציה) שסורק את טבלת Registrations, מאתר
            // רשומות שחלפו 7 שנים ממועד הכניסה למצב, ומפעיל את הפעולה
            // בפועל. גם אופי הפעולה עצמה (מחיקה לצמיתות מול העברה לארכיון)
            // טרם הוכרע בקבוצה - אין ליישם עד להחלטה.
            throw new NotImplementedException("purge() טרם מומש - נדרש תהליך מתוזמן, ראו הערת TODO.");
        }

        public static void initRegistrations()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_registration_get_all";
            SQL_CON SC = new SQL_CON();
            SqlDataReader rdr = SC.execute_query(cmd);

            Program.Registrations = new List<Registration>();

            // rdr ריק פירושו ש-execute_query כבר דיווח על תקלת התחברות;
            // משאירים רשימה ריקה במקום ליפול על NullReferenceException.
            if (rdr == null) return;

            while (rdr.Read())
            {
                int id = rdr.GetInt32(0);
                Trainee trainee = Trainee.seekTrainee(rdr.GetInt32(1));
                TrainingSession trainingSession = TrainingSession.seekTrainingSession(rdr.GetInt32(2));
                DateTime registrationDate = rdr.GetDateTime(3);
                DateTime? cancellationTimestamp = rdr.IsDBNull(4) ? (DateTime?)null : rdr.GetDateTime(4);
                string cancellationReason = rdr.IsDBNull(5) ? null : rdr.GetString(5);
                RegistrationStatus status = (RegistrationStatus)Enum.Parse(typeof(RegistrationStatus), rdr.GetString(6));

                Registration r = new Registration(id, trainee, trainingSession, registrationDate, cancellationTimestamp, cancellationReason, status, false);
                Program.Registrations.Add(r);
            }
        }

        public static Registration seekRegistration(int registrationId)
        {
            foreach (Registration r in Program.Registrations)
            {
                if (r.getRegistrationId() == registrationId)
                    return r;
            }
            return null;
        }

        public static int getNextRegistrationId()
        {
            int maxId = 0;
            foreach (Registration r in Program.Registrations)
            {
                if (r.getRegistrationId() > maxId)
                    maxId = r.getRegistrationId();
            }
            return maxId + 1;
        }
    }
}
