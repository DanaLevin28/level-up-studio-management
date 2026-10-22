using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace LevelUp_Management
{
    public class Equipment
    {
        private int equipmentId;
        private Room room;
        private string barcode;
        private string itemName;
        private DateTime purchaseDate;
        private EquipmentCondition conditionStatus;

        public Equipment(int equipmentId, Room room, string barcode, string itemName,
            DateTime purchaseDate, EquipmentCondition conditionStatus, bool is_new)
        {
            this.equipmentId = equipmentId;
            this.room = room;
            this.barcode = barcode;
            this.itemName = itemName;
            this.purchaseDate = purchaseDate;
            this.conditionStatus = conditionStatus;
            if (is_new)
            {
                this.createEquipment();
                Program.Equipment.Add(this);
            }
        }

        public int getEquipmentId() { return this.equipmentId; }
        public Room getRoom() { return this.room; }
        public string getBarcode() { return this.barcode; }
        public string getItemName() { return this.itemName; }
        public DateTime getPurchaseDate() { return this.purchaseDate; }
        public EquipmentCondition getConditionStatus() { return this.conditionStatus; }

        public void setRoom(Room room) { this.room = room; }
        public void setBarcode(string barcode) { this.barcode = barcode; }
        public void setItemName(string itemName) { this.itemName = itemName; }
        public void setPurchaseDate(DateTime purchaseDate) { this.purchaseDate = purchaseDate; }
        public void setConditionStatus(EquipmentCondition conditionStatus) { this.conditionStatus = conditionStatus; }

        public void createEquipment()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_equipment_create @equipment_id, @room_id, @barcode, @itemName, @purchaseDate, @conditionStatus";
            cmd.Parameters.AddWithValue("@equipment_id", this.equipmentId);
            cmd.Parameters.AddWithValue("@room_id", this.room.getRoomId());
            cmd.Parameters.AddWithValue("@barcode", this.barcode);
            cmd.Parameters.AddWithValue("@itemName", this.itemName);
            cmd.Parameters.AddWithValue("@purchaseDate", this.purchaseDate);
            cmd.Parameters.AddWithValue("@conditionStatus", this.conditionStatus.ToString());
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        public void updateEquipment()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_equipment_update @equipment_id, @room_id, @barcode, @itemName, @purchaseDate, @conditionStatus";
            cmd.Parameters.AddWithValue("@equipment_id", this.equipmentId);
            cmd.Parameters.AddWithValue("@room_id", this.room.getRoomId());
            cmd.Parameters.AddWithValue("@barcode", this.barcode);
            cmd.Parameters.AddWithValue("@itemName", this.itemName);
            cmd.Parameters.AddWithValue("@purchaseDate", this.purchaseDate);
            cmd.Parameters.AddWithValue("@conditionStatus", this.conditionStatus.ToString());
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        public void deleteEquipment()
        {
            Program.Equipment.Remove(this);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_equipment_delete @equipment_id";
            cmd.Parameters.AddWithValue("@equipment_id", this.equipmentId);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        public static void initEquipment()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_equipment_get_all";
            SQL_CON SC = new SQL_CON();
            SqlDataReader rdr = SC.execute_query(cmd);

            Program.Equipment = new List<Equipment>();

            // rdr ריק פירושו ש-execute_query כבר דיווח על תקלת התחברות;
            // משאירים רשימה ריקה במקום ליפול על NullReferenceException.
            if (rdr == null) return;

            while (rdr.Read())
            {
                int id = rdr.GetInt32(0);
                Room room = Room.seekRoom(rdr.GetInt32(1));
                string barcode = rdr.GetString(2);
                string itemName = rdr.GetString(3);
                DateTime purchaseDate = rdr.GetDateTime(4);
                EquipmentCondition conditionStatus = (EquipmentCondition)Enum.Parse(typeof(EquipmentCondition), rdr.GetString(5));

                Equipment e = new Equipment(id, room, barcode, itemName, purchaseDate, conditionStatus, false);
                Program.Equipment.Add(e);
            }
        }

        public static Equipment seekEquipment(int equipmentId)
        {
            foreach (Equipment e in Program.Equipment)
            {
                if (e.getEquipmentId() == equipmentId)
                    return e;
            }
            return null;
        }

        public static int getNextEquipmentId()
        {
            int maxId = 0;
            foreach (Equipment e in Program.Equipment)
            {
                if (e.getEquipmentId() > maxId)
                    maxId = e.getEquipmentId();
            }
            return maxId + 1;
        }
    }
}
