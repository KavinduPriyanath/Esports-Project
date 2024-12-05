using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models
{
    public class OrganizationJoinRequest
    {
        public int OrganizationId { get; set; }
        public int UserId { get; set; }
        public DateTime RequestedTime { get; set; } = DateTime.Now;
        public int? ApprovedAdminId { get; set; } = null;
        public DateTime? ApprovedTime { get; set; } = null;
        public int RequestStatus { get; set; } = 0; // Pending=0, Approved=1, Rejected=-1
    }
}