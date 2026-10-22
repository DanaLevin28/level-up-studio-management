using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace LevelUp_Management
{
    public class WorkoutType
    {
        private int workoutTypeId;
        private string name;
        private int maxDurationMinutes;
        private int maxCapacity;
        private string difficultyLevel;
        private FacilityStatus status;

        public WorkoutType(int workoutTypeId, string name, int maxDurationMinutes, int maxCapacity,
            string difficultyLevel, FacilityStatus status, bool is_new)
        {
            this.workoutTypeId = workoutTypeId;
            this.name = name;
            this.maxDurationMinutes = maxDurationMinutes;
            this.maxCapacity = maxCapacity;
            this.difficultyLevel = difficultyLevel;
            this.status = status;
            if (is_new)
            {
                this.createWorkoutType();
                Program.WorkoutTypes.Add(this);
            }
        }

        public int getWorkoutTypeId() { return this.workoutTypeId; }
        public string getName() { return this.name; }
        public int getMaxDurationMinutes() { return this.maxDurationMinutes; }
        public int getMaxCapacity() { return this.maxCapacity; }
        public string getDifficultyLevel() { return this.difficultyLevel; }
        public FacilityStatus getStatus() { return this.status; }

        public void setName(string name) { this.name = name; }
        public void setMaxDurationMinutes(int maxDurationMinutes) { this.maxDurationMinutes = maxDurationMinutes; }
        public void setMaxCapacity(int maxCapacity) { this.maxCapacity = maxCapacity; }
        public void setDifficultyLevel(string difficultyLevel) { this.difficultyLevel = difficultyLevel; }
        public void setStatus(FacilityStatus status) { this.status = status; }

        public void createWorkoutType()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_workout_type_create @workout_type_id, @name, @maxDurationMinutes, @maxCapacity, @difficultyLevel, @status";
            cmd.Parameters.AddWithValue("@workout_type_id", this.workoutTypeId);
            cmd.Parameters.AddWithValue("@name", this.name);
            cmd.Parameters.AddWithValue("@maxDurationMinutes", this.maxDurationMinutes);
            cmd.Parameters.AddWithValue("@maxCapacity", this.maxCapacity);
            cmd.Parameters.AddWithValue("@difficultyLevel", this.difficultyLevel);
            cmd.Parameters.AddWithValue("@status", this.status.ToString());
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        public void updateWorkoutType()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_workout_type_update @workout_type_id, @name, @maxDurationMinutes, @maxCapacity, @difficultyLevel, @status";
            cmd.Parameters.AddWithValue("@workout_type_id", this.workoutTypeId);
            cmd.Parameters.AddWithValue("@name", this.name);
            cmd.Parameters.AddWithValue("@maxDurationMinutes", this.maxDurationMinutes);
            cmd.Parameters.AddWithValue("@maxCapacity", this.maxCapacity);
            cmd.Parameters.AddWithValue("@difficultyLevel", this.difficultyLevel);
            cmd.Parameters.AddWithValue("@status", this.status.ToString());
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        public void deleteWorkoutType()
        {
            Program.WorkoutTypes.Remove(this);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_workout_type_delete @workout_type_id";
            cmd.Parameters.AddWithValue("@workout_type_id", this.workoutTypeId);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        public static void initWorkoutTypes()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_workout_type_get_all";
            SQL_CON SC = new SQL_CON();
            SqlDataReader rdr = SC.execute_query(cmd);

            Program.WorkoutTypes = new List<WorkoutType>();

            // rdr ריק פירושו ש-execute_query כבר דיווח על תקלת התחברות;
            // משאירים רשימה ריקה במקום ליפול על NullReferenceException.
            if (rdr == null) return;

            while (rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string name = rdr.GetString(1);
                int maxDurationMinutes = rdr.GetInt32(2);
                int maxCapacity = rdr.GetInt32(3);
                string difficultyLevel = rdr.GetString(4);
                FacilityStatus status = (FacilityStatus)Enum.Parse(typeof(FacilityStatus), rdr.GetString(5));

                WorkoutType wt = new WorkoutType(id, name, maxDurationMinutes, maxCapacity, difficultyLevel, status, false);
                Program.WorkoutTypes.Add(wt);
            }
        }

        public static WorkoutType seekWorkoutType(int workoutTypeId)
        {
            foreach (WorkoutType wt in Program.WorkoutTypes)
            {
                if (wt.getWorkoutTypeId() == workoutTypeId)
                    return wt;
            }
            return null;
        }

        public static int getNextWorkoutTypeId()
        {
            int maxId = 0;
            foreach (WorkoutType wt in Program.WorkoutTypes)
            {
                if (wt.getWorkoutTypeId() > maxId)
                    maxId = wt.getWorkoutTypeId();
            }
            return maxId + 1;
        }
    }
}
