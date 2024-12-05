using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.Dtos.Organization;
using backend.Dtos.User;
using backend.Interfaces;
using backend.Mappers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Models;
using backend.Helpers;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace backend.Controllers
{
    [Route("backend/organization")]
    [ApiController]
    [Authorize]
    public class OrganizationController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IUserRepository _userRepo;
        private readonly IOrganizationRepository _organizationRepo;
        private readonly UserHelper _userHelper;

        public OrganizationController(ApplicationDBContext context, IUserRepository userRepo, IOrganizationRepository organizationRepository, UserHelper userHelper)
        {
            _context = context;
            _userRepo = userRepo;
            _organizationRepo = organizationRepository;
            _userHelper = userHelper;
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

        // Get all organization details
        [HttpGet]
        public async Task<IActionResult> GetAllOrganizations()
        {
            var allOrganizations = await _organizationRepo.GetAllAsync();
            var allOrganizationsDto = allOrganizations.Select(pm => pm.ToOrganizationDto());
            return Ok(allOrganizations);
        }

        // Get selected organization details
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOrganizationById([FromRoute] int id)
        {
            var organizationDetails = await _organizationRepo.GetByIdAsync(id);
            if (organizationDetails == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(organizationDetails.ToOrganizationDto());
            }
        }

        // Get all organizations owned by the selected user
        [HttpGet("owner/{uid:int}")]
        public async Task<IActionResult> GetAllOrganizationsOwnedByUser([FromRoute] int uid)
        {
            var allOrganizationsOwned = await _organizationRepo.GetAllByIdAsync(uid);
            if (!allOrganizationsOwned.Any())
            {
                return NotFound();
            }
            else
            {
                return Ok(allOrganizationsOwned.Select(o => o.ToOrganizationDto()));
            }
        }

        // Get all organizations which has membership by the selected user
        [HttpGet("member/{uid:int}")]
        public async Task<IActionResult> GetAllOrganizationsMemberByUser([FromRoute] int uid)
        {
            var allOrganizationsOwned = await _organizationRepo.GetAllMemberOrganizationsByIdAsync(uid);
            if (!allOrganizationsOwned.Any())
            {
                return NotFound();
            }
            else
            {
                return Ok(allOrganizationsOwned.Select(o => o.ToOrganizationDto()));
            }
        }


        // Create an organization. Current user is assigned as the owner.
        // TODO: Ownership change functionality.
        [HttpPost]
        public async Task<IActionResult> CreateOrganization([FromBody] CreateOrganizationDto organizationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var currUserId = await _userHelper.GetCurrentUserIdAsync(HttpContext);
            if (currUserId == null)
            {
                return Unauthorized("User not found.");
            }
            var organizationModel = organizationDto.ToUserOrganizationFromCreateDto(currUserId.Value);
            await _organizationRepo.CreateAsync(currUserId.Value, organizationModel);
            return CreatedAtAction(nameof(GetOrganizationById), new { id = organizationModel.OrganizationId }, organizationModel.ToOrganizationDto());
        }

        // Update organization details - only authorized to admins and owner
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateOrganization([FromRoute] int id, [FromBody] UpdateOrganizationDto updateOrganizationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var currUserId = await _userHelper.GetCurrentUserIdAsync(HttpContext);
            if (currUserId == null)
            {
                return Unauthorized("User not found.");
            }
            var organizationDetails = await _organizationRepo.GetByIdAsync(id);
            if (currUserId.Value == organizationDetails.Owner || currUserId.Value == organizationDetails.Admin1 || currUserId.Value == organizationDetails.Admin2 || currUserId.Value == organizationDetails.Admin3)
            {
                var organizationModel = await _organizationRepo.UpdateAsync(id, updateOrganizationDto);
                if (organizationModel == null)
                {
                    return NotFound();
                }
                return Ok(organizationModel.ToOrganizationDto());
            }
            else
            {
                return Unauthorized("User is not authorized to update organization details");
            }
        }

        //=======================================================================
        //=====================  ORGANIZATION ACTIVE STATUS =====================
        //=======================================================================

        // Delete an Organization
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteOrganization([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var currUserId = await _userHelper.GetCurrentUserIdAsync(HttpContext);
            if (currUserId == null)
            {
                return Unauthorized("User not found.");
            }
            var organizationDetails = await _organizationRepo.GetByIdAsync(id);
            if (currUserId.Value == organizationDetails.Owner || currUserId.Value == organizationDetails.Admin1 || currUserId.Value == organizationDetails.Admin2 || currUserId.Value == organizationDetails.Admin3)
            {
                var organizationModel = await _organizationRepo.DeleteAsync(id);
                if (organizationModel == null)
                {
                    return NotFound();
                }
                return NoContent();
            }
            else
            {
                return Unauthorized("User is not authorized to delete organization");
            }
        }

        // Suspend an Organization
        // TODO: Suspend is done by sys admins - check whether the current user is a sys admin rather than a organization admin
        [HttpPut("org-suspend/{id:int}")]
        public async Task<IActionResult> MakeSuspendOrganization([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var currUserId = await _userHelper.GetCurrentUserIdAsync(HttpContext);
            if (currUserId == null)
            {
                return Unauthorized("User not found.");
            }
            var organizationDetails = await _organizationRepo.GetByIdAsync(id);
            if (currUserId.Value == organizationDetails.Owner || currUserId.Value == organizationDetails.Admin1 || currUserId.Value == organizationDetails.Admin2 || currUserId.Value == organizationDetails.Admin3)
            {
                var organizationModel = await _organizationRepo.SuspendAsync(id);
                if (organizationModel == null)
                {
                    return NotFound();
                }
                return NoContent();
            }
            else
            {
                return Unauthorized("User is not authorized to suspend organization");
            }
        }

        // Deactivate an organization
        [HttpPut("org-deactivate/{id:int}")]
        public async Task<IActionResult> DeactivateOrganization([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var currUserId = await _userHelper.GetCurrentUserIdAsync(HttpContext);
            if (currUserId == null)
            {
                return Unauthorized("User not found.");
            }
            var organizationDetails = await _organizationRepo.GetByIdAsync(id);
            if (currUserId.Value == organizationDetails.Owner || currUserId.Value == organizationDetails.Admin1 || currUserId.Value == organizationDetails.Admin2 || currUserId.Value == organizationDetails.Admin3)
            {
                var organizationModel = await _organizationRepo.DeactivateAsync(id);
                if (organizationModel == null)
                {
                    return NotFound();
                }
                return NoContent();
            }
            else
            {
                return Unauthorized("User is not authorized to suspend organization");
            }
        }

        // Reactivate an organization
        // TODO: If it is suspended sys admin should reactivate. If it is deactivated owner or admins should reactivate. If ot os deleted none can reactivate.
        [HttpPut("org-reactivate/{id:int}")]
        public async Task<IActionResult> ReactivateOrganization([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var currUserId = await _userHelper.GetCurrentUserIdAsync(HttpContext);
            if (currUserId == null)
            {
                return Unauthorized("User not found.");
            }
            var organizationDetails = await _organizationRepo.GetByIdAsync(id);
            if (organizationDetails.OrganizationStatus == (int)OrganizationStatus.Deleted)
            {
                return BadRequest("Organization cannot be reactivated as it is in deleted state");
            }
            else if (organizationDetails.OrganizationStatus == (int)OrganizationStatus.Inactive)
            {
                if (currUserId.Value == organizationDetails.Owner || currUserId.Value == organizationDetails.Admin1 || currUserId.Value == organizationDetails.Admin2 || currUserId.Value == organizationDetails.Admin3)
                {
                    var organizationModel = await _organizationRepo.ReactivateAsync(id);
                    if (organizationModel == null)
                    {
                        return NotFound();
                    }
                    return NoContent();
                }
                else
                {
                    return Unauthorized("User is not authorized to suspend organization");
                }
            }
            else if (organizationDetails.OrganizationStatus == (int)OrganizationStatus.Suspended)
            {
                // TODO: If it is suspended sys admin should reactivate
                return Ok("Sys admins should reactivate");
            }
            else
            {
                return BadRequest("Organization is on Active Status");
            }
        }

        //=======================================================================
        //=========================  MEMBER JOIN STATUS =========================
        //=======================================================================

        // Close an organization
        [HttpPut("org-close/{id:int}")]
        public async Task<IActionResult> MakeCloseOrganization([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var currUserId = await _userHelper.GetCurrentUserIdAsync(HttpContext);
            if (currUserId == null)
            {
                return Unauthorized("User not found.");
            }
            var organizationDetails = await _organizationRepo.GetByIdAsync(id);
            if (currUserId.Value == organizationDetails.Owner || currUserId.Value == organizationDetails.Admin1 || currUserId.Value == organizationDetails.Admin2 || currUserId.Value == organizationDetails.Admin3)
            {
                var organizationModel = await _organizationRepo.MakeCloseAsync(id);
                if (organizationModel == null)
                {
                    return NotFound();
                }
                return NoContent();
            }
            else
            {
                return Unauthorized("User is not authorized to suspend organization");
            }
        }

        // Make private an organization
        [HttpPut("org-private/{id:int}")]
        public async Task<IActionResult> MakePrivateOrganization([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var currUserId = await _userHelper.GetCurrentUserIdAsync(HttpContext);
            if (currUserId == null)
            {
                return Unauthorized("User not found.");
            }
            var organizationDetails = await _organizationRepo.GetByIdAsync(id);
            if (currUserId.Value == organizationDetails.Owner || currUserId.Value == organizationDetails.Admin1 || currUserId.Value == organizationDetails.Admin2 || currUserId.Value == organizationDetails.Admin3)
            {
                var organizationModel = await _organizationRepo.MakePrivateAsync(id);
                if (organizationModel == null)
                {
                    return NotFound();
                }
                return NoContent();
            }
            else
            {
                return Unauthorized("User is not authorized to suspend organization");
            }
        }

        // Make public an organization
        // TODO: Check whether there exists join requests OR Accept all jon requestes automatically.
        [HttpPut("org-public/{id:int}")]
        public async Task<IActionResult> MakePublicOrganization([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var currUserId = await _userHelper.GetCurrentUserIdAsync(HttpContext);
            if (currUserId == null)
            {
                return Unauthorized("User not found.");
            }
            var organizationDetails = await _organizationRepo.GetByIdAsync(id);
            if (currUserId.Value == organizationDetails.Owner || currUserId.Value == organizationDetails.Admin1 || currUserId.Value == organizationDetails.Admin2 || currUserId.Value == organizationDetails.Admin3)
            {
                var organizationModel = await _organizationRepo.MakePublicAsync(id);
                if (organizationModel == null)
                {
                    return NotFound();
                }
                return NoContent();
            }
            else
            {
                return Unauthorized("User is not authorized to suspend organization");
            }
        }

        //=======================================================================
        //============================  MEMBER JOIN  ============================
        //=======================================================================

        // Join to an Organization :
        // If JoinStatus is Public -> Directly add to OrganizationMember Table
        // If JoinStatus is Private -> Add to OrganizationJoinRequest Table
        [HttpPost("join-org/{oid:int}")]
        public async Task<IActionResult> JoinOrganization([FromRoute] int oid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var currUserId = await _userHelper.GetCurrentUserIdAsync(HttpContext);
            if (currUserId == null)
            {
                return Unauthorized("User not found.");
            }
            var organizationDetails = await _organizationRepo.GetByIdAsync(oid);
            if (organizationDetails.JoinStatus == (int)OrganizationJoinStatus.Private)
            {
                var organizationJoinRequest = await _organizationRepo.AddMemberJoinRequest(currUserId.Value, oid);
                return Ok(organizationJoinRequest);
            }
            else if (organizationDetails.JoinStatus == (int)OrganizationJoinStatus.Public)
            {
                var organizationMember = await _organizationRepo.AddMemberToOrganization(currUserId.Value, oid);
                return Ok(organizationMember);
            }
            else
            {
                return Ok("Organization is currently closed");
            }
        }

        // View Join Requests for Admins and Owner for selected organization
        [HttpGet("join-org-req/{oid:int}")]
        public async Task<IActionResult> ViewOrganizationJoinRequests([FromRoute] int oid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var currUserId = await _userHelper.GetCurrentUserIdAsync(HttpContext);
            if (currUserId == null)
            {
                return Unauthorized("User not found.");
            }
            var organizationDetails = await _organizationRepo.GetByIdAsync(oid);
            if (organizationDetails.JoinStatus == (int)OrganizationJoinStatus.Private)
            {
                if (currUserId.Value == organizationDetails.Owner || currUserId.Value == organizationDetails.Admin1 || currUserId.Value == organizationDetails.Admin2 || currUserId.Value == organizationDetails.Admin3)
                {
                    var organizationMemberJoinRequests = await _organizationRepo.GetAllOrganizationMemberJoinRequestsById(oid);
                    var organizationMemberJoinRequestsDto = organizationMemberJoinRequests.Select(ojr => ojr.ToOrganizationJoinRequestDto());
                    if (organizationMemberJoinRequests == null)
                    {
                        return NotFound();
                    }
                    return Ok(organizationMemberJoinRequests);
                }
                else
                {
                    return Unauthorized("User is not authorized to suspend organization");
                }
            }
            else if (organizationDetails.JoinStatus == (int)OrganizationJoinStatus.Closed)
            {
                return BadRequest("Organization is in Closed State. Cannot approve or decline requests.");
            }
            else
            {
                return BadRequest("Organization is in Public State");
            }
        }

        // Accept Reject JoinRequests : If Accepted add to OrganizationMember
        [HttpPut("join-org-req/{oid:int}/{uid:int}/{approveStatus:bool}")]
        public async Task<IActionResult> ActionOnJoinRequest([FromRoute] int oid, [FromRoute] int uid, [FromRoute] bool approveStatus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var currUserId = await _userHelper.GetCurrentUserIdAsync(HttpContext);
            if (currUserId == null)
            {
                return Unauthorized("User not found.");
            }
            var organizationDetails = await _organizationRepo.GetByIdAsync(oid);
            if (currUserId.Value == organizationDetails.Owner || currUserId.Value == organizationDetails.Admin1 || currUserId.Value == organizationDetails.Admin2 || currUserId.Value == organizationDetails.Admin3)
            {
                if (approveStatus == true)
                {
                    var organizationJoinRequestModel = await _organizationRepo.AppproveJoinRequest(oid, uid, currUserId.Value);
                    if (organizationJoinRequestModel == null)
                    {
                        return NotFound();
                    }
                    var organizationMember = await _organizationRepo.AddMemberToOrganization(uid, oid);
                    return Ok(organizationMember);
                }
                else
                {
                    var organizationJoinRequestModel = await _organizationRepo.DeclineJoinRequest(oid, uid, currUserId.Value);
                    if (organizationJoinRequestModel == null)
                    {
                        return NotFound();
                    }
                    return NoContent();
                }
            }
            else
            {
                return Unauthorized("User is not authorized to take actions on join requests");
            }
        }

        // Leave an Organization
        [HttpPut("org-leave/{oid:int}")]
        public async Task<IActionResult> LeaveOrganization([FromRoute] int oid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var currUserId = await _userHelper.GetCurrentUserIdAsync(HttpContext);
            if (currUserId == null)
            {
                return Unauthorized("User not found.");
            }
            var memberDetails = await _organizationRepo.LeaveOrganization(oid, currUserId.Value);
            if (memberDetails == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        // Suspend a user from Organization
        [HttpPut("org-user-suspend/{oid:int}/{uid:int}")]
        public async Task<IActionResult> LeaveOrganization([FromRoute] int oid, [FromRoute] int uid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var currUserId = await _userHelper.GetCurrentUserIdAsync(HttpContext);
            if (currUserId == null)
            {
                return Unauthorized("User not found.");
            }
            var memberDetails = await _organizationRepo.SuspendUserFromOrganization(oid, currUserId.Value);
            if (memberDetails == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        // Remove Suspend a user from Organization
        [HttpPut("org-user-remove-suspend/{oid:int}/{uid:int}")]
        public async Task<IActionResult> RemoveUserSuspedOnOrganization([FromRoute] int oid, [FromRoute] int uid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var currUserId = await _userHelper.GetCurrentUserIdAsync(HttpContext);
            if (currUserId == null)
            {
                return Unauthorized("User not found.");
            }
            var memberDetails = await _organizationRepo.RemoveSuspendUserFromOrganization(oid, currUserId.Value);
            if (memberDetails == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        // Demote Admin by Owner 
        [HttpPut("removeadmin/{oid:int}/{uid:int}")]
        public async Task<IActionResult> DemoteAdmin([FromRoute] int oid, [FromRoute] int uid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var currUserId = await _userHelper.GetCurrentUserIdAsync(HttpContext);
            if (currUserId == null)
            {
                return Unauthorized("User not found.");
            }
            var organizationDetails = await _organizationRepo.GetByIdAsync(oid);
            if (currUserId.Value == organizationDetails.Owner)
            {
                var updatedOrganizationDetails = await _organizationRepo.RemoveAdmin(oid, uid);
                if (updatedOrganizationDetails == null)
                {
                    return NotFound();
                }
                return NoContent();
            }
            else
            {
                return BadRequest("Only Owner of the Organization can demote admins");
            }
        }

        // Promote Admin by Owner 
        [HttpPut("makeadmin/{oid:int}/{uid:int}")]
        public async Task<IActionResult> PromoteAdmin([FromRoute] int oid, [FromRoute] int uid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var currUserId = await _userHelper.GetCurrentUserIdAsync(HttpContext);
            if (currUserId == null)
            {
                return Unauthorized("User not found.");
            }
            var organizationDetails = await _organizationRepo.GetByIdAsync(oid);
            if (currUserId.Value == organizationDetails.Owner)
            {
                if (organizationDetails.Admin3 != null)
                {
                    var updatedOrganizationDetails = await _organizationRepo.MakeAdmin(oid, uid);
                    if (updatedOrganizationDetails == null)
                    {
                        return NotFound();
                    }
                    return NoContent();
                }
                else
                {
                    return BadRequest("Maximum number of admins have been reached");
                }
            }
            else
            {
                return BadRequest("Only Owner of the Organization can promote admins");
            }
        }

        // View all members of a selected organization
        [HttpGet("org-members/{oid:int}")]
        public async Task<IActionResult> GetAllOrganizationMembers([FromRoute] int oid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var organizationMembers = await _organizationRepo.GetOrganizationAllMembers(oid);
            if (organizationMembers == null)
            {
                return NotFound();
            }
            return Ok(organizationMembers.Select(om => om.ToOrganizationMemberDto()));
        }
    }
}