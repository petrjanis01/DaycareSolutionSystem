namespace DaycareSolutionSystem.Database.Migrator
{
    public interface IDemoDataInitializer
    {
        void CreateDemoData();
        void DatabaseInit();
    }
}
