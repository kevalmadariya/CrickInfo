using crickinfo_mvc_ef_core.Models.Interface;

namespace crickinfo_mvc_ef_core.Models.SQL
{
    public class SQLTeamsRepo : ITeamsRepo
    {
        private CrickInfoContext _context;
        private IUnitOfWork _unitOfWork;

        public SQLTeamsRepo(CrickInfoContext context)
        {
            _context = context;
        }

        Team ITeamsRepo.Add(Team team, int tournament_id)
        {
            //add team into TeamTournamet.cs..
            TeamTournament teamTournament = new TeamTournament
            {
                TeamId = team.TeamId,
                TournamentId = tournament_id,
                Team = team,
                Tournament = _context.Tournaments.Find(tournament_id),
                DateJoined = DateTime.Now,
            };
            _unitOfWork.Team.Add(team, tournament_id);
            //_context.TeamTournaments.Add(teamTournament);
            _context.Teams.Add(team);
            _context.SaveChanges();
            return team;
        }

        Team ITeamsRepo.GetTeamById(int id)
        {
            return _context.Teams.Find(id);
        }

        Team ITeamsRepo.Update(Team team)
        {
            var t = _context.Teams.Attach(team);
            t.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return team;
        }

        Team ITeamsRepo.Delete(int id)
        {
            Team t = _context.Teams.Find(id);
            if(t != null)
            {
                _context.Teams.Remove(t);
                _context.SaveChanges();
            }
            return t;
        }

        IEnumerable<Team> ITeamsRepo.GetAllTeams()
        {
            return _context.Teams;
        }


    }
}
