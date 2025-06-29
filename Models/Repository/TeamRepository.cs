using crickinfo_mvc_ef_core.Models.Interfaces;

namespace crickinfo_mvc_ef_core.Models.Repository
{
    public class TeamRepository : ITeamRepo
    {
        private readonly CrickDbContext _context;

        public TeamRepository(CrickDbContext crickDbContext)
        {
            _context = crickDbContext;
        }

        public Team Add(Team  team, int tournamet_id)
        {
            _context.Teams.Add(team);
            _context.SaveChanges();
            Console.WriteLine("start adding");
            
            TeamTournament teamTournament = new TeamTournament
            {
                TeamId = team.Id,
                TournamentId = tournamet_id,
                Team = team,
                Tournament = _context.Tournaments.Find(tournamet_id)
            };

            Console.WriteLine("start adding 2");


            _context.TeamsTournaments.Add(teamTournament);
            _context.SaveChanges();

            return team;
        }

        public string Delete(int id)
        {
            Team  u = _context.Teams.Find(id);
            if (u != null)
            {
                _context.Teams.Remove(u);
                _context.SaveChanges();
            }
            return "team deleted successfully";
        }


        public Team  GetTeamById(int team_id)
        {
            return _context.Teams.Find(team_id);
        }

        public IEnumerable<Team> GetTeamsByTournametId(int tournament_id)
        {
            var teams = _context.TeamsTournaments
                .Where(tt => tt.TournamentId == tournament_id)
                .Select(tt => tt.Team)
                .ToList();
            return teams;
        }

        public Team  Update(Team team)
        {
            var u = _context.Teams.Attach(team);
            u.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return team;
        }
    }
}
