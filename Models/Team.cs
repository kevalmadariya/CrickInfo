using System.ComponentModel.DataAnnotations;

namespace crickinfo_mvc_ef_core.Models
{
    public class Team
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public byte[]? Logo { get; set; }

        public ICollection<TeamTournament>? TeamTournamets { get; set; }
    }
}
