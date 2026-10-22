using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace LevelUp_Management
{
    static class Program
    {
        // =====================================================================
        // In-memory lists — one per entity, loaded from the DB at startup.
        // Load order (see CLAUDE.md "Domain Entities and Load Order"):
        //   1. Base entities
        //   2. Entities with FK references to base entities
        //   3. Entities with FK references to level-2 entities
        //   4. Association classes last
        // =====================================================================

        // 1. Base entities
        public static List<Trainee> Trainees;
        public static List<Trainer> Trainers;
        public static List<WorkoutType> WorkoutTypes;
        public static List<Room> Rooms;
        public static List<StudioManager> StudioManagers;

        // 2. Entities with FK references to base entities
        public static List<Membership> Memberships;
        public static List<HealthDeclaration> HealthDeclarations;
        public static List<Equipment> Equipment;
        public static List<TrainingSession> TrainingSessions;

        // 3. Entities with FK references to level-2 entities
        public static List<Payment> Payments;
        public static List<RoomBooking> RoomBookings;
        public static List<FaultReport> FaultReports;

        // 4. Association classes
        public static List<Registration> Registrations;
        public static List<WaitlistEntry> WaitlistEntries;
        public static List<WorkoutPerformance> WorkoutPerformances;

        // =====================================================================
        // Initialize all lists — order must match the dependency order above.
        // =====================================================================
        public static void initLists()
        {
            // 1. Base entities
            Trainee.initTrainees();
            Trainer.initTrainers();
            WorkoutType.initWorkoutTypes();
            Room.initRooms();
            StudioManager.initStudioManagers();

            // 2. Entities with FK references to base entities
            Membership.initMemberships();
            HealthDeclaration.initHealthDeclarations();
            LevelUp_Management.Equipment.initEquipment(); // qualified: "Equipment" here would otherwise resolve to the Program.Equipment field, not the type
            TrainingSession.initTrainingSessions();

            // 3. Entities with FK references to level-2 entities
            Payment.initPayments();
            RoomBooking.initRoomBookings();
            FaultReport.initFaultReports();

            // 4. Association classes last
            Registration.initRegistrations();
            WaitlistEntry.initWaitlistEntries();
            WorkoutPerformance.initWorkoutPerformances();
        }

        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            initLists();
            Application.Run(new mainForm());
        }
    }
}
