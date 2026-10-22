namespace LevelUp_Management
{
    // Backed by CK constraint on Registrations.status (create_database.sql).
    // WaitlistEntries.status uses the separate WaitlistStatus enum — see WaitlistStatus.cs.
    public enum RegistrationStatus
    {
        PENDING,
        CONFIRMED,
        ACTIVE,
        COMPLETED,
        WAITLISTED,
        CANCELLED,
        NO_SHOW
    }
}
