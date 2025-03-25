using BronesWebAPI.WebApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BronesWebAPI.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        //private readonly IAuthenticationService _authenticationService;
        private readonly ILogger<UserController> _logger;


        public UserController(IUserRepository userRepository, IAuthenticationService authenticationService, ILogger<UserController> logger) 
        {
            _userRepository = userRepository;
            //_authenticationService = authenticationService;
            _logger = logger;
        }

        [HttpGet("{id:guid}",Name = "GetUser")]
        public async Task<IEnumerable<Models.Users>> Get(Guid UserId)
        {
            return await _userRepository.GetById(UserId);
  
        }

        [HttpPost(Name = "AddUser")]
        public Models.Users Post([FromBody] Models.Users user)
        {
            user.UserId = Guid.NewGuid();
            _userRepository.Add(user);
            return user;
        }

        [HttpDelete("{id:guid}", Name = "DeleteUser")]
        public async Task<IActionResult> Delete(Guid UserId)
        {
            var user = await _userRepository.GetById(UserId);
            if (user == null)
            {
                return NotFound(new { message = "Environment not found" });
            }

            await _userRepository.DeleteAsync(UserId);
            return NoContent();
        }

        //we gaan geen users hoeven updaten maar hier heb je m voor nu
        [HttpPut("{id:Guid}", Name = "UpdateUser")]
        public async Task<IActionResult> Put(Guid UserId, [FromBody] Models.Users updatedUser)
        {
            var user = await _userRepository.GetById(UserId);
            if (user == null)
            {
                return NotFound(new { message = "Environment not found" });
            }

            // Use async update method
            await _userRepository.UpdateAsync(UserId, updatedUser);
            return Ok(updatedUser);
        }
    }
}
