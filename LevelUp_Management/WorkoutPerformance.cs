using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace LevelUp_Management
{
    // Association Class: links Trainee <-> TrainingSession, taught by a Trainer
    // (per CLAUDE.md Association Class pattern, all sides are object references).
    public class WorkoutPerformance
    {
        private int workoutPerformanceId;
        private Trainee trainee;
        private TrainingSession trainingSession;
        private Trainer trainer;
        private DateTime date;
        private decimal weight;
        private int sets;
        private int reps;
        private int calorieTarget;
        private int proteinTarget;
        private int dailyStepGoal;
        private string notes;

        public WorkoutPerformance(int workoutPerformanceId, Trainee trainee, TrainingSession trainingSession,
            Trainer trainer, DateTime date, decimal weight, int sets, int reps, int calorieTarget,
            int proteinTarget, int dailyStepGoal, string notes, bool is_new)
        {
            this.workoutPerformanceId = workoutPerformanceId;
            this.trainee = trainee;
            this.trainingSession = trainingSession;
            this.trainer = trainer;
            this.date = date;
            this.weight = weight;
            this.sets = sets;
            this.reps = reps;
            this.calorieTarget = calorieTarget;
            this.proteinTarget = proteinTarget;
            this.dailyStepGoal = dailyStepGoal;
            this.notes = notes;
            if (is_new)
            {
                this.createWorkoutPerformance();
                Program.WorkoutPerformances.Add(this);
            }
        }

        public int getWorkoutPerformanceId() { return this.workoutPerformanceId; }
        public Trainee getTrainee() { return this.trainee; }
        public TrainingSession getTrainingSession() { return this.trainingSession; }
        public Trainer getTrainer() { return this.trainer; }
        public DateTime getDate() { return this.date; }
        public decimal getWeight() { return this.weight; }
        public int getSets() { return this.sets; }
        public int getReps() { return this.reps; }
        public int getCalorieTarget() { return this.calorieTarget; }
        public int getProteinTarget() { return this.proteinTarget; }
        public int getDailyStepGoal() { return this.dailyStepGoal; }
        public string getNotes() { return this.notes; }

        public void setTrainee(Trainee trainee) { this.trainee = trainee; }
        public void setTrainingSession(TrainingSession trainingSession) { this.trainingSession = trainingSession; }
        public void setTrainer(Trainer trainer) { this.trainer = trainer; }
        public void setDate(DateTime date) { this.date = date; }
        public void setWeight(decimal weight) { this.weight = weight; }
        public void setSets(int sets) { this.sets = sets; }
        public void setReps(int reps) { this.reps = reps; }
        public void setCalorieTarget(int calorieTarget) { this.calorieTarget = calorieTarget; }
        public void setProteinTarget(int proteinTarget) { this.proteinTarget = proteinTarget; }
        public void setDailyStepGoal(int dailyStepGoal) { this.dailyStepGoal = dailyStepGoal; }
        public void setNotes(string notes) { this.notes = notes; }

        public void createWorkoutPerformance()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_workout_performance_create @workout_performance_id, @trainee_id, @training_session_id, @trainer_id, @date, @weight, @sets, @reps, @calorieTarget, @proteinTarget, @dailyStepGoal, @notes";
            cmd.Parameters.AddWithValue("@workout_performance_id", this.workoutPerformanceId);
            cmd.Parameters.AddWithValue("@trainee_id", this.trainee.getTraineeId());
            cmd.Parameters.AddWithValue("@training_session_id", this.trainingSession.getTrainingSessionId());
            cmd.Parameters.AddWithValue("@trainer_id", this.trainer.getTrainerId());
            cmd.Parameters.AddWithValue("@date", this.date);
            cmd.Parameters.AddWithValue("@weight", this.weight);
            cmd.Parameters.AddWithValue("@sets", this.sets);
            cmd.Parameters.AddWithValue("@reps", this.reps);
            cmd.Parameters.AddWithValue("@calorieTarget", this.calorieTarget);
            cmd.Parameters.AddWithValue("@proteinTarget", this.proteinTarget);
            cmd.Parameters.AddWithValue("@dailyStepGoal", this.dailyStepGoal);
            cmd.Parameters.AddWithValue("@notes", (object)this.notes ?? DBNull.Value);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        public void updateWorkoutPerformance()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_workout_performance_update @workout_performance_id, @trainee_id, @training_session_id, @trainer_id, @date, @weight, @sets, @reps, @calorieTarget, @proteinTarget, @dailyStepGoal, @notes";
            cmd.Parameters.AddWithValue("@workout_performance_id", this.workoutPerformanceId);
            cmd.Parameters.AddWithValue("@trainee_id", this.trainee.getTraineeId());
            cmd.Parameters.AddWithValue("@training_session_id", this.trainingSession.getTrainingSessionId());
            cmd.Parameters.AddWithValue("@trainer_id", this.trainer.getTrainerId());
            cmd.Parameters.AddWithValue("@date", this.date);
            cmd.Parameters.AddWithValue("@weight", this.weight);
            cmd.Parameters.AddWithValue("@sets", this.sets);
            cmd.Parameters.AddWithValue("@reps", this.reps);
            cmd.Parameters.AddWithValue("@calorieTarget", this.calorieTarget);
            cmd.Parameters.AddWithValue("@proteinTarget", this.proteinTarget);
            cmd.Parameters.AddWithValue("@dailyStepGoal", this.dailyStepGoal);
            cmd.Parameters.AddWithValue("@notes", (object)this.notes ?? DBNull.Value);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        public void deleteWorkoutPerformance()
        {
            Program.WorkoutPerformances.Remove(this);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_workout_performance_delete @workout_performance_id";
            cmd.Parameters.AddWithValue("@workout_performance_id", this.workoutPerformanceId);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        public static void initWorkoutPerformances()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_workout_performance_get_all";
            SQL_CON SC = new SQL_CON();
            SqlDataReader rdr = SC.execute_query(cmd);

            Program.WorkoutPerformances = new List<WorkoutPerformance>();

            // rdr ריק פירושו ש-execute_query כבר דיווח על תקלת התחברות;
            // משאירים רשימה ריקה במקום ליפול על NullReferenceException.
            if (rdr == null) return;

            while (rdr.Read())
            {
                int id = rdr.GetInt32(0);
                Trainee trainee = Trainee.seekTrainee(rdr.GetInt32(1));
                TrainingSession trainingSession = TrainingSession.seekTrainingSession(rdr.GetInt32(2));
                Trainer trainer = Trainer.seekTrainer(rdr.GetInt32(3));
                DateTime date = rdr.GetDateTime(4);
                decimal weight = rdr.GetDecimal(5);
                int sets = rdr.GetInt32(6);
                int reps = rdr.GetInt32(7);
                int calorieTarget = rdr.GetInt32(8);
                int proteinTarget = rdr.GetInt32(9);
                int dailyStepGoal = rdr.GetInt32(10);
                string notes = rdr.IsDBNull(11) ? null : rdr.GetString(11);

                WorkoutPerformance wp = new WorkoutPerformance(id, trainee, trainingSession, trainer, date, weight, sets, reps, calorieTarget, proteinTarget, dailyStepGoal, notes, false);
                Program.WorkoutPerformances.Add(wp);
            }
        }

        public static WorkoutPerformance seekWorkoutPerformance(int workoutPerformanceId)
        {
            foreach (WorkoutPerformance wp in Program.WorkoutPerformances)
            {
                if (wp.getWorkoutPerformanceId() == workoutPerformanceId)
                    return wp;
            }
            return null;
        }

        public static int getNextWorkoutPerformanceId()
        {
            int maxId = 0;
            foreach (WorkoutPerformance wp in Program.WorkoutPerformances)
            {
                if (wp.getWorkoutPerformanceId() > maxId)
                    maxId = wp.getWorkoutPerformanceId();
            }
            return maxId + 1;
        }
    }
}
