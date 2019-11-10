namespace DaycareSolutionSystem.Services
{
    public interface IPasswordHashService
    {
        string HashPassword(string password);
    }
}
