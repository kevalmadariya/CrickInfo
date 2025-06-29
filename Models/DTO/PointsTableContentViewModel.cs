using crickinfo_mvc_ef_core.Models;

namespace crickinfo_mvc_ef_core.Models.DTO
{
    public class PointsTableContentViewModel
    {
        public int TournamentId { get; set; }
        public IEnumerable<Team>? Teams { get; set; }
        public IEnumerable<Pointstable>? PointsTableEntries { get; set; }
        public bool ShowForm { get; set; }
    }
}
