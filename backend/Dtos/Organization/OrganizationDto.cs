using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;

namespace backend.Dtos.Organization
{
    public class OrganizationDto
    {
        public int OrganizationId { get; set; }
        public required DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime? Offlinetime { get; set; }
        public int OrganizationStatus { get; set; } = 1;
        public int JoinStatus { get; set; } = 1;
        public bool JoinQuestions { get; set; } = false;
        public required string OrganizationName { get; set; }
        public required int Owner { get; set; }
        public int? Admin1 { get; set; } = null;
        public int? Admin2 { get; set; } = null;
        public int? Admin3 { get; set; } = null;
        public string? Bio { get; set; } = null;
        public required string Country { get; set; }
        public int MemberCount { get; set; } = 0;
    }
}