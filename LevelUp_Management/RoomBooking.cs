using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace LevelUp_Management
{
    public class RoomBooking
    {
        private int roomBookingId;
        private Room room;
        private Trainer trainer;
        private DateTime date;
        private TimeSpan startTime;
        private TimeSpan endTime;
        private BookingStatus status;
        private double cancellationFee;
        private DateTime? cancellationTimestamp;

        public RoomBooking(int roomBookingId, Room room, Trainer trainer, DateTime date,
            TimeSpan startTime, TimeSpan endTime, BookingStatus status, double cancellationFee,
            DateTime? cancellationTimestamp, bool is_new)
        {
            this.roomBookingId = roomBookingId;
            this.room = room;
            this.trainer = trainer;
            this.date = date;
            this.startTime = startTime;
            this.endTime = endTime;
            this.status = status;
            this.cancellationFee = cancellationFee;
            this.cancellationTimestamp = cancellationTimestamp;
            if (is_new)
            {
                this.createRoomBooking();
                Program.RoomBookings.Add(this);
            }
        }

        public int getRoomBookingId() { return this.roomBookingId; }
        public Room getRoom() { return this.room; }
        public Trainer getTrainer() { return this.trainer; }
        public DateTime getDate() { return this.date; }
        public TimeSpan getStartTime() { return this.startTime; }
        public TimeSpan getEndTime() { return this.endTime; }
        public BookingStatus getStatus() { return this.status; }
        public double getCancellationFee() { return this.cancellationFee; }
        public DateTime? getCancellationTimestamp() { return this.cancellationTimestamp; }

        public void setRoom(Room room) { this.room = room; }
        public void setTrainer(Trainer trainer) { this.trainer = trainer; }
        public void setDate(DateTime date) { this.date = date; }
        public void setStartTime(TimeSpan startTime) { this.startTime = startTime; }
        public void setEndTime(TimeSpan endTime) { this.endTime = endTime; }
        public void setStatus(BookingStatus status) { this.status = status; }
        public void setCancellationFee(double cancellationFee) { this.cancellationFee = cancellationFee; }
        public void setCancellationTimestamp(DateTime? cancellationTimestamp) { this.cancellationTimestamp = cancellationTimestamp; }

        public void createRoomBooking()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_room_booking_create @room_booking_id, @room_id, @trainer_id, @date, @startTime, @endTime, @status, @cancellationFee, @cancellationTimestamp";
            cmd.Parameters.AddWithValue("@room_booking_id", this.roomBookingId);
            cmd.Parameters.AddWithValue("@room_id", this.room.getRoomId());
            cmd.Parameters.AddWithValue("@trainer_id", this.trainer.getTrainerId());
            cmd.Parameters.AddWithValue("@date", this.date);
            cmd.Parameters.AddWithValue("@startTime", this.startTime);
            cmd.Parameters.AddWithValue("@endTime", this.endTime);
            cmd.Parameters.AddWithValue("@status", this.status.ToString());
            cmd.Parameters.AddWithValue("@cancellationFee", this.cancellationFee);
            cmd.Parameters.AddWithValue("@cancellationTimestamp", (object)this.cancellationTimestamp ?? DBNull.Value);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        public void updateRoomBooking()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_room_booking_update @room_booking_id, @room_id, @trainer_id, @date, @startTime, @endTime, @status, @cancellationFee, @cancellationTimestamp";
            cmd.Parameters.AddWithValue("@room_booking_id", this.roomBookingId);
            cmd.Parameters.AddWithValue("@room_id", this.room.getRoomId());
            cmd.Parameters.AddWithValue("@trainer_id", this.trainer.getTrainerId());
            cmd.Parameters.AddWithValue("@date", this.date);
            cmd.Parameters.AddWithValue("@startTime", this.startTime);
            cmd.Parameters.AddWithValue("@endTime", this.endTime);
            cmd.Parameters.AddWithValue("@status", this.status.ToString());
            cmd.Parameters.AddWithValue("@cancellationFee", this.cancellationFee);
            cmd.Parameters.AddWithValue("@cancellationTimestamp", (object)this.cancellationTimestamp ?? DBNull.Value);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        public void deleteRoomBooking()
        {
            Program.RoomBookings.Remove(this);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_room_booking_delete @room_booking_id";
            cmd.Parameters.AddWithValue("@room_booking_id", this.roomBookingId);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        public static void initRoomBookings()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_room_booking_get_all";
            SQL_CON SC = new SQL_CON();
            SqlDataReader rdr = SC.execute_query(cmd);

            Program.RoomBookings = new List<RoomBooking>();

            // rdr ריק פירושו ש-execute_query כבר דיווח על תקלת התחברות;
            // משאירים רשימה ריקה במקום ליפול על NullReferenceException.
            if (rdr == null) return;

            while (rdr.Read())
            {
                int id = rdr.GetInt32(0);
                Room room = Room.seekRoom(rdr.GetInt32(1));
                Trainer trainer = Trainer.seekTrainer(rdr.GetInt32(2));
                DateTime date = rdr.GetDateTime(3);
                TimeSpan startTime = rdr.GetTimeSpan(4);
                TimeSpan endTime = rdr.GetTimeSpan(5);
                BookingStatus status = (BookingStatus)Enum.Parse(typeof(BookingStatus), rdr.GetString(6));
                double cancellationFee = (double)rdr.GetDecimal(7);
                DateTime? cancellationTimestamp = rdr.IsDBNull(8) ? (DateTime?)null : rdr.GetDateTime(8);

                RoomBooking rb = new RoomBooking(id, room, trainer, date, startTime, endTime, status, cancellationFee, cancellationTimestamp, false);
                Program.RoomBookings.Add(rb);
            }
        }

        public static RoomBooking seekRoomBooking(int roomBookingId)
        {
            foreach (RoomBooking rb in Program.RoomBookings)
            {
                if (rb.getRoomBookingId() == roomBookingId)
                    return rb;
            }
            return null;
        }

        public static int getNextRoomBookingId()
        {
            int maxId = 0;
            foreach (RoomBooking rb in Program.RoomBookings)
            {
                if (rb.getRoomBookingId() > maxId)
                    maxId = rb.getRoomBookingId();
            }
            return maxId + 1;
        }
    }
}
