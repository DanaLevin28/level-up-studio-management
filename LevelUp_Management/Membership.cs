using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace LevelUp_Management
{
    public class Membership
    {
        private int membershipId;
        private Trainee trainee;
        private DateTime purchaseDate;
        private string planName;
        private string planType;
        private double price;
        private int allowedEntries;
        private int validityDays;
        private DateTime startDate;
        private DateTime endDate;
        private int remainingEntries;
        private MembershipStatus status;

        public Membership(int membershipId, Trainee trainee, DateTime purchaseDate, string planName,
            string planType, double price, int allowedEntries, int validityDays,
            DateTime startDate, DateTime endDate, int remainingEntries, MembershipStatus status,
            bool is_new)
        {
            this.membershipId = membershipId;
            this.trainee = trainee;
            this.purchaseDate = purchaseDate;
            this.planName = planName;
            this.planType = planType;
            this.price = price;
            this.allowedEntries = allowedEntries;
            this.validityDays = validityDays;
            this.startDate = startDate;
            this.endDate = endDate;
            this.remainingEntries = remainingEntries;
            this.status = status;
            if (is_new)
            {
                this.createMembership();
                Program.Memberships.Add(this);
            }
        }

        public int getMembershipId() { return this.membershipId; }
        public Trainee getTrainee() { return this.trainee; }
        public DateTime getPurchaseDate() { return this.purchaseDate; }
        public string getPlanName() { return this.planName; }
        public string getPlanType() { return this.planType; }
        public double getPrice() { return this.price; }
        public int getAllowedEntries() { return this.allowedEntries; }
        public int getValidityDays() { return this.validityDays; }
        public DateTime getStartDate() { return this.startDate; }
        public DateTime getEndDate() { return this.endDate; }
        public int getRemainingEntries() { return this.remainingEntries; }
        public MembershipStatus getStatus() { return this.status; }

        public void setTrainee(Trainee trainee) { this.trainee = trainee; }
        public void setPurchaseDate(DateTime purchaseDate) { this.purchaseDate = purchaseDate; }
        public void setPlanName(string planName) { this.planName = planName; }
        public void setPlanType(string planType) { this.planType = planType; }
        public void setPrice(double price) { this.price = price; }
        public void setAllowedEntries(int allowedEntries) { this.allowedEntries = allowedEntries; }
        public void setValidityDays(int validityDays) { this.validityDays = validityDays; }
        public void setStartDate(DateTime startDate) { this.startDate = startDate; }
        public void setEndDate(DateTime endDate) { this.endDate = endDate; }
        public void setRemainingEntries(int remainingEntries) { this.remainingEntries = remainingEntries; }
        public void setStatus(MembershipStatus status) { this.status = status; }

        public void createMembership()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_membership_create @membership_id, @trainee_id, @purchaseDate, @planName, @planType, @price, @allowedEntries, @validityDays, @startDate, @endDate, @remainingEntries, @status";
            cmd.Parameters.AddWithValue("@membership_id", this.membershipId);
            cmd.Parameters.AddWithValue("@trainee_id", this.trainee.getTraineeId());
            cmd.Parameters.AddWithValue("@purchaseDate", this.purchaseDate);
            cmd.Parameters.AddWithValue("@planName", this.planName);
            cmd.Parameters.AddWithValue("@planType", this.planType);
            cmd.Parameters.AddWithValue("@price", this.price);
            cmd.Parameters.AddWithValue("@allowedEntries", this.allowedEntries);
            cmd.Parameters.AddWithValue("@validityDays", this.validityDays);
            cmd.Parameters.AddWithValue("@startDate", this.startDate);
            cmd.Parameters.AddWithValue("@endDate", this.endDate);
            cmd.Parameters.AddWithValue("@remainingEntries", this.remainingEntries);
            cmd.Parameters.AddWithValue("@status", this.status.ToString());
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        public void updateMembership()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_membership_update @membership_id, @trainee_id, @purchaseDate, @planName, @planType, @price, @allowedEntries, @validityDays, @startDate, @endDate, @remainingEntries, @status";
            cmd.Parameters.AddWithValue("@membership_id", this.membershipId);
            cmd.Parameters.AddWithValue("@trainee_id", this.trainee.getTraineeId());
            cmd.Parameters.AddWithValue("@purchaseDate", this.purchaseDate);
            cmd.Parameters.AddWithValue("@planName", this.planName);
            cmd.Parameters.AddWithValue("@planType", this.planType);
            cmd.Parameters.AddWithValue("@price", this.price);
            cmd.Parameters.AddWithValue("@allowedEntries", this.allowedEntries);
            cmd.Parameters.AddWithValue("@validityDays", this.validityDays);
            cmd.Parameters.AddWithValue("@startDate", this.startDate);
            cmd.Parameters.AddWithValue("@endDate", this.endDate);
            cmd.Parameters.AddWithValue("@remainingEntries", this.remainingEntries);
            cmd.Parameters.AddWithValue("@status", this.status.ToString());
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        public void deleteMembership()
        {
            Program.Memberships.Remove(this);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_membership_delete @membership_id";
            cmd.Parameters.AddWithValue("@membership_id", this.membershipId);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        // ====================================================================
        // State-transition methods (docs/design/state-diagram.md — "1.
        // Membership State Diagram", transitions 4-7). Domain-verb methods,
        // not CRUD: each checks its transition's guard against this object's
        // current in-memory status, then calls the matching orchestrated,
        // transactional stored procedure (scripts/stored_procedures.sql),
        // which applies the status update and any side effect in another
        // table atomically. On success, the in-memory lists are mirrored
        // directly so the running app reflects the change without a reload.
        // ====================================================================

        /// <summary>Active -> FROZEN (transition 4, guard [userRequested]).</summary>
        public void freeze()
        {
            if (this.status != MembershipStatus.ACTIVE)
                throw new InvalidOperationException("ניתן להקפיא רק מנוי פעיל.");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_membership_freeze @membership_id";
            cmd.Parameters.AddWithValue("@membership_id", this.membershipId);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query_flow(cmd);

            this.status = MembershipStatus.FROZEN;
        }

        /// <summary>FROZEN -> Active (transition 5, no guard).</summary>
        public void unfreeze()
        {
            if (this.status != MembershipStatus.FROZEN)
                throw new InvalidOperationException("ניתן לבטל הקפאה רק למנוי מוקפא.");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_membership_unfreeze @membership_id";
            cmd.Parameters.AddWithValue("@membership_id", this.membershipId);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query_flow(cmd);

            this.status = MembershipStatus.ACTIVE;
        }

        /// <summary>
        /// Valid Membership (Active or FROZEN) -> SUSPENDED (transition 6,
        /// guard [paymentFailed]). There is no automated payment-failure
        /// detection flow yet, so this method cannot verify the guard itself
        /// — the caller must have already determined that a payment failed
        /// before calling this.
        /// </summary>
        public void suspend(bool paymentFailed)
        {
            if (this.status != MembershipStatus.ACTIVE && this.status != MembershipStatus.FROZEN)
                throw new InvalidOperationException("ניתן להשעות רק מנוי פעיל או מוקפא.");

            if (!paymentFailed)
                throw new InvalidOperationException("לא ניתן להשעות מנוי ללא כשל תשלום מתועד.");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_membership_suspend @membership_id";
            cmd.Parameters.AddWithValue("@membership_id", this.membershipId);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query_flow(cmd);

            this.status = MembershipStatus.SUSPENDED;

            // TODO: state-diagram.md's SUSPENDED entry lists `do / checkPayment()`
            // but does not document what that action actually does. Needs a
            // group decision before any side effect is built out here.
        }

        /// <summary>
        /// Valid Membership (Active or FROZEN) -> EXPIRED (transition 7,
        /// guard [endDateReached]).
        /// </summary>
        public void expire()
        {
            if (this.status != MembershipStatus.ACTIVE && this.status != MembershipStatus.FROZEN)
                throw new InvalidOperationException("ניתן לפקוע רק מנוי פעיל או מוקפא.");

            if (this.endDate > DateTime.Today)
                throw new InvalidOperationException("לא ניתן לפוג מנוי שטרם הגיע תוקפו.");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_membership_expire @membership_id";
            cmd.Parameters.AddWithValue("@membership_id", this.membershipId);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query_flow(cmd);

            this.status = MembershipStatus.EXPIRED;

            // TODO: state-diagram.md's EXPIRED entry lists `/ archiveMembership()`
            // but does not document what that action actually does. Needs a
            // group decision before any side effect is built out here.
        }

        /// <summary>
        /// EXPIRED or SUSPENDED -> Final (transitions 8/9, trigger
        /// after(7 years), effect / purge). Stub only, kept for Class
        /// Diagram / State Diagram consistency — deliberately not wired to
        /// any UI button, since this transition is time-triggered rather
        /// than user-triggered.
        /// </summary>
        public void purge()
        {
            if (this.status != MembershipStatus.EXPIRED && this.status != MembershipStatus.SUSPENDED)
                throw new InvalidOperationException("ניתן למחוק לצמיתות רק מנוי שפג תוקפו או הושעה.");

            // TODO: לא מומש בפועל. המעבר למצב Final מתבצע כעבור 7 שנים משעת
            // הכניסה למצב EXPIRED/SUSPENDED (מעברים 8-9 בתרשים המצבים), ולא
            // כתוצאה מפעולת משתמש במסך - אין לחבר כפתור UI לפעולה זו.
            // מימוש אמיתי דורש תהליך מתוזמן (SQL Agent Job, או Scheduler
            // ברמת האפליקציה) שסורק את טבלת Memberships, מאתר רשומות
            // EXPIRED/SUSPENDED שחלפו 7 שנים ממועד הכניסה למצב, ומפעיל את
            // הפעולה בפועל. גם אופי הפעולה עצמה (מחיקת השורה לצמיתות מול
            // העברה לארכיון) טרם הוכרע בקבוצה - אין ליישם עד להחלטה.
            throw new NotImplementedException("purge() טרם מומש - נדרש תהליך מתוזמן, ראו הערת TODO.");
        }

        public static void initMemberships()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_membership_get_all";
            SQL_CON SC = new SQL_CON();
            SqlDataReader rdr = SC.execute_query(cmd);

            Program.Memberships = new List<Membership>();

            // rdr ריק פירושו ש-execute_query כבר דיווח על תקלת התחברות;
            // משאירים רשימה ריקה במקום ליפול על NullReferenceException.
            if (rdr == null) return;

            while (rdr.Read())
            {
                int id = rdr.GetInt32(0);
                Trainee trainee = Trainee.seekTrainee(rdr.GetInt32(1));
                DateTime purchaseDate = rdr.GetDateTime(2);
                string planName = rdr.GetString(3);
                string planType = rdr.GetString(4);
                double price = (double)rdr.GetDecimal(5);
                int allowedEntries = rdr.GetInt32(6);
                int validityDays = rdr.GetInt32(7);
                DateTime startDate = rdr.GetDateTime(8);
                DateTime endDate = rdr.GetDateTime(9);
                int remainingEntries = rdr.GetInt32(10);
                MembershipStatus status = (MembershipStatus)Enum.Parse(typeof(MembershipStatus), rdr.GetString(11));

                Membership m = new Membership(id, trainee, purchaseDate, planName, planType, price,
                    allowedEntries, validityDays, startDate, endDate, remainingEntries, status, false);
                Program.Memberships.Add(m);
            }
        }

        public static Membership seekMembership(int membershipId)
        {
            foreach (Membership m in Program.Memberships)
            {
                if (m.getMembershipId() == membershipId)
                    return m;
            }
            return null;
        }

        public static int getNextMembershipId()
        {
            int maxId = 0;
            foreach (Membership m in Program.Memberships)
            {
                if (m.getMembershipId() > maxId)
                    maxId = m.getMembershipId();
            }
            return maxId + 1;
        }
    }
}
