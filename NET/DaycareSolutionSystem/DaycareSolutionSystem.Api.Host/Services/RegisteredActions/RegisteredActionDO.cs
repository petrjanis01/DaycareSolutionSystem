using DaycareSolutionSystem.Database.Entities.Entities;

namespace DaycareSolutionSystem.Api.Host.Services.RegisteredActions
{
    public class RegisteredActionDO
    {
        public Client Client { get; set; }

        public Action Action { get; set; }

        public RegisteredClientAction RegisteredClientAction { get; set; }

        public bool IsLast { get; set; }
    }
}
