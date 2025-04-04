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
        public async Task<ActionResult<Models.PatientInfo>> Get()
        {
            var currentUserId = Guid.Parse(_authenticationService.GetCurrentAuthenticatedUserId());
            var patientInfo = await _userRepository.GetById(currentUserId);
            Console.WriteLine(patientInfo.ToString());
            if (patientInfo == null)
            {
                return NotFound(new { message = "PatientInfo not found" });
            }

            return Ok(patientInfo);
        }

        [HttpPost(Name = "AddUser")]
        public async Task<Models.PatientInfo> Post([FromBody] Models.PatientInfo user)
        {
            user.UserId = Guid.NewGuid();
            user.OwnerUserId = Guid.Parse(_authenticationService.GetCurrentAuthenticatedUserId());

            //user.OwnerUserId = Guid.NewGuid();
            await _userRepository.Add(user);
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
        [HttpPut("{PositionX} ", Name = "UpdateUser")]
        public async Task<IActionResult> Put(string PositionX)
        {
            //var user = await _userRepository.GetById(id);
            //if (user == null)
            //{
            //    return NotFound(new { message = "Environment not found" });
            //}

            Console.WriteLine("Test");
            var currentUserId = _authenticationService.GetCurrentAuthenticatedUserId();
            if (currentUserId == null)
                BadRequest();

            var positionX = float.Parse(PositionX);

            // Use async update method
            await _userRepository.UpdateAsync(Guid.Parse(currentUserId), positionX);
            return Ok();
        }
    }
}
