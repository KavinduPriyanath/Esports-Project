using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Interfaces;
using backend.Models;
using backend.Data;
using Microsoft.EntityFrameworkCore;
using backend.Dtos.Organization;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Http.HttpResults;

namespace backend.Repository
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly ApplicationDBContext _context;
        public OrganizationRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public enum OrganizationStatus
        {
            Active = 1,
            Inactive = 0,
            Deleted = -1,
            Suspended = -2
        }

        public enum OrganizationJoinStatus
        {
            Private = 1,
            Public = 0,
            Closed = -1,
            Suspended = -2
        }

        public enum JoinRequestStatus
        {
            Approved = 1,
            Pending = 0,
            Rejected = -1
        }

        public enum JoinMemberStatus
        {
            Active = 1,
            Left = 0,
            Suspended = -1
        }

        public async Task<List<Organization>?> GetAllAsync()
        {
            return await _context.Organizations.ToListAsync();
        }

        public async Task<List<Organization>?> GetAllByIdAsync(int uid)
        {
            return await _context.Organizations.Where(o => o.Owner == uid).ToListAsync();
        }

        public async Task<List<Organization>?> GetAllMemberOrganizationsByIdAsync(int uid)
        {
            return await _context.Organizations.Join(
                _context.OrganizationMembers,
                org => org.OrganizationId,
                member => member.OrganizationId,
                (org, member) => new { org, member }
            ).Where(joined => joined.member.UserId == uid).Select(joined => joined.org).ToListAsync();
        }

        public async Task<Organization?> GetByIdAsync(int id)
        {
            return await _context.Organizations.FindAsync(id);
        }

        public async Task<List<OrganizationMember>> GetOrganizationAllMembers(int oid)
        {
            return await _context.OrganizationMembers.Where(m => m.OrganizationId == oid).ToListAsync();
        }

        public async Task<Organization> CreateAsync(int id, Organization organization)
        {
            await _context.Organizations.AddAsync(organization);
            await _context.SaveChangesAsync();
            return organization;
        }

        public async Task<Organization?> DeleteAsync(int id)
        {
            var organizationModel = await _context.Organizations.FirstOrDefaultAsync(o => o.OrganizationId == id);
            if (organizationModel == null)
            {
                return null;
            }
            organizationModel.OrganizationStatus = (int)OrganizationStatus.Deleted;
            await _context.SaveChangesAsync();
            return organizationModel;
        }

        public async Task<Organization?> DeactivateAsync(int id)
        {
            var organizationModel = await _context.Organizations.FirstOrDefaultAsync(o => o.OrganizationId == id);
            if (organizationModel == null)
            {
                return null;
            }
            organizationModel.OrganizationStatus = (int)OrganizationStatus.Inactive;
            await _context.SaveChangesAsync();
            return organizationModel;
        }

        public async Task<Organization?> SuspendAsync(int id)
        {
            var organizationModel = await _context.Organizations.FirstOrDefaultAsync(o => o.OrganizationId == id);
            if (organizationModel == null)
            {
                return null;
            }
            organizationModel.OrganizationStatus = (int)OrganizationStatus.Suspended;
            await _context.SaveChangesAsync();
            return organizationModel;
        }

        public async Task<Organization?> ReactivateAsync(int id)
        {
            var organizationModel = await _context.Organizations.FirstOrDefaultAsync(o => o.OrganizationId == id);
            if (organizationModel == null)
            {
                return null;
            }
            organizationModel.OrganizationStatus = (int)OrganizationStatus.Active;
            await _context.SaveChangesAsync();
            return organizationModel;
        }

        public async Task<Organization?> MakeCloseAsync(int id)
        {
            var organizationModel = await _context.Organizations.FirstOrDefaultAsync(o => o.OrganizationId == id);
            if (organizationModel == null)
            {
                return null;
            }
            organizationModel.JoinStatus = (int)OrganizationJoinStatus.Closed;
            await _context.SaveChangesAsync();
            return organizationModel;
        }

        public async Task<Organization?> MakePrivateAsync(int id)
        {
            var organizationModel = await _context.Organizations.FirstOrDefaultAsync(o => o.OrganizationId == id);
            if (organizationModel == null)
            {
                return null;
            }
            organizationModel.JoinStatus = (int)OrganizationJoinStatus.Private;
            await _context.SaveChangesAsync();
            return organizationModel;
        }

        public async Task<Organization?> MakePublicAsync(int id)
        {
            var organizationModel = await _context.Organizations.FirstOrDefaultAsync(o => o.OrganizationId == id);
            if (organizationModel == null)
            {
                return null;
            }
            organizationModel.JoinStatus = (int)OrganizationJoinStatus.Public;
            await _context.SaveChangesAsync();
            return organizationModel;
        }

        public async Task<Organization?> UpdateAsync(int id, UpdateOrganizationDto updateOrganizationDto)
        {
            var organizationModel = await _context.Organizations.FirstOrDefaultAsync(o => o.OrganizationId == id);
            if (organizationModel == null)
            {
                return null;
            }
            organizationModel.JoinStatus = updateOrganizationDto.JoinStatus;
            organizationModel.JoinQuestions = updateOrganizationDto.JoinQuestions;
            organizationModel.OrganizationName = updateOrganizationDto.OrganizationName;
            organizationModel.Admin1 = updateOrganizationDto.Admin1;
            organizationModel.Admin2 = updateOrganizationDto.Admin2;
            organizationModel.Admin3 = updateOrganizationDto.Admin3;
            organizationModel.Bio = updateOrganizationDto.Bio;
            organizationModel.Country = updateOrganizationDto.Country;
            organizationModel.MemberCount = updateOrganizationDto.MemberCount;
            await _context.SaveChangesAsync();
            return organizationModel;
        }

        public async Task<OrganizationJoinRequest> AddMemberJoinRequest(int oid, int uid)
        {
            var organizationJoinRequest = new OrganizationJoinRequest
            {
                OrganizationId = oid,
                UserId = oid,
            };
            await _context.OrganizationJoinRequests.AddAsync(organizationJoinRequest);
            await _context.SaveChangesAsync();
            return organizationJoinRequest;
        }

        public async Task<OrganizationMember> AddMemberToOrganization(int oid, int uid)
        {
            var organizationMember = new OrganizationMember
            {
                OrganizationId = oid,
                UserId = oid,
            };
            await _context.OrganizationMembers.AddAsync(organizationMember);
            await _context.SaveChangesAsync();
            return organizationMember;
        }

        public async Task<List<OrganizationJoinRequest>> GetAllOrganizationMemberJoinRequestsById(int oid)
        {
            return await _context.OrganizationJoinRequests.Where(ojr => ojr.OrganizationId == oid).ToListAsync();
        }

        public async Task<OrganizationJoinRequest> AppproveJoinRequest(int oid, int uid, int admin)
        {
            var organizationJoinRequest = await _context.OrganizationJoinRequests.FirstOrDefaultAsync(o => o.OrganizationId == oid && o.UserId == uid);
            if (organizationJoinRequest == null)
            {
                return null;
            }
            organizationJoinRequest.ApprovedTime = DateTime.Now;
            organizationJoinRequest.ApprovedAdminId = admin;
            organizationJoinRequest.RequestStatus = (int)JoinRequestStatus.Approved;
            await _context.SaveChangesAsync();
            return organizationJoinRequest;
        }

        public async Task<OrganizationJoinRequest> DeclineJoinRequest(int oid, int uid, int admin)
        {
            var organizationJoinRequest = await _context.OrganizationJoinRequests.FirstOrDefaultAsync(o => o.OrganizationId == oid && o.UserId == uid);
            if (organizationJoinRequest == null)
            {
                return null;
            }
            organizationJoinRequest.ApprovedTime = DateTime.Now;
            organizationJoinRequest.ApprovedAdminId = admin;
            organizationJoinRequest.RequestStatus = (int)JoinRequestStatus.Rejected;
            await _context.SaveChangesAsync();
            return organizationJoinRequest;
        }

        public async Task<OrganizationMember> LeaveOrganization(int oid, int uid)
        {
            var organizationMember = await _context.OrganizationMembers.FirstOrDefaultAsync(om => om.OrganizationId == oid && om.UserId == uid);
            if (organizationMember == null)
            {
                return null;
            }
            organizationMember.MemberStatus = (int)JoinMemberStatus.Left;
            organizationMember.LeftDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return organizationMember;
        }

        public async Task<OrganizationMember> SuspendUserFromOrganization(int oid, int uid)
        {
            var organizationMember = await _context.OrganizationMembers.FirstOrDefaultAsync(om => om.OrganizationId == oid && om.UserId == uid);
            if (organizationMember == null)
            {
                return null;
            }
            organizationMember.MemberStatus = (int)JoinMemberStatus.Suspended;
            await _context.SaveChangesAsync();
            return organizationMember;
        }

        public async Task<OrganizationMember> RemoveSuspendUserFromOrganization(int oid, int uid)
        {
            var organizationMember = await _context.OrganizationMembers.FirstOrDefaultAsync(om => om.OrganizationId == oid && om.UserId == uid);
            if (organizationMember == null)
            {
                return null;
            }
            organizationMember.MemberStatus = (int)JoinMemberStatus.Active;
            await _context.SaveChangesAsync();
            return organizationMember;
        }

        public async Task<Organization> MakeAdmin(int oid, int uid)
        {
            var organizationDetails = await _context.Organizations.FindAsync(oid);
            if (organizationDetails.Admin1 == null)
            {
                organizationDetails.Admin1 = uid;
                return organizationDetails;
            }
            else if (organizationDetails.Admin2 == null)
            {
                organizationDetails.Admin2 = uid;
                return organizationDetails;
            }
            else if (organizationDetails.Admin3 == null)
            {
                organizationDetails.Admin3 = uid;
                return organizationDetails;
            }
            else
            {
                return null;
            }
        }

        public async Task<Organization> RemoveAdmin(int oid, int uid)
        {
            var organizationDetails = await _context.Organizations.FindAsync(oid);
            if (organizationDetails.Admin1 == uid)
            {
                // organizationDetails.Admin1 = null;
                if (organizationDetails.Admin2 != null)
                {
                    organizationDetails.Admin1 = organizationDetails.Admin2;
                    if (organizationDetails.Admin3 != null)
                    {
                        organizationDetails.Admin2 = organizationDetails.Admin3;
                        organizationDetails.Admin3 = null;
                    }
                    else
                    {
                        organizationDetails.Admin2 = null;
                    }
                }
                return organizationDetails;
            }
            else if (organizationDetails.Admin2 == uid)
            {
                // organizationDetails.Admin2 = null;
                if (organizationDetails.Admin3 != null)
                {
                    organizationDetails.Admin2 = organizationDetails.Admin3;
                    organizationDetails.Admin3 = null;
                }
                else
                {
                    organizationDetails.Admin2 = null;
                }
                return organizationDetails;
            }
            else if (organizationDetails.Admin3 == uid)
            {
                organizationDetails.Admin3 = null;
                return organizationDetails;
            }
            else
            {
                return null;
            }
        }
    }
}
