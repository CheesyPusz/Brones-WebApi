namespace BronesWebAPI.WebApi.Models
{
    public class Users
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public DateOnly BirthDate { get; set; }
        public string BehandelPlan { get; set; }
        public string NaamArts { get; set; }
        public DateOnly EersteAfspraak { get; set; }
    }
}
