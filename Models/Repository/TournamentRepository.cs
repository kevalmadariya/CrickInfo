using crickinfo_mvc_ef_core.Models.Interfaces;

namespace crickinfo_mvc_ef_core.Models.Repository
{
    public class TournamentRepository : ITournamentRepo
    {
        private readonly CrickDbContext _context;

        public TournamentRepository(CrickDbContext crickDbContext)
        {
            _context = crickDbContext;
        }

        public Tournament Add(Tournament tournament,int? user_id)
        {
            User user = _context.Users.Find(user_id);
            tournament.User = user;
            _context.Tournaments.Add(tournament);
            _context.SaveChanges();
            return tournament;
        }

        public string Delete(int id)
        {
            Tournament u = _context.Tournaments.Find(id);
            if (u != null)
            {
                _context.Tournaments.Remove(u);
                _context.SaveChanges();
            }
            return "tournament deleted successfully";
        }


        public Tournament GetTournamentById(int tournament_id)
        {
            return _context.Tournaments.Find(tournament_id);
        }

        public IEnumerable<Tournament> GetTournaments()
        {
            return _context.Tournaments;
        }

        public Tournament Update(Tournament tournament)
        {
            var u = _context.Tournaments.Attach(tournament);
            u.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return tournament;
        }
    }
}
