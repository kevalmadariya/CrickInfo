using System.ComponentModel.DataAnnotations;

namespace crickinfo_mvc_ef_core.Models
{
    public class User
    {
       public User() {
            TournamentList = new List<Tournament>();
        }

        public int Id { get; set; }
       
        [Required(ErrorMessage ="Name is required.")]
       [MaxLength(50)]
       public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
        public ICollection<Tournament> TournamentList { get; set; }

    }
}
