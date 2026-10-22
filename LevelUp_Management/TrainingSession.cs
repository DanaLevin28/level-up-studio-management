using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace LevelUp_Management
{
    public class TrainingSession
    {
        private int trainingSessionId;
        private Room room;
        private WorkoutType workoutType;
        private Trainer trainer;
        private DateTime date;
        private TimeSpan startTime;
        private TimeSpan endTime;
        private SessionCategory sessionCategory;
        private int availableSlots;

        public TrainingSession(int trainingSessionId, Room room, WorkoutType workoutType, Trainer trainer,
            DateTime date, TimeSpan startTime, TimeSpan endTime, SessionCategory sessionCategory, int availableSlots,
            bool is_new)
        {
            this.trainingSessionId = trainingSessionId;
            this.room = room;
            this.workoutType = workoutType;
            this.trainer = trainer;
            this.date = date;
            this.startTime = startTime;
            this.endTime = endTime;
            this.sessionCategory = sessionCategory;
            this.availableSlots = availableSlots;
            if (is_new)
            {
                this.createTrainingSession();
                Program.TrainingSessions.Add(this);
            }
        }

        public int getTrainingSessionId() { return this.trainingSessionId; }
        public Room getRoom() { return this.room; }
        public WorkoutType getWorkoutType() { return this.workoutType; }
        public Trainer getTrainer() { return this.trainer; }
        public DateTime getDate() { return this.date; }
        public TimeSpan getStartTime() { return this.startTime; }
        public TimeSpan getEndTime() { return this.endTime; }
        public SessionCategory getSessionCategory() { return this.sessionCategory; }
        public int getAvailableSlots() { return this.availableSlots; }

        public void setRoom(Room room) { this.room = room; }
        public void setWorkoutType(WorkoutType workoutType) { this.workoutType = workoutType; }
        public void setTrainer(Trainer trainer) { this.trainer = trainer; }
        public void setDate(DateTime date) { this.date = date; }
        public void setStartTime(TimeSpan startTime) { this.startTime = startTime; }
        public void setEndTime(TimeSpan endTime) { this.endTime = endTime; }
        public void setSessionCategory(SessionCategory sessionCategory) { this.sessionCategory = sessionCategory; }
        public void setAvailableSlots(int availableSlots) { this.availableSlots = availableSlots; }

        public void createTrainingSession()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_training_session_create @training_session_id, @room_id, @workout_type_id, @trainer_id, @date, @startTime, @endTime, @sessionCategory, @availableSlots";
            cmd.Parameters.AddWithValue("@training_session_id", this.trainingSessionId);
            cmd.Parameters.AddWithValue("@room_id", this.room.getRoomId());
            cmd.Parameters.AddWithValue("@workout_type_id", this.workoutType.getWorkoutTypeId());
            cmd.Parameters.AddWithValue("@trainer_id", this.trainer.getTrainerId());
            cmd.Parameters.AddWithValue("@date", this.date);
            cmd.Parameters.AddWithValue("@startTime", this.startTime);
            cmd.Parameters.AddWithValue("@endTime", this.endTime);
            cmd.Parameters.AddWithValue("@sessionCategory", this.sessionCategory.ToString());
            cmd.Parameters.AddWithValue("@availableSlots", this.availableSlots);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        public void updateTrainingSession()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_training_session_update @training_session_id, @room_id, @workout_type_id, @trainer_id, @date, @startTime, @endTime, @sessionCategory, @availableSlots";
            cmd.Parameters.AddWithValue("@training_session_id", this.trainingSessionId);
            cmd.Parameters.AddWithValue("@room_id", this.room.getRoomId());
            cmd.Parameters.AddWithValue("@workout_type_id", this.workoutType.getWorkoutTypeId());
            cmd.Parameters.AddWithValue("@trainer_id", this.trainer.getTrainerId());
            cmd.Parameters.AddWithValue("@date", this.date);
            cmd.Parameters.AddWithValue("@startTime", this.startTime);
            cmd.Parameters.AddWithValue("@endTime", this.endTime);
            cmd.Parameters.AddWithValue("@sessionCategory", this.sessionCategory.ToString());
            cmd.Parameters.AddWithValue("@availableSlots", this.availableSlots);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        public void deleteTrainingSession()
        {
            Program.TrainingSessions.Remove(this);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_training_session_delete @training_session_id";
            cmd.Parameters.AddWithValue("@training_session_id", this.trainingSessionId);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        public static void initTrainingSessions()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_training_session_get_all";
            SQL_CON SC = new SQL_CON();
            SqlDataReader rdr = SC.execute_query(cmd);

            Program.TrainingSessions = new List<TrainingSession>();

            // rdr ריק פירושו ש-execute_query כבר דיווח על תקלת התחברות;
            // משאירים רשימה ריקה במקום ליפול על NullReferenceException.
            if (rdr == null) return;

            while (rdr.Read())
            {
                int id = rdr.GetInt32(0);
                Room room = Room.seekRoom(rdr.GetInt32(1));
                WorkoutType workoutType = WorkoutType.seekWorkoutType(rdr.GetInt32(2));
                Trainer trainer = Trainer.seekTrainer(rdr.GetInt32(3));
                DateTime date = rdr.GetDateTime(4);
                TimeSpan startTime = rdr.GetTimeSpan(5);
                TimeSpan endTime = rdr.GetTimeSpan(6);
                SessionCategory sessionCategory = (SessionCategory)Enum.Parse(typeof(SessionCategory), rdr.GetString(7));
                int availableSlots = rdr.GetInt32(8);

                TrainingSession ts = new TrainingSession(id, room, workoutType, trainer, date, startTime, endTime, sessionCategory, availableSlots, false);
                Program.TrainingSessions.Add(ts);
            }
        }

        public static TrainingSession seekTrainingSession(int trainingSessionId)
        {
            foreach (TrainingSession ts in Program.TrainingSessions)
            {
                if (ts.getTrainingSessionId() == trainingSessionId)
                    return ts;
            }
            return null;
        }

        public static int getNextTrainingSessionId()
        {
            int maxId = 0;
            foreach (TrainingSession ts in Program.TrainingSessions)
            {
                if (ts.getTrainingSessionId() > maxId)
                    maxId = ts.getTrainingSessionId();
            }
            return maxId + 1;
        }
    }
}
