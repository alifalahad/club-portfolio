namespace EventHorizon.Models
{
    public class TeamMember
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }
}