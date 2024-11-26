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
        Task<Organization> GetByIdAsync(int oid);
        Task<Organization> CreateAsync(int id, Organization organization);
        Task<Organization> UpdateAsync(int id, UpdateOrganizationDto updateOrganizationDto);
        Task<Organization> DeleteAsync(int id);
    }
}