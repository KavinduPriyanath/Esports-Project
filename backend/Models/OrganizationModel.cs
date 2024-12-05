using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models
{
    public class Organization
    {
        public int OrganizationId { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime? Offlinetime { get; set; } // One from according to the OrganizationStatus [DeactivatedTime, BannedTime, DeletedTime]
        public int OrganizationStatus { get; set; } = 1; // Active=1, Inactive=0, Deleted=-1, Suspended=-2 etc...
        public int JoinStatus { get; set; } = 1; // Public=0, Private=1, Closed=-1
        public bool JoinQuestions { get; set; } = false;
        public required string OrganizationName { get; set; }
        public required int Owner { get; set; }
        public int? Admin1 { get; set; } = null;
        public int? Admin2 { get; set; } = null;
        public int? Admin3 { get; set; } = null;
        public string? Bio { get; set; } = null;
        public required string Country { get; set; }
        public int MemberCount { get; set; } = 0;

        // public string? Info { get; set; } = null;
        // public float LikeCount { get; set; } = 0;
        // public float FollowerCount { get; set; } = 0;
        // public float Rating { get; set; } = 0;

        // Foreign Key References
        // public int? UserId_Owner { get; set; }
        // public User? User_Owner { get; set; }
    }
}