using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace crickinfo_mvc_ef_core.Models
{
    public class Match
    {
        public int Id { get; set; }

        [Required]
        public int TeamAId { get; set; }

        [ForeignKey("TeamAId")]
        public virtual Team? TeamA { get; set; }

        [Required]
        public int TeamBId { get; set; }

        [ForeignKey("TeamBId")]
        public virtual Team? TeamB { get; set; }

        public string Result { get; set; } = string.Empty;

        [Required]
        public int MatchNo { get; set; }

        [Required]
        public DateTime MatchDate { get; set; }

        public int TournamentId { get; set; }

        [ForeignKey("TournamentId")]
        public virtual Tournament? Tournament { get; set; }
    }
}