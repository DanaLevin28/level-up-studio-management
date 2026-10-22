namespace LevelUp_Management
{
    // Backed by CK constraint on Memberships.status (create_database.sql).
    public enum MembershipStatus
    {
        ACTIVE,
        FROZEN,
        EXPIRED,
        SUSPENDED
    }
}
