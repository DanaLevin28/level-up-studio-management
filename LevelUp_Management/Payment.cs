using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace LevelUp_Management
{
    public class Payment
    {
        private int paymentId;
        private Membership membership;
        private decimal amount;
        private DateTime paymentDate;
        private string paymentMethod;
        private string paymentStatus;
        private string receiptNumber;

        public Payment(int paymentId, Membership membership, decimal amount, DateTime paymentDate,
            string paymentMethod, string paymentStatus, string receiptNumber, bool is_new)
        {
            this.paymentId = paymentId;
            this.membership = membership;
            this.amount = amount;
            this.paymentDate = paymentDate;
            this.paymentMethod = paymentMethod;
            this.paymentStatus = paymentStatus;
            this.receiptNumber = receiptNumber;
            if (is_new)
            {
                this.createPayment();
                Program.Payments.Add(this);
            }
        }

        public int getPaymentId() { return this.paymentId; }
        public Membership getMembership() { return this.membership; }
        public decimal getAmount() { return this.amount; }
        public DateTime getPaymentDate() { return this.paymentDate; }
        public string getPaymentMethod() { return this.paymentMethod; }
        public string getPaymentStatus() { return this.paymentStatus; }
        public string getReceiptNumber() { return this.receiptNumber; }

        public void setMembership(Membership membership) { this.membership = membership; }
        public void setAmount(decimal amount) { this.amount = amount; }
        public void setPaymentDate(DateTime paymentDate) { this.paymentDate = paymentDate; }
        public void setPaymentMethod(string paymentMethod) { this.paymentMethod = paymentMethod; }
        public void setPaymentStatus(string paymentStatus) { this.paymentStatus = paymentStatus; }
        public void setReceiptNumber(string receiptNumber) { this.receiptNumber = receiptNumber; }

        public void createPayment()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_payment_create @payment_id, @membership_id, @amount, @paymentDate, @paymentMethod, @paymentStatus, @receiptNumber";
            cmd.Parameters.AddWithValue("@payment_id", this.paymentId);
            cmd.Parameters.AddWithValue("@membership_id", this.membership.getMembershipId());
            cmd.Parameters.AddWithValue("@amount", this.amount);
            cmd.Parameters.AddWithValue("@paymentDate", this.paymentDate);
            cmd.Parameters.AddWithValue("@paymentMethod", this.paymentMethod);
            cmd.Parameters.AddWithValue("@paymentStatus", this.paymentStatus);
            cmd.Parameters.AddWithValue("@receiptNumber", (object)this.receiptNumber ?? DBNull.Value);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        public void updatePayment()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_payment_update @payment_id, @membership_id, @amount, @paymentDate, @paymentMethod, @paymentStatus, @receiptNumber";
            cmd.Parameters.AddWithValue("@payment_id", this.paymentId);
            cmd.Parameters.AddWithValue("@membership_id", this.membership.getMembershipId());
            cmd.Parameters.AddWithValue("@amount", this.amount);
            cmd.Parameters.AddWithValue("@paymentDate", this.paymentDate);
            cmd.Parameters.AddWithValue("@paymentMethod", this.paymentMethod);
            cmd.Parameters.AddWithValue("@paymentStatus", this.paymentStatus);
            cmd.Parameters.AddWithValue("@receiptNumber", (object)this.receiptNumber ?? DBNull.Value);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        public void deletePayment()
        {
            Program.Payments.Remove(this);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_payment_delete @payment_id";
            cmd.Parameters.AddWithValue("@payment_id", this.paymentId);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        public static void initPayments()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_payment_get_all";
            SQL_CON SC = new SQL_CON();
            SqlDataReader rdr = SC.execute_query(cmd);

            Program.Payments = new List<Payment>();

            // rdr ריק פירושו ש-execute_query כבר דיווח על תקלת התחברות;
            // משאירים רשימה ריקה במקום ליפול על NullReferenceException.
            if (rdr == null) return;

            while (rdr.Read())
            {
                int id = rdr.GetInt32(0);
                Membership membership = Membership.seekMembership(rdr.GetInt32(1));
                decimal amount = rdr.GetDecimal(2);
                DateTime paymentDate = rdr.GetDateTime(3);
                string paymentMethod = rdr.GetString(4);
                string paymentStatus = rdr.GetString(5);
                string receiptNumber = rdr.IsDBNull(6) ? null : rdr.GetString(6);

                Payment p = new Payment(id, membership, amount, paymentDate, paymentMethod, paymentStatus, receiptNumber, false);
                Program.Payments.Add(p);
            }
        }

        public static Payment seekPayment(int paymentId)
        {
            foreach (Payment p in Program.Payments)
            {
                if (p.getPaymentId() == paymentId)
                    return p;
            }
            return null;
        }

        public static int getNextPaymentId()
        {
            int maxId = 0;
            foreach (Payment p in Program.Payments)
            {
                if (p.getPaymentId() > maxId)
                    maxId = p.getPaymentId();
            }
            return maxId + 1;
        }
    }
}
