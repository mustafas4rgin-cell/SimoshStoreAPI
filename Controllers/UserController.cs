using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimoshStore;
using SimoshStoreAPI;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserDTO user)
        {
            
            var result = await _userService.CreateUserAsync(user);
            if(!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(user);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if(user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetUsersAsync();
            if(users is null)
            {
                return NotFound();
            }
            return Ok(users);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(UserDTO user, int id)
        {
            var result = await _userService.UpdateUserAsync(user, id);
            if(!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if(!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }
        
    }
}
