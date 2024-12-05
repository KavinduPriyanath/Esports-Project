using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Dtos.Organization
{
    public class OrganizationJoinRequestDto
    {
        public int OrganizationId { get; set; }
        public int UserId { get; set; }
        public DateTime RequestedTime { get; set; }
        public int? ApprovedAdminId { get; set; } = null;
        public DateTime? ApprovedTime { get; set; } = null;
        public int RequestStatus { get; set; } = 0;
    }
}