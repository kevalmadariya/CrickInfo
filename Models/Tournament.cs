using System.ComponentModel.DataAnnotations;

namespace crickinfo_mvc_ef_core.Models
{
    public class Tournament
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Date is required.")]
        public DateTime DateOfTournament { get; set; }

        public string Status { get; set; } = string.Empty;

        public int UserId { get; set; }
        public User? User { get; set; }
        public ICollection<TeamTournament>? TeamTournamets { get; set; }
        public ICollection<Pointstable>? PointsTables { get; set; }

    }
}
