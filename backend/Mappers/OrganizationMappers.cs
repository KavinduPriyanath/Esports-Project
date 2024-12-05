using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Dtos.Organization;
using backend.Models;

namespace backend.Mappers
{
    public static class OrganizationMappers
    {
        public static OrganizationDto ToOrganizationDto(this Organization organization)
        {
            return new OrganizationDto
            {
                OrganizationId = organization.OrganizationId,
                CreatedOn = organization.CreatedOn,
                Offlinetime = organization.Offlinetime,
                OrganizationStatus = organization.OrganizationStatus,
                JoinStatus = organization.JoinStatus,
                JoinQuestions = organization.JoinQuestions,
                OrganizationName = organization.OrganizationName,
                Owner = organization.Owner,
                Admin1 = organization.Admin1,
                Admin2 = organization.Admin2,
                Admin3 = organization.Admin3,
                Bio = organization.Bio,
                Country = organization.Country,
                MemberCount = organization.MemberCount
            };
        }

        public static Organization ToUserOrganizationFromCreateDto(this CreateOrganizationDto organizationDto, int ownerId)
        {
            return new Organization
            {
                JoinStatus = organizationDto.JoinStatus,
                JoinQuestions = organizationDto.JoinQuestions,
                OrganizationName = organizationDto.OrganizationName,
                Owner = ownerId,
                Admin1 = organizationDto.Admin1,
                Admin2 = organizationDto.Admin2,
                Admin3 = organizationDto.Admin3,
                Bio = organizationDto.Bio,
                Country = organizationDto.Country,
            };
        }

        public static OrganizationJoinRequestDto ToOrganizationJoinRequestDto(this OrganizationJoinRequest organizationJoinRequest)
        {
            return new OrganizationJoinRequestDto
            {
                OrganizationId = organizationJoinRequest.OrganizationId,
                UserId = organizationJoinRequest.UserId,
                RequestedTime = organizationJoinRequest.RequestedTime,
                ApprovedAdminId = organizationJoinRequest.ApprovedAdminId,
                ApprovedTime = organizationJoinRequest.ApprovedTime,
                RequestStatus = organizationJoinRequest.RequestStatus
            };
        }

        public static OrganizationMemberDto ToOrganizationMemberDto(this OrganizationMember organizationMember)
        {
            return new OrganizationMemberDto
            {
                OrganizationId = organizationMember.OrganizationId,
                UserId = organizationMember.UserId,
                JoinDate = organizationMember.JoinDate,
                MemberStatus = organizationMember.MemberStatus,
                LeftDate = organizationMember.LeftDate
            };
        }
    }
}