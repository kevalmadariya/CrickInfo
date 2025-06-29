using crickinfo_mvc_ef_core.Models;

namespace crickinfo_mvc_ef_core.Models.DTO
{
    public class MatchContentViewModel
    {
        public int TournamentId { get; set; }
        public List<Team>? Teams { get; set; } = new();
        public List<Match>? Matches { get; set; } = new();
        public MatchFormModel FormModel { get; set; } = new();
        public bool ShowForm { get; set; }
    }
}
