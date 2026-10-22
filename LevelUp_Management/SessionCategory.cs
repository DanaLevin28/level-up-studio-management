namespace LevelUp_Management
{
    // Backed by CK constraint on TrainingSessions.sessionCategory (create_database.sql).
    public enum SessionCategory
    {
        GROUP,
        PERSONAL,
        COUPLE
    }
}
