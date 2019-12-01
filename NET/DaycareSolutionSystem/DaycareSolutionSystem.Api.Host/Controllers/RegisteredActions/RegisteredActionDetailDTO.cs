using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaycareSolutionSystem.Api.Host.Controllers.Schedule
{
    public class RegisteredActionDetailDTO
    {
        public Guid Id { get; set; }

        public ClientBasicInfoDTO ClientBasicInfo { get; set; }

        public DateTime ActionStartedDateTime { get; set; }

        public DateTime ActionFinishedDateTime { get; set; }

        public ActionDTO ActionInfo { get; set; }

        public string ClientActionSpecificDescription { get; set; }

        public string Comment { get; set; }

        public bool IsCompleted { get; set; }

        public bool IsCanceled { get; set; }

        public string PhotoUri { get; set; }
    }
}
