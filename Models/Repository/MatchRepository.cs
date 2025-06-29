using crickinfo_mvc_ef_core.Models.Interfaces;

namespace crickinfo_mvc_ef_core.Models.Repository
{
    public class MatchRepository : IMatchRepo
    {
        private readonly CrickDbContext _context;

        public MatchRepository(CrickDbContext crickDbContext)
        {
            _context = crickDbContext;
        }

        public Match Add(Match match, int tournamet_id)
        {
            _context.Matches.Add(match);
            _context.SaveChanges();
            return match;
        }

        public string Delete(int id)
        {
            Match u = _context.Matches.Find(id);
            if (u != null)
            {
                _context.Matches.Remove(u);
                _context.SaveChanges();
            }
            return "match deleted successfully";
        }

        public IEnumerable<Match> GetPointsTables()
        {
            return _context.Matches;
        }

        public Match GetMatchById(int match_id)
        {
            return _context.Matches.Find(match_id);
        }

        public IEnumerable<Match> GetMatchByTournamentId(int tournament_id)
        {
            var matchs = _context.Matches
                .Where(tt => tt.TournamentId == tournament_id)
                .ToList();
            return matchs;
        }

        public Match Update(Match match)
        {
            var u = _context.Matches.Attach(match);
            u.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return match;
        }
    }
}
