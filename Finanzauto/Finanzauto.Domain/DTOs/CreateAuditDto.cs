using Finanzauto.Common.Enums;
using Finanzauto.Domain.Entities;

namespace Finanzauto.Domain.DTOs
{
    public class CreateAuditDto
    {
        public ActionAudit ActionAudit { get; set; }
        public DateTime Created { get; set; }
        public int VehicleId { get; set; }
        public User User { get; set; }
        public string PreviousValue { get; set; }
        public string NewValue { get; set; }
    }
}
