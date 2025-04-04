namespace BronesWebAPI.WebApi.Models
{
    public class PatientInfo
    {
        public Guid? UserId { get; set; }
        public Guid? OwnerUserId { get; set; }
        public string? Name { get; set; }
        public string? DateOfBirth { get; set; }
        public string? BehandelPlan { get; set; }
        public string? NaamArts { get; set; }
        public string? EersteAfspraak { get; set; }
        public float PositionX { get; set; }
    }
}
