using System.ComponentModel.DataAnnotations;

namespace crickinfo_mvc_ef_core.Models
{
    public class Pointstable
    {
        public int Id { get; set; }
        [Required]
        public int Points { get; set; }
        [Required]
        public int Wins { get; set; }
        [Required]
        public int Loss { get; set; }
        [Required]
        public int Draw { get; set; }
        [Required]
        public float NRR  { get; set; }
        public int TeamId { get; set; }
        public Team? Team { get; set; }
        public int TournamentId {  get; set; }
        public Tournament? Tournament { get; set; }


    }
}
