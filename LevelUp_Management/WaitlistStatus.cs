namespace LevelUp_Management
{
    // Backed by CK constraint on WaitlistEntries.status (create_database.sql).
    public enum WaitlistStatus
    {
        CONFIRMED,
        WAITLISTED,
        CANCELLED,
        NO_SHOW
    }
}
