using System.ComponentModel.DataAnnotations;

namespace crickinfo_mvc_ef_core.Models
{
	public class Team
	{
		public Team() { }

		public Team(int id) { this.TeamId = id; }
		public int TeamId { get; set; }  // Primary key

		[Required(ErrorMessage = "Team name is required")]
		public string Name { get; set; } = string.Empty;

		public string? Logo { get; set; } // Optional field for team logo

		// Navigation property for many-to-many relationship with Tournament
		public ICollection<TeamTournament>? TeamTournaments { get; set; }
	}
}
