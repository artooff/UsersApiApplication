using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsersApi.Application.DTO;
using UsersApi.Application.Services.UsersOperations;
using UsersApi.Domain.Exceptions;

namespace UsersApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("")]
        public async Task<IActionResult> GetUsers([FromQuery] RequestParams requestParams)
        {
            var users = await _userService.GetUsersList(requestParams);
            return Ok(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute] int id)
        {
            try
            {
                var user = await _userService.GetUserById(id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> AddUser([FromBody] AddUserDto user)
        {
            try
            {
                var userId = await _userService.AddUser(user);
                return CreatedAtAction(nameof(GetUser), new { id = userId, controller = "Users" }, userId);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (LoginBlockedException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            try
            {
                var deletedId = await _userService.DeleteUser(id);
                return Ok(deletedId);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
