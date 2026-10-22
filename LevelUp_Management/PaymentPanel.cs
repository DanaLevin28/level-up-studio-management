using System;
using System.Data;
using System.Windows.Forms;

namespace LevelUp_Management
{
    public partial class PaymentPanel : UserControl
    {
        private Payment selectedPayment;

        public PaymentPanel()
        {
            InitializeComponent();
            UITheme.applyPanel(this, "התשלומים שבוצעו עבור מנויי הסטודיו");
            loadCombos();
            dtpPaymentDate.Value = DateTime.Today;
            loadPayments();
        }

        private void loadCombos()
        {
            DataTable membershipTable = new DataTable();
            membershipTable.Columns.Add("id", typeof(int));
            membershipTable.Columns.Add("display", typeof(string));
            foreach (Membership m in Program.Memberships)
                membershipTable.Rows.Add(m.getMembershipId(), m.getTrainee().getFullName() + " - " + m.getPlanName() + " (" + m.getMembershipId() + ")");
            cboMembership.DataSource = membershipTable;
            cboMembership.DisplayMember = "display";
            cboMembership.ValueMember = "id";
        }

        private void loadPayments()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("payment_id", typeof(int));
            dt.Columns.Add("membership", typeof(string));
            dt.Columns.Add("amount", typeof(decimal));
            dt.Columns.Add("paymentDate", typeof(DateTime));
            dt.Columns.Add("paymentMethod", typeof(string));
            dt.Columns.Add("paymentStatus", typeof(string));

            foreach (Payment p in Program.Payments)
            {
                dt.Rows.Add(p.getPaymentId(), p.getMembership().getTrainee().getFullName() + " (" + p.getMembership().getMembershipId() + ")",
                    p.getAmount(), p.getPaymentDate(), p.getPaymentMethod(), p.getPaymentStatus());
            }

            dataGridView1.DataSource = dt;

            dataGridView1.Columns["payment_id"].HeaderText = "מזהה";
            dataGridView1.Columns["membership"].HeaderText = "מנוי";
            dataGridView1.Columns["amount"].HeaderText = "סכום";
            dataGridView1.Columns["paymentDate"].HeaderText = "תאריך תשלום";
            dataGridView1.Columns["paymentMethod"].HeaderText = "אמצעי תשלום";
            dataGridView1.Columns["paymentStatus"].HeaderText = "סטטוס תשלום";
            dataGridView1.Columns["paymentDate"].DefaultCellStyle.Format = "dd/MM/yyyy";
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;  //לחיצה על כותרת — מתעלמים

            int id = int.Parse(dataGridView1.Rows[e.RowIndex].Cells["payment_id"].Value.ToString());
            selectedPayment = Payment.seekPayment(id);
            if (selectedPayment == null) return;

            cboMembership.SelectedValue = selectedPayment.getMembership().getMembershipId();
            txtAmount.Text = selectedPayment.getAmount().ToString();
            dtpPaymentDate.Value = selectedPayment.getPaymentDate();
            txtPaymentMethod.Text = selectedPayment.getPaymentMethod();
            txtPaymentStatus.Text = selectedPayment.getPaymentStatus();
        }

        private bool validateFields()
        {
            if (cboMembership.SelectedValue == null)
            {
                MessageBox.Show("יש לבחור מנוי", "שגיאה", MessageBoxButtons.OK);
                return false;
            }
            if (!decimal.TryParse(txtAmount.Text, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("יש להזין סכום תקין", "שגיאה", MessageBoxButtons.OK);
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtPaymentMethod.Text))
            {
                MessageBox.Show("יש להזין אמצעי תשלום", "שגיאה", MessageBoxButtons.OK);
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtPaymentStatus.Text))
            {
                MessageBox.Show("יש להזין סטטוס תשלום", "שגיאה", MessageBoxButtons.OK);
                return false;
            }
            return true;
        }

        private void clearFields()
        {
            selectedPayment = null;
            if (cboMembership.Items.Count > 0) cboMembership.SelectedIndex = 0;
            txtAmount.Clear();
            dtpPaymentDate.Value = DateTime.Today;
            txtPaymentMethod.Clear();
            txtPaymentStatus.Clear();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!validateFields()) return;

            int id = Payment.getNextPaymentId();
            Membership membership = Membership.seekMembership((int)cboMembership.SelectedValue);

            Payment p = new Payment(id, membership, decimal.Parse(txtAmount.Text), dtpPaymentDate.Value.Date,
                txtPaymentMethod.Text, txtPaymentStatus.Text, null, true);

            MessageBox.Show("התשלום נוצר בהצלחה", "הודעה", MessageBoxButtons.OK);
            clearFields();
            loadPayments();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedPayment == null)
            {
                MessageBox.Show("יש לבחור תשלום מהרשימה", "שגיאה", MessageBoxButtons.OK);
                return;
            }
            if (!validateFields()) return;

            selectedPayment.setMembership(Membership.seekMembership((int)cboMembership.SelectedValue));
            selectedPayment.setAmount(decimal.Parse(txtAmount.Text));
            selectedPayment.setPaymentDate(dtpPaymentDate.Value.Date);
            selectedPayment.setPaymentMethod(txtPaymentMethod.Text);
            selectedPayment.setPaymentStatus(txtPaymentStatus.Text);
            selectedPayment.updatePayment();

            MessageBox.Show("התשלום עודכן בהצלחה", "הודעה", MessageBoxButtons.OK);
            clearFields();
            loadPayments();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedPayment == null)
            {
                MessageBox.Show("יש לבחור תשלום מהרשימה", "שגיאה", MessageBoxButtons.OK);
                return;
            }

            selectedPayment.deletePayment();

            MessageBox.Show("התשלום נמחק בהצלחה", "הודעה", MessageBoxButtons.OK);
            clearFields();
            loadPayments();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            mainForm.showPanel(null);
        }
    }
}
