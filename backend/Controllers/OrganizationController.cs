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

        public OrganizationController(ApplicationDBContext context, IUserRepository userRepo, IOrganizationRepository organizationRepository)
        {
            _context = context;
            _userRepo = userRepo;
            _organizationRepo = organizationRepository;
        }

        private string GetCurrentUserEmail()
        {
            return User.FindFirstValue(ClaimTypes.Email);
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUserDetails()
        {
            var userEmail = GetCurrentUserEmail();
            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized($"User not found {userEmail}");
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserEmail == userEmail);
            if (user == null)
            {
                return NotFound($"User details not found {userEmail}");
            }
            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrganizations()
        {
            var allOrganizations = await _organizationRepo.GetAllAsync();
            var allOrganizationsDto = allOrganizations.Select(pm => pm.ToOrganizationDto());
            return Ok(allOrganizations);
        }

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
                return Ok(organizationDetails);
            }
        }

        // [HttpPost("{uid:int}")]
        // public async Task<IActionResult> CreateOrganization([FromRoute] int uid, [FromBody] CreateOrganizationDto organizationDto)
        // {
        //     if (!ModelState.IsValid)
        //     {
        //         return BadRequest(ModelState);
        //     }
        //     if (!await _userRepo.IsUserExist(uid))
        //     {
        //         return BadRequest("User does not exists");
        //     }
        //     var organizationModel = organizationDto.ToUserOrganizationFromCreateDto(uid);
        //     await _organizationRepo.CreateAsync(uid, organizationModel);
        //     return CreatedAtAction(nameof(GetOrganizationById), new { id = organizationModel.OrganizationId }, organizationModel.ToOrganizationDto());
        // }
        [HttpPost]
        public async Task<IActionResult> CreateOrganization([FromBody] CreateOrganizationDto organizationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var currUserEmail = GetCurrentUserEmail();
            if (string.IsNullOrEmpty(currUserEmail))
            {
                return Unauthorized($"User not found {currUserEmail}");
            }
            var currUser = await _context.Users.FirstOrDefaultAsync(u => u.UserEmail == currUserEmail);
            if (currUser == null)
            {
                return NotFound($"User details not found {currUserEmail}");
            }
            var organizationModel = organizationDto.ToUserOrganizationFromCreateDto(currUser.UserId);
            await _organizationRepo.CreateAsync(currUser.UserId, organizationModel);
            return CreatedAtAction(nameof(GetOrganizationById), new { id = organizationModel.OrganizationId }, organizationModel.ToOrganizationDto());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateOrganizationMethod([FromRoute] int id, [FromBody] UpdateOrganizationDto updateOrganizationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var organizationMethodModel = await _organizationRepo.UpdateAsync(id, updateOrganizationDto);
            if (organizationMethodModel == null)
            {
                return NotFound();
            }
            return Ok(organizationMethodModel.ToOrganizationDto());
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteOrganizationMethod([FromRoute] int id)
        {
            var organizationModel = await _organizationRepo.DeleteAsync(id);
            if (organizationModel == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}