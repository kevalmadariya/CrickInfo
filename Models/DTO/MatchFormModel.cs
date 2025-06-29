using System.ComponentModel.DataAnnotations;

namespace crickinfo_mvc_ef_core.Models.DTO
{
    public class MatchFormModel
    {
        public int Id { get; set; }

        [Required]
        public List<int> SelectedTeamIds { get; set; } = new();

        [Required]
        public int MatchNo { get; set; }

        [Required]
        public DateTime MatchDate { get; set; }

        public string Result { get; set; } = string.Empty;
    }
}
