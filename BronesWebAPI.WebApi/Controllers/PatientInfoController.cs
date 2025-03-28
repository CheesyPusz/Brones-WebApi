using BronesWebAPI.WebApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BronesWebAPI.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatientInfoController : ControllerBase
    {
        private readonly IPatientInfoRepository _userRepository;
        private readonly IAuthenticationService _authenticationService;
        private readonly ILogger<PatientInfoController> _logger;


        public PatientInfoController(IPatientInfoRepository userRepository, IAuthenticationService authenticationService, ILogger<PatientInfoController> logger) 
        {
            _userRepository = userRepository;
            _authenticationService = authenticationService;
            _logger = logger;
        }

        [HttpGet(Name = "GetUser")]
        public async Task<Models.PatientInfo> Get()
        {
            var currentUserId = Guid.Parse(_authenticationService.GetCurrentAuthenticatedUserId());
            return await _userRepository.GetById(currentUserId);
  
        }

        [HttpPost(Name = "AddUser")]
        public Models.PatientInfo Post([FromBody] Models.PatientInfo user)
        {
            user.UserId = Guid.NewGuid();
            user.OwnerUserId = Guid.Parse(_authenticationService.GetCurrentAuthenticatedUserId());
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
        public async Task<IActionResult> Put(Guid UserId, [FromBody] Models.PatientInfo updatedUser)
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
