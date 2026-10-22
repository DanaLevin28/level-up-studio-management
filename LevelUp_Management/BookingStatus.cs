namespace LevelUp_Management
{
    // Backed by CK constraint on RoomBookings.status (create_database.sql).
    public enum BookingStatus
    {
        CONFIRMED,
        CANCELLED_ON_TIME,
        CANCELLED_LATE,
        RESCHEDULED
    }
}
