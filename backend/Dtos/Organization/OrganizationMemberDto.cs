using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Dtos.Organization
{
    public class OrganizationMemberDto
    {
        public int OrganizationId { get; set; }
        public int UserId { get; set; }
        public DateTime JoinDate { get; set; } = DateTime.Now;
        public int MemberStatus { get; set; } = 1;
        public DateTime LeftDate { get; set; }
    }
}