using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Dtos.Organization;
using backend.Models;

namespace backend.Interfaces
{
    public interface IOrganizationRepository
    {
        Task<List<Organization>> GetAllAsync();
        Task<List<Organization>> GetAllByIdAsync(int uid);
        Task<List<Organization>> GetAllMemberOrganizationsByIdAsync(int uid);
        Task<Organization> GetByIdAsync(int oid);
        Task<List<OrganizationMember>> GetOrganizationAllMembers(int oid);
        Task<Organization> CreateAsync(int id, Organization organization);
        Task<Organization> UpdateAsync(int id, UpdateOrganizationDto updateOrganizationDto);
        Task<Organization> DeleteAsync(int id);
        Task<Organization> SuspendAsync(int id);
        Task<Organization> DeactivateAsync(int id);
        Task<Organization> ReactivateAsync(int id);
        Task<Organization> MakeCloseAsync(int id);
        Task<Organization> MakePrivateAsync(int id);
        Task<Organization> MakePublicAsync(int id);
        Task<OrganizationJoinRequest> AddMemberJoinRequest(int oid, int uid);
        Task<OrganizationMember> AddMemberToOrganization(int oid, int uid);
        Task<List<OrganizationJoinRequest>> GetAllOrganizationMemberJoinRequestsById(int oid);
        Task<OrganizationJoinRequest> AppproveJoinRequest(int oid, int uid, int admin);
        Task<OrganizationJoinRequest> DeclineJoinRequest(int oid, int uid, int admin);
        Task<OrganizationMember> LeaveOrganization(int oid, int uid);
        Task<OrganizationMember> SuspendUserFromOrganization(int oid, int uid);
        Task<OrganizationMember> RemoveSuspendUserFromOrganization(int oid, int uid);
        Task<Organization> MakeAdmin(int oid, int uid);
        Task<Organization> RemoveAdmin(int oid, int uid);
    }
}