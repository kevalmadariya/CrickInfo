namespace crickinfo_mvc_ef_core.Models.DTO
{
    
        public class TeamContentViewModel
        {
            public IEnumerable<Team>? Teams { get; set; }
            public Team TeamForm { get; set; } // Used for both Add and Edit
            public bool ShowForm { get; set; } // Flag to control form visibility
            public int TournamentId {  get; set; }
        }

}
