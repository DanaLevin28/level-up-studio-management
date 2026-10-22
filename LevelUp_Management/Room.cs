using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace LevelUp_Management
{
    public class Room
    {
        private int roomId;
        private string roomName;
        private int maxCapacity;
        private FacilityStatus status;

        public Room(int roomId, string roomName, int maxCapacity, FacilityStatus status, bool is_new)
        {
            this.roomId = roomId;
            this.roomName = roomName;
            this.maxCapacity = maxCapacity;
            this.status = status;
            if (is_new)
            {
                this.createRoom();
                Program.Rooms.Add(this);
            }
        }

        public int getRoomId() { return this.roomId; }
        public string getRoomName() { return this.roomName; }
        public int getMaxCapacity() { return this.maxCapacity; }
        public FacilityStatus getStatus() { return this.status; }

        public void setRoomName(string roomName) { this.roomName = roomName; }
        public void setMaxCapacity(int maxCapacity) { this.maxCapacity = maxCapacity; }
        public void setStatus(FacilityStatus status) { this.status = status; }

        public void createRoom()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_room_create @room_id, @roomName, @maxCapacity, @status";
            cmd.Parameters.AddWithValue("@room_id", this.roomId);
            cmd.Parameters.AddWithValue("@roomName", this.roomName);
            cmd.Parameters.AddWithValue("@maxCapacity", this.maxCapacity);
            cmd.Parameters.AddWithValue("@status", this.status.ToString());
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        public void updateRoom()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_room_update @room_id, @roomName, @maxCapacity, @status";
            cmd.Parameters.AddWithValue("@room_id", this.roomId);
            cmd.Parameters.AddWithValue("@roomName", this.roomName);
            cmd.Parameters.AddWithValue("@maxCapacity", this.maxCapacity);
            cmd.Parameters.AddWithValue("@status", this.status.ToString());
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        public void deleteRoom()
        {
            Program.Rooms.Remove(this);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_room_delete @room_id";
            cmd.Parameters.AddWithValue("@room_id", this.roomId);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        public static void initRooms()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_room_get_all";
            SQL_CON SC = new SQL_CON();
            SqlDataReader rdr = SC.execute_query(cmd);

            Program.Rooms = new List<Room>();

            // rdr ריק פירושו ש-execute_query כבר דיווח על תקלת התחברות;
            // משאירים רשימה ריקה במקום ליפול על NullReferenceException.
            if (rdr == null) return;

            while (rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string roomName = rdr.GetString(1);
                int maxCapacity = rdr.GetInt32(2);
                FacilityStatus status = (FacilityStatus)Enum.Parse(typeof(FacilityStatus), rdr.GetString(3));

                Room r = new Room(id, roomName, maxCapacity, status, false);
                Program.Rooms.Add(r);
            }
        }

        public static Room seekRoom(int roomId)
        {
            foreach (Room r in Program.Rooms)
            {
                if (r.getRoomId() == roomId)
                    return r;
            }
            return null;
        }

        public static int getNextRoomId()
        {
            int maxId = 0;
            foreach (Room r in Program.Rooms)
            {
                if (r.getRoomId() > maxId)
                    maxId = r.getRoomId();
            }
            return maxId + 1;
        }
    }
}
