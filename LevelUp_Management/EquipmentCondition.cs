namespace LevelUp_Management
{
    // Backed by CK constraint on Equipment.condition (create_database.sql).
    public enum EquipmentCondition
    {
        FUNCTIONAL,
        DAMAGED,
        OUT_OF_ORDER,
        RETIRED
    }
}
