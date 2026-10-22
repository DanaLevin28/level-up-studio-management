using System;
using System.Data;
using System.Windows.Forms;

namespace LevelUp_Management
{
    public partial class RoomBookingPanel : UserControl
    {
        private RoomBooking selectedRoomBooking;

        public RoomBookingPanel()
        {
            InitializeComponent();
            UITheme.applyPanel(this, "הזמנות חדרים עבור מאמנים שוכרים ומניעת התנגשויות");
            loadCombos();
            dtpDate.Value = DateTime.Today;
            dtpStartTime.Value = DateTime.Today.AddHours(9);
            dtpEndTime.Value = DateTime.Today.AddHours(10);
            loadRoomBookings();
        }

        private void loadCombos()
        {
            DataTable roomTable = new DataTable();
            roomTable.Columns.Add("id", typeof(int));
            roomTable.Columns.Add("display", typeof(string));
            foreach (Room r in Program.Rooms)
                roomTable.Rows.Add(r.getRoomId(), r.getRoomName());
            cboRoom.DataSource = roomTable;
            cboRoom.DisplayMember = "display";
            cboRoom.ValueMember = "id";

            DataTable trainerTable = new DataTable();
            trainerTable.Columns.Add("id", typeof(int));
            trainerTable.Columns.Add("display", typeof(string));
            foreach (Trainer tr in Program.Trainers)
                trainerTable.Rows.Add(tr.getTrainerId(), tr.getFullName() + " (" + tr.getEmploymentType() + ")");
            cboTrainer.DataSource = trainerTable;
            cboTrainer.DisplayMember = "display";
            cboTrainer.ValueMember = "id";

            cboStatus.DataSource = Enum.GetValues(typeof(BookingStatus));
        }

        private void loadRoomBookings()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("room_booking_id", typeof(int));
            dt.Columns.Add("room", typeof(string));
            dt.Columns.Add("trainer", typeof(string));
            dt.Columns.Add("date", typeof(DateTime));
            dt.Columns.Add("startTime", typeof(string));
            dt.Columns.Add("endTime", typeof(string));
            dt.Columns.Add("status", typeof(string));

            foreach (RoomBooking rb in Program.RoomBookings)
            {
                dt.Rows.Add(rb.getRoomBookingId(), rb.getRoom().getRoomName(), rb.getTrainer().getFullName(),
                    rb.getDate(), rb.getStartTime().ToString(@"hh\:mm"), rb.getEndTime().ToString(@"hh\:mm"), rb.getStatus().ToString());
            }

            dataGridView1.DataSource = dt;

            dataGridView1.Columns["room_booking_id"].HeaderText = "מזהה";
            dataGridView1.Columns["room"].HeaderText = "חדר";
            dataGridView1.Columns["trainer"].HeaderText = "מאמן";
            dataGridView1.Columns["date"].HeaderText = "תאריך";
            dataGridView1.Columns["startTime"].HeaderText = "שעת התחלה";
            dataGridView1.Columns["endTime"].HeaderText = "שעת סיום";
            dataGridView1.Columns["status"].HeaderText = "סטטוס";
            dataGridView1.Columns["date"].DefaultCellStyle.Format = "dd/MM/yyyy";
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;  //לחיצה על כותרת — מתעלמים

            int id = int.Parse(dataGridView1.Rows[e.RowIndex].Cells["room_booking_id"].Value.ToString());
            selectedRoomBooking = RoomBooking.seekRoomBooking(id);
            if (selectedRoomBooking == null) return;

            cboRoom.SelectedValue = selectedRoomBooking.getRoom().getRoomId();
            cboTrainer.SelectedValue = selectedRoomBooking.getTrainer().getTrainerId();
            dtpDate.Value = selectedRoomBooking.getDate();
            dtpStartTime.Value = DateTime.Today.Add(selectedRoomBooking.getStartTime());
            dtpEndTime.Value = DateTime.Today.Add(selectedRoomBooking.getEndTime());
            cboStatus.SelectedItem = selectedRoomBooking.getStatus();
        }

        private bool validateFields()
        {
            if (cboRoom.SelectedValue == null)
            {
                MessageBox.Show("יש לבחור חדר", "שגיאה", MessageBoxButtons.OK);
                return false;
            }
            if (cboTrainer.SelectedValue == null)
            {
                MessageBox.Show("יש לבחור מאמן", "שגיאה", MessageBoxButtons.OK);
                return false;
            }
            if (dtpEndTime.Value.TimeOfDay <= dtpStartTime.Value.TimeOfDay)
            {
                MessageBox.Show("שעת הסיום חייבת להיות אחרי שעת ההתחלה", "שגיאה", MessageBoxButtons.OK);
                return false;
            }
            return true;
        }

        private void clearFields()
        {
            selectedRoomBooking = null;
            if (cboRoom.Items.Count > 0) cboRoom.SelectedIndex = 0;
            if (cboTrainer.Items.Count > 0) cboTrainer.SelectedIndex = 0;
            dtpDate.Value = DateTime.Today;
            dtpStartTime.Value = DateTime.Today.AddHours(9);
            dtpEndTime.Value = DateTime.Today.AddHours(10);
            if (cboStatus.Items.Count > 0) cboStatus.SelectedIndex = 0;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!validateFields()) return;

            int id = RoomBooking.getNextRoomBookingId();
            Room room = Room.seekRoom((int)cboRoom.SelectedValue);
            Trainer trainer = Trainer.seekTrainer((int)cboTrainer.SelectedValue);
            BookingStatus status = (BookingStatus)cboStatus.SelectedItem;

            RoomBooking rb = new RoomBooking(id, room, trainer, dtpDate.Value.Date,
                dtpStartTime.Value.TimeOfDay, dtpEndTime.Value.TimeOfDay, status, 0, null, true);

            MessageBox.Show("ההזמנה נוצרה בהצלחה", "הודעה", MessageBoxButtons.OK);
            clearFields();
            loadRoomBookings();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedRoomBooking == null)
            {
                MessageBox.Show("יש לבחור הזמנה מהרשימה", "שגיאה", MessageBoxButtons.OK);
                return;
            }
            if (!validateFields()) return;

            selectedRoomBooking.setRoom(Room.seekRoom((int)cboRoom.SelectedValue));
            selectedRoomBooking.setTrainer(Trainer.seekTrainer((int)cboTrainer.SelectedValue));
            selectedRoomBooking.setDate(dtpDate.Value.Date);
            selectedRoomBooking.setStartTime(dtpStartTime.Value.TimeOfDay);
            selectedRoomBooking.setEndTime(dtpEndTime.Value.TimeOfDay);
            selectedRoomBooking.setStatus((BookingStatus)cboStatus.SelectedItem);
            selectedRoomBooking.updateRoomBooking();

            MessageBox.Show("ההזמנה עודכנה בהצלחה", "הודעה", MessageBoxButtons.OK);
            clearFields();
            loadRoomBookings();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedRoomBooking == null)
            {
                MessageBox.Show("יש לבחור הזמנה מהרשימה", "שגיאה", MessageBoxButtons.OK);
                return;
            }

            selectedRoomBooking.deleteRoomBooking();

            MessageBox.Show("ההזמנה נמחקה בהצלחה", "הודעה", MessageBoxButtons.OK);
            clearFields();
            loadRoomBookings();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            mainForm.showPanel(null);
        }
    }
}
