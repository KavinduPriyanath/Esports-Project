using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Interfaces;
using backend.Models;
using backend.Data;
using Microsoft.EntityFrameworkCore;
using backend.Dtos.Organization;

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

        public async Task<List<Organization>> GetAllAsync()
        {
            return await _context.Organizations.ToListAsync();
        }

        public Task<List<Organization>> GetAllByIdAsync(int uid)
        {
            return _context.Organizations.Where(o => o.Owner == uid).ToListAsync();
        }

        public async Task<Organization?> GetByIdAsync(int id)
        {
            return await _context.Organizations.FindAsync(id);
        }

        public async Task<Organization> CreateAsync(int id, Organization organization)
        {
            await _context.Organizations.AddAsync(organization);
            await _context.SaveChangesAsync();
            return organization;
        }

        public async Task<Organization?> DeleteAsync(int id)
        {
            var organizationModel = await  _context.Organizations.FirstOrDefaultAsync(o => o.OrganizationId == id);
            if (organizationModel == null)
            {
                return null;
            }
            organizationModel.OrganizationStatus = (int)OrganizationStatus.Deleted;
            await _context.SaveChangesAsync();
            return organizationModel;
        }

        public async Task<Organization?> DeactiveAsync(int id)
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
    }
}