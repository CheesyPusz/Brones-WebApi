using System.ComponentModel.DataAnnotations;

namespace BronesWebAPI.WebApi.Models
{
    public class Environments
    {
        public Guid EnvironmentId { get; set; }

        [Required]
        public string Route { get; set; }

        public Guid OwnerUserId { get; set; }
    }
}