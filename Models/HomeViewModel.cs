namespace EventHorizon.Models
{
    public class HomeViewModel
    {
        public IEnumerable<Event> Events { get; set; } = new List<Event>();
        public IEnumerable<TeamMember> TeamMembers { get; set; } = new List<TeamMember>();
    }
}