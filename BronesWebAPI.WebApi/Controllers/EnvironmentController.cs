using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using BronesWebAPI.WebApi.Models;
using BronesWebAPI.WebApi.Repositories;

namespace BronesWebAPI.WebApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class EnvironmentController : ControllerBase
    {

        private readonly IEnvironmentRepository _environmentRepository;
        private readonly IAuthenticationService _authenticationService;

        private readonly ILogger<EnvironmentController> _logger;

        public EnvironmentController(IEnvironmentRepository environmentRepository, IAuthenticationService authenticationService, ILogger<EnvironmentController> logger)
        {
            _environmentRepository = environmentRepository;
            _authenticationService = authenticationService;
            _logger = logger;
        }

        [HttpGet(Name = "GetEnvironment")]
        public async Task<IEnumerable<Models.Environments>> Get()
        {
            var currentUserId = _authenticationService.GetCurrentAuthenticatedUserId();
            return await _environmentRepository.GetByUserId(currentUserId);

            //return await _environmentRepository.GetAll();
        }

        [HttpPost(Name = "AddEnvironment")]
        public Models.Environments Post([FromBody] Models.Environments environment)
        {
            Console.WriteLine("Reached Post method");
            var currentUserId = _authenticationService.GetCurrentAuthenticatedUserId();

            environment.EnvironmentId = Guid.NewGuid();
            environment.OwnerUserId = Guid.Parse(currentUserId);
            _environmentRepository.Add(environment);
            Console.WriteLine("Executed post method");
            return environment;
        }

        [HttpPut("{name}", Name = "UpdateEnvironment")]
        public async Task<IActionResult> Put(Guid EnvironmentId, [FromBody] Models.Environments updatedEnvironment)
        {
            var environment = await _environmentRepository.GetById(EnvironmentId);
            if (environment == null)
            {
                return NotFound(new { message = "Environment not found" });
            }

            // Use async update method
            await _environmentRepository.UpdateAsync(EnvironmentId.ToString(), updatedEnvironment);
            return Ok(updatedEnvironment);
        }


        [HttpDelete("{name}", Name = "DeleteEnvironment")]
        public async Task<IActionResult> Delete(Guid EnvironmentId)
        {
            var environment = await _environmentRepository.GetById(EnvironmentId);
            if (environment == null)
            {
                return NotFound(new { message = "Environment not found" });
            }

            await _environmentRepository.DeleteAsync(EnvironmentId.ToString());
            return NoContent();
        }
    }
}
